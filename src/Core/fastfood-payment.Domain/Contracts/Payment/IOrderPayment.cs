using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Domain.Contracts.Payment
{
    public interface IOrderPayment
    {
        Task<string> GerarQRCodeParaPagamentoDePedido(PaymentEntity paymentEntity);
    }
}
