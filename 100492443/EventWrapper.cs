using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Event delegate for messages.
	/// </summary>
	/// <typeparam name="TMessage">Type of message.</typeparam>
	/// <param name="message">The message object.</param>
	public delegate void MessageEvent<TMessage>(TMessage message);

	/// <summary>
	/// Manages an internal event, can be added to collections.
	/// </summary>
	class EventWrapper<TMessage> where TMessage : IMessage
	{
		/// <summary>
		/// The internal event handler.
		/// </summary>
		private event MessageEvent<TMessage> InternalEvent;

		/// <summary>
		/// Creates a new event wrapper.
		/// </summary>
		/// <param name="initialEvents">List of initial events to bind to the wrapper.</param>
		public EventWrapper(params MessageEvent<TMessage>[] initialEvents)
		{
			foreach (var initialEvent in initialEvents)
			{
				InternalEvent += initialEvent;
			}
		}

		/// <summary>
		/// Invokes the internal event.
		/// </summary>
		/// <param name="args">The arguments for the event.</param>
		public void Invoke(TMessage args)
		{
			InternalEvent?.Invoke(args);
		}

		/// <summary>
		/// Adds a new handler to the internal event.
		/// </summary>
		/// <param name="lhs">The base class containing the internal event.</param>
		/// <param name="rhs">The new event handler.</param>
		public static EventWrapper<TMessage> operator+(EventWrapper<TMessage> lhs, MessageEvent<TMessage> rhs)
		{
			lhs.InternalEvent += rhs;
			return lhs;
		}

		/// <summary>
		/// Removes an event handler from the internal event.
		/// </summary>
		/// <param name="lhs">The base class containing the internal event.</param>
		/// <param name="rhs">The event handler.</param>
		public static EventWrapper<TMessage> operator-(EventWrapper<TMessage> lhs, MessageEvent<TMessage> rhs)
		{
			lhs.InternalEvent -= rhs;
			return lhs;
		}
	}
}
