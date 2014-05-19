using Topshelf;

namespace MassTransit.Host.RabbitMQ
{
	class Program
	{
		static void Main(string[] args)
		{
			HostFactory.Run(x =>
			{
				x.Service<ServiceHost>(s =>
				{
					s.ConstructUsing(name => new ServiceHost());
					s.WhenStarted(tc => tc.Start());
					s.WhenStopped(tc => tc.Stop());
				});
				x.RunAsLocalSystem();
				
				x.SetDescription("MassTransit RabbitMQ Service Bus Host");
				x.SetDisplayName("MassTranitServiceBusHost");
				x.SetServiceName("MassTransitServiceBusHost");
			});
			 

			//TODO: extra parameter stuff here

		}
	}
}
