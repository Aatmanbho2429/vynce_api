using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface IJwtTokenDataProvider
    {
        Task<BaseResponse<MemberAuthResponse>> defaultToken(string email, string password);
    }
}
