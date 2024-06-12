using fastfood_payment.API.Controllers;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Application.UseCases.ReceivePayment;
using fastfood_payment.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace fastfood_payment.Tests.UnitTests.Infrastructure.Email;

public class EmailClientTest : TestFixture
{
    [Test, Description("")]
    public async Task ShouldReceiveOrderPayment()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<ReceivePaymentRequest>(), default))
            .ReturnsAsync(Result<ReceivePaymentResponse>.Success(_modelFakerFactory.GenerateRequest<ReceivePaymentResponse>()));

        PaymentController service = new(_mediatorMock.Object);

        IActionResult result = await service.ReceiveOrderPayment(It.IsAny<int>(), request, default);

        AssertExtensions.AssertResponse<ReceivePaymentRequest, ReceivePaymentResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), null);
    }
}