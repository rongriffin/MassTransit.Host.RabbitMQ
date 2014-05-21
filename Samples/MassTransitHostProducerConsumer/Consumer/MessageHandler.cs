
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
