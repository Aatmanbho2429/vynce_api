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
        public async Task<BaseResponse<MemberAuthResponse>> MemberAuth(string email, string password)
        {
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_Membername",email),
                new SqlParameter("i_password",password),
            };

            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.LOGIN_V1_PROC, ReaderMemberGet, sqlParameters.ToArray());

            return new BaseResponse<MemberAuthResponse>()
            {
                data = response,
                status = 1
            };
        }

        #region reader
        private async Task<MemberAuthResponse> ReaderMemberGet(SqlDataReader dataReader)
        {
            MemberAuthResponse response = new MemberAuthResponse();

            while (await dataReader.ReadAsync())
            {
                response = (new MemberAuthResponse
                {
                    member_id = ConvertString(dataReader, "MemberID"),
                    email = ConvertString(dataReader, "EMAIL"),
                    member_name = ConvertString(dataReader, "MemberNAME"),
                    role_id = ConvertString(dataReader, "ROLEID"),
                    member_type = ConvertString(dataReader, "MemberTYPE"),
                });
            }
            return response;
        }
        #endregion
    }
}
