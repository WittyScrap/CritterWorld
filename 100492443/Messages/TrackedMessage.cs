using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Messages
{
	class TrackedMessage : SimpleMessage, ITrackedMessage
	{
		/// <summary>
		/// All tracked messages.
		/// </summary>
		private static ConcurrentDictionary<int, TrackedMessage> TrackedMessages { get; } = new ConcurrentDictionary<int, TrackedMessage>();

		/// <summary>
		/// The request ID for this message.
		/// </summary>
		public int RequestID { get; }

		/// <summary>
		/// Constructs a new tracked message.
		/// </summary>
		/// <param name="sourceMessage">The source formatted message.</param>
		public TrackedMessage(string sourceMessage) : base(sourceMessage)
		{
			RequestID = GetInteger(0);
		}
	}
}
