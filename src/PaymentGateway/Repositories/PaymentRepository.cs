using Microsoft.EntityFrameworkCore;
using PaymentGateway.DbContexts;
using PaymentGateway.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private PaymentGatewayDbContext _context;

        public PaymentRepository(PaymentGatewayDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetPaymentAsync(string identifier)
        {
            return await _context.Payments
                .Include(e => e.Currency)
                .Include(e => e.PaymentCard)
                .FirstOrDefaultAsync(e => e.Identifier == identifier);
        }

        public void AddPayment(Payment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Date = DateTime.Now;

            _context.Payments.Add(entity);
        }


        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
