namespace vynce_api.Model.Request
{
    public class RazorOrderRequest
    {
        public int amount { get; set; }
        public string currency { get; set; }="INR";
        public string receipt { get; set; }="order_rcptid_11";
    }
}
