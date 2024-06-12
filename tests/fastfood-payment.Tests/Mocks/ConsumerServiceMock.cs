using fastfood_payment.Domain.Contracts.RabbitMq;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Tests.UnitTests;
using Moq;

namespace fastfood_payment.Tests.Mocks;

public class ConsumerServiceMock : BaseCustomMock<IConsumerService>
{
    public ConsumerServiceMock(TestFixture testFixture) : base(testFixture)
    {
        SetupPublishProduction();
        SetupPublishOrder();
    }

    public void SetupPublishProduction()
        => Setup(x => x.PublishProduction(It.IsAny<PaymentEntity>()));

    public void SetupExceptionPublishProduction()
        => Setup(x => x.PublishProduction(It.IsAny<PaymentEntity>())).Throws(new Exception());

    public void SetupPublishOrder()
        => Setup(x => x.PublishOrder(It.IsAny<int>()));

    public void VerifyPublishProduction(Times? times = null)
        => Verify(x => x.PublishProduction(It.IsAny<PaymentEntity>()), times ?? Times.Once());

    public void VerifyPublishOrder(Times? times = null)
        => Verify(x => x.PublishOrder(It.IsAny<int>()), times ?? Times.Once());
}
