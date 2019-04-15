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
	class ActionBinder<TMessage> : ConcurrentDictionary<string, Action<TMessage>> where TMessage : IMessage
	{
		/// <summary>
		/// This action is called when the specified key is not found in the dictionary.
		/// </summary>
		public Action<TMessage> DefaultAction { get; set; } = message => { };

		public new Action<TMessage> this[string header] {
			get
			{
				if (!ContainsKey(header))
				{
					return DefaultAction;
				}
				return base[header];
			}
			set => base[header] = value;
		}
	}
}
