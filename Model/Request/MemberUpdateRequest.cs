namespace vynce_api.Model.Request
{
    public class MemberUpdateRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int? role_id { get; set; }
        public int? status { get; set; }
        public int? membership_id { get; set; }
    }
}
