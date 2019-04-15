using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterController;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Message handler for simple and tracked messages.
	/// </summary>
	class MessageHandler : IMessageSender<ISimpleMessage>, IMessageSender<ITrackableMessage>
	{
		/// <summary>
		/// Defines the type of request that
		/// needs to be resolved.
		/// </summary>
		public enum RequestType
		{
			ScanRequest,
			NormalRequest
		}

		/// <summary>
		/// Trackable callbacks monitor.
		/// </summary>
		private MessageBinder<ITrackableMessage> TrackableCallbacks => new MessageBinder<ITrackableMessage>();

		/// <summary>
		/// Relates a message header to a callback for simple
		/// parsed messages.
		/// </summary>
		public ActionBinder<ISimpleMessage> MethodBindings => new ActionBinder<ISimpleMessage>();

		/// <summary>
		/// Binds a header string to a type for parsing.
		/// </summary>
		public TypeBinder<IMessage> TypeBindings => new TypeBinder<IMessage>();

		/// <summary>
		/// Communicator used as a medium to send messages to the environment.
		/// </summary>
		public Send Communicator { get; set; }

		/// <summary>
		/// Submits a simple message to the CritterWorld
		/// enviromnent.
		/// </summary>
		/// <param name="message">The message to be sent to the enviromnent.</param>
		public void SendMessage(ISimpleMessage message)
		{
			string composedMessage = message.Compose();
			Communicator(composedMessage);
		}

		/// <summary>
		/// Submits a trackable message to the CritterWorld
		/// enviroment.
		/// </summary>
		/// <param name="message">The message to be sent to the environment.</param>
		public void SendMessage(ITrackableMessage message)
		{
			TrackableCallbacks[message.RequestID] = new EventWrapper<ITrackableMessage>(message.Callback);
			string composedMessage = message.Compose();
			Communicator(composedMessage);
		}

		/// <summary>
		/// Resolves an incoming simple message and
		/// invokes the correct methods.
		/// </summary>
		/// <param name="message">The string containing the incoming message.</param>
		public void ResolveMessage(string message)
		{
			string messageHeader = ExtractHeader(message);
			var parsedMessage = TypeBindings[messageHeader].ParseMessage(message);
			if (parsedMessage is ISimpleMessage)
			{
				ISimpleMessage simpleMessage = parsedMessage as ISimpleMessage;
				MethodBindings[messageHeader].Invoke(simpleMessage);
			}
			else if (parsedMessage is ITrackableMessage)
			{
				ITrackableMessage trackableMessage = parsedMessage as ITrackableMessage;
				TrackableCallbacks[trackableMessage.RequestID].Invoke(trackableMessage);
			}
			else
			{
				throw new CritterException("Invalid message type: " + parsedMessage.GetType() + ", " + message);
			}
		}

		/// <summary>
		/// Attempts to extract the header from a
		/// message.
		/// </summary>
		/// <param name="message">The full message containing the header.</param>
		/// <returns>The header from the message.</returns>
		private string ExtractHeader(string message)
		{
			string[] split = message.Split('\n', ':');
			return split[0];
		}
	}
}
