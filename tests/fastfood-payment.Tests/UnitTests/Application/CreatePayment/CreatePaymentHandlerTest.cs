using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Application.UseCases.CreatePayment;
using fastfood_payment.Domain.Contracts.Payment;
using fastfood_payment.Domain.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_payment.Tests.UnitTests.Application.CreatePayment;

public class CreatePaymentHandlerTest : TestFixture
{
    [Test, Description("Should return payment created successfully")]
    public async Task ShouldCreatePaymentAsync()
    {
        CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();
        string qrcode = Faker.Lorem.Text();

        _repositoryMock.SetupFindAsync(null);
        _orderHttpClientMock.SetupGetOrderAmount(_modelFakerFactory.GenerateRequest<PaymentEntity>());
        _orderPaymentMock.SetupGerarQRCodeParaPagamentoDePedido(qrcode);

        CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _orderHttpClientMock.Object, _repositoryMock.Object);

        Result<CreatePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.Created);

        Assert.That(result.Value.PaymentQrCode, Is.EqualTo(qrcode));

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyInsertAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderHttpClientMock.VerifyGetOrderAmount(Times.Once());
        _orderHttpClientMock.VerifyNoOtherCalls();
        _orderPaymentMock.VerifyGerarQRCodeParaPagamentoDePedido(Times.Once());
        _orderPaymentMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return payment created previously")]
    public async Task ShouldReturnPaymentCreatedPreviously()
    {
        CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();
        var payment = _modelFakerFactory.GenerateRequest<PaymentEntity>();

        _repositoryMock.SetupFindAsync(payment);

        CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _orderHttpClientMock.Object, _repositoryMock.Object);

        Result<CreatePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.Created);

        Assert.That(result.Value.PaymentQrCode, Is.EqualTo(payment.PaymentQrCode));

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderHttpClientMock.VerifyNoOtherCalls();
        _orderPaymentMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return order not found")]
    public async Task ShouldReturnOrderNotFound()
    {
        CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();

        _repositoryMock.SetupFindAsync(null);
        _orderHttpClientMock.SetupGetOrderAmount(null);

        CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _orderHttpClientMock.Object, _repositoryMock.Object);

        Result<CreatePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "PBE002", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderHttpClientMock.VerifyGetOrderAmount(Times.Once());
        _orderHttpClientMock.VerifyNoOtherCalls();
        _orderPaymentMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return failed QRCode generation")]
    public async Task ShouldReturnFailedQRCodeGeneration()
    {
        CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();

        _repositoryMock.SetupFindAsync(null);
        _orderHttpClientMock.SetupGetOrderAmount(_modelFakerFactory.GenerateRequest<PaymentEntity>());
        _orderPaymentMock.SetupGerarQRCodeParaPagamentoDePedido(null);

        CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _orderHttpClientMock.Object, _repositoryMock.Object);

        Result<CreatePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "PBE003", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderHttpClientMock.VerifyGetOrderAmount(Times.Once());
        _orderHttpClientMock.VerifyNoOtherCalls();
        _orderPaymentMock.VerifyGerarQRCodeParaPagamentoDePedido(Times.Once());
        _orderPaymentMock.VerifyNoOtherCalls();
    }
}