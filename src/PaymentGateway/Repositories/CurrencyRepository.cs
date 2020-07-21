using Microsoft.EntityFrameworkCore;
using PaymentGateway.DbContexts;
using PaymentGateway.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private PaymentGatewayDbContext _context;

        public CurrencyRepository(PaymentGatewayDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task<Currency> GetCurrencyByCodeAsync(string code)
        {
            return await _context.Currencies.FirstOrDefaultAsync(c => c.Code == code);
        }

        public bool CurrencyExists(string code)
        {
            return _context.Currencies.Any(e => e.Code == code);
        }
    }
}
