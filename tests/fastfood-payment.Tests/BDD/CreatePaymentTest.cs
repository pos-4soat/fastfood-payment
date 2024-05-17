using fastfood_payment.API.Controllers;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Application.UseCases.CreatePayment;
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
    private CreatePaymentRequest _request;
    private IActionResult _result;

    [Test, Description("")]
    public async Task CreateANewPayment()
    {
        GivenIHaveAValidCreatePaymentRequest();
        GivenTheRepositoryReturnsASuccessfulResult();
        await WhenIRequestAPaymentCreation();
        ThenTheResultShouldBeACreatedResult();
    }

    [Given(@"I have a valid create payment request")]
    public void GivenIHaveAValidCreatePaymentRequest()
    {
        _request = new CreatePaymentRequest(1);
    }

    [Given(@"the repository returns a successful result")]
    public void GivenTheRepositoryReturnsASuccessfulResult()
    {
        _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreatePaymentRequest>(), default))
            .ReturnsAsync(Result<CreatePaymentResponse>.Success(new CreatePaymentResponse("paymentQRCodeString"), StatusResponse.CREATED));
    }

    [When(@"I request a payment creation")]
    public async Task WhenIRequestAPaymentCreation()
    {
        PaymentController controller = new PaymentController(_mediatorMock.Object);

        _result = await controller.CreatePayment(_request, default);
    }

    [Then(@"the result should be a CreatedResult")]
    public void ThenTheResultShouldBeACreatedResult()
    {
        ObjectResult? objectResult = _result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.That(objectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        Response<object>? response = objectResult.Value as Response<object>;
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Status, Is.EqualTo(nameof(StatusResponse.CREATED)));

        CreatePaymentResponse? body = response.Body as CreatePaymentResponse;
        Assert.That(body.PaymentQrCode, Is.Not.Null);
    }
}
