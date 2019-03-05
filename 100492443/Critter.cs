using System;
using CritterController;

/// <summary>
/// Project bounds namespace.
/// </summary>
namespace _100492443.Critters.AI
{
	/// <summary>
	/// Base class that implements basic functionality for
	/// all types of Critter AI.
	/// </summary>
	class Critter : ICritterController
	{
		/// <summary>
		/// The name given to the current critter.
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// The filepath that this critter controller can 
		/// write to.
		/// </summary>
		public string Filepath { get; set; }

		/// <summary>
		/// Responder object will handle outgoing messages.
		/// </summary>
		public Send Responder { get; set; }

		/// <summary>
		/// Logger object will handle sending log messages
		/// to the CritterWorld environment.
		/// </summary>
		public Send Logger { get; set; }

		/// <summary>
		/// Handles an incoming message from the CritterWorld
		/// environment.
		/// </summary>
		/// <param name="message">The message that was received.</param>
		public void Receive(string message)
		{
			/// TODO: Actually implement the method!!!
			throw new NotImplementedException();
		}

		/// <summary>
		/// Control to open a Window Form to configure
		/// various aspects of the critter.
		/// </summary>
		public void LaunchUI()
		{
			/// TODO: Actually implement the method!!!
			throw new NotImplementedException();
		}
	}
}
