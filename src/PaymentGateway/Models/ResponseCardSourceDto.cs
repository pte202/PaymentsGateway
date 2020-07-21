namespace PaymentGateway.Models
{
    public class ResponseCardSourceDto : ISourceDto
    {
        public string Type { get; set; }
        public string LastFourDigitsOfNumber { get; set; }
        public string ExpiryDate { get; set; }
    }
}
