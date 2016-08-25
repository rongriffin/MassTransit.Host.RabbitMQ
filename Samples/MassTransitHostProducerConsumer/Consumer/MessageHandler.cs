
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
using System.Threading.Tasks;
using Magnum.Extensions;
using Messaging;
using MassTransit;

namespace Consumer
{
	/// <summary>
	/// Handles a message of type MyMessage that is received from the bus.
	/// </summary>
	public class MessageHandler : IConsumer<MyMessage>
	{

		public Task Consume(ConsumeContext<MyMessage> context)
		{

			var message = context.Message;

			return Console.Out.WriteLineAsync(
				$"Message recieved.\n\tid: {message.CorrelationId.Stringify()}\n\ttimestamp: {message.Timestamp.ToLongTimeString()}\n\tmessage: {message.Message}\n\n"
			);
		}
	}
}
