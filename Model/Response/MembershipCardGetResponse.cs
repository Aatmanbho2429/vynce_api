namespace vynce_api.Model.Response
{
    public class MembershipCardGetResponse
    {
        public string membership_card_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public int duration { get; set; }
        public int status { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
    }
}
