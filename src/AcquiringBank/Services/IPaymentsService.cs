using AcquiringBank.Models;

namespace AcquiringBank.Services
{
    public interface IPaymentsService
    {
        PaymentResponseDto MakePayment();
    }
}
