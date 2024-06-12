using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Domain.Contracts.Mongo
{
    public interface IPaymentRepository
    {
        Task<PaymentEntity> FindAsync(int orderId, CancellationToken cancellationToken);
        Task InsertAsync(PaymentEntity paymentEntity, CancellationToken cancellationToken);
        Task UpdateAsync(int orderId, bool payed, CancellationToken cancellationToken);
    }
}
