using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Application.UseCases.ReceivePayment;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Domain.Enum;
using Moq;
using System.Net;

namespace fastfood_payment.Tests.UnitTests.Application.ReceivePayment;

public class ReceivePaymentHandlerTest : TestFixture
{
    [Test, Description("Should receive payment confirmation")]
    public async Task ShouldReceivePaymentConfirmation()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();
        request.Action = "payment.created";
        PaymentEntity entity = _modelFakerFactory.GenerateRequest<PaymentEntity>();
        entity.Payed = false;

        _repositoryMock.SetupFindAsync(entity);

        ReceivePaymentHandler service = new(_mapper, _repositoryMock.Object, _consumerMock.Object, _emailMock.Object);

        Result<ReceivePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.OK);

        Assert.That(result.Value.Status, Is.EqualTo(PaymentStatus.PaymentConfirmed));

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyUpdateAsync(request.OrderId, request.Action.Equals("payment.created"), Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _emailMock.VerifyNoOtherCalls();
        _consumerMock.VerifyPublishProduction();
        _consumerMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return payment not found")]
    public async Task ShouldReturnPaymentNotFound()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();
        request.Action = "payment.created";
        PaymentEntity entity = _modelFakerFactory.GenerateRequest<PaymentEntity>();
        entity.Payed = false;

        _repositoryMock.SetupFindAsync(null);

        ReceivePaymentHandler service = new(_mapper, _repositoryMock.Object, _consumerMock.Object, _emailMock.Object);

        Result<ReceivePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "PBE004", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _emailMock.VerifyNoOtherCalls();
        _consumerMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return payment already received")]
    public async Task ShouldReturnPaymentAlreadyReceived()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();
        request.Action = "payment.created";
        PaymentEntity entity = _modelFakerFactory.GenerateRequest<PaymentEntity>();
        entity.Payed = true;

        _repositoryMock.SetupFindAsync(entity);

        ReceivePaymentHandler service = new(_mapper, _repositoryMock.Object, _consumerMock.Object, _emailMock.Object);

        Result<ReceivePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "PBE005", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _emailMock.VerifyNoOtherCalls();
        _consumerMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return payment denied")]
    public async Task ShouldReturnPaymentDenied()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();
        request.Action = "payment.denied";
        PaymentEntity entity = _modelFakerFactory.GenerateRequest<PaymentEntity>();
        entity.Payed = false;

        _repositoryMock.SetupFindAsync(entity);

        ReceivePaymentHandler service = new(_mapper, _repositoryMock.Object, _consumerMock.Object, _emailMock.Object);

        Result<ReceivePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "PBE012", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyUpdateAsync(request.OrderId, request.Action.Equals("payment.created"), Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _emailMock.VerifySendEmailAsync();
        _consumerMock.VerifyPublishOrder();
        _consumerMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return failed update status")]
    public async Task ShouldReturnFailedUpdateStatus()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();
        request.Action = "payment.created";
        PaymentEntity entity = _modelFakerFactory.GenerateRequest<PaymentEntity>();
        entity.Payed = false;

        _repositoryMock.SetupFindAsync(entity);
        _consumerMock.SetupExceptionPublishProduction();

        ReceivePaymentHandler service = new(_mapper, _repositoryMock.Object, _consumerMock.Object, _emailMock.Object);

        Result<ReceivePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "PBE007", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _emailMock.VerifyNoOtherCalls();
        _consumerMock.VerifyPublishProduction();
        _consumerMock.VerifyNoOtherCalls();
    }
}
