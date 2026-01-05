namespace vynce_api.Model.Request
{
    public class ProcessingFeeTypeListRequest
    {
        public int? page_no { get; set; }
        public int? page_size { get; set; }
        public string? sorting_column { get; set; }
        public string? sorting_by { get; set; }
    }
}
