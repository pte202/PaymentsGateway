using System.Collections.Generic;

namespace PaymentGateway.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual List<Payment> Payments { get; set; }
    }
}
