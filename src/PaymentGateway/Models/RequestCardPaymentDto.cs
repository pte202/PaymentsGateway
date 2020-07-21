using PaymentGateway.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PaymentGateway.Models
{
    public class RequestCardPaymentDto : IPaymentDto, IValidatableObject
    {
        public string Number { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new CardPaymentDtoValidator();

            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
