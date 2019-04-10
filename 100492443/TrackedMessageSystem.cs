using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Handles tracked messages (messages with a request ID).
	/// </summary>
	class TrackedMessageSystem : IMessageSender<TrackableMessage>, IMessageReceiver<TrackableMessage>
	{
		/// <summary>
		/// Moves the message to the bottom of a queue
		/// for later submission.
		/// </summary>
		/// <param name="message">The message to be enqueued.</param>
		public void EnqueueMessage(TrackableMessage message)
		{

		}

		/// <summary>
		/// Handles parsing an incoming string formatted message
		/// into a <see cref="SimpleMessage" /> object.
		/// </summary>
		/// <param name="message">The string based message that was received from the environment.</param>
		/// <returns>The parsed message from the string.</returns>
		public void HandleMessage(string message)
		{

		}
	}
}
