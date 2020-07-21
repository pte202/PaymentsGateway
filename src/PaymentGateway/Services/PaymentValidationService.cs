using PaymentGateway.Helpers;
using PaymentGateway.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public class PaymentValidationService : IPaymentValidationService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IPaymentCardService _paymentCardService;

        public PaymentValidationService(
            ICurrencyRepository currencyRepository,
            IPaymentCardService paymentCardService)
        {
            _currencyRepository = currencyRepository;
            _paymentCardService = paymentCardService;
        }

        public async Task<ValidationData> ValidateAsync(string number, string cvv, string currencyCode, int amount)
        {
            var paymentCardIssuer = await _paymentCardService.GetCardIssuer(number);
            
            if (paymentCardIssuer == null) throw new ValidationException("Unsupported Payment Card Issuer");

            var isValidPaymentCard = IsValidPaymentCard(number, cvv, paymentCardIssuer.Name);

            if (!isValidPaymentCard) throw new ValidationException("Invalid Payment Card");

            var currency = await _currencyRepository.GetCurrencyByCodeAsync(currencyCode);

            if (currency == null) throw new ValidationException("Unsupported Payment Currency");

            var isValidPayment = IsValidPayment(amount, currency.Code);

            if (!isValidPayment) throw new ValidationException("Invalid Payment Request");

            return new ValidationData { CardIssuer = paymentCardIssuer, Currency = currency };
        }

        public bool IsValidPaymentCard(string cardNumber, string cvv, string cardIssuer)
        {
            var validCardNumber = IsValidCardNumber(cardNumber);

            if (!validCardNumber) return false;

            if (string.IsNullOrEmpty(cardIssuer)) return false;

            var validCvv = IsValidCardCvv(cvv, cardIssuer);

            if (!validCvv) return false;

            return true;
        }

        public bool IsValidCardNumber(string cardNumber)
        {
            if (!cardNumber.All(char.IsDigit)) return false;

            return CheckLuhn(cardNumber);
        }

        public static bool IsValidCardExpiryDate(string expiryDate)
        {
            var dateTimeValue = Convert.ToDateTime(expiryDate);

            var expiryDateObj = new DateTime(dateTimeValue.Year, dateTimeValue.Month, DateTime.DaysInMonth(dateTimeValue.Year, dateTimeValue.Month));

            var expiryDateHasNotPassed = DateTime.Compare(expiryDateObj, DateTime.Now) >= 0;

            return expiryDateHasNotPassed;
        }

        public bool IsValidCardCvv(string cvv, string issuer)
        {
            if (!cvv.All(char.IsDigit)) return false;
            if (cvv.Length < 3 || cvv.Length > 4) return false;
            if (issuer == PaymentCardIssuers.AmericanExpress.ToString() && cvv.Length != 4) return false;

            return true;
        }

        public bool IsValidPayment(int amount, string currencyCode)
        {
            if (amount <= 0) return false;
            if (string.IsNullOrEmpty(currencyCode)) return false;

            var isCurrencyCodeNotMadeOfLettersOnly = !Regex.IsMatch(currencyCode, @"^[a-zA-Z]+$");

            if (isCurrencyCodeNotMadeOfLettersOnly) return false;

            return true;
        }

        private bool CheckLuhn(string cardNumber)
        {
            int cardNumberLength = cardNumber.Length;

            int cardNumberSum = 0;
            bool isSecond = false;

            for (int i = cardNumberLength - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());

                if (isSecond) digit *= 2;

                // handle cases when doubling results in a double digit number
                cardNumberSum += digit / 10 + digit % 10;

                isSecond = !isSecond;
            }

            return cardNumberSum % 10 == 0;
        }
    }
}
