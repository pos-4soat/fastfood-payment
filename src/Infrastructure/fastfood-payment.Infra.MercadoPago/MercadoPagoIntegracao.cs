using fastfood_payment.Domain.Contracts.Payment;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Infra.MercadoPago.Dtos.Request;
using fastfood_payment.Infra.MercadoPago.Dtos.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace fastfood_payment.Infra.MercadoPago;

public class MercadoPagoPayment : IOrderPayment
{
    private readonly string _accessToken;
    private readonly string _baseUrl;
    private readonly string _userId;
    private readonly string _externalPosId;

    public MercadoPagoPayment(IConfiguration config)
    {
        _accessToken = config.GetSection("MercadoPago:AccessToken").Value/* ?? throw new ArgumentNullException("Null Access token")*/;
        _baseUrl = config.GetSection("MercadoPago:BaseUrl").Value /*?? throw new ArgumentNullException("Null Base Url")*/;
        _userId = config.GetSection("MercadoPago:UserId").Value /*?? throw new ArgumentNullException("Null User Id")*/;
        _externalPosId = config.GetSection("MercadoPago:ExternalPosId").Value /*?? throw new ArgumentNullException("Null External Pos Id")*/;
    }

    public async Task<string> GerarQRCodeParaPagamentoDePedido(PaymentEntity paymentEntity)
    {
        GerarQRCodeRequest request = new GerarQRCodeRequest()
        {
            TotalAmount = paymentEntity.Amount,
            ExternalReference = paymentEntity.OrderId.ToString(),
            Title = "Pedido Lanchonete",
            Description = "Pedido Lanchonete",
            Items = paymentEntity.Items.Select(item => new Item()
            {
                Title = item.Name,
                UnitMeasure = "unit",
                Quantity = item.Quantity,
                UnitPrice = item.Price,
                TotalAmount = item.GetTotalAmount(),
            }).ToList(),
            NotificationUrl = $"https://webhook.d3fkon1.com/e8ff9837-9920-44c9-a0a1-434d2a02ea7c/payment/{paymentEntity.Id}?source_news=webhook"
        };

        using (HttpClient httpRequest = new HttpClient())
        {
            httpRequest.BaseAddress = new Uri(_baseUrl);
            httpRequest.DefaultRequestHeaders.Clear();
            httpRequest.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage result = await httpRequest.PostAsync($"/instore/orders/qr/seller/collectors/{_userId}/pos/{_externalPosId}/qrs", content);

            string resultString = await result.Content.ReadAsStringAsync();

            return result.IsSuccessStatusCode ? JsonConvert.DeserializeObject<GerarQRCodeResponse>(resultString).QrData : null;
        }
    }
}

