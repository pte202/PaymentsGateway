using AutoMapper;
using PaymentGateway.Entities;
using PaymentGateway.ExternalModels;
using PaymentGateway.Models;

namespace PaymentGateway.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<RequestCardPaymentDto, PaymentCard>();
            CreateMap<RequestCardPaymentDto, RequestExternalPaymentDto>();

            CreateMap<RequestCardPaymentDto, Payment>()
                .ForMember(e => e.Currency, opt => opt.Ignore());

            CreateMap<ResponseExternalPaymentDto, Payment>();

            CreateMap<Payment, ResponseCardPaymentDto>()
                .ForPath(dto => dto.Source.Type, opt => opt.MapFrom(e => e.Method))
                .ForPath(dto => dto.Source.LastFourDigitsOfNumber, opt => opt.MapFrom(e => e.PaymentCard.Number.Substring(e.PaymentCard.Number.Length - 4)))
                .ForPath(dto => dto.Source.ExpiryDate, opt => opt.MapFrom(e => e.PaymentCard.ExpiryDate.ToString("MM/yyyy")))
                .ForMember(dto => dto.Currency, opt => opt.MapFrom(e => e.Currency.Code));
        }
    }
}