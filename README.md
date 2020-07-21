# CheckoutPayments

## This project provides a sample PaymentsGateway, which would accept payments requests from a merchant and communicate with a 3rd-party API to do the actual payouts

### The project is developed with ASP.NET Core 3.1
### SQL Server has been used to store data
### Xunit has been used to perform Unit tests
### Swagger has been included to document the APIs

## The solutions is formed of:

- src
  - PaymentsGateway Project
  - AcquiringBank Project (dummy service, which returns successful/uncessful payment request based on probability calculation - 60% success rate to easily simulate both scenarious)
- test
  - PaymentsGateway.UnitTests Project
  
## PaymentsGateway Project 

- API Versioning
- Request Caching
- Request Rate Limit

On Start Swagger UI is loaded

Note: No need to run migration, database is created automatically on first start

## The Databse is organized in a way that it can be easily extend in the future

It has the following tables: 

- Payments (keep track of the payment transactions)
- PaymetCards (store card information)
- PaymentIssuers (store a record of available issuers such as Visa, MasterCard, etc.)
- Currencies (store a record of available currecies such as GBP, USD, etc.)

Initialization Data:

- Payments 

```csharp
  new Payment
  {
      Id = 1,
      Identifier = "63d46c20-61bf-4c47-bfdc-6f08bc217406",
      Date = new DateTime(2001, 11, 01),
      Status = "success",
      Amount = 1243,
      CurrencyId = 1,
      Method = "Card",
      PaymentCardId = 1
  }
```

- PaymentCards

```csharp
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
```

- PaymentIssuers

```csharp
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
```

- Currencies

```csharp
  new Currency
  {
      Id = 1,
      Name = "Pound sterling",
      Code = "GBP"
  },
  new Currency
  {
      Id =2,
      Name = "United States Dollar",
      Code = "USD"
  }
```

## Running the application

When the application is ran the two projects in the source folder are going to be launched.

## Imrpovements for Production

- The AquiringBank project can be removed and replaced by an actual service
- Card Numbers should be encrypted if they are stored in database
- Parameters for Http requests can also be encrypted
- Logging Should be added
- Authentication Should be added
- More functionality can be exposed through additional endpoints to manage Currencies, CardIssuers, Payments, PaymentCards, etc.
- More comprehensive testing needs to be done  - database, network, external calls, integration testing, et.c
