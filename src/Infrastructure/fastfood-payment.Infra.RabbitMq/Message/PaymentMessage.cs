using System.Diagnostics.CodeAnalysis;

namespace fastfood_payment.Infra.RabbitMq.Message
{
    [ExcludeFromCodeCoverage]
    public class PaymentMessage
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public IEnumerable<Items> Items { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Items
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
