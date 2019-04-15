using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Binds a string key to an action of a specific type.
	/// </summary>
	class ActionBinder<TMessage> : ConcurrentDictionary<string, EventWrapper<TMessage>> where TMessage : IMessage
	{
	}
}
