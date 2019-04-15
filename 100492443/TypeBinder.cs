using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Binds a header string to a specific message
	/// type.
	/// </summary>
	class TypeBinder<TMessage> : ConcurrentDictionary<string, MessageResolver<TMessage>> where TMessage : class, IMessage
	{ }
}
