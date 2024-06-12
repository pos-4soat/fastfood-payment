using fastfood_payment.Domain.Entity;
using System.Diagnostics.CodeAnalysis;

namespace fastfood_payment.Infra.RabbitMq.Message;

[ExcludeFromCodeCoverage]
public class ProductionMessage
{
    public int OrderId { get; set; }
    public IEnumerable<object> Items { get; set; }

    public ProductionMessage(PaymentEntity entity)
    {
        OrderId = entity.OrderId;
        Items = entity.Items;
    }
}
