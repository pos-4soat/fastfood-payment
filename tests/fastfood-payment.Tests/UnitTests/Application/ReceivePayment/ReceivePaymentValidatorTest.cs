using fastfood_payment.Application.UseCases.ReceivePayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_payment.Tests.UnitTests.Application.ReceivePayment;

public class ReceivePaymentValidatorTest : TestFixture
{
    private ReceivePaymentValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new ReceivePaymentValidator();
    }

    [Test]
    public void ShouldValidateActionRequirement()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();

        request.Action = null;

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "PBE008");
    }

    [Test]
    public void ShouldValidateUserRequirement()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "PBE009");
    }

    [Test]
    public void ShouldValidateUserCpf()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();

        request.Action = "payment.created";
        request.Data = null;

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "PBE010");
    }

    [Test]
    public void ShouldValidateProductIdRequirement()
    {
        ReceivePaymentRequest request = _modelFakerFactory.GenerateRequest<ReceivePaymentRequest>();

        request.Action = "payment.created";
        request.Data.Id = null;

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "PBE011");
    }
}