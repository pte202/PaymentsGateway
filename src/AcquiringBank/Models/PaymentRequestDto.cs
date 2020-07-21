namespace AcquiringBank.Models
{
    public class PaymentRequestDto
    {
        public string Number { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
