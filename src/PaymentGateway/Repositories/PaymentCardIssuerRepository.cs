using Microsoft.EntityFrameworkCore;
using PaymentGateway.DbContexts;
using PaymentGateway.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Repositories
{
    public class PaymentCardIssuerRepository : IPaymentCardIssuerRepository
    {
        private PaymentGatewayDbContext _context;

        public PaymentCardIssuerRepository(PaymentGatewayDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<PaymentCardIssuer>> GetPaymentCardIssuersAsync()
        {
            return await _context.PaymentCardIssuers.ToListAsync();
        }

        public async Task<PaymentCardIssuer> GetPaymentCardIssuerByNameAsync(string Name)
        {
            return await _context.PaymentCardIssuers.FirstOrDefaultAsync(e => e.Name == Name);
        }
    }
}
