using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Entities;
using PaymentGateway.ExternalModels;
using PaymentGateway.Helpers;
using PaymentGateway.Models;
using PaymentGateway.Repositories;
using PaymentGateway.Services;
using PaymentGateway.UnitTests.Data;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.UnitTests.Controllers
{
    public class PaymentsControllerTests
    {
        private readonly Mock<IPaymentService> _mockPaymentService;
        private readonly Mock<IPaymentValidationService> _mockPaymentValidationService;
        private readonly Mock<IPaymentRepository> _mockPaymentRepository;
        private readonly Mock<IMapper> _mockMapper;

        private readonly PaymentsController _paymentsController;

        public PaymentsControllerTests()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _mockPaymentValidationService = new Mock<IPaymentValidationService>();
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _mockMapper = new Mock<IMapper>();

            _paymentsController = new PaymentsController(
                _mockPaymentService.Object,
                _mockPaymentValidationService.Object,
                _mockPaymentRepository.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task GetPayment_ReturnsOkObjectResultOfTypeResponseCardPaymentDto()
        {
            // arrange
            var identifier = "aa297f35-5158-40b0-8529-29755ec7f456";

            _mockPaymentRepository.Setup(repo => repo.GetPaymentAsync(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetPayment()));

            _mockMapper.Setup(m => m.Map<Payment, ResponseCardPaymentDto>(It.IsAny<Payment>())).Returns(TestData.GetResponseCardPaymentDto());

            // act
            var result = await _paymentsController.GetPayment(identifier);

            // assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<ResponseCardPaymentDto>(objectResult.Value);
        }

        [Fact]
        public async Task GetPayment_ReturnsNotFound()
        {
            // arrange
            var identifier = "aa297f35-5158-40b0-8529-29755ec7f456";

            _mockPaymentRepository.Setup(repo => repo.GetPaymentAsync(It.IsAny<string>())).Returns(Task.FromResult<Payment>(null));

            // act
            var result = await _paymentsController.GetPayment(identifier);

            // assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCardPayment_ReturnsCreatedAtRouteResultOfTypeResponseCardPaymentDto()
        {
            // arrange
            var requestCardPaymentDto = TestData.GetRequestCardPaymentDto();

            _mockPaymentValidationService.Setup(repo => repo.ValidateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult<ValidationData>(TestData.GetValidationData()));
            _mockMapper.Setup(m => m.Map<RequestCardPaymentDto, RequestExternalPaymentDto>(It.IsAny<RequestCardPaymentDto>())).Returns(TestData.GetRequestExternalPaymentDto());
            _mockPaymentService.Setup(repo => repo.PayThroughAcquiringBankAsync(It.IsAny<RequestExternalPaymentDto>())).Returns(Task.FromResult<ResponseExternalPaymentDto>(TestData.GetResponseExternalPaymentDto()));
            _mockPaymentService.Setup(repo => repo.PersistPaymentRequestToDatabase(It.IsAny<RequestCardPaymentDto>(), It.IsAny<ResponseExternalPaymentDto>(), It.IsAny<ValidationData>())).Returns(Task.FromResult<ResponseCardPaymentDto>(TestData.GetResponseCardPaymentDto()));

            // act
            var result = await _paymentsController.CreateCardPayment(requestCardPaymentDto);

            // assert
            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.IsType<ResponseCardPaymentDto>(objectResult.Value);
        }
    }
}
