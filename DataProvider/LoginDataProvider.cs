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
            string decrypt_pass=CryptoJsAes.Decrypt(password, ApplicationConfigurations.encryptionKey);
            string decrypted_password = System.Text.Json.JsonSerializer.Deserialize<string>(decrypt_pass);
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_email",email),
                new SqlParameter("i_password",decrypted_password),
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
                    member_id = ConvertString(dataReader, "member_id"),
                    email = ConvertString(dataReader, "email"),
                    name = ConvertString(dataReader, "name"),
                    role_id = ConvertString(dataReader, "role_id"),
                    membership_id = ConvertString(dataReader, "membership_id"),
                    o_output_status = ConvertIntiger(dataReader, "o_output_status"),
                    o_output_message= ConvertString(dataReader, "o_output_message"),
                });
            }
            return response;
        }
        #endregion
    }
}
