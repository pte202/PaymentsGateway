using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public ResponseErrorDto Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            var code = 500;

            if (exception is ValidationException) code = 400;

            Response.StatusCode = code; 

            return new ResponseErrorDto(exception);
        }
    }
}
