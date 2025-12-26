using Microsoft.AspNetCore.Identity.Data;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface ILoginDataProvider
    {
        Task<BaseResponse<MemberAuthResponse>> MemberAuth(string email, string password);
        Task<BaseResponse<int>> Register(RegistrationRequest request);
        Task<BaseResponse<int>> RegistrationMail(RegistrationMailRequest request);

    }
}
