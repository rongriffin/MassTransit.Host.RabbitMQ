using MassTransit.Host.RabbitMQ.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MassTransit.Host.RabbitMQ.Tests
{
	/// <summary>
	/// Unit Tests for ServiceBusConfigTests
	/// </summary>
	[TestClass]
	public class ServiceBusConfigTests
	{

		/// <summary>
		/// Verify that loading the config from the app.config works successfully
		/// relies on this connection string in the app.config:
		///		<add name="Host.RabbitMQConnection" connectionString="endpoint=rabbitmq://test/test; userId=testUser; password=testPassword" />
		/// </summary>
		[TestMethod]
		public void ServiceBusConfigLoadFromConfigSuccess()
		{
			var result = ServiceBusConfig.LoadFromConfig();

			Assert.IsNotNull(result);
			Assert.AreEqual(result.HostAddress, "rabbitmq://test/test");
			Assert.AreEqual(result.QueueName, "test_queue");
			Assert.AreEqual(result.RabbitMqUserName, "testUser");
			Assert.AreEqual(result.RabbitMqPassword, "testPassword");
		}
	}
}
