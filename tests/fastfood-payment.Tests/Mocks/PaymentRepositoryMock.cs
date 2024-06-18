using fastfood_payment.Domain.Contracts.Mongo;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Tests.UnitTests;
using Moq;

namespace fastfood_payment.Tests.Mocks;

public class PaymentRepositoryMock : BaseCustomMock<IPaymentRepository>
{
    public PaymentRepositoryMock(TestFixture testFixture) : base(testFixture)
    {
        SetupInsertAsync();
        SetupUpdateAsync();
    }

    public void SetupFindAsync(PaymentEntity expectedReturn)
        => Setup(x => x.FindAsync(It.IsAny<int>(), default))
            .ReturnsAsync(expectedReturn);

    public void SetupInsertAsync()
        => Setup(x => x.InsertAsync(It.IsAny<PaymentEntity>(), default))
            .Returns(Task.CompletedTask);

    public void SetupUpdateAsync()
        => Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<bool>(), default))
            .Returns(Task.CompletedTask);

    public void VerifyFindAsync(Times? times = null)
        => Verify(x => x.FindAsync(It.IsAny<int>(), default), times ?? Times.Once());

    public void VerifyInsertAsync(Times? times = null)
        => Verify(x => x.InsertAsync(It.IsAny<PaymentEntity>(), default), times ?? Times.Once());

    public void VerifyUpdateAsync(int orderId, bool payed, Times? times = null)
        => Verify(x => x.UpdateAsync(orderId, payed, default), times ?? Times.Once());
}
