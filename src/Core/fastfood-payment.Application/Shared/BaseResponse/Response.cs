using fastfood_payment.Domain.Enum;
using Newtonsoft.Json;

namespace fastfood_payment.Application.Shared.BaseResponse;

public class Response<TBody> : BaseResponse where TBody : class
{
    [JsonProperty(Order = 3)]
    public TBody? Body { get; set; }

    public Response(TBody body, StatusResponse status) : base(status) => Body = body;
}