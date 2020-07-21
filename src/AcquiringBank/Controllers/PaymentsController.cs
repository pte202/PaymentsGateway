using AcquiringBank.Models;
using AcquiringBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcquiringBank.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentsService _bankService;

        public PaymentsController(IPaymentsService bankService)
        {
            _bankService = bankService;
        }

        /// <summary>
        /// Simulates a payout to a merchant, returns unique identifier
        /// and a status that indicates whether the payment was successful.
        /// </summary>
        [HttpPost]
        public IActionResult Payment(PaymentRequestDto dto)
        {
            var responseObject = _bankService.MakePayment();

            return new OkObjectResult(responseObject);
        }
    }
}
