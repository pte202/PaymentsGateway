using PaymentGateway.Models;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.Configuration.Swagger
{
    public class RequestCardPaymentExample : IExamplesProvider<RequestCardPaymentDto>
    {
        public RequestCardPaymentDto GetExamples()
        {
            return new RequestCardPaymentDto
            {
                Number = "4111111111111111",
                ExpiryDate = "12/2020",
                Cvv = "542",
                Amount = 433,
                Currency = "GBP"
            };
        }
    }
}
