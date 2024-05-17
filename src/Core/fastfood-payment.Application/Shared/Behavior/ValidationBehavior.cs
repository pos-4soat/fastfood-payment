using fastfood_payment.Application.Shared.BaseResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace fastfood_payment.Application.Shared.Behavior;

[ExcludeFromCodeCoverage]
public class ValidationBehavior : ActionFilterAttribute
{
    public ValidationBehavior()
    {
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            string? errorCode = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .FirstOrDefault();

            if (errorCode.Contains("There is an open JSON object") || errorCode.Contains("BytePositionInLine:"))
                errorCode = "PBE001";

            Error errorReponse = new(errorCode);

            ErrorResponse<Error> responseObj = new ErrorResponse<Error>(errorReponse!);

            context.Result = new JsonResult(responseObj)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
