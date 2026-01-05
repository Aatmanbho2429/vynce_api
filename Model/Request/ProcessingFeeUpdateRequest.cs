namespace vynce_api.Model.Request
{
    public class ProcessingFeeUpdateRequest
    {
        public string name { get; set; }
        public int amount { get; set; }
        public int type { get; set; }
        public int status { get; set; }
    }
}
