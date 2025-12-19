using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using vynce_api.DataProvider.Interface;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class JwtTokenDataProvider: IJwtTokenDataProvider
    {
        private IConfiguration _config;
        private ILoginDataProvider _loginDataProvider;

        public JwtTokenDataProvider(IConfiguration config, ILoginDataProvider loginDataProvider)
        {
            _config = config;
            _loginDataProvider = loginDataProvider;
        }

        public async Task<BaseResponse<MemberAuthResponse>> defaultToken(string email, string password)
        {
            var response = await _loginDataProvider.MemberAuth(email, password);

            if (response is not null && response.data is not null && response.data.member_id != null && response.data.email != null) 
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] {
                        //new Claim(JwtRegisteredClaimNames.Sub,response.data.UserID),
                        //new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        //new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, response.data.member_id),
                        new Claim(ClaimTypes.Role, response.data.role_id)
                    };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                              _config["Jwt:Issuer"],
                                              claims,
                                              expires: DateTime.Now.AddDays(1),
                                              signingCredentials: credentials);

                string stToken = new JwtSecurityTokenHandler().WriteToken(token);
                response.data.jwt_token = stToken;
            }
            return response;
        }
    }
}
