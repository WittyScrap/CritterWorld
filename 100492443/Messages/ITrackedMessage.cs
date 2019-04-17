using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Messages
{
	/// <summary>
	/// Provides an additional layer to the
	/// standard message for basic tracking.
	/// </summary>
	interface ITrackedMessage : IMessage
	{
		/// <summary>
		/// The attached request ID.
		/// </summary>
		int RequestID { get; }
	}
}
