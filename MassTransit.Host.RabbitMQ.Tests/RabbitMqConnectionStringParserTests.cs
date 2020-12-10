using MassTransit.Host.RabbitMQ.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MassTransit.Host.RabbitMQ.Tests
{
	/// <summary>
	/// Unit test cases for the RabbitMqConnectionStringParser class.
	/// </summary>
	[TestClass]
	public class RabbitMqConnectionStringParserTests
	{
		private const string ConectionStringFormat = "endpoint={0}; userid={1}; password={2}; queueName={3};something=";

		/// <summary>
		/// Test a well-formed connection string
		/// </summary>
		[TestMethod]
		public void RabbitMqConnectionStringParserSuccess()
		{
			const string endpoint = "rabbitmq://test/test";
			const string userid = "testUser";
			const string password = "testPassw0rd1";
			const string queueName = "testQueueName";
			var connectionString = string.Format(ConectionStringFormat, endpoint, userid, password, queueName);
			var result = RabbitMqConnectionStringParser.Parse(connectionString);

			Assert.IsNotNull(result);
			Assert.AreEqual(endpoint, result.HostAddress);
			Assert.AreEqual(userid, result.RabbitMqUserName);
			Assert.AreEqual(password, result.RabbitMqPassword);
			Assert.AreEqual(queueName, result.QueueName);
		}

		/// <summary>
		/// Test what happens when an '=' is in the connection string values
		/// </summary>
		[TestMethod]
		public void RabbitMqConnectionStringParserValuesHaveEqualSign()
		{
			const string endpoint = "rabbitmq://test/test";
			const string userid = "test=User";
			const string password = "test=Passw0rd=1";
			const string queueName = "queuName";
			var connectionString = string.Format(ConectionStringFormat, endpoint, userid, password, queueName);
			var result = RabbitMqConnectionStringParser.Parse(connectionString);

			Assert.IsNotNull(result);
			Assert.AreEqual(endpoint, result.HostAddress);
			Assert.AreEqual(userid, result.RabbitMqUserName);
			Assert.AreEqual(password, result.RabbitMqPassword);
			Assert.AreEqual(queueName, result.QueueName);
		}

		/// <summary>
		/// Parser should return null if connection string is null
		/// </summary>
		[TestMethod]
		public void RabbitMqConnectionStringParserReturnsNullWithNullString()
		{
			Assert.IsNull(RabbitMqConnectionStringParser.Parse(null));
		}

		/// <summary>
		/// Parser should return null if connection string is empty.
		/// </summary>
		[TestMethod]
		public void RabbitMqConnectionStringParserReturnsNullWithEmptyString()
		{
			Assert.IsNull(RabbitMqConnectionStringParser.Parse(string.Empty));
		}

		/// <summary>
		/// If connection string doesn't have recognized values, still return successfully
		/// </summary>
		[TestMethod]
		public void RabbitMqConnectionStringParserReturnsWhenValuesAreMissing()
		{
			const string badConectionStringFormat = "badendpoint={0}; baduserid={1}; badpassword={2}; badqueuename={3}";
			const string endpoint = "rabbitmq://test/test";
			const string userid = "testUser";
			const string password = "testPassw0rd1";
			const string queueName = "queuename";
			var connectionString = string.Format(badConectionStringFormat, endpoint, userid, password, queueName);
			var result = RabbitMqConnectionStringParser.Parse(connectionString);

			Assert.IsNotNull(result);
			Assert.IsNull(result.HostAddress);
			Assert.IsNull(result.RabbitMqUserName);
			Assert.IsNull(result.RabbitMqPassword);
			Assert.IsNull(result.QueueName);
		}
	}
}
