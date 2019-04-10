using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterController;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Represents a sent message request.
	/// </summary>
	abstract class SimpleMessage : IMessage
	{
		/// <summary>
		/// The string based message to bundle with the message.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Submits the message as-is through the provided
		/// sender.
		/// </summary>
		/// <param name="sender">The sender delegate object.</param>
		public void Submit(Send sender)
		{
			sender(Message);
		}

		/// <summary>
		/// Generate a simple request with a specific message.
		/// </summary>
		/// <param name="message">The request message.</param>
		public SimpleMessage(string message)
		{
			Message = message;
		}
	}
}
