using AutoFixture;
using AutoMapper;
using Bogus;
using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Domain.Contracts.Http;
using fastfood_payment.Domain.Contracts.Mongo;
using fastfood_payment.Domain.Contracts.Payment;
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
    protected OrderHttpClientMock _orderHttpClientMock;
    protected ProductionHttpClientMock _productionHttpClientMock;
    protected OrderPaymentMock _orderPaymentMock;
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
        _autoMocker.Use(new OrderHttpClientMock(this).ConvertToBaseType());
        _autoMocker.Use(new ProductionHttpClientMock(this).ConvertToBaseType());
        _autoMocker.Use(new OrderPaymentMock(this).ConvertToBaseType());
    }

    private void InstantiateCustomMocks()
    {
        _repositoryMock = GetCustomMock<IPaymentRepository, PaymentRepositoryMock>();
        _orderHttpClientMock = GetCustomMock<IOrderHttpClient, OrderHttpClientMock>();
        _productionHttpClientMock = GetCustomMock<IProductionHttpClient, ProductionHttpClientMock>();
        _orderPaymentMock = GetCustomMock<IOrderPayment, OrderPaymentMock>();
    }

    private void CreateMapper()
    {
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Result).Assembly));

        _mapper = config.CreateMapper();
    }
    #endregion Private Methods
}
