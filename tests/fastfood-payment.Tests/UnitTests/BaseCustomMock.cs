using Moq;

namespace fastfood_payment.Tests.UnitTests;

public abstract class BaseCustomMock<TInterface>(TestFixture testFixture) : Mock<TInterface> where TInterface : class
{
    public TestFixture TestFixture { get; } = testFixture;

    public Mock<TInterface> ConvertToBaseType()
    {
        return As<TInterface>();
    }
}
