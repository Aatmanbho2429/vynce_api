namespace vynce_api.Model.Response
{
    public class MemberGetProfileResponse
    {
        public string member_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int? role_id { get; set; }
        public int? status { get; set; }
        public int? membership_id { get; set; }
        public string? membership_name { get; set; }
        public string membership_start_date { get; set; }
        public string membership_end_date { get; set; }
        public string created_date { get; set; }
        public string modified_date { get; set; }
    }
}
