using System;

namespace PaymentGateway.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Identifier { get; set; }
        public string Status { get; set; }
        public int Amount { get; set; }
        public string Method { get; set; }

        public int CurrencyId { get; set; }
        public int PaymentCardId { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual PaymentCard PaymentCard { get; set; }

    }
}