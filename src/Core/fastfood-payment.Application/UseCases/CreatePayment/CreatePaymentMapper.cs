using AutoMapper;
using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Application.UseCases.CreatePayment;

internal class CreatePaymentMapper : Profile
{
    public CreatePaymentMapper()
    {
        CreateMap<CreatePaymentRequest, PaymentEntity>();
    }
}
