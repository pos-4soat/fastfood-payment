using fastfood_payment.Domain.Contracts.Payment;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Tests.UnitTests;
using Moq;

namespace fastfood_payment.Tests.Mocks;

public class OrderPaymentMock : BaseCustomMock<IOrderPayment>
{
    public OrderPaymentMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupGerarQRCodeParaPagamentoDePedido(string expectedReturn)
        => Setup(x => x.GerarQRCodeParaPagamentoDePedido(It.IsAny<PaymentEntity>()))
            .ReturnsAsync(expectedReturn);

    public void SetupExceptionGerarQRCodeParaPagamentoDePedido()
        => Setup(x => x.GerarQRCodeParaPagamentoDePedido(It.IsAny<PaymentEntity>()))
            .ThrowsAsync(new Exception());

    public void VerifyGerarQRCodeParaPagamentoDePedido(Times? times = null)
        => Verify(x => x.GerarQRCodeParaPagamentoDePedido(It.IsAny<PaymentEntity>()), times ?? Times.Once());
}

