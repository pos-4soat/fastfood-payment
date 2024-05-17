using MongoDB.Bson.Serialization.Attributes;

namespace fastfood_payment.Domain.Entity
{
    public class PaymentEntity
    {
        [BsonElement("paymentId")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentQrCode { get; set; }
        public bool Payed { get; set; } = false;
        public IEnumerable<Items> Items { get; set; }
    }

    public class Items
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal GetTotalAmount()
        => Price * Quantity;
    }
}
