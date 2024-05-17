using fastfood_payment.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_payment.Application.UseCases.CreatePayment;

public sealed record class CreatePaymentRequest(int OrderId) : IRequest<Result<CreatePaymentResponse>>;