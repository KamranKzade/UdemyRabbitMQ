using Shared;
using System;
using System.IO;
using System.Data;
using System.Text;
using System.Linq;
using System.Net.Http;
using ClosedXML.Excel;
using RabbitMQ.Client;
using System.Text.Json;
using System.Threading;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FileCreateWorkerService.Models;
using FileCreateWorkerService.Services;
using Microsoft.Extensions.DependencyInjection;


namespace FileCreateWorkerService
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly RabbitMQCilentService _rabbitMQCilentService;
		private readonly IServiceProvider _serviceProvider;
		private IModel _channel;


		public Worker(ILogger<Worker> logger, RabbitMQCilentService rabbitMQCilentService, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_rabbitMQCilentService = rabbitMQCilentService;
			_serviceProvider = serviceProvider;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_channel = _rabbitMQCilentService.Connect();
			_channel.BasicQos(0, 1, false);

			return base.StartAsync(cancellationToken);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new AsyncEventingBasicConsumer(_channel);

			consumer.Received += Consumer_Received;

			return Task.CompletedTask;
		}

		private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
		{
			await Task.Delay(5000);

			var CreateExcelMessage = JsonSerializer.Deserialize<CreateExcelMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));
			using var ms = new MemoryStream();

			var wb = new XLWorkbook();
			var ds = new DataSet();
			ds.Tables.Add(GetTable("products"));

			wb.Worksheets.Add(ds);
			wb.SaveAs(ms);


			// Datani FileControllerin Upload metoduna gondermek

			MultipartFormDataContent multipartFormDataContent = new();
			multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()), "file", Guid.NewGuid().ToString() + ".xlsx");


			var baseUrl = "https://localhost:44321/api/files";

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.PostAsync($"{baseUrl}?fileId={CreateExcelMessage.FileId}", multipartFormDataContent);

				if (response.IsSuccessStatusCode)
				{
					_logger.LogInformation($"File ( Id: {CreateExcelMessage.FileId}) was created by successful");
					_channel.BasicAck(@event.DeliveryTag, false);
				}
			}
		}

		private DataTable GetTable(string tableName)
		{
			List<FileCreateWorkerService.Models.Product> products;

			using (var scope = _serviceProvider.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<AdventureWorks2019Context>();

				products = context.Products.ToList();
			}


			DataTable table = new DataTable
			{
				TableName = tableName,
			};

			table.Columns.Add("ProductId", typeof(int));
			table.Columns.Add("Name", typeof(string));
			table.Columns.Add("ProductNumber", typeof(string));
			table.Columns.Add("Color", typeof(string));


			products.ForEach(x =>
			{
				table.Rows.Add(x.ProductId, x.Name, x.ProductNumber, x.Color);
			});

			return table;
		}
	}
}
