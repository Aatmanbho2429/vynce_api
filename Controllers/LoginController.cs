using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vynce_api.DataProvider.Interface;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.Controllers
{
    [Route("api/login")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private IJwtTokenDataProvider _jwtDataProvider;
        private ILoginDataProvider _loginProvider;
        public LoginController(IJwtTokenDataProvider jwtDataProvider, IHttpContextAccessor httpContextAccessor, ILoginDataProvider loginProvider) : base(httpContextAccessor)
        {
            _jwtDataProvider = jwtDataProvider;
            _loginProvider = loginProvider;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<BaseResponse<Model.Response.MemberAuthResponse>> GetUserAuth([FromBody] MemberAuthRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _jwtDataProvider.defaultToken(request.email, request.password);
            });
        }
    }
}
