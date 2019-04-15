using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Binds a string header to a series of tracked messages.
	/// </summary>
	class MessageBinder<TMessage> : ConcurrentDictionary<int, Action<TMessage>> where TMessage : ITrackableMessage
	{
	}
}
