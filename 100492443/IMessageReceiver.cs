using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Handles receiving messages from the CritterWorld
	/// environment.
	/// </summary>
	interface IMessageReceiver<TMessage> where TMessage : IMessage
	{
		/// <summary>
		/// Handles parsing an incoming string formatted message
		/// into a <typeparamref name="TMessage"/> object.
		/// </summary>
		/// <param name="message">The string based message that was received from the environment.</param>
		/// <returns>The parsed message from the string.</returns>
		TMessage ParseMessage(string message);
	}
}
