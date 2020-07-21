using Moq;
using PaymentGateway.Entities;
using PaymentGateway.Helpers;
using PaymentGateway.Repositories;
using PaymentGateway.Services;
using PaymentGateway.UnitTests.Data;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayTests
{
    public class PaymentValidationServiceTests
    {
        private readonly Mock<ICurrencyRepository> _mockCurrencyRepository;
        private readonly Mock<IPaymentCardService> _mockPaymentCardService;

        private readonly IPaymentValidationService _paymentValidationService;


        public PaymentValidationServiceTests()
        {
            _mockCurrencyRepository = new Mock<ICurrencyRepository>();
            _mockPaymentCardService = new Mock<IPaymentCardService>();

            _paymentValidationService = new PaymentValidationService(
                _mockCurrencyRepository.Object,
                _mockPaymentCardService.Object);
        }

        [Fact]
        public async Task Validate_WhenValidCardPayment_RetursValidationDataObject()
        {
            // arrange
            var cardNumber = "4111111111111111";
            var cvv = "432";
            var currencyCode = "GBP";
            var amount = 513;

            _mockCurrencyRepository.Setup(repo => repo.GetCurrencyByCodeAsync(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetCurrency()));
            _mockPaymentCardService.Setup(repo => repo.GetCardIssuer(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetCardIssuer()));

            // act
            var result = await _paymentValidationService.ValidateAsync(cardNumber, cvv, currencyCode, amount);

            // assert
            Assert.IsType<ValidationData>(result);
        }

        [Fact]
        public async Task Validate_WhenUnsupportedPaymentCardIssuer_ThrowsValidationException()
        {
            // arrange
            var cardNumber = "30000000000004";
            var cvv = "432";
            var currencyCode = "USD";
            var amount = 513;

            _mockCurrencyRepository.Setup(repo => repo.GetCurrencyByCodeAsync(It.IsAny<string>())).Returns(Task.FromResult<Currency>(null));
            _mockPaymentCardService.Setup(repo => repo.GetCardIssuer(It.IsAny<string>())).Returns(Task.FromResult<PaymentCardIssuer>(null));

            // act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _paymentValidationService.ValidateAsync(cardNumber, cvv, currencyCode, amount));

            // assert
            Assert.Equal("Unsupported Payment Card Issuer", exception.Message);
        }

        [Fact]
        public async Task Validate_WhenInvalidPaymentCurrency_ThrowsValidationException()
        {
            // arrange
            var cardNumber = "4111111111111111";
            var cvv = "432";
            var currencyCode = "USD";
            var amount = 513;

            _mockCurrencyRepository.Setup(repo => repo.GetCurrencyByCodeAsync(It.IsAny<string>())).Returns(Task.FromResult<Currency>(null));
            _mockPaymentCardService.Setup(repo => repo.GetCardIssuer(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetCardIssuer()));

            // act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _paymentValidationService.ValidateAsync(cardNumber, cvv, currencyCode, amount));

            // assert
            Assert.Equal("Unsupported Payment Currency", exception.Message);
        }

        [Fact]
        public async Task Validate_WhenInvalidPaymentCard_ThrowsValidationException()
        {
            // arrange
            var cardNumber = "41888561545322221111";
            var cvv = "4325";
            var currencyCode = "USD";
            var amount = 513;

            _mockCurrencyRepository.Setup(repo => repo.GetCurrencyByCodeAsync(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetCurrency()));
            _mockPaymentCardService.Setup(repo => repo.GetCardIssuer(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetCardIssuer()));

            // act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _paymentValidationService.ValidateAsync(cardNumber, cvv, currencyCode, amount));

            // assert
            Assert.Equal("Invalid Payment Card", exception.Message);
        }

        [Fact]
        public async Task Validate_WhenInvalidPaymentRequest_ThrowsValidationException()
        {
            // arrange
            var cardNumber = "4111111111111111";
            var cvv = "435";
            var currencyCode = "USD";
            var amount = -100;

            _mockCurrencyRepository.Setup(repo => repo.GetCurrencyByCodeAsync(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetCurrency()));
            _mockPaymentCardService.Setup(repo => repo.GetCardIssuer(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetCardIssuer()));

            // act
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _paymentValidationService.ValidateAsync(cardNumber, cvv, currencyCode, amount));

            // assert
            Assert.Equal("Invalid Payment Request", exception.Message);
        }

        [Fact]
        public void IsValidPaymentCard_WhenValidVisaProperties_RetursTrue()
        {
            // arrange
            var cardNumber = "4111111111111111";
            var cvv = "432";
            var cardIssuer = "Visa";

            // act
            var result = _paymentValidationService.IsValidPaymentCard(cardNumber, cvv, cardIssuer);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPaymentCard_WhenInvalidVisaProperties_RetursFalse()
        {
            // arrange
            var cardNumber = "2111111111111111";
            var cvv = "432";
            var cardIssuer = "Visa";

            // act
            var result = _paymentValidationService.IsValidPaymentCard(cardNumber, cvv, cardIssuer);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPaymentCard_WhenValidMasterCardProperties_RetursTrue()
        {
            // arrange
            var cardNumber = "5500000000000004";
            var cvv = "422";
            var cardIssuer = "MasterCard";

            // act
            var result = _paymentValidationService.IsValidPaymentCard(cardNumber, cvv, cardIssuer);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPaymentCard_WhenInvalidMasterCardProperties_RetursFalse()
        {
            // arrange
            var cardNumber = "112120000000000004";
            var cvv = "422";
            var cardIssuer = "MasterCard";

            // act
            var result = _paymentValidationService.IsValidPaymentCard(cardNumber, cvv, cardIssuer);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPaymentCard_WhenValidAmericanExpressProperties_RetursTrue()
        {
            // arrange
            var cardNumber = "340000000000009";
            var cvv = "4227";
            var cardIssuer = "AmericanExpress";

            // act
            var result = _paymentValidationService.IsValidPaymentCard(cardNumber, cvv, cardIssuer);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPaymentCard_WhenInvalidAmericanExpressProperties_RetursFalse()
        {
            // arrange
            var cardNumber = "340000000000009";
            var cvv = "421";
            var cardIssuer = "AmericanExpress";

            // act
            var result = _paymentValidationService.IsValidPaymentCard(cardNumber, cvv, cardIssuer);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardNumber_WhenValidCard_ReturnsTrue()
        {
            // arrange
            var creditCardNumber = "4111111111111111";

            // act
            var result = _paymentValidationService.IsValidCardNumber(creditCardNumber);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidCardNumber_WhenInvalidCard_ReturnsFalse()
        {
            // arrange
            var creditCardNumber = "4111112411111171";

            // act
            var result = _paymentValidationService.IsValidCardNumber(creditCardNumber);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardNumber_WhenCardIncludesNonDigits_ReturnsFalse()
        {
            // arrange
            var creditCardNumber = "41111e2e2111171";

            // act
            var result = _paymentValidationService.IsValidCardNumber(creditCardNumber);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardNumber_WhenCardLengthIsLessThanExpected_ReturnsFalse()
        {
            // arrange
            var creditCardNumber = "4111211171";

            // act
            var result = _paymentValidationService.IsValidCardNumber(creditCardNumber);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardNumber_WhenCardLengthIsMoreThanExpected_ReturnsFalse()
        {
            // arrange
            var creditCardNumber = "411121117132324341234134311231231231232312435434534531";

            // act
            var result = _paymentValidationService.IsValidCardNumber(creditCardNumber);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardExpiryDate_WhenExpiryDateNotPassed_ReturnsTrue()
        {
            // arrange
            var expiryDate = "01/2022";

            // act
            var result = PaymentValidationService.IsValidCardExpiryDate(expiryDate);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidCardExpiryDate_WhenExpiryDateHasPassed_ReturnsFalse()
        {
            // arrange
            var expiryDate = "01/2002";

            // act
            var result = PaymentValidationService.IsValidCardExpiryDate(expiryDate);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardCvv_WhenCvvIsThreeCharactersAndIssuerIsVisa_ReturnsTrue()
        {
            // arrange
            var creditCardCvv = "533";
            var creditCardIssuer = "Visa";

            // act
            var result = _paymentValidationService.IsValidCardCvv(creditCardCvv, creditCardIssuer);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidCardCvv_WhenCvvIsThreeCharactersAndIssuerIsMastercard_ReturnsTrue()
        {
            // arrange
            var creditCardCvv = "433";
            var creditCardIssuer = "MasterCard";

            // act
            var result = _paymentValidationService.IsValidCardCvv(creditCardCvv, creditCardIssuer);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidCardCvv_WhenCvvIsFourCharactersAndIssuerIsAmericanExpress_ReturnsTrue()
        {
            // arrange
            var creditCardCvv = "5323";
            var creditCardIssuer = "AmericanExpress";

            // act
            var result = _paymentValidationService.IsValidCardCvv(creditCardCvv, creditCardIssuer);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidCardCvv_WhenCvvIsThreeCharactersAndIssuerIsAmericanExpress_ReturnsFalse()
        {
            // arrange
            var creditCardCvv = "531";
            var creditCardIssuer = "AmericanExpress";

            // act
            var result = _paymentValidationService.IsValidCardCvv(creditCardCvv, creditCardIssuer);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardCvv_WhenCvvIsIncludesNonDigitCharacters_ReturnsFalse()
        {
            // arrange
            var creditCardCvv = "5a1";
            var creditCardIssuer = "Visa";

            // act
            var result = _paymentValidationService.IsValidCardCvv(creditCardCvv, creditCardIssuer);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardCvv_WhenCvvHasLenghtEqualZero_ReturnsFalse()
        {
            // arrange
            var creditCardCvv = "12";
            var creditCardIssuer = "MasterCard";

            // act
            var result = _paymentValidationService.IsValidCardCvv(creditCardCvv, creditCardIssuer);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardCvv_WhenCvvHasLenghtSmallerThanThree_ReturnsFalse()
        {
            // arrange
            var creditCardCvv = "12";
            var creditCardIssuer = "Visa";

            // act
            var result = _paymentValidationService.IsValidCardCvv(creditCardCvv, creditCardIssuer);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidCardCvv_WhenCvvHasLenghtLargerThanThree_ReturnsFalse()
        {
            // arrange
            var creditCardCvv = "12223";
            var creditCardIssuer = "Visa";

            // act
            var result = _paymentValidationService.IsValidCardCvv(creditCardCvv, creditCardIssuer);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPayment_WhenPositiveAmountAndValidCurrencyCode_ReturnsTrue()
        {
            // arrange
            var ammount = 123;
            var currencyCode = "GBP";

            // act
            var result = _paymentValidationService.IsValidPayment(ammount, currencyCode);
            // assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPayment_WhenNegativeAmountAndValidCurrencyCode_ReturnsFalse()
        {
            // arrange
            var ammount = -1;
            var currencyCode = "GBP";

            // act
            var result = _paymentValidationService.IsValidPayment(ammount, currencyCode);
            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPayment_WhenAmountIsZeroAndValidCurrencyCode_ReturnsFalse()
        {
            // arrange
            var ammount = 0;
            var currencyCode = "GBP";

            // act
            var result = _paymentValidationService.IsValidPayment(ammount, currencyCode);
            // assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPayment_WhenAmountIsZeroAndInvalidCurrencyCode_ReturnsFalse()
        {
            // arrange
            var ammount = 0;
            var currencyCode = "GB2!P";

            // act
            var result = _paymentValidationService.IsValidPayment(ammount, currencyCode);
            // assert
            Assert.False(result);
        }
    }
}
