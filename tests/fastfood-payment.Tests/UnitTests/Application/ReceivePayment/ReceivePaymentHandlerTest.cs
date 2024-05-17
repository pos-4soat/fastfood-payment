using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Application.UseCases.CreatePayment;
using fastfood_payment.Application.UseCases.ReceivePayment;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Domain.Enum;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_payment.Tests.UnitTests.Application.ReceivePayment;

public class ReceivePaymentHandlerTest : TestFixture
{
    [Test, Description("Should receive payment confirmation")]
    public async Task ShouldReceivePaymentConfirmation()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();
        var entity = _modelFakerFactory.GenerateRequest<PaymentEntity>();
        entity.Payed = false;

        _repositoryMock.SetupFindAsync(entity);
        _orderHttpClientMock.SetupGetOrderStatus(1);
        _productionHttpClientMock.SetupRequestProduction(true);

        ReceivePaymentHandler service = new(_mapper, _orderHttpClientMock.Object, _productionHttpClientMock.Object, _repositoryMock.Object);

        Result<ReceivePaymentResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.OK);

        Assert.That(result.Value.Status, Is.EqualTo(PaymentStatus.PaymentConfirmed));

        _repositoryMock.VerifyFindAsync(Times.Once());
        _repositoryMock.VerifyUpdateAsync(request.OrderId, Times.Once());
        _repositoryMock.VerifyNoOtherCalls();
        _orderHttpClientMock.VerifyGetOrderStatus(request.OrderId, Times.Once());
        _orderHttpClientMock.VerifyNoOtherCalls();
        _productionHttpClientMock.VerifyRequestProduction(Times.Once());
        _productionHttpClientMock.VerifyNoOtherCalls();
    }

    //[Test, Description("Should return payment created previously")]
    //public async Task ShouldReturnPaymentCreatedPreviously()
    //{
    //    CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();
    //    var payment = _modelFakerFactory.GenerateRequest<PaymentEntity>();

    //    _repositoryMock.SetupFindAsync(payment);

    //    CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _orderHttpClientMock.Object, _repositoryMock.Object);

    //    Result<CreatePaymentResponse> result = await service.Handle(request, default);

    //    AssertExtensions.ResultIsSuccess(result, HttpStatusCode.Created);

    //    Assert.That(result.Value.PaymentQrCode, Is.EqualTo(payment.PaymentQrCode));

    //    _repositoryMock.VerifyFindAsync(Times.Once());
    //    _repositoryMock.VerifyNoOtherCalls();
    //    _orderHttpClientMock.VerifyNoOtherCalls();
    //    _orderPaymentMock.VerifyNoOtherCalls();
    //}

    //[Test, Description("Should return order not found")]
    //public async Task ShouldReturnOrderNotFound()
    //{
    //    CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();

    //    _repositoryMock.SetupFindAsync(null);
    //    _orderHttpClientMock.SetupGetOrderAmount(null);

    //    CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _orderHttpClientMock.Object, _repositoryMock.Object);

    //    Result<CreatePaymentResponse> result = await service.Handle(request, default);

    //    AssertExtensions.ResultIsFailure(result, "PBE002", HttpStatusCode.BadRequest);

    //    _repositoryMock.VerifyFindAsync(Times.Once());
    //    _repositoryMock.VerifyNoOtherCalls();
    //    _orderHttpClientMock.VerifyGetOrderAmount(Times.Once());
    //    _orderHttpClientMock.VerifyNoOtherCalls();
    //    _orderPaymentMock.VerifyNoOtherCalls();
    //}

    //[Test, Description("Should return failed QRCode generation")]
    //public async Task ShouldReturnFailedQRCodeGeneration()
    //{
    //    CreatePaymentRequest request = _modelFakerFactory.GenerateRequest<CreatePaymentRequest>();

    //    _repositoryMock.SetupFindAsync(null);
    //    _orderHttpClientMock.SetupGetOrderAmount(_modelFakerFactory.GenerateRequest<PaymentEntity>());
    //    _orderPaymentMock.SetupGerarQRCodeParaPagamentoDePedido(null);

    //    CreatePaymentHandler service = new(_mapper, _orderPaymentMock.Object, _orderHttpClientMock.Object, _repositoryMock.Object);

    //    Result<CreatePaymentResponse> result = await service.Handle(request, default);

    //    AssertExtensions.ResultIsFailure(result, "PBE003", HttpStatusCode.BadRequest);

    //    _repositoryMock.VerifyFindAsync(Times.Once());
    //    _repositoryMock.VerifyNoOtherCalls();
    //    _orderHttpClientMock.VerifyGetOrderAmount(Times.Once());
    //    _orderHttpClientMock.VerifyNoOtherCalls();
    //    _orderPaymentMock.VerifyGerarQRCodeParaPagamentoDePedido(Times.Once());
    //    _orderPaymentMock.VerifyNoOtherCalls();
    //}
}
