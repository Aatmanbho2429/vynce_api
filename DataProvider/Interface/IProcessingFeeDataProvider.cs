using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface IProcessingFeeDataProvider
    {
        Task<BaseResponse<ProcessingFeeListResponse>> ProcessingFeeList(ProcessingFeeListRequest request);
        Task<BaseResponse<int>> AddProcessingFee(ProcessingFeeAddRequest request);
        Task<BaseResponse<ProcessingFeeGetResponse>> ProcessingFeeGet(string id);
        Task<BaseResponse<int>> ProcessingFeeDelete(string id);
        Task<BaseResponse<int>> ProcessingFeeUpdate(string id, ProcessingFeeUpdateRequest request);
        Task<BaseResponse<ProcessingFeeListResponse>> GetProcessingFeeList(ProcessingFeeListRequest request);
        Task<BaseResponse<ProcessingFeeTypeListResponse>> GetProcessingFeeTypeList(ProcessingFeeTypeListRequest request);
    }
}
