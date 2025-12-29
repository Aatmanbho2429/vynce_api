namespace vynce_api.Model.Response
{
    public class MemberGetFreeSearchResponse
    {
        public int members_free_search_id { get; set; }
        public int member_id { get; set; }
        public int free_search_remaining { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
    }
}
