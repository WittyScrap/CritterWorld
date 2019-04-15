using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CritterController;

/// <summary>
/// Project bounds namespace.
/// </summary>
namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Implements basic functionality for
	/// all types of Critter AI.
	/// </summary>
	abstract class Critter : ICritterController
	{
		#region ICritterController interface components
		/// <summary>
		/// The actual logger object.
		/// </summary>
		private Send m_loggerObject;

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
		public Send Logger {
			get
			{
				return m_loggerObject;
			}
			set
			{
				m_loggerObject = value;
				Debugger.UpdateLogger(value);
			}
		}
		#endregion
		
		/// <summary>
		/// The logger obejct to show debugging messages.
		/// </summary>
		protected Debug Debugger { get; private set; }

		/// <summary>
		/// The full size of the arena map.
		/// </summary>
		protected static Arena Map { get; private set; }

		/// <summary>
		/// Message system for sending and receiving messages.
		/// </summary>
		protected MessageHandler MessageSystem => new MessageHandler();

		#region Critter properties
		/// <summary>
		/// The current velocity of the critter.
		/// </summary>
		protected Vector Velocity { get; private set; } = Vector.Zero;

		/// <summary>
		/// The current location of the critter.
		/// </summary>
		protected Point Location { get; private set; } = Point.Empty;

		/// <summary>
		/// The amount of time elapsed since the start of the level.
		/// </summary>
		protected float ElapsedTime { get; private set; } = 0f;

		/// <summary>
		/// The amount of time left before the end of the level.
		/// </summary>
		protected float RemainingTime { get; private set; } = 0f;

		/// <summary>
		/// The current health of the critter.
		/// </summary>
		protected float Health { get; private set; } = 1f;

		/// <summary>
		/// The current energy of the critter.
		/// </summary>
		protected float Energy { get; private set; } = 1f;

		/// <summary>
		/// If this is false, no messages can be sent to the CritterWorld
		/// enviroment.
		/// </summary>
		private bool IsInitialized { get; set; }
		#endregion

		/// <summary>
		/// Handles an incoming message from the CritterWorld
		/// environment.
		/// </summary>
		/// <param name="message">The message that was received.</param>
		public void Receive(string message)
		{
			string[] splitMessage = message.Split(':');
			string header = splitMessage[0];

//			MessageDecoder(header, message);
			MessageSystem.ResolveMessage(message);
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

		/// <summary>
		/// Creates a new critter and sets its name.
		/// </summary>
		/// <param name="critterName">The name for this critter.</param>
		public Critter(string critterName)
		{
			Name = critterName;
			Debugger = new Debug(Logger, "100492443:" + critterName);

			// Bind messages to methods...
		}

		#region Event driven methods
		
		/// <summary>
		/// Initializes this critter and sends out the first fundamental
		/// requests.
		/// </summary>
		private void InitializeCritter(ISimpleMessage launchEvent)
		{
			IsInitialized = true;
		}

		/// <summary>
		/// Decodes an incoming message and calls the appropriate events.
		/// </summary>
		/// <param name="header">The message header.</param>
		/// <param name="message">The message sub-sections.</param>
/*		private void MessageDecoder(string header, string message)
		{
			switch (header)
			{
			case "SEE":
			case "LAUNCH":
			case "ESCAPE":
			case "SCORED":
			case "ATE":
			case "FIGHT":
			case "BUMP":
			case "FATALITY":
			case "STARVED":
			case "BOMBED":
			case "CRASHED":
			case "REACHED_DESTINATION":
			case "SHUTDOWN":
			// ^^^ Simple messages ^^^
			// -----------------------
			// vvv Tracked messages vvv
			case "SCAN":
			case "ARENA_SIZE":
			case "LEVEL_DURATION":
			case "LEVEL_TIME_REMAINING":
			case "HEALTH":
			case "ENERGY":
			case "LOCATION":
			case "SPEED":
			default:
				Debugger.LogError("Environment error: " + message);
				break;
			}
		}*/

		#endregion

		#region Message senders

		/// <summary>
		/// Runs a SCAN operation.
		/// </summary>
		protected void Scan()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
