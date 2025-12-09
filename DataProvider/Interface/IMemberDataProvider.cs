using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface IMemberDataProvider
    {
        Task<BaseResponse<int>> AddMember(MemberAddRequest request);
        Task<BaseResponse<MemberListResponse>> MemberList(MemberListRequest request);
        Task<BaseResponse<MemberGetResponse>> MemberGet(string id);
        Task<BaseResponse<int>> MemberDelete(string id);
        Task<BaseResponse<int>> MemberUpdate(string id, MemberUpdateRequest request);
    }
}
