using PaymentGateway.Helpers;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IPaymentValidationService
    {
        Task<ValidationData> ValidateAsync(string number, string cvv, string currencyCode, int amount);
        bool IsValidPaymentCard(string cardNumber, string cvv, string cardIssuer);
        bool IsValidCardNumber(string cardNumber);
        bool IsValidCardCvv(string cvv, string issuer);
        bool IsValidPayment(int amount, string currencyCode);
    }
}
