using Microsoft.EntityFrameworkCore;
using PaymentGateway.DbContexts;
using PaymentGateway.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Repositories
{
    public class PaymentCardRepository : IPaymentCardRepository
    {
        private PaymentGatewayDbContext _context;

        public PaymentCardRepository(PaymentGatewayDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<PaymentCard>> GetPaymentCards()
        {
            return await _context.PaymentCards.ToListAsync();
        }

        public async Task<PaymentCard> GetPaymentCardByNumberAsync(string number)
        {
            return await _context.PaymentCards.FirstOrDefaultAsync(e => e.Number == number);
        }

        public void AddPaymentCard(PaymentCard entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.PaymentCards.Add(entity);
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }


    }
}
