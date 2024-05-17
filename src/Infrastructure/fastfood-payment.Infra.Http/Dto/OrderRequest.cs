namespace fastfood_payment.Infra.Http.Dto
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public int Status { get; set; } = 2;
        public OrderRequest(int OrderId)
        {
            Id = OrderId;
        }
    }
}
