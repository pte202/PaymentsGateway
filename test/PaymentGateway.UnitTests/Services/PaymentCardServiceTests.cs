using Moq;
using PaymentGateway.Entities;
using PaymentGateway.Repositories;
using PaymentGateway.Services;
using PaymentGateway.UnitTests.Data;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.UnitTests.Services
{
    public class PaymentCardServiceTests
    {
        private readonly Mock<IPaymentCardIssuerRepository> _mockPaymentCardIssuerRepository;

        private readonly IPaymentCardService _paymentCardService;


        public PaymentCardServiceTests()
        {
            _mockPaymentCardIssuerRepository = new Mock<IPaymentCardIssuerRepository>();

            _paymentCardService = new PaymentCardService(
                _mockPaymentCardIssuerRepository.Object);
        }

        [Fact]
        public async Task GetCardIssuer_WhenSupportedIssuer_RetursPaymentCardIssuerObject()
        {
            // arrange
            var cardNumber = "4111111111111111";

            _mockPaymentCardIssuerRepository.Setup(repo => repo.GetPaymentCardIssuersAsync()).Returns(Task.FromResult(TestData.GetPaymentCardIssuers()));

            var expectedCardIssuerName = "Visa";

            // act
            var paymentCardIssuer = await _paymentCardService.GetCardIssuer(cardNumber);

            var result = paymentCardIssuer.Name;

            // assert
            Assert.IsType<PaymentCardIssuer>(paymentCardIssuer);
            Assert.True(result.Equals(expectedCardIssuerName));
        }

        [Fact]
        public async Task GetCardIssuer_WhenunsupportedIssuer_RetursNull()
        {
            // arrange
            var cardNumber = "30000000000004";

            _mockPaymentCardIssuerRepository.Setup(repo => repo.GetPaymentCardIssuersAsync()).Returns(Task.FromResult(TestData.GetPaymentCardIssuers()));

            // act
            var paymentCardIssuer = await _paymentCardService.GetCardIssuer(cardNumber);

            // assert
            Assert.Null(paymentCardIssuer);
        }
    }
}
