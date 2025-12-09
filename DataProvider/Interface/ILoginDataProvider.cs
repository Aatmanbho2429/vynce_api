using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface ILoginDataProvider
    {
        Task<BaseResponse<UserAuthResponse>> userAuth(string username, string password);
    }
}
