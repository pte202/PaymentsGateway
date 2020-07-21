using System.Collections.Generic;

namespace PaymentGateway.Entities
{
    public class PaymentCardIssuer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }

        public virtual List<PaymentCard> PaymentCards { get; set; }
    }
}
