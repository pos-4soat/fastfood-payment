using AutoMapper;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Domain.Contracts.Mongo;
using fastfood_payment.Domain.Contracts.Payment;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Domain.Enum;
using MediatR;

namespace fastfood_payment.Application.UseCases.CreatePayment
{
    public class CreatePaymentHandler(
        IMapper mapper,
        IOrderPayment orderPayment,
        IPaymentRepository repository) : IRequestHandler<CreatePaymentRequest, Result<CreatePaymentResponse>>
    {
        private readonly IOrderPayment _orderPayment = orderPayment ?? throw new ArgumentNullException(nameof(orderPayment));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IPaymentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<Result<CreatePaymentResponse>> Handle(CreatePaymentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                PaymentEntity paymentEntity = _mapper.Map<PaymentEntity>(request);

                PaymentEntity existingPayment = await _repository.FindAsync(paymentEntity.OrderId, cancellationToken);

                if (existingPayment != null)
                    return Result<CreatePaymentResponse>.Success(new(existingPayment.PaymentQrCode), StatusResponse.CREATED);

                paymentEntity.PaymentQrCode = await _orderPayment.GerarQRCodeParaPagamentoDePedido(paymentEntity);

                if (string.IsNullOrEmpty(paymentEntity.PaymentQrCode))
                    return Result<CreatePaymentResponse>.Failure("PBE003");

                await _repository.InsertAsync(paymentEntity, cancellationToken);

                return Result<CreatePaymentResponse>.Success(new(paymentEntity.PaymentQrCode), StatusResponse.CREATED);
            }
            catch
            {
                return Result<CreatePaymentResponse>.Failure("PBE003");
            }
        }
    }
}
