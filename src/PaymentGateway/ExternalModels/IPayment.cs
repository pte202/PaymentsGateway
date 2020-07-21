namespace PaymentGateway.ExternalModels
{
    public interface IPayment
    {
        int Amount { get; set; }
        string Currency { get; set; }
    }
}
