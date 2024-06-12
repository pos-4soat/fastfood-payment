using fastfood_payment.Domain.Contracts.Http;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Infra.Http.Dto;
using System.Net.Http.Json;
using Items = fastfood_payment.Domain.Entity.Items;

namespace fastfood_payment.Infra.Http;

public class OrderHttpClient : HttpClient, IOrderHttpClient
{
    public OrderHttpClient(string baseAddress)
    {
        BaseAddress = new Uri(baseAddress);
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
