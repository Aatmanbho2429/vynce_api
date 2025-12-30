using Microsoft.Data.SqlClient;
using System.Data;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class MembershipCardDataProvider : BaseDataProvider, IMembershipCardDataProvider
    {
        IDataProviderHelper _dataProviderHelper;
        public MembershipCardDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }

        #region reader
        public async Task<MembershipCardListResponse> MembershipcardListReader(SqlDataReader reader)
        {
            MembershipCardListResponse response = new MembershipCardListResponse();
            var members = new List<MembershipCard>();

            while (await reader.ReadAsync())
            {
                members.Add(new MembershipCard()
                {
                    membership_card_id = ConvertString(reader, "membership_card_id"),
                    name = ConvertString(reader, "name"),
                    description = ConvertString(reader, "descrption"),
                    price = ConvertString(reader, "price"),
                    duration = ConvertIntiger(reader, "duration"),
                    status = ConvertIntiger(reader, "status"),
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

        public async Task<MembershipCardListResponse> GetMembershipcardListReader(SqlDataReader reader)
        {
            MembershipCardListResponse response = new MembershipCardListResponse();
            var members = new List<MembershipCard>();

            while (await reader.ReadAsync())
            {
                members.Add(new MembershipCard()
                {
                    membership_card_id = ConvertString(reader, "membership_card_id"),
                    name = ConvertString(reader, "name"),
                    description = ConvertString(reader, "descrption"),
                    price = ConvertString(reader, "price"),
                    duration = ConvertIntiger(reader, "duration"),
                    status = ConvertIntiger(reader, "status"),
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
        public async Task<MembershipCardGetResponse> MembershipcardGetReader(SqlDataReader reader)
        {
            MembershipCardGetResponse response = new MembershipCardGetResponse();
            while (await reader.ReadAsync())
            {
                response.membership_card_id = ConvertString(reader, "membership_card_id");
                response.name = ConvertString(reader, "name");
                response.description = ConvertString(reader, "descrption");
                response.price = ConvertString(reader, "price");
                response.duration = ConvertIntiger(reader, "duration");
                response.status = ConvertIntiger(reader, "status");
                response.created_date = ConvertToDate(reader, "created_date");
                response.modified_date = ConvertToDate(reader, "modified_date");
            }
            return response;
        }
        #endregion

        public async Task<BaseResponse<int>> AddMembershipCard(MembershipCardAddRequest request)
        {
            var created_date = DateTime.UtcNow;
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_description",request.description),
                new SqlParameter("i_price",request.price),
                new SqlParameter("i_duration",request.duration),
                new SqlParameter("i_status",request.status),
                new SqlParameter("i_created_date",created_date),
            };

            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBERSHIPCARD_ADD_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<int>> MembershipCardDelete(string id)
        {
            var modified_date = DateTime.Now;
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_membership_card_id",id),
                new SqlParameter("i_modified_date",modified_date)
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });

            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBERSHIPCARD_DELETE_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<MembershipCardGetResponse>> MembershipCardGet(string id)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_membership_card_id",id)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBERSHIPCARD_GET_V1, MembershipcardGetReader, sqlParameters.ToArray());

            return new BaseResponse<MembershipCardGetResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<MembershipCardListResponse>> MembershipCardList(MembershipCardListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("page_size",request.page_size),
                new SqlParameter("page_no",request.page_no),
                new SqlParameter("sorting_by",request.sorting_by),
                new SqlParameter("sorting_column",request.sorting_column)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBERSHIPCARD_LIST_V1, MembershipcardListReader, sqlParameters.ToArray());

            return new BaseResponse<MembershipCardListResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<int>> MembershipCardUpdate(string id, MembershipCardUpdateRequest request)
        {
            var modified_date = DateTime.Now;
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_membership_card_id",id),
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_description",request.description),
                new SqlParameter("i_price",request.price),
                new SqlParameter("i_duration",request.duration),
                new SqlParameter("i_status",request.status),
                new SqlParameter("i_modified_date",modified_date),
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBERSHIPCARD_UPDATE_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<MembershipCardListResponse>> GetMembershipList(MembershipCardListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("page_size",request.page_size),
                new SqlParameter("page_no",request.page_no),
                new SqlParameter("sorting_by",request.sorting_by),
                new SqlParameter("sorting_column",request.sorting_column)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBERSHIP_DROPDOWN_LIST_V1, GetMembershipcardListReader, sqlParameters.ToArray());

            return new BaseResponse<MembershipCardListResponse>()
            {
                data = response
            };
        }
    }
}
