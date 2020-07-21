using PaymentGateway.Entities;

namespace PaymentGateway.Helpers
{
    public class ValidationData
    {
        public PaymentCardIssuer CardIssuer { get; set; }
        public Currency Currency { get; set; }
    }
}
