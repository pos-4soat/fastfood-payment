using AutoMapper;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Domain.Contracts.Http;
using fastfood_payment.Domain.Contracts.Mongo;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Domain.Enum;
using MediatR;

namespace fastfood_payment.Application.UseCases.ReceivePayment;

public class ReceivePaymentHandler(
        IMapper mapper,
        IOrderHttpClient httpClientOrder,
        IProductionHttpClient httpClientProduction,
        IPaymentRepository repository) : IRequestHandler<ReceivePaymentRequest, Result<ReceivePaymentResponse>>
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IOrderHttpClient _httpClientOrder = httpClientOrder ?? throw new ArgumentNullException(nameof(httpClientOrder));
    private readonly IProductionHttpClient _httpClientProduction = httpClientProduction ?? throw new ArgumentNullException(nameof(httpClientProduction));
    private readonly IPaymentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<Result<ReceivePaymentResponse>> Handle(ReceivePaymentRequest request, CancellationToken cancellationToken)
    {
        PaymentEntity paymentEntity = _mapper.Map<PaymentEntity>(request);

        PaymentEntity existingPayment = await _repository.FindAsync(paymentEntity.OrderId, cancellationToken);

        if (existingPayment == null)
            return Result<ReceivePaymentResponse>.Failure("PBE004");

        if (existingPayment.Payed)
            return Result<ReceivePaymentResponse>.Failure("PBE005");

        int orderStatus = await _httpClientOrder.GetOrderStatus(paymentEntity.OrderId, cancellationToken);

        if (!orderStatus.Equals((int)PaymentStatus.AwaitingPayment))
            return Result<ReceivePaymentResponse>.Failure("PBE006");

        bool updated = await _httpClientProduction.RequestProduction(existingPayment, cancellationToken);

        if (!updated)
            return Result<ReceivePaymentResponse>.Failure("PBE007");

        await _repository.UpdateAsync(paymentEntity.OrderId, cancellationToken);

        return Result<ReceivePaymentResponse>.Success(new(paymentEntity.OrderId));
    }
}
