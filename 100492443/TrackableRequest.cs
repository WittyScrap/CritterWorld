using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterController;

namespace _100492443.Critters.AI
{
	/// <summary>
	/// Represents a request that can be tracked by
	/// a numeric ID.
	/// </summary>
	abstract class TrackableRequest
	{
		/// <summary>
		/// Event called when this request has been resolved.
		/// </summary>
		public event EventHandler<string> Resolved;

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
		/// Submits the message as-is through the provided
		/// sender.
		/// </summary>
		/// <param name="sender">The sender delegate object.</param>
		public void Submit(Send sender)
		{
			sender(Message + ":" + RequestID.ToString());
		}

		/// <summary>
		/// Creates a <see cref="TrackableRequest"/> with
		/// a specific message.
		/// </summary>
		/// <param name="message">The message.</param>
		public TrackableRequest(string message)
		{
			RequestID = IDPool++;
			Message = message;
		}
		
		/// <summary>
		/// Resolves the current trackable request.
		/// </summary>
		public virtual void Resolve(string responseMessage)
		{
			Resolved?.Invoke(this, responseMessage);
		}
	}
}
