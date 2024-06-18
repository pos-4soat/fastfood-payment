using AutoFixture;
using AutoMapper;
using Bogus;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Domain.Contracts.Email;
using fastfood_payment.Domain.Contracts.Mongo;
using fastfood_payment.Domain.Contracts.Payment;
using fastfood_payment.Domain.Contracts.RabbitMq;
using fastfood_payment.Tests.Mocks;
using Moq;
using Moq.AutoMock;

namespace fastfood_payment.Tests.UnitTests;

[TestFixture]
public abstract class TestFixture
{
    public Fixture AutoFixture { get; init; } = new();
    public Faker Faker { get; init; } = new();

    protected PaymentRepositoryMock _repositoryMock;
    protected OrderPaymentMock _orderPaymentMock;
    protected ConsumerServiceMock _consumerMock;
    protected EmailClientMock _emailMock;
    protected IMapper _mapper;

    protected ModelFakerFactory _modelFakerFactory;
    protected AutoMocker _autoMocker;

    [OneTimeSetUp]
    public void GlobalPrepare()
    {
        _autoMocker = new();
        _modelFakerFactory = new(AutoFixture, Faker);
    }

    [SetUp]
    public async Task SetUpAsync()
    {
        AddCustomMocksToContainer();
        InstantiateCustomMocks();
        CreateMapper();
    }

    [TearDown]
    public void TearDown()
    {
        foreach (KeyValuePair<Type, object?> resolvedObject in _autoMocker.ResolvedObjects)
            (resolvedObject.Value as Mock)?.Invocations.Clear();
    }

    protected T CreateInstance<T>() where T : class
        => _autoMocker.CreateInstance<T>();

    protected TCustomMock GetCustomMock<TInterface, TCustomMock>() where TCustomMock : BaseCustomMock<TInterface> where TInterface : class
        => (_autoMocker.GetMock<TInterface>() as TCustomMock)!;

    protected Mock<T> GetMock<T>() where T : class
        => _autoMocker.GetMock<T>();

    #region Private Methods

    private void AddCustomMocksToContainer()
    {
        _autoMocker.Use(new PaymentRepositoryMock(this).ConvertToBaseType());
        _autoMocker.Use(new OrderPaymentMock(this).ConvertToBaseType());
        _autoMocker.Use(new ConsumerServiceMock(this).ConvertToBaseType());
        _autoMocker.Use(new EmailClientMock(this).ConvertToBaseType());
    }

    private void InstantiateCustomMocks()
    {
        _repositoryMock = GetCustomMock<IPaymentRepository, PaymentRepositoryMock>();
        _orderPaymentMock = GetCustomMock<IOrderPayment, OrderPaymentMock>();
        _consumerMock = GetCustomMock<IConsumerService, ConsumerServiceMock>();
        _emailMock = GetCustomMock<IEmailClient, EmailClientMock>();
    }

    private void CreateMapper()
    {
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Result).Assembly));

        _mapper = config.CreateMapper();
    }
    #endregion Private Methods
}
