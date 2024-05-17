using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Domain.Contracts.Http
{
    public interface IProductionHttpClient
    {
        Task<bool> RequestProduction(PaymentEntity entity, CancellationToken cancellationToken);
    }
}
