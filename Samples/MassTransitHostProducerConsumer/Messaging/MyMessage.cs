using System;
using Magnum;
using MassTransit;

namespace Messaging
{
	/// <summary>
	/// The message published by the producer and handled by the consumer.
	/// </summary>
	public class MyMessage : CorrelatedBy<Guid>
	{
		/// <summary>
		/// CombGuid is a db friendly guid (for ordering)
		/// </summary>
		private readonly Guid _correlationId = CombGuid.Generate();

		/// <summary>
		/// Property needed for correlation.
		/// </summary>
		public Guid CorrelationId
		{
			get { return _correlationId; }
		}

		public DateTime Timestamp { get; set; }

		public string Message { get; set; }
	}
}
