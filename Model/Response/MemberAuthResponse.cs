namespace vynce_api.Model.Response
{
    public class MemberAuthResponse
    {
        public string member_id { get; set; }
        public string member_name { get; set; }

        public string role_id { get; set; }

        public string email { get; set; }

        public string jwt_token { get; set; }
        public string member_type { get; set; }
    }
}
