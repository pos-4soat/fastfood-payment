using fastfood_payment.Domain.Contracts.Http;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Infra.Http.Dto;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Items = fastfood_payment.Domain.Entity.Items;

namespace fastfood_payment.Infra.Http;

public class OrderHttpClient : HttpClient, IOrderHttpClient
{
    public OrderHttpClient(string baseAddress)
    {
        BaseAddress = new Uri(baseAddress);
    }

    public async Task<PaymentEntity> GetOrderAmount(PaymentEntity paymentEntity, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await GetAsync(paymentEntity.OrderId.ToString(), cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        OrderResponse? responseObj = await response.Content.ReadFromJsonAsync<OrderResponse>(cancellationToken);

        if (!responseObj.body.Status.Equals(1))
            return null;

        paymentEntity.Amount = responseObj.body.Amount;
        paymentEntity.Items = responseObj.body.Items.Select(item => new Items()
        {
            Name = item.Name,
            Quantity = item.Quantity,
            Price = item.Price,
        }).ToList();

        return paymentEntity;
    }

    public async Task<int> GetOrderStatus(int orderId, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await GetAsync(orderId.ToString(), cancellationToken);

        if (!response.IsSuccessStatusCode)
            return 0;

        try
        {
            OrderResponse? responseObj = await response.Content.ReadFromJsonAsync<OrderResponse>(cancellationToken);

            return responseObj.body.Status;
        }
        catch
        {
            return 0;
        }
    }
}
