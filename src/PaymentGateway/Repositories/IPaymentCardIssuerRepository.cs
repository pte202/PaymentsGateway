using PaymentGateway.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Repositories
{
    public interface IPaymentCardIssuerRepository
    {
        Task<IEnumerable<PaymentCardIssuer>> GetPaymentCardIssuersAsync();
        Task<PaymentCardIssuer> GetPaymentCardIssuerByNameAsync(string Name);
    }
}
