using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MassTransit;
using System;
using System.IO;
using System.Reflection;

namespace MassTransit.Host.RabbitMQ
{
	internal class ServiceHost
	{
		private IServiceBus _bus;

		/// <summary>
		/// The directory this program is executing from.
		/// </summary>
		public static string AssemblyDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		/// <summary>
		/// Code the executes when the service host should start
		/// </summary>
		public void Start()
		{
			try
			{
				var busConfig = ServiceBusConfig.LoadFromConfig();

				var container = new WindsorContainer().Install(FromAssembly.This());
				RegisterTypes(container);

				_bus = ServiceBusFactory.New(sbc =>
				{
					sbc.UseRabbitMq();
					sbc.ReceiveFrom(busConfig.EndpointAddress);
					sbc.Subscribe(x => x.LoadFrom(container));
				});
			}
			catch (Exception e)
			{
				// need something better here
				Console.Write(e.ToString());
			}
		}

		/// <summary>
		/// Code that executes when the service shuts down.
		/// </summary>
		public void Stop()
		{
			_bus.Dispose();
		}

		/// <summary>
		/// Load the MassTransit handlers into the container.
		/// </summary>
		/// <param name="container">The Windsor type container to add types to.</param>
		private static void RegisterTypes(IWindsorContainer container)
		{
			container
				.Register(Types
					.FromAssemblyInDirectory(new AssemblyFilter(AssemblyDirectory))
				//	.IncludeNonPublicTypes()
					.BasedOn<IConsumer>()
					.Unless(t => t.Namespace != null && t.Namespace.StartsWith("MassTransit", StringComparison.OrdinalIgnoreCase)));
		}
	}
}
