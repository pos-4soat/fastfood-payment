using Amazon.Runtime;
using AutoFixture;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Infra.MercadoPago;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using fastfood_payment.Application.UseCases.ReceivePayment;
using RichardSzalay.MockHttp;
using Microsoft.Extensions.Configuration;

namespace fastfood_payment.Tests.UnitTests.Infrastructure.MercadoPago;

public class MercadoPagoPaymentTest : TestFixture
{
    [Test]
    public async Task GenerateQrCodeForOrderPayment_ThrowsException_WhenAccessTokenIsNull()
    {
        PaymentEntity request = _modelFakerFactory.GenerateRequest<PaymentEntity>();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new KeyValuePair<string, string>("MercadoPago:AccessToken", "access_token"),
                new KeyValuePair<string, string>("MercadoPago:BaseUrl", "https://api.mercadopago.com"),
                new KeyValuePair<string, string>("MercadoPago:UserId", "user_id"),
                new KeyValuePair<string, string>("MercadoPago:ExternalPosId", "external_pos_id"),
            ])
            .Build();

        MercadoPagoPayment service = new(config);

        var response = await service.GerarQRCodeParaPagamentoDePedido(request);

        Assert.Null(response);
    }
}
