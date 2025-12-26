using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vynce_api.DataProvider;
using vynce_api.DataProvider.Interface;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.Controllers
{
    [Route("api/master")]
    [ApiController]
    public class MasterController : BaseController
    {
        private readonly IMasterDataProvider _masterDataProvider;
        public MasterController(IHttpContextAccessor httpContextAccessor, IMasterDataProvider masterDataProvider) : base(httpContextAccessor)
        {
            _masterDataProvider = masterDataProvider;
        }

        [HttpPost("menu-item")]
        public async Task<BaseResponse<MenuItemListResponse>> MenuItemList(MenuItemListRequest request)
        {
            return await DelegateControllerCall(async () =>
            {
                return await _masterDataProvider.MenuItemList(request);
            });
        }
    }
}
