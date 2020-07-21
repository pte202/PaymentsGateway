using System;
using System.Collections.Generic;

namespace PaymentGateway.Entities
{
    public class PaymentCard
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime ExpiryDate { get; set; }

        public int CardIssuerId { get; set; }
        public virtual PaymentCardIssuer CardIssuer { get; set; }

        public virtual List<Payment> Payments { get; set; }
    }
}
