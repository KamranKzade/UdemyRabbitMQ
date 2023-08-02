using Shared;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;


namespace UdemyRabbitMQWeb.ExcelCreate.Services
{
	public class RabbitMQPublisher
	{
		private readonly RabbitMQClientService _rabbitmqClientService;

		public RabbitMQPublisher(RabbitMQClientService rabbitmqClientService)
		{
			_rabbitmqClientService = rabbitmqClientService;
		}

		public void Publish(CreateExcelMessage createExcelMessage)
		{
			var channel = _rabbitmqClientService.Connect();

			var bodyString = JsonSerializer.Serialize(createExcelMessage);
			var bodyByte = Encoding.UTF8.GetBytes(bodyString);

			var property = channel.CreateBasicProperties();
			property.Persistent = true;

			channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingExcel, basicProperties: property, body: bodyByte);
		}
	}
}
