using Microsoft.Data.SqlClient;
using Razorpay.Api;
using System.Data;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class PaymentDataProvider: BaseDataProvider,IPaymentDataProvider
    {
        private string _razor_key_id = ApplicationConfigurations.razor_key_id;
        private string _razor_key_secret = ApplicationConfigurations.razor_key_secret;
        IDataProviderHelper _dataProviderHelper;
        public PaymentDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }

        #region reader
        public async Task<PurchaseHistoryListResponse> PurchaseHistoryListReader(SqlDataReader reader)
        {
            PurchaseHistoryListResponse response = new PurchaseHistoryListResponse();
            var members = new List<PurchaseHistory>();

            while (await reader.ReadAsync())
            {
                members.Add(new PurchaseHistory()
                {
                    payment_date = ConvertString(reader, "payment_date"),
                    amount = ConvertString(reader, "amount"),
                    membership_card_name = ConvertString(reader, "membership_card_name"),
                    membership_card_id = ConvertIntiger(reader, "membership_card_id"),
                    //membership_start_date = ConvertToDate(reader, "membership_start_date"),
                    //membership_end_date = ConvertToDate(reader, "membership_end_date")
                });
            }

            if (reader.NextResult())
            {
                while (await reader.ReadAsync())
                {
                    response.total_count = ConvertIntiger(reader, "total_record");
                }
            }

            response.list = members;
            return response;
        }
        #endregion

        public async Task<BaseResponse<int>> AddPayment(PaymentAddRequest request)
        {
            var payment_date = DateTime.UtcNow;
            var sqlParameters = new List<SqlParameter>() {

                new SqlParameter("i_member_id",request.member_id),
                new SqlParameter("i_membership_card_id",request.membership_card_id),
                new SqlParameter("i_amount",request.amount),
                new SqlParameter("i_razorpay_order_id",request.razorpay_order_id),
                new SqlParameter("i_razorpay_payment_id",request.razorpay_payment_id),
                new SqlParameter("i_razorpay_signature",request.razorpay_signature),
                new SqlParameter("i_payment_date",payment_date),
            };

            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.PAYMENT_ADD_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<RazorOrderResponse>> RazorOrder(RazorOrderRequest request)
        {
            RazorOrderResponse response = new RazorOrderResponse();
            RazorpayClient razorpayClient = new RazorpayClient(_razor_key_id, _razor_key_secret);
            Dictionary<string, object> options = new Dictionary<string, object>
            {
                {  "amount", request.amount * 100 },
                { "currency", request.currency },
                { "receipt", request.receipt }

            };

            try
            {
                Razorpay.Api.Order order = razorpayClient.Order.Create(options);
                response.order_id = order["id"].ToString();
                response.amount = order["amount"].ToString();
                response.currency = order["currency"].ToString();
                return new BaseResponse<RazorOrderResponse>()
                {
                    data = response,
                    message = "Razor Order Created Successfully",
                    status = 1
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<RazorOrderResponse>()
                {
                    data = response,
                    message = "Razor Order Created Failed",
                    status = 0
                };
            }
            
        }

        public async Task<BaseResponse<int>> RazorPaymentVerify(RazorPayVerifyRequest request)
        {
            RazorpayClient razorpayClient = new RazorpayClient(_razor_key_id, _razor_key_secret);
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("razorpay_order_id", request.razorpay_order_id);
            options.Add("razorpay_payment_id", request.razorpay_payment_id);
            options.Add("razorpay_signature", request.razorpay_signature);

            try
            {
                Utils.verifyPaymentSignature(options);
                return new BaseResponse<int>()
                {
                    data = 1,
                    message = "Payment verified successfully",
                    status = 1
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    data = 0,
                    message = "Payment verification failed",
                    status = 0
                };
            }

        }

        public async Task<BaseResponse<PurchaseHistoryListResponse>> PurchaseHistoryList(PurchaseHistoryListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("page_size",request.page_size),
                new SqlParameter("page_no",request.page_no),
                new SqlParameter("sorting_by",request.sorting_by),
                new SqlParameter("sorting_column",request.sorting_column),
                new SqlParameter("i_member_id",request.member_id)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.PURCHASE_HISTORY_LIST_V1, PurchaseHistoryListReader, sqlParameters.ToArray());

            return new BaseResponse<PurchaseHistoryListResponse>()
            {
                data = response
            };
        }
    }
}
