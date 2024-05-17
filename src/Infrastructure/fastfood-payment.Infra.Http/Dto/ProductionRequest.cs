using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Infra.Http.Dto;

public class ProductionRequest
{
    public int OrderId { get; set; }
    public IEnumerable<object> Items { get; set; }

    public ProductionRequest(PaymentEntity entity)
    {
        OrderId = entity.OrderId;
        Items = entity.Items;
    }
}
