namespace PaymentGateway.ExternalModels
{
    public class RequestExternalPaymentDto : IPayment
    {
        public string Number { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}
