using FluentValidation;

namespace fastfood_payment.Application.UseCases.CreatePayment;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentValidator() { }
}