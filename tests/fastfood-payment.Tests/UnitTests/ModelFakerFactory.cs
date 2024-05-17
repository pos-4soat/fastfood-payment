using AutoFixture;
using Bogus;

namespace fastfood_payment.Tests.UnitTests;

public class ModelFakerFactory(Fixture autoFixture, Faker faker)
{
    private readonly Fixture _autoFixture = autoFixture;
    private readonly Faker _faker = faker;

    public TRequest GenerateRequest<TRequest>()
        => _autoFixture.Build<TRequest>()
            .Create();

    public IEnumerable<TRequest> GenerateManyRequest<TRequest>()
        => _autoFixture.CreateMany<TRequest>();
}
