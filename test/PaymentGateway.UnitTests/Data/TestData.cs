using PaymentGateway.Entities;
using PaymentGateway.ExternalModels;
using PaymentGateway.Helpers;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.UnitTests.Data
{
    public class TestData
    {
        public static ResponseCardPaymentDto GetResponseCardPaymentDto()
        {
            return new ResponseCardPaymentDto
            {
                Identifier = "75f2d02b-2b25-4b28-9bdc-25b75c0c59d1",
                Status = "successful",
                Amount = 433,
                Currency = "GBP",
                Source = new ResponseCardSourceDto
                {
                    Type = "Card",
                    LastFourDigitsOfNumber = "1111",
                    ExpiryDate = "06/2022"
                }
            };
        }

        public static IEnumerable<PaymentCardIssuer> GetPaymentCardIssuers()
        {
            return new List<PaymentCardIssuer>
            {
                 new PaymentCardIssuer
                    {
                       Id = 1,
                       Name = "AmericanExpress",
                       Pattern = @"^3[47][0-9]{5,}$"
                    },
                    new PaymentCardIssuer
                    {
                        Id = 2,
                        Name = "Visa",
                        Pattern = @"^4[0-9]{6,}$"
                    },
                    new PaymentCardIssuer
                    {
                        Id = 3,
                        Name = "Mastercard",
                        Pattern = @"^5[1-5][0-9]{5,}|222[1-9][0-9]{3,}|22[3-9][0-9]{4,}|2[3-6][0-9]{5,}|27[01][0-9]{4,}|2720[0-9]{3,}$"
                    }
            };
        }

        public static RequestCardPaymentDto GetRequestCardPaymentDto()
        {
            return new RequestCardPaymentDto
            {
                Number = "4111111111111111",
                ExpiryDate = "12/2020",
                Cvv = "542",
                Amount = 433,
                Currency = "GBP"
            };
        }

        public static RequestExternalPaymentDto GetRequestExternalPaymentDto()
        {
            return new RequestExternalPaymentDto
            {
                Number = "4111111111111111",
                ExpiryDate = "12/2020",
                Cvv = "542",
                Amount = 433,
                Currency = "GBP"
            };
        }

        public static ResponseExternalPaymentDto GetResponseExternalPaymentDto()
        {
            return new ResponseExternalPaymentDto
            {
                Identifier = "75f2d02b-2b25-4b28-9bdc-25b75c0c59d1",
                Status = "successful"
            };
        }

        public static PaymentCard GetPaymentCard()
        {
            return new PaymentCard
            {
                Id = 1,
                Number = "4111111111111111",
                ExpiryDate = new DateTime(2022, 06, 30),
                CardIssuerId = 1
            };
        }

        public static Payment GetPayment()
        {
            return new Payment
            {
                Id = 1,
                Identifier = "63d46c20-61bf-4c47-bfdc-6f08bc217406",
                Date = new DateTime(2001, 11, 01),
                Status = "success",
                Amount = 433,
                CurrencyId = 1,
                Method = "Card",
                PaymentCardId = 1
            };
        }

        public static ValidationData GetValidationData()
        {
            return new ValidationData
            {
                CardIssuer = new PaymentCardIssuer
                {
                    Id = 2,
                    Name = "Visa",
                    Pattern = @"^4[0-9]{6,}$"
                },
                Currency = new Currency
                {
                    Id = 1,
                    Name = "Sterling pound",
                    Code = "GBP"
                }
            };
        }

        public static Currency GetCurrency()
        {
            return new Currency
            {
                Id = 1,
                Name = "Sterling pound",
                Code = "GBP"
            };
        }

        public static PaymentCardIssuer GetCardIssuer()
        {
            return new PaymentCardIssuer
            {
                Id = 2,
                Name = "Visa",
                Pattern = @"^4[0-9]{6,}$"
            };
        }
    }
}
