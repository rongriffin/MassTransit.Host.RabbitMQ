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

using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MassTransit.Host.RabbitMQ.Configuration;
using NLog;
using System;
using System.IO;
using System.Reflection;

namespace MassTransit.Host.RabbitMQ
{
    /// <summary>
    /// The Service Host creates and configures and instance of the bus and registers any consumers found
    /// as subscribers to the bus.
    /// </summary>
    internal class ServiceHost
	{
		private IBusControl _bus;
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		internal static string AssemblyDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				var assemblyDir = Path.GetDirectoryName(path);
				Logger.Trace("Loading subscribers from path " + assemblyDir);
				return assemblyDir;
			}
		}

		public void Start()
		{
			try
			{
				var busConfig = ServiceBusConfig.LoadFromConfig();

				// Find all consumers in the install directory
				var container = new WindsorContainer().Install(FromAssembly.This());				
				container.AddMassTransit(c =>
				{
					c.AddConsumersFromContainer(container);

					c.UsingRabbitMq((context, cfg) =>
					{
						Logger.Trace($"Connecting to RabbitMQ at endpoint {busConfig.HostAddress}");
						cfg.Host(busConfig.HostAddress, busConfig.Port, busConfig.VirtualHost ?? string.Empty, h =>
						{
							h.Username(busConfig.RabbitMqUserName);
							h.Password(busConfig.RabbitMqPassword);							
						});
						
						cfg.ReceiveEndpoint(busConfig.QueueName, endpoint => {
							endpoint.ConfigureConsumers(context);							
						});
						Logger.Trace($"Connecting to RabbitMQ queue {busConfig.QueueName}");

					});

				});

				_bus = container.Kernel.Resolve<IBusControl>();
				_bus.Start();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString);
				throw;
			}
		}

		public void Stop()
		{
			_bus?.Stop();
		}

		internal static void LogRegisteredSubscribers(IWindsorContainer container)
		{
			if (container == null) return;

			foreach (var handler in container.Kernel.GetAssignableHandlers(typeof(object)))
			{
				Logger.Trace(
					$"Loaded subscriber {handler.ComponentModel.ComponentName.Name} {handler.ComponentModel.Implementation}");
			}
		}
	}
}
