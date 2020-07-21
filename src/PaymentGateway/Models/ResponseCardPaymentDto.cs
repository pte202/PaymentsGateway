namespace PaymentGateway.Models
{
    public class ResponseCardPaymentDto : IPaymentDto
    {
        public string Identifier { get; set; }
        public string Status { get; set; }    
        public int Amount { get; set; }
        public string Currency { get; set; }

        public ResponseCardSourceDto Source { get; set; }
    }
}
