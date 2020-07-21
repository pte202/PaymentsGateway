namespace PaymentGateway.Models
{
    public interface IPaymentDto
    {
        int Amount { get; set; }
        string Currency { get; set; }
    }
}
