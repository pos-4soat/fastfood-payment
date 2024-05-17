namespace fastfood_payment.Application.UseCases.CreatePayment;

public sealed record CreatePaymentResponse
{
    public CreatePaymentResponse(string paymentQrCode)
    {
        PaymentQrCode = paymentQrCode;
    }

    public string PaymentQrCode { get; set; }
}
