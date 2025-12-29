using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface ISearchDataProvider
    {
        Task<BaseResponse<int>> SearchPost(SearchPostRequest request);
    }
}
