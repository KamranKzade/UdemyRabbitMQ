using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCreateWorkerService.Services
{
	public class RabbitMQCilentService
	{
		private IModel _channel;
		private IConnection _connection;
		private readonly ConnectionFactory _connectionFactory;
		public static string QueueName = "queue-excel-file";


		private readonly ILogger<RabbitMQCilentService> _logger;


		public RabbitMQCilentService(ConnectionFactory connectionFactory, ILogger<RabbitMQCilentService> logger)
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
}
