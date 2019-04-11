using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Represents a messaging system that handles both incoming and
	/// outgoing messages.
	/// </summary>
	interface IMessageSystem<TMessage> : IMessageSender<TMessage>, IMessageReceiver<TMessage> where TMessage : IMessage
	{ }
}
