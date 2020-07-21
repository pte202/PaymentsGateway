using System;

namespace PaymentGateway.Models
{
    public class ResponseErrorDto
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public ResponseErrorDto(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
        }
    }    
}