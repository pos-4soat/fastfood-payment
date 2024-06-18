using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Domain.Contracts.RabbitMq;

public interface IConsumerService
{
    void PublishProduction(PaymentEntity paymentEntity);
    void PublishOrder(int orderId);
    void StartConsuming();
    void Dispose();
}
