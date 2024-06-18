using AutoMapper;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Domain.Contracts.Email;
using fastfood_payment.Domain.Contracts.Mongo;
using fastfood_payment.Domain.Contracts.RabbitMq;
using fastfood_payment.Domain.Entity;
using MediatR;

namespace fastfood_payment.Application.UseCases.ReceivePayment;

public class ReceivePaymentHandler(
        IMapper mapper,
        IPaymentRepository repository,
        IConsumerService consumerService,
        IEmailClient emailClient) : IRequestHandler<ReceivePaymentRequest, Result<ReceivePaymentResponse>>
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IPaymentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IConsumerService _consumerService = consumerService ?? throw new ArgumentNullException(nameof(consumerService));
    private readonly IEmailClient _emailClient = emailClient ?? throw new ArgumentNullException(nameof(emailClient));

    public async Task<Result<ReceivePaymentResponse>> Handle(ReceivePaymentRequest request, CancellationToken cancellationToken)
    {
        PaymentEntity paymentEntity = _mapper.Map<PaymentEntity>(request);

        PaymentEntity existingPayment = await _repository.FindAsync(paymentEntity.OrderId, cancellationToken);

        if (existingPayment == null)
            return Result<ReceivePaymentResponse>.Failure("PBE004");

        if (existingPayment.Payed)
            return Result<ReceivePaymentResponse>.Failure("PBE005");

        bool success = request.Action.Equals("payment.created");
        if (!success)
        {
            _consumerService.PublishOrder(paymentEntity.OrderId);

            await _emailClient.SendEmailAsync("Falha no pagamento",
                                           "Seu pedido não pode ser processado devido a uma falha no pagamento.",
                                           "marcellocorassin@hotmail.com");
        }
        else
        {
            try
            {
                _consumerService.PublishProduction(existingPayment);
            }
            catch
            {
                return Result<ReceivePaymentResponse>.Failure("PBE007");
            }
        }

        await _repository.UpdateAsync(paymentEntity.OrderId, success, cancellationToken);

        return !success ?
            Result<ReceivePaymentResponse>.Failure("PBE012") :
            Result<ReceivePaymentResponse>.Success(new(paymentEntity.OrderId));
    }
}
