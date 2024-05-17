using fastfood_payment.Domain.Enum;
using Newtonsoft.Json;

namespace fastfood_payment.Application.Shared.BaseResponse;

public class ErrorResponse<TBody> : BaseResponse where TBody : class
{
    [JsonProperty(Order = 3)]
    public TBody? Error { get; set; }

    public ErrorResponse(TBody body) : base(StatusResponse.ERROR) => Error = body;
}