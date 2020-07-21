using PaymentGateway.ExternalModels;
using PaymentGateway.Helpers;
using PaymentGateway.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IPaymentService
    {
        Task<ResponseCardPaymentDto> PersistPaymentRequestToDatabase(RequestCardPaymentDto requestDto, ResponseExternalPaymentDto responseDto, ValidationData validationData);
        Task<ResponseExternalPaymentDto> PayThroughAcquiringBankAsync(RequestExternalPaymentDto dto);
    }
}
