using PaymentGateway.ExternalModels;
using PaymentGateway.Models;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.Configuration.Swagger
{
    public class ResponseCardPaymentDtoExample : IExamplesProvider<ResponseCardPaymentDto>
    {
        public ResponseCardPaymentDto GetExamples()
        {
            return new ResponseCardPaymentDto
            {
                Identifier = "75f2d02b-2b25-4b28-9bdc-25b75c0c59d1",
                Status = "successful",
                Amount = 433,
                Currency = "GBP",
                Source = new ResponseCardSourceDto
                {
                    Type = "Card",
                    LastFourDigitsOfNumber = "1111",
                    ExpiryDate = "06/2022"
                }
            };
        }            
    }
}
