using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vynce_api.DataProvider;
using vynce_api.DataProvider.Interface;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.Controllers
{
    [Route("api/membership-card")]
    [ApiController]
    public class MembershipCardController : BaseController
    {
        private readonly IMembershipCardDataProvider _membershipcardDataProvider;
        public MembershipCardController(IHttpContextAccessor httpContextAccessor, IMembershipCardDataProvider membershipcardDataProvider) : base(httpContextAccessor)
        {
            _membershipcardDataProvider = membershipcardDataProvider;
        }

        [HttpPost("list")]
        public async Task<BaseResponse<MembershipCardListResponse>> MembershipCardList(MembershipCardListRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _membershipcardDataProvider.MembershipCardList(request);
            });
        }

        [HttpPost("add")]
        public async Task<BaseResponse<int>> AddMembershipCard(MembershipCardAddRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _membershipcardDataProvider.AddMembershipCard(request);
            });
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<MembershipCardGetResponse>> MembershipCardGet(string id)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _membershipcardDataProvider.MembershipCardGet(id);
            });
        }
        [HttpDelete("{id}")]
        public async Task<BaseResponse<int>> MembershipCardDelete(string id)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _membershipcardDataProvider.MembershipCardDelete(id);
            });
        }
        [HttpPut("{id}")]
        public async Task<BaseResponse<int>> MembershipCardUpdate(string id, MembershipCardUpdateRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _membershipcardDataProvider.MembershipCardUpdate(id, request);
            });
        }
        [HttpPost("membership-list")]
        public async Task<BaseResponse<MembershipCardListResponse>> GetMembershipList(MembershipCardListRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _membershipcardDataProvider.GetMembershipList();
            });
        }
    }
}
