using fastfood_payment.API.Controllers;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Application.UseCases.ReceivePayment;
using fastfood_payment.Domain.Enum;
using fastfood_payment.Tests.UnitTests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using TechTalk.SpecFlow;

namespace fastfood_payment.Tests.BDD;

[TestFixture]
public class CreatePaymentTest : TestFixture
{
    private Mock<IMediator> _mediatorMock;
    private ReceivePaymentRequest _request;
    private IActionResult _result;

    [Test, Description("")]
    public async Task ReceivePaymentApproved()
    {
        GivenIHaveAValidReceivePaymentRequest();
        GivenTheRepositoryReturnsASuccessfulResult();
        await WhenIRequestAPaymentCreation();
        ThenTheResultShouldBeACreatedResult();
    }

    [Given(@"I have a valid receive payment request")]
    private void GivenIHaveAValidReceivePaymentRequest()
    {
        _request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();
    }

    [Given(@"the repository returns a successful result")]
    private void GivenTheRepositoryReturnsASuccessfulResult()
    {
        _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<ReceivePaymentRequest>(), default))
            .ReturnsAsync(Result<ReceivePaymentResponse>.Success(new ReceivePaymentResponse(1)));
    }

    [When(@"I request a payment creation")]
    private async Task WhenIRequestAPaymentCreation()
    {
        PaymentController controller = new(_mediatorMock.Object);

        _result = await controller.ReceiveOrderPayment(1, _request, default);
    }

    [Then(@"the result should be a CreatedResult")]
    private void ThenTheResultShouldBeACreatedResult()
    {
        ObjectResult? objectResult = _result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.That(objectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        Response<object>? response = objectResult.Value as Response<object>;
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Status, Is.EqualTo(nameof(StatusResponse.SUCCESS)));

        ReceivePaymentResponse? body = response.Body as ReceivePaymentResponse;
        Assert.That(body.Id, Is.EqualTo(1));
        Assert.That(body.Status, Is.EqualTo(PaymentStatus.PaymentConfirmed));
    }
}
