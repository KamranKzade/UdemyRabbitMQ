using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Drawing;
using System.Text;
using System.Text.Json;
using UdemyRabbitMQWeb.Watermark.Services;

namespace UdemyRabbitMQWeb.Watermark.BackgroundServices;


public class ImageWatermarkProcessBackgroundService : BackgroundService
{
	private IModel _channel;
	private readonly RabbitMQClientService _rabbitMQClientService;
	private readonly ILogger<ImageWatermarkProcessBackgroundService> _logger;


	public ImageWatermarkProcessBackgroundService(RabbitMQClientService rabbitMQClientService, ILogger<ImageWatermarkProcessBackgroundService> logger)
	{
		_rabbitMQClientService = rabbitMQClientService;
		_logger = logger;
	}

	public override Task StartAsync(CancellationToken cancellationToken)
	{
		// Kanalda, RabbitMQ ile elaqe yaradiriq 
		_channel = _rabbitMQClientService.Connect();

		// Kanalda datalarin nece geleceyini gosteririk
		_channel.BasicQos(0, 1, false);

		return base.StartAsync(cancellationToken);
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var consumer = new AsyncEventingBasicConsumer(_channel);
		_channel.BasicConsume(queue: RabbitMQClientService.QueueName, autoAck: false, consumer: consumer);

		consumer.Received += Consumer_Received;

		return Task.CompletedTask;
	}

	private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
	{

		try
		{
			var productImageCreatedEvent = JsonSerializer.Deserialize<productImageCreatedEvent>
												(Encoding.UTF8.GetString(@event.Body.ToArray()));

			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", productImageCreatedEvent.ImageName);

			var siteName = "www.mysite.com";
			using var img = Image.FromFile(path);
			using var graphic = Graphics.FromImage(img);
			var font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Bold, GraphicsUnit.Pixel);
			var textSize = graphic.MeasureString(siteName, font);

			var color = Color.FromArgb(128, 255, 255, 255);
			var brush = new SolidBrush(color);

			var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));

			graphic.DrawString(siteName, font, brush, position);

			img.Save("www.root/images/watermarks/" + productImageCreatedEvent.ImageName);

			img.Dispose();
			graphic.Dispose();
			_channel.BasicAck(@event.DeliveryTag, false);
		}
		catch (Exception ex)
		{
			throw;
		}

		return Task.CompletedTask;
	}

	public override Task StopAsync(CancellationToken cancellationToken)
	{
		return base.StopAsync(cancellationToken);
	}
}
