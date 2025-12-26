using Microsoft.Data.SqlClient;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class MasterDataProvider : BaseDataProvider, IMasterDataProvider
    {
        IDataProviderHelper _dataProviderHelper;
        public MasterDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }

        #region reader
        public async Task<MenuItemListResponse> MenuItemListReader(SqlDataReader reader)
        {
            MenuItemListResponse response = new MenuItemListResponse();
            var menu = new List<MenuItem>();

            while (await reader.ReadAsync())
            {
                menu.Add(new MenuItem()
                {
                    path = ConvertString(reader, "path"),
                    icon = ConvertString(reader, "icon"),
                    title = ConvertString(reader, "title"),
                    order = ConvertIntiger(reader, "order"),
                });
            }

            response.list = menu;
            return response;
        }
        #endregion

        public async Task<BaseResponse<MenuItemListResponse>> MenuItemList(MenuItemListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_role_id",request.role_id)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MENUITEM_LIST_V1, MenuItemListReader, sqlParameters.ToArray());

            return new BaseResponse<MenuItemListResponse>()
            {
                data = response
            };
        }
    }
}
