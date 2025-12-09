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
    }
}
