using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Resolves a IMessage of a specific type.
	/// </summary>
	class MessageResolver<TMessage> : IMessageReceiver<TMessage> where TMessage : class, IMessage
	{
		/// <summary>
		/// Call a compiled version of this
		/// delegate to obtain a new instance of a <typeparamref name="TMessage"/>.
		/// </summary>
		/// <returns>An instance of <typeparamref name="TMessage"/>.</returns>
		private delegate object InstanceCreator();

		/// <summary>
		/// Creates a new message object.
		/// </summary>
		private InstanceCreator NewMessage => CreateInstance();

		/// <summary>
		/// Creates an instance of a <typeparamref name="TMessage"/> type
		/// of object.
		/// </summary>
		/// <returns>A new <typeparamref name="TMessage"/>.</returns>
		private InstanceCreator CreateInstance()
		{
			Type messageType = typeof(TMessage);
			ConstructorInfo defaultConstructor = messageType.GetConstructor(Type.EmptyTypes);
			DynamicMethod dynamicMethod = new DynamicMethod("CreateInstance", messageType, Type.EmptyTypes, false);
			ILGenerator generator = dynamicMethod.GetILGenerator();
			generator.Emit(OpCodes.Nop);
			generator.Emit(OpCodes.Newobj, defaultConstructor);
			generator.Emit(OpCodes.Ret);
			return (InstanceCreator)dynamicMethod.CreateDelegate(typeof(InstanceCreator));
		}

		/// <summary>
		/// Parses an input message into an instance
		/// of <typeparamref name="TMessage"/>.
		/// </summary>
		/// <param name="inputMessage">The formatted input message.</param>
		public TMessage ParseMessage(string message)
		{
			TMessage parsedMessage = NewMessage() as TMessage;
			parsedMessage.FromString(message);
			return parsedMessage;
		}
	}
}