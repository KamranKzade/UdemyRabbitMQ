using System.Text;
using RabbitMQ.Client;

class Program
{
	static void Main(string[] args)
	{
		// RabbitMQ ile baglanti yaratmaq ucun lazim olan class
		var factory = new ConnectionFactory();

		// RabbitMQ de olan uri bura yaziriq 
		factory.Uri = new Uri("amqps://fmlpbokz:f8hq9qko1fLnHM_dUC324pRqFNCwQsl2@moose.rmq.cloudamqp.com/fmlpbokz");

		// RabbitMq ile baglanti yaradiriq
		using var connection = factory.CreateConnection();

		// Rabbit ile elaqe ucun kanal yaradiriq
		var channel = connection.CreateModel();


		// Rabbit mq icerisinde queue yaradiriq, false bu queue -lar ramda saxlanir, true olsa Hdd ve ya SSd de 
		channel.QueueDeclare("hello-queue", true, false, false);

		string message = "Hello World";

		var messageBody = Encoding.UTF8.GetBytes(message);

		// Default Exchange 
		channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

		Console.WriteLine("Message gonderilmisdir");

		Console.ReadLine();
	}
}