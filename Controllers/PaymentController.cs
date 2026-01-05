using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using vynce_api.DataProvider;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : BaseController
    {
        
        private readonly IPaymentDataProvider _paymentDataProvider;
        public PaymentController(IHttpContextAccessor httpContextAccessor, IPaymentDataProvider paymentDataProvider) : base(httpContextAccessor)
        {
            _paymentDataProvider = paymentDataProvider;
        }

        [HttpPost("razor-order")]
        public async Task<BaseResponse<RazorOrderResponse>> RazorOrder(RazorOrderRequest request)
        {
            
            return await DelegateControllerCall(async () =>
            {
                return await _paymentDataProvider.RazorOrder(request);
            });
        }

        [HttpPost("verify-payment")]
        public async Task<BaseResponse<int>> RazorPaymentVerify(RazorPayVerifyRequest request)
        {

            return await DelegateControllerCall(async () =>
            {
                return await _paymentDataProvider.RazorPaymentVerify(request);
            });
        }
        [HttpPost("add")]
        public async Task<BaseResponse<int>> AddPayment(PaymentAddRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _paymentDataProvider.AddPayment(request);
            });
        }
        [HttpPost("purchase-history")]
        public async Task<BaseResponse<PurchaseHistoryListResponse>> PurchaseHistoryList(PurchaseHistoryListRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _paymentDataProvider.PurchaseHistoryList(request);
            });
        }
    }
}
