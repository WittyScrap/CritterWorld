using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Messages
{
	/// <summary>
	/// Represents the SCAN message in an object
	/// oriented form.
	/// </summary>
	public class ScanMessage : SeeMessage, ITrackedMessage
	{
		/// <summary>
		/// This message's request ID.
		/// </summary>
		public int RequestID { get; }

		/// <summary>
		/// Constructs a new SCAN message.
		/// </summary>
		/// <param name="sourceMessage">The source formatted message string.</param>
		public ScanMessage(string sourceMessage) : base(sourceMessage)
		{
			RequestID = GetInteger(0);
		}
	}
}
