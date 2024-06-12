using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Application.UseCases.CreatePayment;
using fastfood_payment.Domain.Entity;
using Moq;
using System.Net;

namespace fastfood_payment.Tests.UnitTests.Application.CreatePayment;

public class CreatePaymentHandlerTest : TestFixture
{
    [Test, Description("Should return payment created successfully")]
    public async Task ShouldCreatePaymentAsync()
    {
        CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();
        string qrcode = Faker.Lorem.Text();

        _repositoryMock.SetupFindAsync(null);
        _orderPaymentMock.SetupGerarQRCodeParaPagamentoDePedido(qrcode);

        CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _repositoryMock.Object);

        Result<CreatePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.Created);

        Assert.That(result.Value.PaymentQrCode, Is.EqualTo(qrcode));

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyInsertAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderPaymentMock.VerifyGerarQRCodeParaPagamentoDePedido(Times.Once());
        _orderPaymentMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return payment created previously")]
    public async Task ShouldReturnPaymentCreatedPreviously()
    {
        CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();
        PaymentEntity payment = _modelFakerFactory.GenerateRequest<PaymentEntity>();

        _repositoryMock.SetupFindAsync(payment);

        CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _repositoryMock.Object);

        Result<CreatePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.Created);

        Assert.That(result.Value.PaymentQrCode, Is.EqualTo(payment.PaymentQrCode));

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderPaymentMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return failed QRCode generation")]
    public async Task ShouldReturnFailedQRCodeGeneration()
    {
        CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();

        _repositoryMock.SetupFindAsync(null);
        _orderPaymentMock.SetupGerarQRCodeParaPagamentoDePedido(null);

        CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _repositoryMock.Object);

        Result<CreatePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "PBE003", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderPaymentMock.VerifyGerarQRCodeParaPagamentoDePedido(Times.Once());
        _orderPaymentMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return failed QRCode generation")]
    public async Task ShouldReturnCatchFailedQRCodeGeneration()
    {
        CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();

        _repositoryMock.SetupFindAsync(null);
        _orderPaymentMock.SetupExceptionGerarQRCodeParaPagamentoDePedido();

        CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _repositoryMock.Object);

        Result<CreatePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "PBE003", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderPaymentMock.VerifyGerarQRCodeParaPagamentoDePedido(Times.Once());
        _orderPaymentMock.VerifyNoOtherCalls();
    }
}