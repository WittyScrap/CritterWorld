using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Represents a trackable message.
	/// </summary>
	interface ITrackableMessage : IMessage
	{
		/// <summary>
		/// Unique request ID for messages.
		/// </summary>
		int RequestID { get; }
	}
}
