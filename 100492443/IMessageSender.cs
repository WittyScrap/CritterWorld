using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Handles sending messages to the
	/// CritterWorld environment.
	/// </summary>
	interface IMessageSender<TMessage> where TMessage : IMessage
	{
		/// <summary>
		/// Moves the message to the bottom of a queue
		/// for later submission.
		/// </summary>
		/// <param name="message">The message to be enqueued.</param>
		void EnqueueMessage(TMessage message);
	}
}
