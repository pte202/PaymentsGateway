using PaymentGateway.Entities;
using PaymentGateway.Repositories;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public class PaymentCardService : IPaymentCardService
    {
        private readonly IPaymentCardIssuerRepository _paymentCardIssuerRepository;

        public PaymentCardService(IPaymentCardIssuerRepository paymentCardIssuerRepository)
        {
            _paymentCardIssuerRepository = paymentCardIssuerRepository;
        }

        public async Task<PaymentCardIssuer?> GetCardIssuer(string cardNumber)
        {
            var supportedCardIssuers = await _paymentCardIssuerRepository.GetPaymentCardIssuersAsync();

            foreach (var supportedCardIssuer in supportedCardIssuers)
            {
                var reg = new Regex(supportedCardIssuer.Pattern);

                var isSupported = reg.IsMatch(cardNumber);

                if (isSupported) return supportedCardIssuer;
            }

            return null;
        }
    }
}
