namespace vynce_api.Model.Response
{
    public class BaseResponse<T>
    {
        public int status { get; set; }
        public T data { get; set; }
        public string message { get; set; }
    }
}
