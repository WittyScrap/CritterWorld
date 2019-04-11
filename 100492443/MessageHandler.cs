using System;
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
	class MessageHandler : IMessageSystem<ISimpleMessage>, IMessageSystem<ITrackableMessage>
	{
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
			Communicator(message.Compose());
		}

		/// <summary>
		/// Submits a trackable message to the CritterWorld
		/// enviroment.
		/// </summary>
		/// <param name="message">The message to be sent to the environment.</param>
		public void SendMessage(ITrackableMessage message)
		{
			Communicator(message.Compose());
		}

		/// <summary>
		/// Parses a message string into a message object.
		/// </summary>
		/// <param name="message">The message string.</param>
		/// <returns>A message object derived from the string.</returns>
		ITrackableMessage IMessageReceiver<ITrackableMessage>.ParseMessage(string message)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Parses a message string into a message object.
		/// </summary>
		/// <param name="message">The message string.</param>
		/// <returns>A message object derived from the string.</returns>
		ISimpleMessage IMessageReceiver<ISimpleMessage>.ParseMessage(string message)
		{
			throw new NotImplementedException();
		}
	}
}
