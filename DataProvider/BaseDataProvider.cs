using cp.common;
using System.Data.Common;
using System.Globalization;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class BaseDataProvider
    {
        protected async Task<BaseResponse<T>> DelegateControllerCall<T>(Func<Task<BaseResponse<T>>> call)
        {
            BaseResponse<T> result;
            try
            {
                result = await call();
            }
            catch (Exception ex)
            {
                AppTrace.Error(ex);
                result = new BaseResponse<T>()
                {
                    status = 0,
                    message = "Technical Error Occured",
                };
            }
            return result;
        }

        internal T ConvertStructNoNull<T>(DbDataReader reader, string column, Func<object, T> convert) where T : struct
        {

            var value = ConvertStruct(reader, column, convert);
            return value ?? default;
        }

        internal T? ConvertStruct<T>(DbDataReader reader, string column, Func<object, T> convert) where T : struct
        {
            var value = reader[column];
            try
            {
                return value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) ? null : new T?(convert(value));
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal string ConvertString(DbDataReader reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? null : Convert.ToString(value);
        }



        internal bool ConvertBoolean(DbDataReader reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? false : Convert.ToBoolean(value);
        }

        internal int ConvertIntiger(DbDataReader reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? 0 : Convert.ToInt32(value);
        }

        internal int ConvertLong(DbDataReader reader, string column)
        {
            var value = reader[column];
            return (int)(value == DBNull.Value ? 0 : Convert.ToDouble(value));
        }

        internal decimal ConvertDecimal(DbDataReader reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? 0 : Convert.ToDecimal(value);
        }

        internal DateTime ConvertToDate(DbDataReader reader, string column)
        {

            var value = reader[column];
            return value == DBNull.Value ? Convert.ToDateTime(null) : Convert.ToDateTime(value);
        }

        //TODO
        internal string ConvertOrderTotal(DbDataReader reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? "$0" : string.Format(new CultureInfo("en-US"), "{0:C2}", Convert.ToDecimal(value));
        }

        internal string ConvertOrderDate(DbDataReader reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? "" : Convert.ToDateTime(value).ToString("MM-dd-yyyy");
        }
    }
}
