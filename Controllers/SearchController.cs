using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vynce_api.DataProvider;
using vynce_api.DataProvider.Interface;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : BaseController
    {
        private readonly ISearchDataProvider _searchDataProvider; 
        public SearchController(IHttpContextAccessor httpContextAccessor, ISearchDataProvider searchDataProvider) : base(httpContextAccessor)
        {
            _searchDataProvider = searchDataProvider;
        }

        [HttpPost("search")]
        public async Task<BaseResponse<int>> SearchPost(SearchPostRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _searchDataProvider.SearchPost(request);
            });
        }
    }
}
