using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider.Interface
{
    public interface IMasterDataProvider
    {
        Task<BaseResponse<MenuItemListResponse>> MenuItemList(MenuItemListRequest request);
    }
}
