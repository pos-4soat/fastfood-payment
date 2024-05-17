using AutoFixture;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Infra.Http.Dto;
using fastfood_payment.Infra.Http;
using fastfood_payment.Infra.MercadoPago.Dtos.Request;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecFlow.Internal.Json;
using Moq.Protected;

namespace fastfood_payment.Tests.UnitTests.Infrastructure.OrderHttp
{
    public class OrderHttpClientTests : TestFixture
    {
        [Test]
        public async Task ShouldFailReadFromJsonAsyncOnGetStatus()
        {
            var client = new OrderHttpClient("https://webhook.d3fkon1.com/e5c1145f-4673-4f68-b29d-19fc8ef61f2f/");
            OrderRequest request = new(1);

            var result = await client.GetOrderStatus(request.Id, CancellationToken.None);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldFailReadFromJsonAsyncOnGetOrderAmount()
        {
            var client = new OrderHttpClient("https://webhook.d3fkon1.com/e5c1145f-4673-4f68-f/");

            var result = await client.GetOrderAmount(_modelFakerFactory.GenerateRequest<PaymentEntity>(), CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}
