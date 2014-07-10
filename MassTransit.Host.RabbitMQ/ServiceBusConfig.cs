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

using System.Collections.Specialized;
using System.Configuration;

namespace MassTransit.Host.RabbitMQ
{
	/// <summary>
	/// Loads the bus configuration data from the .config file.
	/// </summary>
	internal class ServiceBusConfig
	{
		public string EndpointAddress { get; set; }
		public string RabbitMqUserName { get; set; }
		public string RabbitMqPassword { get; set; }

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

			var rabbitMqUserName = configSection["rabbitMqUserName"];
			if (string.IsNullOrWhiteSpace(rabbitMqUserName))
			{
				throw new ConfigurationErrorsException("massTransitServiceBusConfig.rabbitMqUserName setting is missing");
			}

			var rabbitMqPassword = configSection["rabbitMqPassword"];
			if (string.IsNullOrWhiteSpace(rabbitMqPassword))
			{
				throw new ConfigurationErrorsException("massTransitServiceBusConfig.rabbitMqPassword setting is missing");
			}

			return new ServiceBusConfig
			{
				EndpointAddress = endpointAddress,
				RabbitMqUserName = rabbitMqUserName,
				RabbitMqPassword = rabbitMqPassword
			};
		}
	}
}
