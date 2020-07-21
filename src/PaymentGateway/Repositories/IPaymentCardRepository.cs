using PaymentGateway.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Repositories
{
    public interface IPaymentCardRepository
    {
        Task<IEnumerable<PaymentCard>> GetPaymentCards();
        Task<PaymentCard> GetPaymentCardByNumberAsync(string number);
        void AddPaymentCard(PaymentCard entity);
        Task<bool> CommitAsync();
    }
}
