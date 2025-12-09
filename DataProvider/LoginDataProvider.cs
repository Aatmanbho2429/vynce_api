using Microsoft.Data.SqlClient;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class LoginDataProvider: BaseDataProvider, ILoginDataProvider
    {
        private IDataProviderHelper _dataProviderHelper;
        public LoginDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }
        public async Task<BaseResponse<UserAuthResponse>> userAuth(string username, string password)
        {
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_username",username),
                new SqlParameter("i_password",password),
            };

            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.LOGIN_V1_PROC, ReaderUserGet, sqlParameters.ToArray());

            return new BaseResponse<UserAuthResponse>()
            {
                data = response,
                status = 1
            };
        }

        #region reader
        private async Task<UserAuthResponse> ReaderUserGet(SqlDataReader dataReader)
        {
            UserAuthResponse response = new UserAuthResponse();

            while (await dataReader.ReadAsync())
            {
                response = (new UserAuthResponse
                {
                    user_id = ConvertString(dataReader, "USERID"),
                    email = ConvertString(dataReader, "EMAIL"),
                    user_name = ConvertString(dataReader, "USERNAME"),
                    role_id = ConvertString(dataReader, "ROLEID"),
                    user_type = ConvertString(dataReader, "USERTYPE"),
                });
            }
            return response;
        }
        #endregion
    }
}
