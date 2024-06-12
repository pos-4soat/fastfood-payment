using System.Diagnostics.CodeAnalysis;

namespace fastfood_payment.Infra.RabbitMq.Message;

[ExcludeFromCodeCoverage]
public class OrderMessage(int orderId, int status)
{
    public int Id { get; set; } = orderId;
    public int Status { get; set; } = status;
}
