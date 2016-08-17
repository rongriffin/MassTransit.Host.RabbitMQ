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

using MassTransit;
using Messaging;
using System;

namespace Producer
{
	/// <summary>
	/// Program that publishes messages across the rabbitmq bus.
	/// </summary>
	class Program
	{
		/// <summary>
		/// intialized the bus to use rabbitmq.
		/// </summary>
		private static readonly IBusControl Bus =
			MassTransit.Bus.Factory.CreateUsingRabbitMq(sbc =>
			{
				
				sbc.Host(new Uri("rabbitmq://localhost"), hostConfigurator =>
				{
					hostConfigurator.Username("guest");
					hostConfigurator.Password("guest");
					hostConfigurator.Heartbeat(10);
				});
			});
			
		static void Main()
		{
			while (true)
			{
				Console.WriteLine("Press enter to send a message...");
				Console.ReadLine();

				Bus.Publish(new MyMessage
				{
					Timestamp = DateTime.Now,
					Message = "Message from producer"
				});
			}
		}
	}
}
