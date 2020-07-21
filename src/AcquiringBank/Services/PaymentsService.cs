using AcquiringBank.Models;
using System;

namespace AcquiringBank.Services
{
    public class PaymentsService : IPaymentsService
    {
        public PaymentResponseDto MakePayment()
        {
            // in percentage 
            var requestSuccessRate = 60;

            var isRequestSuccessful = GenerateProbabilityBasedBoolean(requestSuccessRate);

            return GeneratePaymentResponse(isRequestSuccessful);
        }

        private bool GenerateProbabilityBasedBoolean(int successRatePercentage)
        {
            const int TotalPercentage = 100;

            var random = new Random();
            var randomNumber = random.Next(TotalPercentage);
            
            return randomNumber <= successRatePercentage;
        }

        private PaymentResponseDto GeneratePaymentResponse(bool status)
        {
            return new PaymentResponseDto
            {
                Identifier = Guid.NewGuid().ToString(),
                Status = status ? "successful" : "unsuccessful"
            };
        }
    }
}
