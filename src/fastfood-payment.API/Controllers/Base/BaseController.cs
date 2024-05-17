using fastfood_payment.Application.Shared.BaseResponse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace fastfood_payment.API.Controllers.Base;

[Produces("application/json")]
[SwaggerResponse((int)HttpStatusCode.Unauthorized, "Access Denied", typeof(ErrorResponse<Error>))]
[SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unknown Error", typeof(ErrorResponse<Error>))]
public abstract class BaseController : ControllerBase
{
    public BaseController()
    {
    }

    protected ValueTask<IActionResult> GetResponseFromResult<TValue>(
        Result<TValue> result)
    {
        if (result.IsFailure)
        {
            ErrorResponse<Error> errorResponse = CreateErrorResponseFromResult(result);

            return new ValueTask<IActionResult>(StatusCode((int)result.StatusCode, errorResponse));
        }

        Response<object> response = new(result.Value!, result.Status);

        return new ValueTask<IActionResult>(StatusCode((int)HttpStatusCode.OK, response));
    }

    private static ErrorResponse<Error> CreateErrorResponseFromResult(Result result)
    {
        Error errorReponse = new(result.ErrorCode);
        return new ErrorResponse<Error>(errorReponse!);
    }
}