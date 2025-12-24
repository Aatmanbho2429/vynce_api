using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface IMembershipCardDataProvider
    {
        Task<BaseResponse<MembershipCardListResponse>> MembershipCardList(MembershipCardListRequest request);
        Task<BaseResponse<int>> AddMembershipCard(MembershipCardAddRequest request);
        Task<BaseResponse<MembershipCardGetResponse>> MembershipCardGet(string id);
        Task<BaseResponse<int>> MembershipCardDelete(string id);
        Task<BaseResponse<int>> MembershipCardUpdate(string id, MembershipCardUpdateRequest request);
    }
}
