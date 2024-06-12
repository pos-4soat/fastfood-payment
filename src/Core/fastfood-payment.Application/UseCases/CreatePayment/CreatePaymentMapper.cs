using AutoMapper;
using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Application.UseCases.CreatePayment;

internal class CreatePaymentMapper : Profile
{
    public CreatePaymentMapper()
    {
        CreateMap<CreatePaymentRequest, PaymentEntity>()
           .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
           .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<Items, Domain.Entity.Items>();
    }
}
