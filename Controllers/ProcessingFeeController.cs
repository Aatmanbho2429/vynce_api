using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vynce_api.DataProvider.Interface;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.Controllers
{
    [Route("api/processing-fee")]
    [ApiController]
    public class ProcessingFeeController : BaseController
    {
        private readonly IProcessingFeeDataProvider _processingFeeDataProvider;
        public ProcessingFeeController(IHttpContextAccessor httpContextAccessor, IProcessingFeeDataProvider processingFeeDataProvider) : base(httpContextAccessor)
        {
            _processingFeeDataProvider = processingFeeDataProvider;
        }

        [HttpPost("list")]
        public async Task<BaseResponse<ProcessingFeeListResponse>> ProcessingFeeList(ProcessingFeeListRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _processingFeeDataProvider.ProcessingFeeList(request);
            });
        }

        [HttpPost("add")]
        public async Task<BaseResponse<int>> AddProcessingFee(ProcessingFeeAddRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _processingFeeDataProvider.AddProcessingFee(request);
            });
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<ProcessingFeeGetResponse>> ProcessingFeeGet(string id)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _processingFeeDataProvider.ProcessingFeeGet(id);
            });
        }
        [HttpDelete("{id}")]
        public async Task<BaseResponse<int>> ProcessingFeeDelete(string id)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _processingFeeDataProvider.ProcessingFeeDelete(id);
            });
        }
        [HttpPut("{id}")]
        public async Task<BaseResponse<int>> ProcessingFeeUpdate(string id, ProcessingFeeUpdateRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _processingFeeDataProvider.ProcessingFeeUpdate(id, request);
            });
        }
        [HttpPost("processing-fee-list")]
        public async Task<BaseResponse<ProcessingFeeListResponse>> GetProcessingFeeList(ProcessingFeeListRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _processingFeeDataProvider.GetProcessingFeeList(request);
            });
        }
        [HttpPost("processing-fee-type-list")]
        public async Task<BaseResponse<ProcessingFeeTypeListResponse>> GetProcessingFeeTypeList(ProcessingFeeTypeListRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _processingFeeDataProvider.GetProcessingFeeTypeList(request);
            });
        }
    }
}
