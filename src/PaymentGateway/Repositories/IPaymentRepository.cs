using PaymentGateway.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetPaymentsAsync();
        Task<Payment> GetPaymentAsync(string identifier);
        void AddPayment(Payment entity);
        Task<bool> CommitAsync();
    }
}
