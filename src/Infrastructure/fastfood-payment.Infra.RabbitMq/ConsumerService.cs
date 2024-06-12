using fastfood_payment.Application.UseCases.CreatePayment;
using fastfood_payment.Domain.Contracts.RabbitMq;
using fastfood_payment.Domain.Entity;
using fastfood_payment.Infra.RabbitMq.Message;
using fastfood_payment.Infra.RabbitMq.Settings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace fastfood_payment.Infra.RabbitMq;

[ExcludeFromCodeCoverage]
public class ConsumerService : IConsumerService, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private IConnection _connection;
    private IModel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ConsumerService(IOptions<RabbitMqSettings> options, IServiceScopeFactory serviceScopeFactory)
    {
        _settings = options.Value;
        _serviceScopeFactory = serviceScopeFactory;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        ConnectionFactory factory = new()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _settings.PaymentQueueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void StartConsuming()
    {
        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            string response = ProcessMessage(message);

            IBasicProperties replyProperties = _channel.CreateBasicProperties();
            replyProperties.CorrelationId = ea.BasicProperties.CorrelationId;

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: ea.BasicProperties.ReplyTo,
                basicProperties: replyProperties,
                body: responseBytes);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _settings.PaymentQueueName,
                             autoAck: false,
                             consumer: consumer);
    }

    public void PublishOrder(int orderId)
    {
        OrderMessage message = new(orderId, 6);

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        IBasicProperties properties = _channel.CreateBasicProperties();
        properties.DeliveryMode = 2;

        _channel.BasicPublish(exchange: string.Empty,
                             routingKey: _settings.OrderQueueName,
                             basicProperties: properties,
                             body: body);
    }

    public void PublishProduction(PaymentEntity paymentEntity)
    {
        ProductionMessage message = new(paymentEntity);

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        IBasicProperties properties = _channel.CreateBasicProperties();
        properties.DeliveryMode = 2;

        _channel.BasicPublish(exchange: string.Empty,
                             routingKey: _settings.ProductionQueueName,
                             basicProperties: properties,
                             body: body);
    }

    private string ProcessMessage(string message)
    {
        CreatePaymentRequest? request = JsonSerializer.Deserialize<CreatePaymentRequest>(message);

        if (request == null)
            return string.Empty;

        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            Application.Shared.BaseResponse.Result<CreatePaymentResponse> response = mediator.Send(request, default).Result;
            return response.IsFailure ? string.Empty : response.Value.PaymentQrCode;
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}