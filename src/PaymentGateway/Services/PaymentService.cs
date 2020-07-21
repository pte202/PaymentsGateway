using Newtonsoft.Json;
using PaymentGateway.ExternalModels;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading;
using PaymentGateway.Repositories;
using AutoMapper;
using PaymentGateway.Helpers;
using PaymentGateway.Models;
using PaymentGateway.Entities;
using PaymentGateway.Configuration.AppSettings;
using Microsoft.Extensions.Options;

namespace PaymentGateway.Services
{
    public class PaymentService : IPaymentService, IDisposable
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;

        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentCardRepository _paymentCardRepository;

        private readonly IMapper _mapper;

        public PaymentService(
            IOptions<AppSettings> appSettings,
            IHttpClientFactory httpClientFactory,
            IPaymentRepository paymentRepository,
            IPaymentCardRepository paymentCardRepository,
            IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _paymentCardRepository = paymentCardRepository ?? throw new ArgumentNullException(nameof(paymentCardRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ResponseCardPaymentDto> PersistPaymentRequestToDatabase(RequestCardPaymentDto requestDto, ResponseExternalPaymentDto responseDto, ValidationData validationData)
        {
            var paymentEntity = _mapper.Map<RequestCardPaymentDto, Payment>(requestDto);

            paymentEntity.CurrencyId = validationData.Currency.Id;
            paymentEntity.Method = PaymentMethods.Card.ToString();

            var paymentCardEntity = await _paymentCardRepository.GetPaymentCardByNumberAsync(requestDto.Number);

            // if payment card exists do not add new record of it in database
            if (paymentCardEntity != null)
            {
                paymentEntity.PaymentCardId = paymentCardEntity.Id;
            }
            else
            {
                paymentCardEntity = _mapper.Map<RequestCardPaymentDto, PaymentCard>(requestDto);
                paymentCardEntity.CardIssuerId = validationData.CardIssuer.Id;

                paymentEntity.PaymentCard = paymentCardEntity;
            }

            _mapper.Map(responseDto, paymentEntity);

            _paymentRepository.AddPayment(paymentEntity);

            await _paymentRepository.CommitAsync();

            var responseCardpaymentDto = _mapper.Map<Payment, ResponseCardPaymentDto>(paymentEntity);

            return responseCardpaymentDto;
        }

        public async Task<ResponseExternalPaymentDto> PayThroughAcquiringBankAsync(RequestExternalPaymentDto dto) 
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var requestUrl = new Uri(_appSettings.WebServices.AcquiringBankUrl);
            var requestBody = JsonConvert.SerializeObject(dto);

            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = requestUrl,
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
            };

            var response = await httpClient.SendAsync(request, _cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                return System.Text.Json.JsonSerializer.Deserialize<ResponseExternalPaymentDto>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }

            _cancellationTokenSource.Cancel();

            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }
    }
}
