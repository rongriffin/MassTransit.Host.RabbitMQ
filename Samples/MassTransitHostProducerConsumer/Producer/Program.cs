using MassTransit;
using MassTransit.Advanced;
using Messaging;
using System;

namespace Producer
{
	/// <summary>
	/// Program that publishes messages across the rabbitmq bus.
	/// </summary>
	class Program
	{
		/// <summary>
		/// intialized the bus to use rabbitmq.
		/// </summary>
		private static readonly IServiceBus Bus =
			ServiceBusFactory.New(sbc =>
			{
				sbc.UseRabbitMq();
				sbc.UseJsonSerializer();
				sbc.SetConcurrentReceiverLimit(Environment.ProcessorCount * 2);
				sbc.ReceiveFrom("rabbitmq://localhost/HostProducer"); 
			});

		static void Main()
		{
			while (true)
			{
				Console.WriteLine("Press enter to send a message...");
				Console.ReadLine();

				Bus.Publish<MyMessage>(new MyMessage()
				{
					Timestamp = DateTime.Now,
					Message = "Message from producer"
				});
			}
		}
	}
}
