namespace vynce_api.Model.Request
{
    public class PurchaseHistoryListRequest
    {
        public int? page_no { get; set; }
        public int? page_size { get; set; }
        public string? sorting_column { get; set; }
        public string? sorting_by { get; set; }
        public string? member_id { get; set; }
    }
}
