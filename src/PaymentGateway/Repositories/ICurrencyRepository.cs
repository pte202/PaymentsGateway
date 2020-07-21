using PaymentGateway.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetCurrenciesAsync();
        Task<Currency> GetCurrencyByCodeAsync(string code);
        bool CurrencyExists(string code);
    }
}