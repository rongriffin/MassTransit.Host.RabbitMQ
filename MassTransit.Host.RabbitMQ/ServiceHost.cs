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
using MassTransit;
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
		private IServiceBus _bus;

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
				// Need something better here.  NLog maybe?
				Console.Write(e.ToString());
			}
		}

		public void Stop()
		{
			_bus.Dispose();
		}

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
