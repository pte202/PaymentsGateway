using FluentValidation;
using PaymentGateway.Services;
using System;
using System.Globalization;

namespace PaymentGateway.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> DateIsInValidFormat<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value =>
            {
                DateTime parsed;

                var valid = DateTime.TryParseExact(value, "mm/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed);

                return valid;
            });
        }

        public static IRuleBuilderOptions<T, string> IsNumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value =>
            {
                var isNumeric = int.TryParse(value, out int n);
                return isNumeric;
            });
        }

        public static IRuleBuilderOptions<T, string> ValidPaymentCardExpiryDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value =>
            {
                return PaymentValidationService.IsValidCardExpiryDate(value);
            });
        }
    }
}
