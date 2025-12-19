using Microsoft.Data.SqlClient;
using System.Data;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;

namespace vynce_api.DataProvider
{
    public class DataProviderHelper: IDataProviderHelper
    {
        private string _connectionString = ApplicationConfigurations.ConnectionString;

        public async Task<T> ExecuteReaderAsync<T>(string commandText, Func<SqlDataReader, Task<T>> readerParserAction, params SqlParameter[] commandParameters)
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            try
            {
                await sqlConnection.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 300;

                foreach (SqlParameter parameter in commandParameters)
                {
                    SqlParameter spNew = new SqlParameter();
                    spNew.ParameterName = parameter.ParameterName;
                    spNew.Value = parameter.Value;
                    spNew.SqlDbType = parameter.SqlDbType;
                    spNew.Direction = parameter.Direction;
                    sqlCommand.Parameters.Add(spNew);
                }

                using var reader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                return await readerParserAction(reader);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                Console.WriteLine(ex.StackTrace);
                //logger.Error("Exception :" + ex);
                throw new Exception(ex.Message);

            }
            finally
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    await sqlConnection.CloseAsync();
            }
        }

        public async Task<(int status, string message)> ExecuteNonQueryAsync(string commandText, params SqlParameter[] commandParameters)
        {
            int status = 0;
            string ReturnResult = string.Empty;
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            try
            {
                await sqlConnection.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 300;

                foreach (SqlParameter parameter in commandParameters)
                {
                    SqlParameter spNew = new SqlParameter();
                    spNew.ParameterName = parameter.ParameterName;
                    spNew.Value = parameter.Value;
                    spNew.SqlDbType = parameter.SqlDbType;
                    spNew.Direction = parameter.Direction;

                    if (parameter.SqlDbType == SqlDbType.VarChar || parameter.SqlDbType == SqlDbType.NVarChar)
                    {
                        spNew.Size = parameter.Size > 0 ? parameter.Size : -1;
                    }

                    sqlCommand.Parameters.Add(spNew);

                }

                // sqlCommand.FetchSize = sqlCommand.FetchSize * 64;
                await sqlCommand.ExecuteNonQueryAsync();
                ReturnResult = Convert.ToString(sqlCommand.Parameters["o_output_message"].Value);
                status = Convert.ToInt32(Convert.ToString(sqlCommand.Parameters["o_output_status"].Value));
                //count = Convert.ToInt32(ReturnResult);
            }
            catch (Exception ex)
            {
                //logger.Error("Exception :" + ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    await sqlConnection.CloseAsync();
            }
            return (status, ReturnResult);
        }
    }
}
