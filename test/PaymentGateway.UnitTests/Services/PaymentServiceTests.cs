using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using PaymentGateway.Configuration.AppSettings;
using PaymentGateway.Entities;
using PaymentGateway.ExternalModels;
using PaymentGateway.Helpers;
using PaymentGateway.Models;
using PaymentGateway.Repositories;
using PaymentGateway.Services;
using PaymentGateway.UnitTests.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.UnitTests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IOptions<AppSettings>> _mockAppSettings;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IPaymentRepository> _mockPaymentRepository;
        private readonly Mock<IPaymentCardRepository> _mockPaymentCardRepository;
        private readonly Mock<IMapper> _mockMapper;

        private readonly IPaymentService _paymentService;


        public PaymentServiceTests()
        {
            _mockAppSettings = new Mock<IOptions<AppSettings>>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _mockPaymentCardRepository = new Mock<IPaymentCardRepository>();
            _mockMapper = new Mock<IMapper>();

            _paymentService = new PaymentService(
                _mockAppSettings.Object,
                _mockHttpClientFactory.Object,
                _mockPaymentRepository.Object,
                _mockPaymentCardRepository.Object,
                _mockMapper.Object); ;
        }

        [Fact]
        public async Task PersistPaymentRequestToDatabase_ReturnsResponseCardPaymentDto()
        {
            // arrange
            var requestDto = TestData.GetRequestCardPaymentDto();
            var externalResponseDto = TestData.GetResponseExternalPaymentDto();
            var validationData = TestData.GetValidationData();

            _mockPaymentCardRepository.Setup(repo => repo.GetPaymentCardByNumberAsync(It.IsAny<string>())).Returns(Task.FromResult(TestData.GetPaymentCard()));
            _mockPaymentRepository.Setup(repo => repo.AddPayment(It.IsAny<Payment>()));
            _mockPaymentRepository.Setup(repo => repo.CommitAsync()).Returns(Task.FromResult(true));
            _mockMapper.Setup(m => m.Map<RequestCardPaymentDto, Payment>(It.IsAny<RequestCardPaymentDto>())).Returns(TestData.GetPayment());
            _mockMapper.Setup(m => m.Map<RequestCardPaymentDto, PaymentCard>(It.IsAny<RequestCardPaymentDto>())).Returns(TestData.GetPaymentCard());
            _mockMapper.Setup(m => m.Map<Payment, ResponseCardPaymentDto>(It.IsAny<Payment>())).Returns(TestData.GetResponseCardPaymentDto());
            _mockMapper.Setup(m => m.Map<ResponseExternalPaymentDto, Payment>(It.IsAny<ResponseExternalPaymentDto>())).Returns(TestData.GetPayment());

            // act
            var result = await _paymentService.PersistPaymentRequestToDatabase(requestDto, externalResponseDto, validationData);

            // assert
            Assert.IsType<ResponseCardPaymentDto>(result);
        }
    }
}
