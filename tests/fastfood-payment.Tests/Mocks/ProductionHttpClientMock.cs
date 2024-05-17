using fastfood_payment.Domain.Contracts.Http;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Tests.UnitTests;
using Moq;

namespace fastfood_payment.Tests.Mocks;

public class ProductionHttpClientMock : BaseCustomMock<IProductionHttpClient>
{
    public ProductionHttpClientMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupRequestProduction(bool expectedReturn)
        => Setup(x => x.RequestProduction(It.IsAny<PaymentEntity>(), default))
            .ReturnsAsync(expectedReturn);

    public void VerifyRequestProduction(Times? times = null)
        => Verify(x => x.RequestProduction(It.IsAny<PaymentEntity>(), default), times ?? Times.Once());
}

