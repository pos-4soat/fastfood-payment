using fastfood_payment.Infra.Http.Dto;
using fastfood_payment.Infra.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fastfood_payment.Domain.Entity;

namespace fastfood_payment.Tests.UnitTests.Infrastructure.Http;

public class ProductionHttpClientTest : TestFixture
{
    [Test]
    public async Task ShouldFailReadFromJsonAsyncOnRequestProduction()
    {
        var client = new ProductionHttpClient("https://webhook.d3fkon1.com/e5c1145f-4673-4f68-b29d-19fc8ef61f2f/");

        var result = await client.RequestProduction(_modelFakerFactory.GenerateRequest<PaymentEntity>(), CancellationToken.None);

        Assert.True(result);
    }
}
