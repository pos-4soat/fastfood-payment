using Newtonsoft.Json;

namespace fastfood_payment.Infra.MercadoPago.Dtos.Response;

public class GerarQRCodeResponse
{
    [JsonProperty("qr_data")]
    public string QrData { get; set; }
}
