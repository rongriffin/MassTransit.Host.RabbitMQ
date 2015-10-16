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

using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Magnum.Extensions;
using MassTransit;
using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Reflection;
using MassTransit.Host.RabbitMQ.Configuration;
using NLog;

namespace MassTransit.Host.RabbitMQ
{
	/// <summary>
	/// The Service Host creates and configures and instance of the bus and registers any consumers found
	/// as subscribers to the bus.
	/// </summary>
	internal class ServiceHost
	{
		private IServiceBus _bus;
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		internal static string AssemblyDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				var assemblyDir = Path.GetDirectoryName(path);
				logger.Trace("Loading subscribers from path " + assemblyDir);
				return assemblyDir;
			}
		}

		public void Start()
		{
			try
			{
				var busConfig = ServiceBusConfig.LoadFromConfig();

				var container = new WindsorContainer().Install(FromAssembly.This());
				RegisterTypes(container);
				
				logger.Trace(string.Format("Connecting to RabbitMQ at endpoint {0}", busConfig.EndpointAddress));
				_bus = ServiceBusFactory.New(sbc =>
				{
					sbc.UseRabbitMq(r => r.ConfigureHost(new Uri(busConfig.EndpointAddress), h =>
						{
							h.SetUsername(busConfig.RabbitMqUserName);
							h.SetPassword(busConfig.RabbitMqPassword);
						})
					);
					sbc.ReceiveFrom(busConfig.EndpointAddress);
					sbc.Subscribe(x => x.LoadFrom(container));
				});
			}
			catch (Exception e)
			{
				logger.Error(e.ToString);
			}
		}

		public void Stop()
		{
			if (_bus != null) _bus.Dispose();
		}

		internal static void RegisterTypes(IWindsorContainer container)
		{
			container
				.Register(Types
					.FromAssemblyInDirectory(new AssemblyFilter(AssemblyDirectory))
				//	.IncludeNonPublicTypes()
					.BasedOn<IConsumer>()	
					.Unless(t => t.Namespace != null && t.Namespace.StartsWith("MassTransit", StringComparison.OrdinalIgnoreCase)));

			LogRegisteredSubscribers(container);
			
		}

		internal static void LogRegisteredSubscribers(IWindsorContainer container)
		{
			if (container == null) return;

			foreach (var handler in container.Kernel.GetAssignableHandlers(typeof(object)))
			{
				logger.Trace(string.Format("Loaded subscriber {0} {1}",
					handler.ComponentModel.ComponentName.Name,
					handler.ComponentModel.Implementation));
			}
		}
	}
}
