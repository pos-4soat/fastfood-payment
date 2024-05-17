using fastfood_payment.Domain.Contracts.Http;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Tests.UnitTests;
using Moq;

namespace fastfood_payment.Tests.Mocks;

public class OrderHttpClientMock : BaseCustomMock<IOrderHttpClient>
{
    public OrderHttpClientMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupGetOrderAmount(PaymentEntity expectedReturn)
        => Setup(x => x.GetOrderAmount(It.IsAny<PaymentEntity>(), default))
            .ReturnsAsync(expectedReturn);

    public void SetupGetOrderStatus(int expectedReturn)
        => Setup(x => x.GetOrderStatus(It.IsAny<int>(), default))
            .ReturnsAsync(expectedReturn);

    public void VerifyGetOrderAmount(Times? times = null)
        => Verify(x => x.GetOrderAmount(It.IsAny<PaymentEntity>(), default), times ?? Times.Once());

    public void VerifyGetOrderStatus(int orderId, Times? times = null)
        => Verify(x => x.GetOrderStatus(orderId, default), times ?? Times.Once());
}
