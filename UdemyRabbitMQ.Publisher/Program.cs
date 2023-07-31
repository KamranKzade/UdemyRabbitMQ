// using System.Text;
// using RabbitMQ.Client;

// class Program
// {
// 	static void Main(string[] args)
// 	{
// 		// RabbitMQ ile baglanti yaratmaq ucun lazim olan class
// 		var factory = new ConnectionFactory();

// 		// RabbitMQ de olan uri bura yaziriq 
// 		factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");

// 		// RabbitMq ile baglanti yaradiriq
// 		using var connection = factory.CreateConnection();

// 		// Rabbit ile elaqe ucun kanal yaradiriq
// 		var channel = connection.CreateModel();


// 		// Rabbit mq icerisinde queue yaradiriq, false bu queue -lar ramda saxlanir, true olsa Hdd ve ya SSd de 
// 		// "hello-queue"  --> queue -in adidi
// 		// true  --> Queuenin ramda yoxsa daimi yaddasda saxlanmasidir (true -> Daimi yaddas)
// 		// false --> Subscripe kimi 1 adam qosulacaqsa false, 2+ subscripe olacaqsa true yazriq
// 		// false --> Subscripe -in muracieti biten kimi datalarin queue dan silinmesi ( false )

// 		// 1 Dene Queue uzerinden data gondermek 
// 		{
// 			// channel.QueueDeclare("hello-queue", true, false, false);
// 			// 
// 			// string message = "Hello World";
// 			// 
// 			// var messageBody = Encoding.UTF8.GetBytes(message);
// 			// 
// 			// // Kanalda data gondermek 
// 			// // string.Empty  --> Exchange-in olmamasidir
// 			// // "hello-queue" --> hansi queue ile gonderirikse onun adini yaziriq
// 			// // null --> gelecek dersde baxacayiq
// 			// // messageBody   --> gonderilen message
// 			// 
// 			// channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
// 			// 
// 			// Console.WriteLine("Message gonderilmisdir");
// 		}

// 		// Eyni anda queue uzerinde 50 data gondermek
// 		// channel.QueueDeclare("hello-queue", true, false, false);

// 		// Fanout Exchange ile data gondermek
// 		channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

// 		Enumerable.Range(1, 50).ToList().ForEach(x =>
// 		{

// 			string message = $"log {x}";

// 			var messageBody = Encoding.UTF8.GetBytes(message);
// 			{
// 				// Kanalda data gondermek 
// 				// string.Empty  --> Exchange-in olmamasidir
// 				// "hello-queue" --> hansi queue ile gonderirikse onun adini yaziriq
// 				// null --> gelecek dersde baxacayiq
// 				// messageBody   --> gonderilen message
// 				// channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
// 			}

// 			// Fanout Exchange elave etmek 
// 			channel.BasicPublish("logs-fanout", "", null, messageBody);

// 			Console.WriteLine($"Message gonderilmisdir: {x}");
// 		});

// 		Console.ReadLine();
// 	}
// }


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//  using System.Text;
//  using RabbitMQ.Client;


//  class Program
//  {
//      static void Main(string[] args)
//      {
//          var factory = new ConnectionFactory();
//          factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");

//          using var connection = factory.CreateConnection();

//          var channel = connection.CreateModel();

//          channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

//          Enumerable.Range(1, 50).ToList().ForEach(x =>
//          {

//              string message = $"log {x}";

//              var messageBody = Encoding.UTF8.GetBytes(message);

//              channel.BasicPublish("logs-fanout", "", null, messageBody);

//              Console.WriteLine($"Mesaj gönderilmiştir : {message}");

//          });

//          Console.ReadLine();
//      }
//  }



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// using RabbitMQ.Client;
// using System.Text;
// 
using RabbitMQ.Client;
using System.Text;
/// Fanout Exchange
/// Direct Exchange
public enum LogNames
{
	Critical = 1,
	Error = 2,
	Warning = 3,
	Info = 4
}
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
// 		channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);
// 
// 		Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
// 		{
// 			var routeKey = $"route-{x}";
// 			var queueName = $"direct-queue-{x}";
// 
// 			channel.QueueDeclare(queueName, true, false, false);
// 			channel.QueueBind(queueName, "logs-direct", routeKey, null);
// 
// 		});
// 
// 
// 		Enumerable.Range(1, 50).ToList().ForEach(x =>
// 		{
// 			LogNames log = (LogNames)new Random().Next(1, 5);
// 
// 			string message = $"log-type: {log}";
// 
// 			var messageBody = Encoding.UTF8.GetBytes(message);
// 
// 			var routeKey = $"route-{log}";
// 
// 			channel.BasicPublish("logs-direct", routeKey, null, messageBody);
// 
// 			Console.WriteLine($"Log gönderilmiştir : {message}");
// 
// 		});
// 
// 		Console.ReadLine();
// 	}
// }


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Topic Exchange



class Program
{
	static void Main(string[] args)
	{
		var factory = new ConnectionFactory();
		factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");

		using var connection = factory.CreateConnection();

		var channel = connection.CreateModel();

		channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);


		Enumerable.Range(1, 50).ToList().ForEach(x =>
		{
			Random rnd = new Random();
			LogNames log1 = (LogNames)rnd.Next(1, 5);
			LogNames log2 = (LogNames)rnd.Next(1, 5);
			LogNames log3 = (LogNames)rnd.Next(1, 5);

			var routeKey = $"{log1}.{log2}.{log3}";
			string message = $"log-type: {log1}-{log2}-{log3}";
			var messageBody = Encoding.UTF8.GetBytes(message);


			channel.BasicPublish("logs-topic", routeKey, null, messageBody);
			Console.WriteLine($"Log gönderilmiştir : {message}");
		});

		Console.ReadLine();
	}
}
