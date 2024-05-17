using FluentValidation;

namespace fastfood_payment.Application.UseCases.ReceivePayment;

public class ReceivePaymentValidator : AbstractValidator<ReceivePaymentRequest>
{
    public ReceivePaymentValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(dto => dto.Action)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("PBE008")
            .Must(dto => dto.Equals("payment.created"))
            .WithMessage("PBE009");

        RuleFor(dto => dto.Data)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("PBE010");

        RuleFor(dto => dto.Data.Id)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("PBE011")
            .NotEmpty().WithMessage("PBE011");
    }
}
