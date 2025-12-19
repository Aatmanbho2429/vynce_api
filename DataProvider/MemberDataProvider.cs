using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class MemberDataProvider: BaseDataProvider,IMemberDataProvider
    {
        IDataProviderHelper _dataProviderHelper;
        public MemberDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }

        #region reader 
        public async Task<MemberListResponse> MemberListReader(SqlDataReader reader)
        {
            MemberListResponse response = new MemberListResponse();
            var members = new List<Member>();

            while (await reader.ReadAsync())
            {
                members.Add(new Member()
                {
                    member_id = ConvertString(reader, "member_id"),
                    name = ConvertString(reader, "name"),
                    email = ConvertString(reader, "email"),
                    password = ConvertString(reader, "password"),
                    phone = ConvertString(reader, "phone"),
                    role_id = ConvertIntiger(reader, "role_id"),
                    status = ConvertIntiger(reader, "status"),
                    membership_id = ConvertIntiger(reader, "membership_id"),
                    created_date = ConvertToDate(reader, "created_date"),
                    modified_date = ConvertToDate(reader, "modified_date")
                });
            }

            if (reader.NextResult())
            {
                while (await reader.ReadAsync())
                {
                    response.total_count = ConvertIntiger(reader, "total_record");
                }
            }

            response.list = members;
            return response;
        }

        public async Task<MemberGetResponse> MemberGetReader(SqlDataReader reader)
        {
            MemberGetResponse response = new MemberGetResponse();
            while (await reader.ReadAsync())
            {
                response.member_id = ConvertString(reader, "member_id");
                response.name = ConvertString(reader, "name");
                response.email = ConvertString(reader, "email");
                response.phone = ConvertString(reader, "phone");
                response.password = ConvertString(reader, "password");
                response.role_id = ConvertIntiger(reader, "role_id");
                response.status = ConvertIntiger(reader, "status");
                response.membership_id = ConvertIntiger(reader, "membership_id");
                response.created_date = ConvertToDate(reader, "created_date");
                response.modified_date = ConvertToDate(reader, "modified_date");
            }
            return response;
        }
        #endregion

        public async Task<BaseResponse<int>> AddMember(MemberAddRequest request)
        {
            string decrypt_pass = CryptoJsAes.Decrypt(request.password, ApplicationConfigurations.encryptionKey);
            string decrypted_password = System.Text.Json.JsonSerializer.Deserialize<string>(decrypt_pass);
            var created_date = DateTime.UtcNow;
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_email",request.email),
                new SqlParameter("i_phone",request.phone),
                new SqlParameter("i_password",decrypted_password),
                new SqlParameter("i_role_id",request.role_id),
                new SqlParameter("i_status",request.status),
                new SqlParameter("i_membership_id",request.membership_id),
                new SqlParameter("i_created_date",created_date),
            };

            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBER_ADD_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<MemberListResponse>> MemberList(MemberListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("page_size",request.page_size),
                new SqlParameter("page_no",request.page_no),
                new SqlParameter("sorting_by",request.sorting_by),
                new SqlParameter("sorting_column",request.sorting_column)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBER_LIST_V1, MemberListReader, sqlParameters.ToArray());

            return new BaseResponse<MemberListResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<MemberGetResponse>> MemberGet(string id)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",id)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBER_GET_V1, MemberGetReader, sqlParameters.ToArray());

            return new BaseResponse<MemberGetResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<int>> MemberDelete(string id)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",id)
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });

            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBER_DELETE_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<int>> MemberUpdate(string id, MemberUpdateRequest request)
        {
            var modified_date = DateTime.Now;
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",id),
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_email",request.email),
                new SqlParameter("i_phone",request.phone),
                new SqlParameter("i_password",request.password),
                new SqlParameter("i_role_id",request.role_id),
                new SqlParameter("i_status",request.status),
                new SqlParameter("i_membership_id",request.membership_id),
                new SqlParameter("i_modified_date",modified_date),
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBER_UPDATE_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }
    }
}
