namespace vynce_api.Model.Response
{
    public class ProcessingFeeTypeListResponse
    {
        public List<ProcessingFeeType> list { get; set; }

        public int total_count { get; set; }
    }
    public class ProcessingFeeType
    {
        public int processing_fee_type_id { get; set; }
        public string name { get; set; }
        public string created_date { get; set; }
        public string modified_date { get; set; }

    }
}
