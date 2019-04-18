using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CritterController;
using CritterRobots.Messages;

/// <summary>
/// Project bounds namespace.
/// </summary>
namespace CritterRobots.Critters.Controllers
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
		/// If this is set to true, the critter has exhausted its
		/// movements queue.
		/// </summary>
		protected bool ReachedDestination { get; private set; } = true;

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
			IMessage parsedMessage = ConvertMessage(message);
			if (parsedMessage == null)
			{
				return;
			}
			if (parsedMessage is SeeMessage seeMessage)
			{
				OnSee(seeMessage);
			}
			else if (parsedMessage is ScanMessage scanMessage)
			{
				OnScan(scanMessage);
			}
			else
			{
				HandleMessage(parsedMessage);
			}
		}

		/// <summary>
		/// Handles switching a simple message through to the
		/// correct internal methods.
		/// </summary>
		/// <param name="message">The message to be switched.</param>
		private void HandleMessage(IMessage message)
		{
			int requestID;
			switch (message.Header)
			{
			case "LAUNCH":
				InitializeCritter();
				break;
			case "FATALITY":
			case "STARVED":
			case "BOMBED":
			case "CRASHED":
			case "ESCAPE":
			case "SHUTDOWN":
				StopCritter(message.Header);
				break;
			case "LEVEL_DURATION":
				ElapsedTime = message.GetInteger(1);
				break;
			case "LEVEL_TIME_REMAINING":
				RemainingTime = message.GetInteger(1);
				break;
			case "HEALTH":
				requestID = message.GetInteger(0);
				Health = message.GetInteger(1) / 100.0f;
				OnHealthUpdate(requestID, Health);
				break;
			case "ENERGY":
				requestID = message.GetInteger(0);
				Energy = message.GetInteger(1) / 100.0f;
				OnEnergyUpdate(requestID, Energy);
				break;
			case "LOCATION":
			case "SPEED":
			case "ARENA_SIZE":
			case "SCORED":
			case "ATE":
			case "FIGHT":
			case "BUMP":
			case "REACHED_DESTINATION":
			}
		}

		/// <summary>
		/// Sends a message to the CritterWorld environment.
		/// </summary>
		/// <param name="message">The message to be sent.</param>
		protected void SendMessage(IMessage message)
		{
			Responder(message.Format());
		}

		/// <summary>
		/// Converts a source message into an
		/// <see cref="IMessage"/> object.
		/// </summary>
		/// <param name="message">The source message.</param>
		/// <returns>An <see cref="IMessage"/> equivalent of the <paramref name="message"/>.</returns>
		private IMessage ConvertMessage(string message)
		{
			SimpleMessage.GetHeaderBody(message, out string messageHeader, out string messageBody);
			switch (messageHeader)
			{
			case "ERROR":
				Debugger.LogError(messageBody);
				return null;
			case "SEE":
				return new SeeMessage(message);
			case "SCAN":
				return new ScanMessage(message);
			default:
				return new SimpleMessage(message);
			}
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
		}

		#region Critter messages

		/// <summary>
		/// Initialization event.
		/// This is usually the entry point for any critter logic.
		/// </summary>
		protected virtual void OnInitialize() { }

		/// <summary>
		/// End event.
		/// This signifies that the critter can no longer be controlled.
		/// </summary>
		protected virtual void OnStop(string stopReason) { }

		/// <summary>
		/// Reports what the current critter can see around it.
		/// </summary>
		/// <param name="message">The contents of the SEE message.</param>
		protected virtual void OnSee(SeeMessage message) { }

		/// <summary>
		/// Reports everything the current critter can see, with
		/// some limitations.
		/// </summary>
		/// <param name="message">The contents of the SCAN message.</param>
		protected virtual void OnScan(ScanMessage message) { }

		/// <summary>
		/// Health update events are called when a HEALTH message is
		/// received.
		/// </summary>
		/// <param name="requestID">The request ID for the health request.</param>
		/// <param name="remainingHealth">The amount of remaining health.</param>
		protected virtual void OnHealthUpdate(int requestID, float remainingHealth) { }

		/// <summary>
		/// Energy update events are called when an ENERGY message is
		/// received.
		/// </summary>
		/// <param name="requestID">The request ID for the health request.</param>
		/// <param name="remainingEnergy">The amount of remaining energy.</param>
		protected virtual void OnEnergyUpdate(int requestID, float remainingEnergy) { }

		#endregion

		#region Event driven methods

		/// <summary>
		/// Initializes this critter and sends out the first fundamental
		/// requests.
		/// </summary>
		private void InitializeCritter()
		{
			IsInitialized = true;
			OnInitialize();
		}

		/// <summary>
		/// Uninitializes the critter, making it impossible for it to
		/// send and process any messages.
		/// </summary>
		/// <param name="endEvent"></param>
		private void StopCritter(string reason)
		{
			IsInitialized = false;
			OnStop(reason);
		}

		#endregion

		#region Message senders

		/// <summary>
		/// Runs a SCAN operation.
		/// </summary>
		protected void Scan()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Queries the environmnent for the current
		/// level duration.
		/// </summary>
		protected void GetLevelDuration()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Queries the environment for the amount
		/// of time remaining.
		/// </summary>
		protected void GetTimeRemaining()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Queries the environment about this critter's
		/// health.
		/// </summary>
		protected void GetHealth()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Queries the environment about this critter's
		/// energy.
		/// </summary>
		protected void GetEnergy()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Queries the environment about this critter's
		/// speed.
		/// </summary>
		protected void GetSpeed()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
