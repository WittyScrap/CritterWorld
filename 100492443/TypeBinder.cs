using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Binds a header string to a specific message
	/// type.
	/// </summary>
	class TypeBinder : ConcurrentDictionary<string, Type>
	{
		/// <summary>
		/// Parses an input message into an instance
		/// of <typeparamref name="TMessage"/>.
		/// </summary>
		/// <param name="inputMessage">The formatted input message.</param>
		public IMessage ParseMessage(string message)
		{
			string header = ExtractHeader(message);
			IMessage parsedMessage = (IMessage)Activator.CreateInstance(base[header]);
			parsedMessage.FromString(message);
			return parsedMessage;
		}

		/// <summary>
		/// Attempts to extract the header from a
		/// message.
		/// </summary>
		/// <param name="message">The full message containing the header.</param>
		/// <returns>The header from the message.</returns>
		public static string ExtractHeader(string message)
		{
			string[] split = message.Split('\n', ':');
			return split[0];
		}
	}
}
