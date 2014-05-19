using System.Collections.Specialized;
using System.Configuration;

namespace MassTransit.Host.RabbitMQ
{
	internal class ServiceBusConfig
	{
		public string EndpointAddress { get; set; }

		public static ServiceBusConfig LoadFromConfig()
		{
			var configSection = ConfigurationManager.GetSection("massTransitServiceBusConfig") as NameValueCollection;
			if (configSection == null)
			{
				throw new ConfigurationErrorsException("massTransitServiceBusConfig section is missing");
			}
			var endpointAddress = configSection["endpointAddress"];
			if (string.IsNullOrWhiteSpace(endpointAddress))
			{
				throw new ConfigurationErrorsException("massTransitServiceBusConfig.endpoint setting is missing");
			}
			return new ServiceBusConfig { EndpointAddress = endpointAddress };
		}
	}
}
