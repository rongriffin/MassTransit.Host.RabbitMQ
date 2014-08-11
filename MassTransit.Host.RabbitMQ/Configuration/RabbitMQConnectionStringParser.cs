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

using RabbitMQ.Client.Events;

namespace MassTransit.Host.RabbitMQ.Configuration
{
	/// <summary>
	/// Parses configuration for RabbitMQ into a <see cref="ServiceBusConfig"/>
	/// </summary>
	internal class RabbitMqConnectionStringParser
	{
		public static ServiceBusConfig Parse(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString)) return null;

			var config = new ServiceBusConfig();

			foreach (var setting in connectionString.Split(';'))
			{
				ParseSetting(setting, config);
			}

			return config;
		}

		private static void ParseSetting(string setting, ServiceBusConfig config)
		{
			var parts = setting.Split('=');
			if (parts.Length < 2)
			{
				return;
			}
			var settingName = parts[0].Trim().ToLowerInvariant();
			var settingValue  = setting.Substring(setting.IndexOf('=') + 1);
			switch (settingName)
			{
				case "endpoint" :
					config.EndpointAddress = settingValue;
					break;
				case "userid" :
					config.RabbitMqUserName = settingValue;
					break;
				case "password" :
					config.RabbitMqPassword = settingValue;
					break;
			}
		}

	}
}
