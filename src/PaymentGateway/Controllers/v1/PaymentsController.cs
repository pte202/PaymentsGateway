using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Configuration;
using PaymentGateway.Entities;
using PaymentGateway.ExternalModels;
using PaymentGateway.Models;
using PaymentGateway.Repositories;
using PaymentGateway.Services;
using System.Threading.Tasks;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentValidationService _paymentValidationService;

        private readonly IPaymentRepository _paymentRepository;

        private readonly IMapper _mapper;

        public PaymentsController(
            IPaymentService paymentService,
            IPaymentValidationService paymentValidationService,
            IPaymentRepository paymentRepository,
            IMapper mapper)
        {
            _paymentService = paymentService;
            _paymentValidationService = paymentValidationService;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Get a single payment provided an unique identifier
        /// </summary>
        /// <response code="200">Returns a payment based on given identifier</response>
        [HttpGet(ApiRoutes.Payments.Get, Name = "GetPayment")]
        [ProducesResponseType(typeof(ResponseCardPaymentDto), statusCode: 200)]
        public async Task<IActionResult> GetPayment(string identifier)
        {
            var paymentEntity = await _paymentRepository.GetPaymentAsync(identifier);

            if (paymentEntity == null) return NotFound();

            var dtoToReturn = _mapper.Map<Payment, ResponseCardPaymentDto>(paymentEntity);

            return new OkObjectResult(dtoToReturn);
        }

        /// <summary>
        /// Performs a payment request and creates a record of it in the system database
        /// </summary>
        /// <response code="201">Creates a payment record</response>  
        /// <response code="422">Unprocessable entity provided</response>
        /// <response code="400">Unable to complete request due to request validation failure</response>
        [HttpPost(ApiRoutes.Payments.Create)]
        [ProducesResponseType(typeof(ResponseCardPaymentDto), statusCode: 201)]
        [ProducesResponseType(typeof(ResponseCardPaymentDto), statusCode: 201)]
        [ProducesResponseType(typeof(ResponseErrorDto), statusCode: 400)]
        public async Task<IActionResult> CreateCardPayment(RequestCardPaymentDto dto)
        {
            var validationData = await _paymentValidationService.ValidateAsync(dto.Number, dto.Cvv, dto.Currency, dto.Amount);

            var externalRequestDto = _mapper.Map<RequestCardPaymentDto, RequestExternalPaymentDto>(dto);

            var externalResponseDto = await _paymentService.PayThroughAcquiringBankAsync(externalRequestDto);

            if (externalResponseDto != null)
            {
                var responseCardpaymentDto = await _paymentService.PersistPaymentRequestToDatabase(dto, externalResponseDto, validationData);

                return CreatedAtRoute(nameof(GetPayment), new { identifier = responseCardpaymentDto.Identifier }, responseCardpaymentDto);
            }
            
            return BadRequest();                   
        }
    }
}
