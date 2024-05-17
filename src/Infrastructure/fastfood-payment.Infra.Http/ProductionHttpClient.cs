using fastfood_payment.Domain.Contracts.Http;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Infra.Http.Dto;
using System.Text;
using System.Text.Json;

namespace fastfood_payment.Infra.Http;

public class ProductionHttpClient : HttpClient, IProductionHttpClient
{
    public ProductionHttpClient(string baseAddress)
    {
        BaseAddress = new Uri(baseAddress);
    }

    public async Task<bool> RequestProduction(PaymentEntity entity, CancellationToken cancellationToken)
    {
        StringContent content = new(JsonSerializer.Serialize(new ProductionRequest(entity)), Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await PostAsync(string.Empty, content, cancellationToken);

        return response.IsSuccessStatusCode;
    }
}
