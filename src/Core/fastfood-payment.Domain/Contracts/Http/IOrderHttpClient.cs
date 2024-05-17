using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Domain.Contracts.Http
{
    public interface IOrderHttpClient
    {
        Task<PaymentEntity> GetOrderAmount(PaymentEntity paymentEntity, CancellationToken cancellationToken);
        Task<int> GetOrderStatus(int orderId, CancellationToken cancellationToken);
    }
}
