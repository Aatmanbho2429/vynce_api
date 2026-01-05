using Microsoft.Data.SqlClient;
using System.Data;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class ProcessingFeeDataProvider: BaseDataProvider,IProcessingFeeDataProvider
    {
        IDataProviderHelper _dataProviderHelper;
        public ProcessingFeeDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }

        #region Reader
        public async Task<ProcessingFeeGetResponse> ProcessingFeeGetReader(SqlDataReader reader)
        {
            ProcessingFeeGetResponse response = new ProcessingFeeGetResponse();
            while (await reader.ReadAsync())
            {
                response.processing_fee_id = ConvertIntiger(reader, "processing_fee_id");
                response.name = ConvertString(reader, "name");
                response.amount = ConvertIntiger(reader, "amount");
                response.type = ConvertIntiger(reader, "type");
                response.status_id = ConvertIntiger(reader, "status_id");
                response.status = ConvertString(reader, "status");
                response.created_date = ConvertToDate(reader, "created_date");
                response.modified_date = ConvertToDate(reader, "modified_date");
            }
            return response;
        }

        public async Task<ProcessingFeeListResponse> ProcessingFeeListReader(SqlDataReader reader)
        {
            ProcessingFeeListResponse response = new ProcessingFeeListResponse();
            var processingFees = new List<ProcessingFee>();

            while (await reader.ReadAsync())
            {
                processingFees.Add(new ProcessingFee()
                {
                    processing_fee_id = ConvertIntiger(reader, "processing_fee_id"),
                    name = ConvertString(reader, "name"),
                    amount = ConvertIntiger(reader, "amount"),
                    type_id = ConvertIntiger(reader, "type_id"),
                    processing_fee_type_name = ConvertString(reader, "processing_fee_type_name"),
                    status = ConvertString(reader, "status"),
                    status_id = ConvertIntiger(reader, "status_id"),
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

            response.list = processingFees;
            return response;
        }
        public async Task<ProcessingFeeListResponse> GetProcessingFeeListReader(SqlDataReader reader)
        {
            ProcessingFeeListResponse response = new ProcessingFeeListResponse();
            var processingFees = new List<ProcessingFee>();

            while (await reader.ReadAsync())
            {
                processingFees.Add(new ProcessingFee()
                {
                    processing_fee_id = ConvertIntiger(reader, "processing_fee_id"),
                    name = ConvertString(reader, "name"),
                    amount = ConvertIntiger(reader, "amount"),
                    type_id = ConvertIntiger(reader, "type_id"),
                    processing_fee_type_name = ConvertString(reader, "processing_fee_type_name"),
                    status = ConvertString(reader, "status"),
                    status_id = ConvertIntiger(reader, "status_id"),
                    created_date = ConvertToDate(reader, "created_date"),
                    modified_date = ConvertToDate(reader, "modified_date")
                });
            }

            response.list = processingFees;
            return response;
        }

        public async Task<ProcessingFeeTypeListResponse> GetProcessingFeeTypeListReader(SqlDataReader reader)
        {
            ProcessingFeeTypeListResponse response = new ProcessingFeeTypeListResponse();
            var processingFees = new List<ProcessingFeeType>();

            while (await reader.ReadAsync())
            {
                processingFees.Add(new ProcessingFeeType()
                {
                    processing_fee_type_id = ConvertIntiger(reader, "processing_fee_type_id"),
                    name = ConvertString(reader, "name"),
                    created_date = ConvertToDate(reader, "created_date"),
                    modified_date = ConvertToDate(reader, "modified_date")
                });
            }

            response.list = processingFees;
            return response;
        }
        #endregion

        public async Task<BaseResponse<int>> AddProcessingFee(ProcessingFeeAddRequest request)
        {
            var created_date = DateTime.UtcNow;
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_amount",request.amount),
                new SqlParameter("i_type",request.type),
                new SqlParameter("i_status",request.status),
                new SqlParameter("i_created_date",created_date),
            };

            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.PROCESSINGFEE_ADD_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<int>> ProcessingFeeDelete(string id)
        {
            var modified_date = DateTime.Now;
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_processing_fee_id",id),
                new SqlParameter("i_modified_date",modified_date)
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });

            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.PROCESSINGFEE_DELETE_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<ProcessingFeeGetResponse>> ProcessingFeeGet(string id)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_processing_fee_id",id)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.PROCESSINGFEE_GET_V1, ProcessingFeeGetReader, sqlParameters.ToArray());

            return new BaseResponse<ProcessingFeeGetResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<ProcessingFeeListResponse>> ProcessingFeeList(ProcessingFeeListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("page_size",request.page_size),
                new SqlParameter("page_no",request.page_no),
                new SqlParameter("sorting_by",request.sorting_by),
                new SqlParameter("sorting_column",request.sorting_column)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.PROCESSINGFEE_LIST_V1, ProcessingFeeListReader, sqlParameters.ToArray());

            return new BaseResponse<ProcessingFeeListResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<int>> ProcessingFeeUpdate(string id, ProcessingFeeUpdateRequest request)
        {
            var modified_date = DateTime.Now;
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_processing_fee_id",id),
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_amount",request.amount),
                new SqlParameter("i_type",request.type),
                new SqlParameter("i_status",request.status),
                new SqlParameter("i_modified_date",modified_date),
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.PROCESSINGFEE_UPDATE_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<ProcessingFeeListResponse>> GetProcessingFeeList(ProcessingFeeListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("page_size",request.page_size),
                new SqlParameter("page_no",request.page_no),
                new SqlParameter("sorting_by",request.sorting_by),
                new SqlParameter("sorting_column",request.sorting_column)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.PROCESSINGFEE_CHECKOUT_LIST_V1, GetProcessingFeeListReader, sqlParameters.ToArray());

            return new BaseResponse<ProcessingFeeListResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<ProcessingFeeTypeListResponse>> GetProcessingFeeTypeList(ProcessingFeeTypeListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("page_size",request.page_size),
                new SqlParameter("page_no",request.page_no),
                new SqlParameter("sorting_by",request.sorting_by),
                new SqlParameter("sorting_column",request.sorting_column)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.PROCESSINGFEETYPE_DROPDOWN_LIST_V1, GetProcessingFeeTypeListReader, sqlParameters.ToArray());

            return new BaseResponse<ProcessingFeeTypeListResponse>()
            {
                data = response
            };
        }
    }
}
