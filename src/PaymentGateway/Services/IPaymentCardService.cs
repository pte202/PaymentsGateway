using PaymentGateway.Entities;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IPaymentCardService
    {
        Task<PaymentCardIssuer?> GetCardIssuer(string cardNumber);
    }
}
