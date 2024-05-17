using fastfood_payment.Application.Shared.BaseResponse;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace fastfood_payment.Tests.UnitTests;

public static class AssertExtensions
{
    public static void ResultIsSuccess(Result result, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsFailure, Is.False);
        Assert.That(result.ErrorCode, Is.Null);
        Assert.That(result.StatusCode, Is.EqualTo(statusCode));
    }

    public static void ResultIsFailure(Result result, string errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.ErrorCode, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(errorCode));
        Assert.That(result.StatusCode, Is.EqualTo(statusCode));
    }

    public static void AssertResponse<TRequest, TResponse>(IActionResult result, HttpStatusCode expectedStatusCode, string expectedStatus, TRequest request)
    where TResponse : class
    {
        ObjectResult objectResult = result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.That(objectResult.StatusCode, Is.EqualTo((int)expectedStatusCode));

        Response<object> response = objectResult.Value as Response<object>;
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Status, Is.EqualTo(expectedStatus));

        TResponse responseBody = response.Body as TResponse;
        Assert.That(responseBody, Is.Not.Null);

        if (request != null)
            foreach (PropertyInfo property in typeof(TRequest).GetProperties())
            {
                PropertyInfo responseBodyProperty = typeof(TResponse).GetProperty(property.Name);
                if (responseBodyProperty != null)
                {
                    object requestValue = property.GetValue(request);
                    object responseBodyValue = responseBodyProperty.GetValue(responseBody);
                    Assert.That(responseBodyValue, Is.EqualTo(requestValue));
                }
            }
    }

    public static void AssertValidation(ValidationResult result, string errorCode)
    {
        Assert.That(!result.IsValid);
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(errorCode));
    }

    public static void AssertErrorResponse(IActionResult result, HttpStatusCode expectedStatusCode, string expectedStatus)
    {
        ObjectResult objectResult = result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.That(objectResult.StatusCode, Is.EqualTo((int)expectedStatusCode));

        ErrorResponse<Error> errorResponse = objectResult.Value as ErrorResponse<Error>;
        Assert.That(errorResponse, Is.Not.Null);
        Assert.That(errorResponse.Status, Is.EqualTo(expectedStatus));

        Error responseBody = errorResponse.Error;
        Assert.That(responseBody, Is.Not.Null);
        Assert.That(responseBody.ErrorCode, Is.Not.Null);
        Assert.That(responseBody.Message, Is.Not.Null);
    }
}