// Copyright 2014 Ron Griffin, ...
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

using System;
using System.Collections.Specialized;
using System.Configuration;

namespace MassTransit.Host.RabbitMQ.Configuration
{
	/// <summary>
	/// Loads the bus configuration data from the .config file.
	/// </summary>
	internal class ServiceBusConfig
	{
		public string EndpointAddress { get; set; }
		public string QueueName { get; set; }
		public string RabbitMqUserName { get; set; }
		public string RabbitMqPassword { get; set; }

		public static ServiceBusConfig LoadFromConfig()
		{
			var connectionStringSetting = ConfigurationManager.ConnectionStrings["Host.RabbitMQConnection"];
			if (connectionStringSetting == null)
			{
				throw new Exception("Connection string named Host.RabbitMQConnection not found");
			}

			var config = RabbitMqConnectionStringParser.Parse(connectionStringSetting.ConnectionString);

			var endpointAddress = config.EndpointAddress;
			if (string.IsNullOrWhiteSpace(endpointAddress))
			{
				throw new ConfigurationErrorsException("RabbitMQ endpoint setting is missing from Host.RabbitMQConnection connection string");
			}

			var rabbitMqUserName = config.RabbitMqUserName;
			if (string.IsNullOrWhiteSpace(rabbitMqUserName))
			{
				throw new ConfigurationErrorsException("RabbitMQ userId setting is missing from Host.RabbitMQConnection connection string");
			}

			var rabbitMqPassword = config.RabbitMqPassword;
			if (string.IsNullOrWhiteSpace(rabbitMqPassword))
			{
				throw new ConfigurationErrorsException("RabbitMQ password setting is missing from Host.RabbitMQConnection connection string");
			}

			return config;
		}
	}
}
