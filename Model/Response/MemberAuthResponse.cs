namespace vynce_api.Model.Response
{
    public class MemberAuthResponse
    {
        public string member_id { get; set; }
        public string name { get; set; }

        public string role_id { get; set; }

        public string email { get; set; }

        public string jwt_token { get; set; }
        public string membership_id { get; set; }

        public int o_output_status { get; set; }
        public string o_output_message { get; set; }
    }
}
