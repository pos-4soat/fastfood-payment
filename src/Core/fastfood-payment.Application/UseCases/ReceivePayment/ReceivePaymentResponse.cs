using fastfood_payment.Domain.Enum;

namespace fastfood_payment.Application.UseCases.ReceivePayment;

public sealed record ReceivePaymentResponse
{
    public int Id { get; set; }
    public PaymentStatus Status { get; set; }

    public ReceivePaymentResponse(int orderId)
    {
        Id = orderId;
        Status = PaymentStatus.PaymentConfirmed;
    }
}