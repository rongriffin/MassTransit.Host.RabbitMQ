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
using Magnum.Extensions;
using Topshelf;

namespace MassTransit.Host.RabbitMQ
{
	/// <summary>
	/// Entry point of the host process used to construct the Topshelf service.
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			var exitCode = HostFactory.Run(x =>
			{
				x.Service<ServiceHost>(s =>
				{
					s.ConstructUsing(name => new ServiceHost());
					s.WhenStarted(tc => tc.Start());
					s.WhenStopped(tc => tc.Stop());
				});
				x.RunAsLocalSystem();
				
				x.SetDescription("MassTransit RabbitMQ Service Bus Host");
				x.SetDisplayName("MassTransitServiceBusHost");
				x.SetServiceName("MassTransitServiceBusHost");
			});
			//TODO: extra parameter stuff 

			Environment.Exit((int)exitCode);
		}
	}
}
