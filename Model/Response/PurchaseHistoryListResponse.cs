namespace vynce_api.Model.Response
{
    public class PurchaseHistoryListResponse
    {
        public List<PurchaseHistory> list { get; set; }

        public int total_count { get; set; }
    }

    public class PurchaseHistory
    {
        public string payment_date { get; set; }
        public string amount { get; set; }
        public int membership_card_id { get; set; }
        public string membership_card_name { get; set; }
    }
}
