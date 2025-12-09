using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface IJwtTokenDataProvider
    {
        Task<BaseResponse<UserAuthResponse>> defaultToken(string email, string password);
    }
}
