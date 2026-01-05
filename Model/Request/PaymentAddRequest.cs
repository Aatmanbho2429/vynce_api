namespace vynce_api.Model.Request
{
    public class PaymentAddRequest
    {
        public int member_id { get; set; }
        public int membership_card_id { get; set; }
        public string amount { get; set; }
        public string razorpay_order_id { get; set; }
        public string razorpay_payment_id { get; set; }
        public string razorpay_signature { get; set; }
    }
}
