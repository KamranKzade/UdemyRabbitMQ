using System;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace UdemyRabbitMQWeb.Watermark.Services;

public class RabbitMQClientService : IDisposable
{
	private IModel _channel;
	private IConnection _connection;
	private readonly ConnectionFactory _connectionFactory;
	public static string ExchangeName = "ImageDirectExchange";
	public static string RoutingWaterMark = "watermark-route-image";
	public static string QueueName = "queue-watermark-image";


	private readonly ILogger<RabbitMQClientService> _logger;


	public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
	{
		_connectionFactory = connectionFactory;
		_logger = logger;
	}

	public IModel Connect()
	{
		// Elaqeni yaradiriq
		_connection = _connectionFactory.CreateConnection();

		if (_channel is { IsOpen: true })
		{
			return _channel;
		}

		// Modeli yaradiriq
		_channel = _connection.CreateModel();

		// Exchange-i yaradiriq
		_channel.ExchangeDeclare(ExchangeName, type: "direct", durable: true, autoDelete: false);

		// Queue -i yaradiriq
		_channel.QueueDeclare(QueueName, durable: true, false, false, null);

		// Queue-ni bind edirik
		_channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWaterMark);

		// Log-a informasiyani yaziriq
		_logger.LogInformation("RabbitMQ ile elaqe kuruldu...");

		return _channel;
	}

	public void Dispose()
	{
		// Kanali baglayiriq
		_channel?.Close();
		_channel?.Dispose();

		// Connectioni baglayiriq
		_connection?.Close();
		_connection?.Dispose();

		// Loga melumati yaziriq
		_logger.LogInformation("RabbitMQ ile baglanti kopdu...");
	}
}
