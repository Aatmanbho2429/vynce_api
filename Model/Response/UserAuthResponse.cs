namespace vynce_api.Model.Response
{
    public class UserAuthResponse
    {
        public string user_id { get; set; }
        public string user_name { get; set; }

        public string role_id { get; set; }

        public string email { get; set; }

        public string jwt_token { get; set; }
        public string user_type { get; set; }
    }
}
