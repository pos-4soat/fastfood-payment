using fastfood_payment.Domain.Contracts.Email;
using fastfood_payment.Tests.UnitTests;
using Moq;

namespace fastfood_payment.Tests.Mocks;

public class EmailClientMock : BaseCustomMock<IEmailClient>
{
    public EmailClientMock(TestFixture testFixture) : base(testFixture)
    {
        SetupSendEmailAsync();
    }

    public void SetupSendEmailAsync()
        => Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

    public void VerifySendEmailAsync(Times? times = null)
        => Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), times ?? Times.Once());
}
