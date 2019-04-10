using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterController;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Represents a request that can be tracked by
	/// a numeric ID.
	/// </summary>
	abstract class TrackableMessage : ITrackableMessage
	{
		/// <summary>
		/// Pools of request IDs to pull new unique values from.
		/// </summary>
		private static int IDPool { get; set; }

		/// <summary>
		/// The request ID used to track this request.
		/// </summary>
		public int RequestID { get; } = -1;

		/// <summary>
		/// The request message.
		/// </summary>
		public string Message { get; }
		
		/// <summary>
		/// Creates a <see cref="TrackableMessage"/> with
		/// a specific message.
		/// </summary>
		/// <param name="message">The message.</param>
		public TrackableMessage(string message)
		{
			RequestID = IDPool++;
			Message = message;
		}
	}
}
