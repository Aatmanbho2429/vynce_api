using Microsoft.Data.SqlClient;

namespace vynce_api.DataProvider.Interface
{
    public interface IDataProviderHelper
    {
        Task<T> ExecuteReaderAsync<T>(string commandText, Func<SqlDataReader, Task<T>> readerParserAction, params SqlParameter[] commandParameters);

        Task<int> ExecuteNonQueryAsync(string commandText, params SqlParameter[] commandParameters);
    }
}
