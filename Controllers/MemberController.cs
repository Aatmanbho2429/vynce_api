using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vynce_api.DataProvider.Interface;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.Controllers
{
    [Route("api/member")]
    [ApiController]
    public class MemberController : BaseController
    {
        private readonly IMemberDataProvider _memberDataProvider;

        public MemberController(IHttpContextAccessor httpContextAccessor,IMemberDataProvider memberDataProvider) : base(httpContextAccessor)
        {
            _memberDataProvider= memberDataProvider;
        }

        [HttpPost("add")]
        public async Task<BaseResponse<int>> AddMember(MemberAddRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.AddMember(request);
            });
        }
        [HttpPost("list")]
        public async Task<BaseResponse<MemberListResponse>> MemberList(MemberListRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.MemberList(request);
            });
        }
        [HttpGet("{id}")]
        public async Task<BaseResponse<MemberGetResponse>> MemberGet(string id)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.MemberGet(id);
            });
        }
        [HttpGet("profile/{id}")]
        public async Task<BaseResponse<MemberGetProfileResponse>> MemberGetProfile(string id)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.MemberGetProfile(id);
            });
        }

        [HttpGet("free-search/{id}")]
        public async Task<BaseResponse<MemberGetFreeSearchResponse>> MemberGetFreeSearch(string id)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.MemberGetFreeSearch(id);
            });
        }

        [HttpDelete("{id}")]
        public async Task<BaseResponse<int>> MemberDelete(string id)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.MemberDelete(id);
            });
        }
        [HttpPut("{id}")]
        public async Task<BaseResponse<int>> MemberUpdate(string id, MemberUpdateRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.MemberUpdate(id, request);
            });
        }
        [AllowAnonymous]

        [HttpPost("exist")]
        public async Task<BaseResponse<int>> MemberExist(MemberExistEmailRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.MemberExist(request);
            });
        }

        [AllowAnonymous]

        [HttpPost("send-email")]
        public async Task<BaseResponse<int>> MemberSendEmail(MemberSendEmailRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _memberDataProvider.MemberSendEmail(request);
            });
        }
    }
}
