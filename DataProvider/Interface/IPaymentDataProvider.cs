using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface IPaymentDataProvider
    {
        Task<BaseResponse<int>> AddPayment(PaymentAddRequest request);
        Task<BaseResponse<RazorOrderResponse>> RazorOrder(RazorOrderRequest request);
        Task<BaseResponse<int>> RazorPaymentVerify(RazorPayVerifyRequest request);
    }
}
