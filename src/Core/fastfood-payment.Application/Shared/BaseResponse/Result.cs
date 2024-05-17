using fastfood_payment.Domain.Enum;
using System.Net;

namespace fastfood_payment.Application.Shared.BaseResponse;

public record Result
{
    protected Result() { }

    protected Result(string errorCode, HttpStatusCode statusCode, string? errorMessage)
    {
        ErrorCode = errorCode;
        IsFailure = true;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }

    public string ErrorCode { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsFailure { get; init; }
    public string? ErrorMessage { get; set; }
}

public sealed record Result<TValue> : Result
{
    protected Result() { }
    private Result(TValue value, StatusResponse status = StatusResponse.SUCCESS)
    {
        Value = value;
        Status = status;
        StatusCode = status switch
        {
            StatusResponse.CREATED => HttpStatusCode.Created,
            _ => HttpStatusCode.OK,
        };
    }
    protected Result(string errorCode, HttpStatusCode statusCode, string? errorMessage = null)
        : base(errorCode, statusCode, errorMessage) { }

    public TValue Value { get; set; }
    public StatusResponse Status { get; set; }

    public new static Result<TValue> Success(TValue value) => new(value);

    public new static Result<TValue> Success(TValue value, StatusResponse status) => new(value, status);

    public static Result<TValue> Failure(string errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(errorCode, statusCode, null);
}