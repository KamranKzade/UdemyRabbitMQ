//  using System.Text;
//  using RabbitMQ.Client;
//  using RabbitMQ.Client.Events;

//  class Program
//  {
//  	static void Main(string[] args)
//  	{
//  		// RabbitMQ ile baglanti yaratmaq ucun lazim olan class
//  		var factory = new ConnectionFactory();

//  		// RabbitMQ de olan uri bura yaziriq 
//  		factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");

//  		// RabbitMq ile baglanti yaradiriq
//  		using var connection = factory.CreateConnection();

//  		// Rabbit ile elaqe ucun kanal yaradiriq
//  		var channel = connection.CreateModel();

//  		{
//  			// channel.QueueDeclare("hello-queue", true, false, false);

//  			// "hello-queue"  --> queue -in adidi
//  			// true  --> Queuenin ramda yoxsa daimi yaddasda saxlanmasidir (true -> Daimi yaddas)
//  			// false --> Subscripe kimi 1 adam qosulacaqsa false, 2+ subscripe olacaqsa true yazriq
//  			// false --> Subscripe -in muracieti biten kimi datalarin queue dan silinmesi ( false )

//  			// Elave xususiyyetler : 
//  			// Subscriber terefde yeniden queue yaradiriqsa, eger bele bir queue varsa error atmir, yox eger yoxdursa onda queue yaradir
//  			// ancaq diqqet etmeli oldugumuz yer ondan ibaretdir ki, her 2 queue-nunda gostericileri eyni olmalidir ( yeni QueueDeclare
//  			// funksiyasinin gostericileri eyni olmalidir )
//  		}

//  		// Subscriber-e datalarin nece nece geleceyini gosteririk (BasicQos)
//  		// 0 --> datalarin olcusu ile baglidir
//  		// 5 --> gelen datalarin sayi ile baglidir
//  		// false --> tek seferde 1 subscriber-e say qeder gelir(5) data,
//  		// yox true olarsa subscriber-lerin cemi sayi qeder, datalari bolub verir

//  		channel.BasicQos(0, 1, false);


//  		// Random Queue yaradiriq ve onu exchange bind edirik, Subscriber datalari kecennen sonra queue silinsin deye
//  		// Bind edirik, declare etmirik. Declare etsekdik queue axira kimi qalacaqdi, Subscriber silinmeyine baxmayaraq.
//  		var randomQueueName = channel.QueueDeclare().QueueName;
//  		channel.QueueBind(randomQueueName, "logs-fanout", "", null);


//  		// Nece subscriber varsa, datalara saya bolub gonderecek subscribere 
//  		// channel.BasicQos(0, 10, true);

//  		// Subscriber yaradiriq
//  		var subscireber = new EventingBasicConsumer(channel);

//  		// "hello-queue" --> queue-nin adi
//  		// true --> rabitMq-den data gonderilen kimi, subscriberden cavab gelmeden, o datani queuedan silir
//  		// subscriber --> subscriber-in adi
//  		// channel.BasicConsume("hello-queue", true, subscireber);

//  		// channel.BasicConsume("hello-queue", false, subscireber);

//          Console.WriteLine($"Loglar dinleniyer");

//          // Received --> Subscriber RabitMq-e muraciet edende bu event isleyir
//          subscireber.Received += (object sender, BasicDeliverEventArgs e) =>
//  		{
//  			var message = Encoding.UTF8.GetString(e.Body.ToArray());

//  			Console.WriteLine("Gelen Message: " + message);

//  			// Gonderilen Queue-den gelen datani silirik
//  			// e.DeliveryTag --> RabitMqden gelen data
//  			// bool multiple --> memory-de olub, ama subscriber in islemediyi data haqqinda melumati
//  			// rabitmq-e xbr vere
//  			Thread.Sleep(1500);
//  			channel.BasicAck(e.DeliveryTag, false);
//  		};


//  		Console.ReadLine();
//  	}
//  }



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Fanout Exchange


//  using System.Text;
//  using RabbitMQ.Client;
//  using RabbitMQ.Client.Events;
//  
//  class Program
//  {
//  	static void Main(string[] args)
//  	{
//  		var factory = new ConnectionFactory();
//  		factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");
//  
//  		using var connection = factory.CreateConnection();
//  
//  		var channel = connection.CreateModel();
//  
//  		var randomQueueName = channel.QueueDeclare().QueueName;
//  		channel.QueueBind(randomQueueName, "logs-fanout", "", null);
//  
//  
//  		channel.BasicQos(0, 1, false);
//  		var consumer = new EventingBasicConsumer(channel);
//  
//  		channel.BasicConsume(randomQueueName, false, consumer);
//  
//  		Console.WriteLine("Loglar dinleniyor...");
//  
//  		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
//  		{
//  			var message = Encoding.UTF8.GetString(e.Body.ToArray());
//  
//  			Thread.Sleep(1500);
//  			Console.WriteLine("Gelen Mesaj:" + message);
//  
//  			channel.BasicAck(e.DeliveryTag, false);
//  		};
//  
//  		Console.ReadLine();
//  	}
//  
//  }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Direct Exchange


// using System.Text;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// 
// class Program
// {
// 	static void Main(string[] args)
// 	{
// 		var factory = new ConnectionFactory();
// 		factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");
// 
// 		using var connection = factory.CreateConnection();
// 
// 		var channel = connection.CreateModel();
// 
// 
// 		channel.BasicQos(0, 1, false);
// 		var consumer = new EventingBasicConsumer(channel);
// 
// 		var queueName = "direct-queue-Critical";
// 		channel.BasicConsume(queueName, false, consumer);
// 
// 		Console.WriteLine("Loglar dinleniyor...");
// 
// 		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
// 		{
// 			var message = Encoding.UTF8.GetString(e.Body.ToArray());
// 
// 			Thread.Sleep(1500);
// 			Console.WriteLine("Gelen mesaj: " + message);
// 
// 			File.AppendAllText("log-critical.txt", message + "\n");
// 
// 			channel.BasicAck(e.DeliveryTag, false);
// 		};
// 
// 		Console.ReadLine();
// 	}
// }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Topic Exchange


//using System.Text;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

//class Program
//{
//	static void Main(string[] args)
//	{
//		var factory = new ConnectionFactory();
//		factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");

//		using var connection = factory.CreateConnection();
//		var channel = connection.CreateModel();
//		channel.BasicQos(0, 1, false);

//		var consumer = new EventingBasicConsumer(channel);

//		var queueName = channel.QueueDeclare().QueueName;
//		var routeKey = "*.Error.*";

//		// Yeni burda deyirikki, evveli Info olsun, axri ne olur olsun
//		// var routeKey = "Info.#";


//		channel.QueueBind(queueName, "logs-topic", routeKey);
//		channel.BasicConsume(queueName, false, consumer);


//		Console.WriteLine("Loglar dinleniyor...");

//		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
//		{
//			var message = Encoding.UTF8.GetString(e.Body.ToArray());

//			Thread.Sleep(1500);
//			Console.WriteLine("Gelen mesaj: " + message);

//			channel.BasicAck(e.DeliveryTag, false);
//		};
//		Console.ReadLine();
//	}
//}



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Header Exchange

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

class Program
{
	static void Main(string[] args)
	{
		var factory = new ConnectionFactory();
		factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");

		using var connection = factory.CreateConnection();
		var channel = connection.CreateModel();
		channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);
		channel.BasicQos(0, 1, false);

		var consumer = new EventingBasicConsumer(channel);

		var queueName = channel.QueueDeclare().QueueName;

		Dictionary<string, object> headers = new();

		headers.Add("format", "pdf");
		headers.Add("shape", "a4");
		// En azi 1i uygun olsa alacaq subscriber mesaji
		headers.Add("x-match", "any");
		// Hamsi uygun olsa alacaq subscriber mesaji
		// headers.Add("x-match", "all");

		channel.QueueBind(queueName, "header-exchange", string.Empty, headers);
		channel.BasicConsume(queueName, false, consumer);


		Console.WriteLine("Loglar dinleniyor...");

		consumer.Received += (object sender, BasicDeliverEventArgs e) =>
		{
			var message = Encoding.UTF8.GetString(e.Body.ToArray());

			Product product = JsonSerializer.Deserialize<Product>(message);

			Thread.Sleep(1500);
			Console.WriteLine($"Gelen mesaj: {product.Id}-{product.Name}-{product.Price}-{product.Stock}");

			channel.BasicAck(e.DeliveryTag, false);
		};
		Console.ReadLine();
	}
}