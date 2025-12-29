using Microsoft.Data.SqlClient;
using System.Data;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class SearchDataProvider: BaseDataProvider,ISearchDataProvider
    {
        IDataProviderHelper _dataProviderHelper;
        public SearchDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }

        public async Task<BaseResponse<int>> SearchPost(SearchPostRequest request)
        {
            var modified_date = DateTime.Now;
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",request.member_id),
                new SqlParameter("i_members_free_search_id",request.members_free_search_id),
                new SqlParameter("i_modified_date",modified_date),
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.SEARCH_POST_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }
    }
}
