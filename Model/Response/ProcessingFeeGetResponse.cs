namespace vynce_api.Model.Response
{
    public class ProcessingFeeGetResponse
    {
        public int processing_fee_id { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public int type { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }
        public string created_date { get; set; }
        public string modified_date { get; set; }
    }
}
