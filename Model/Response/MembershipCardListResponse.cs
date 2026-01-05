namespace vynce_api.Model.Response
{
    public class MembershipCardListResponse
    {
        public List<MembershipCard> list { get; set; }

        public int total_count { get; set; }
    }
    public class MembershipCard
    {
        public string membership_card_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public int duration { get; set; }
        public int status { get; set; }
        public string created_date { get; set; }
        public string modified_date { get; set; }
    }
}
