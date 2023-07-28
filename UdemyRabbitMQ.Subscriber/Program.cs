using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


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
		// Subscriber terefde yeniden queue yaradiriqsa, eger bele bir queue varsa error atmir, yox eger yoxdursa onda queue yaradir
		// ancaq diqqet etmeli oldugumuz yer ondan ibaretdir ki, her 2 queue-nunda gostericileri eyni olmalidir ( yeni QueueDeclare
		// funksiyasinin gostericileri eyni olmalidir )

		// channel.QueueDeclare("hello-queue", true, false, false);

		var subscireber = new EventingBasicConsumer(channel);


		channel.BasicConsume("hello-queue", true, subscireber);

		// Dinlemek ucun istifade olunur
		subscireber.Received += (object sender, BasicDeliverEventArgs e) =>
		{
			var message = Encoding.UTF8.GetString(e.Body.ToArray());

			Console.WriteLine("Gelen Message: " + message);
		};

		Console.ReadLine();
	}
}