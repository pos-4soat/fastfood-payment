using fastfood_payment.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_payment.Application.UseCases.CreatePayment;

public sealed record class CreatePaymentRequest : IRequest<Result<CreatePaymentResponse>>
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public IEnumerable<Items> Items { get; set; }
}

public sealed record Items
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}