namespace vynce_api.Model.Response
{
    public class ProcessingFeeListResponse
    {
        public List<ProcessingFee> list { get; set; }

        public int total_count { get; set; }
    }
    public class ProcessingFee
    {
        public int processing_fee_id { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public int type_id { get; set; }
        public string processing_fee_type_name { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }
        public string created_date { get; set; }
        public string modified_date { get; set; }
    }
}
