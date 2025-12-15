namespace vynce_api.Model.Response
{
    public class MemberGetResponse
    {
        public string member_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public int? role_id { get; set; }
        public int? status { get; set; }
        public int? membership_id { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
    }
}
