
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
using Messaging;
using MassTransit;

namespace Consumer
{
	/// <summary>
	/// Handles a message of type MyMessage that is received from the bus.
	/// </summary>
	public class MessageHandler : Consumes<MyMessage>.All
	{
		public void Consume(MyMessage message)
		{
			Console.WriteLine("Message recieved.\n\tid: {0}\n\ttimestamp: {1}\n\tmessage: {2}\n\n",
				message.CorrelationId.Stringify(),
				message.Timestamp.ToLongTimeString(),
				message.Message);
		}
	}
}
