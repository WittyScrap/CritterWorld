using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CritterController;
using CritterRobots.AI;
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
	public abstract class Critter : ICritterController, ILocatableCritter
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
		/// The detected features in the arena.
		/// </summary>
		public Map DetectedMap { get; } = new Map();

		#region Critter properties
		/// <summary>
		/// The current velocity of the critter.
		/// </summary>
		public Vector Velocity { get; private set; } = Vector.Zero;

		/// <summary>
		/// The current location of the critter.
		/// </summary>
		public Point Location { get; private set; } = Point.Empty;

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
		public float Health { get; private set; } = 1f;

		/// <summary>
		/// The current energy of the critter.
		/// </summary>
		public float Energy { get; private set; } = 1f;

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
				DetectedMap.OnSee(seeMessage);
			}
			else if (parsedMessage is ScanMessage scanMessage)
			{
				OnScan(scanMessage);
				DetectedMap.OnScan(scanMessage);
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
			OnMessageReceived(message);

			try
			{
				switch (message.Header)
				{
				case "LAUNCH":
					InitializeCritter(message.GetPoint(0));
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
					OnTimeRemainingUpdate(RemainingTime);
					break;
				case "HEALTH":
					requestID = message.GetInteger(0);
					Health = (float)message.GetDouble(1) / 100.0f;
					OnHealthUpdate(requestID, Health);
					break;
				case "ENERGY":
					requestID = message.GetInteger(0);
					Energy = (float)message.GetDouble(1) / 100.0f;
					OnEnergyUpdate(requestID, Energy);
					break;
				case "LOCATION":
					requestID = message.GetInteger(0);
					Location = message.GetPoint(1);
					OnLocationUpdate(requestID, Location);
					break;
				case "SPEED":
					requestID = message.GetInteger(0);
					Velocity = new Vector(message.GetDouble(2), message.GetDouble(3));
					OnVelocityUpdate(requestID, Velocity);
					break;
				case "ARENA_SIZE":
					DetectedMap.ReportSize(new Size(message.GetInteger(1), message.GetInteger(2)));
					break;
				case "SCORED":
					OnScored(message.GetPoint(0));
					break;
				case "ATE":
					OnAte(message.GetPoint(0));
					break;
				case "FIGHT":
					OnFight(message.GetPoint(0), message.GetInteger(1), message.GetString(2));
					break;
				case "BUMP":
					OnBump(message.GetPoint(0));
					break;
				case "REACHED_DESTINATION":
					OnDestinationReached(message.GetPoint(0));
					break;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show("The critter " + ToString() + " failed with an exception: " + e.Message + "\nStacktrace:\n" + e.StackTrace, "Critter crash!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				StopCritter("ERROR");
			}
		}

		/// <summary>
		/// Sends a message to the CritterWorld environment.
		/// </summary>
		/// <param name="message">The message to be sent.</param>
		protected void SendMessage(IMessage message)
		{
			Responder?.Invoke(message.Format());
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
		public abstract void LaunchUI();

		/// <summary>
		/// Creates a new critter and sets its name.
		/// </summary>
		/// <param name="critterName">The name for this critter.</param>
		public Critter(string critterName)
		{
			Name = critterName;
			Debugger = new Debug(Logger, "100492443:" + critterName);
			Map.Reset();
		}

		#region Critter messages

		/// <summary>
		/// Allows for the creation of custom behaviour
		/// when a message of any type is received.
		/// </summary>
		/// <param name="message">The message that was received.</param>
		protected virtual void OnMessageReceived(IMessage message) { }

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

		/// <summary>
		/// Velocuty update events are called when a SPEED message
		/// is received.
		/// </summary>
		/// <param name="requestID">The request ID for the health request.</param>
		/// <param name="velocity">The current velocity.</param>
		protected virtual void OnVelocityUpdate(int requestID, Vector velocity) { }

		/// <summary>
		/// Location update events are called when a LOCATION message
		/// is received.
		/// </summary>
		/// <param name="requestID">The request ID for the health request.</param>
		/// <param name="location">The current location</param>
		protected virtual void OnLocationUpdate(int requestID, Point location) { }

		/// <summary>
		/// Called when this critter collects a gift anywhere in the
		/// arena.
		/// </summary>
		/// <param name="location">The location in which the gift was collected.</param>
		protected virtual void OnScored(Point location) { }

		/// <summary>
		/// Called when this critter eats food anywhere in the arena.
		/// </summary>
		/// <param name="location">The location in which the food was eaten.</param>
		protected virtual void OnAte(Point location) { }

		/// <summary>
		/// Called when this critter bumps into any other critter in the arena.
		/// </summary>
		/// <param name="location">Where the bump occurred.</param>
		protected virtual void OnFight(Point location, int critterNumber, string critterInfo) { }

		/// <summary>
		/// Called when this critter bumps into any terrain in the arena.
		/// </summary>
		/// <param name="location">Where the bump occurred.</param>
		protected virtual void OnBump(Point location) { }

		/// <summary>
		/// Called when this reaches a previously set destination.
		/// </summary>
		/// <param name="location">The current critter location.</param>
		protected virtual void OnDestinationReached(Point location) { }

		/// <summary>
		/// Called when the remaining time has been given.
		/// </summary>
		/// <param name="timeRemaining">The amount of time left.</param>
		protected virtual void OnTimeRemainingUpdate(double timeRemaining) { }
		#endregion

		#region Event driven methods

		/// <summary>
		/// Initializes this critter and sends out the first fundamental
		/// requests.
		/// </summary>
		private void InitializeCritter(Point initialLocation)
		{
			IsInitialized = true;
			Location = initialLocation;
			Responder("GET_ARENA_SIZE:0");
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

		#region Utilities

		/// <summary>
		/// Parses a location into a <see cref="Point"/>.
		/// </summary>
		public static Point ParsePoint(string pointFormat)
		{
			string[] components = pointFormat.Replace('{', '\0').Replace('}', '\0').Split(',');
			string[] xComponent = components[0].Split('=');
			string[] yComponent = components[1].Split('=');

			if (int.TryParse(xComponent[1], out int x) && int.TryParse(yComponent[1], out int y))
			{
				return new Point(x, y);
			}
			else
			{
				return Point.Empty;
			}
		}

		#endregion
	}
}
