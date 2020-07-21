using FluentValidation;
using PaymentGateway.Extensions;
using PaymentGateway.Models;

namespace PaymentGateway.Validators
{
    public class CardPaymentDtoValidator : AbstractValidator<RequestCardPaymentDto>
    {

        public CardPaymentDtoValidator()
        {
            RuleFor(dto => dto.Number).NotEmpty().WithMessage("Card Number cannot be empty.").DependentRules(() =>
            {
                RuleFor(dto => dto.Number).CreditCard().WithMessage("Card Number is not valid.");
            });
            RuleFor(dto => dto.ExpiryDate).NotEmpty().WithMessage("Expiry Date cannot be empty.").DependentRules(() =>
            {
                RuleFor(dto => dto.ExpiryDate).DateIsInValidFormat().WithMessage("Expiry date format is not valid").DependentRules(() =>
                {
                    RuleFor(dto => dto.ExpiryDate).ValidPaymentCardExpiryDate().WithMessage("Expiry date has passed");
                });
            });
            RuleFor(dto => dto.Cvv).NotEmpty().WithMessage("Cvv cannot be empty.");


            RuleFor(dto => dto.Amount).NotEmpty().WithMessage("Amount cannot be empty or 0.").DependentRules(() =>
            {
                RuleFor(dto => dto.Amount).Must(amount => amount > 0).WithMessage("Amount must be larger than 0");
            });
            RuleFor(dto => dto.Currency).NotEmpty().WithMessage("Currency cannot be empty.");
        }
    }
}