using AutoMapper;
using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Application.UseCases.ReceivePayment;

public class ReceivePaymentMapper : Profile
{
    public ReceivePaymentMapper()
    {
        CreateMap<ReceivePaymentRequest, PaymentEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId));
    }
}