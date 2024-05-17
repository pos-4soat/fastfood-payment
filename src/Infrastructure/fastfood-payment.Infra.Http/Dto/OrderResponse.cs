namespace fastfood_payment.Infra.Http.Dto
{
    public class OrderResponse
    {
        public OrderData body { get; set; }
    }

    public class OrderData
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public IEnumerable<Items> Items { get; set; }
    }

    public class Items
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
