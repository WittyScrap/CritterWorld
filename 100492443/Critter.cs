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

		/// <summary>
		/// Request tracker that uses the request ID to track requests.
		/// </summary>
		private Dictionary<int, TrackableRequest> TrackedRequests { get; } = new Dictionary<int, TrackableRequest>();

		/// <summary>
		/// Handles an incoming message from the CritterWorld
		/// environment.
		/// </summary>
		/// <param name="message">The message that was received.</param>
		public void Receive(string message)
		{
			string[] splitMessage = message.Split(':');
			string header = splitMessage[0];
			string messageBody = string.Join(":", splitMessage.Skip(1).ToArray());

			MessageDecoder(header, messageBody);
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

		/// <summary>
		/// Creates a request message to the CritterWorld environment.
		/// </summary>
		/// <typeparam name="T">The type of the request.</typeparam>
		/// <returns>The generated request object.</returns>
		protected T CreateRequest<T>() where T : TrackableRequest, new()
		{
			T generatedRequest = new T();
			generatedRequest.OnResolved += (sender, args) => { TrackedRequests.Remove(generatedRequest.RequestID); };
			TrackedRequests[generatedRequest.RequestID] = generatedRequest;

			return generatedRequest;
		}

		/// <summary>
		/// Attempts to resolve a message if it is currently being tracked.
		/// </summary>
		/// <param name="messageID">The ID of the message to resolve.</param>
		/// <returns>True if the message was resolved, false otherwise.</returns>
		/// <remarks>
		/// This method does not check to ensure that the response expects
		/// a request ID, and should therefore only be called in those circumstances.
		/// </remarks>
		private bool TryResolve(string receivedMessage)
		{
			string[] components = receivedMessage.Split(':');
			string receivedID = components[0];
			if (int.TryParse(receivedID, out int messageID) && TrackedRequests.ContainsKey(messageID))
			{
				string messageContents = string.Join(":", components.Skip(1).ToArray());
				TrackedRequests[messageID].Resolve(receivedMessage);
				return true;
			}
			else
			{
				Debugger.LogError("Attempted to resove an invalid message type!");
				return false;
			}
		}

		/// <summary>
		/// Allocates the arena map object with the obtained
		/// size information.
		/// </summary>
		/// <param name="arenaSize">The size of the arena.</param>
		/// <param name="pixelSize">The size of a tile.</param>
		private static void AllocateMap(Size arenaSize, int pixelSize)
		{
			if (Map == null)
			{
				Map = new Arena(arenaSize.Width, arenaSize.Height, pixelSize);

				Map.Desirability[Arena.TileContents.Bomb]		 = -1.0f;
				Map.Desirability[Arena.TileContents.Empty]		 = 0.0f;
				Map.Desirability[Arena.TileContents.EscapeHatch] = 0.5f;
				Map.Desirability[Arena.TileContents.Food]		 = 1.0f;
				Map.Desirability[Arena.TileContents.Gift]		 = 1.0f;
				Map.Desirability[Arena.TileContents.Terrain]	 = -1.0f;
			}
		}

		#region Event driven methods

		/// <summary>
		/// Event handler receiver for the GET_ARENA_SIZE responder.
		/// </summary>
		/// <param name="arenaSizeFormat"></param>
		private void ArenaSizeReceived(object sender, string message)
		{
			string[] messageComponents = message.Split(':');
			if (messageComponents.Length < 2)
			{
				Debugger.LogError("Invalid ARENA_SIZE request received, less than 2 blocks separated by a ':'.");
			}
			if (int.TryParse(messageComponents[0], out int width) &&
				int.TryParse(messageComponents[1], out int height))
			{
				AllocateMap(new Size(width, height), 100);
			}
			else
			{
				Debugger.LogError("Invalid ARENA_SIZE request received, values could not be parsed to integers.");
			}
		}

		/// <summary>
		/// Initializes this critter and sends out the first fundamental
		/// requests.
		/// </summary>
		private void InitializeCritter()
		{
			var arenaSizeRequester = CreateRequest<ArenaSizeRequest>();
			arenaSizeRequester.OnResolved += ArenaSizeReceived;
			arenaSizeRequester.Submit(Responder);

			IsInitialized = true;
		}

		/// <summary>
		/// Decodes an incoming message and calls the appropriate events.
		/// </summary>
		/// <param name="header">The message header.</param>
		/// <param name="body">The message sub-sections.</param>
		private void MessageDecoder(string header, string body)
		{
			switch (header)
			{
			case "ERROR":
				Debugger.LogError(body);
				break;
			case "SEE":
				MessageSee(body);
				break;
			case "LAUNCH":
				InitializeCritter();
				break;
			case "SCAN":
			case "ARENA_SIZE":
			case "LEVEL_DURATION":
			case "LEVEL_TIME_REMAINING":
			case "HEALTH":
			case "ENERGY":
			case "LOCATION":
			case "SPEED":
				TryResolve(body);
				break;
			}
		}

		/// <summary>
		/// Decodes the SEE request into different sections
		/// and feeds the results to <seealso cref="OnSee(ICollection{string}, int)"/>.
		/// </summary>
		/// <param name="messageBody">The body of the message.</param>
		private void MessageSee(string messageBody)
		{
			string[] components = messageBody.Split('\n');
			string messageData = components[1];

			if (messageData != "Nothing")
			{
				string[] sightElements = messageData.Split('\t');
				
				OnSee(sightElements);
			}
		}

		/// <summary>
		/// Decodes the SCAN request into different sections
		/// and feeds the results to <seealso cref="OnScan(ICollection{string})"/>.
		/// It also updates the local <seealso cref="Map"/>.
		/// </summary>
		/// <param name="messageBody">The body of the scanned message.</param>
		private void MessageScan(string messageBody)
		{
			string[] components = messageBody.Split('\n');

			if (int.TryParse(components[0], out int requestID))
			{
				string[] sightElements = components[1].Split('\t');

				Map?.Update(sightElements);
			}
			else
			{
				Debugger.LogWarning("A new SCAN message was received, but the requestID could be parsed: " + components[0]);
			}
		}

		/// <summary>
		/// Updates the location of this critter to
		/// the result of a GET_LOCATION request.
		/// </summary>
		/// <param name="message">The message containig the results of the GET_LOCATION request.</param>
		private void UpdateTimeRemaining(string message)
		{

		}

		/// <summary>
		/// Updates the elapsed time since the beginning of the level to
		/// the result of a GET_LEVEL_DURATION request.
		/// </summary>
		/// <param name="message">The message containig the results of the GET_LEVEL_DURATION request.</param>
		private void UpdateTimeElapsed(string message)
		{

		}

		/// <summary>
		/// Updates the remainig time before the end of the level to
		/// the result of a GET_LEVEL_TIME_REMAINING request.
		/// </summary>
		/// <param name="message">The message containig the results of the GET_LEVEL_TIME_REMAINING request.</param>
		private void UpdateHealth(string message)
		{

		}

		/// <summary>
		/// Updates the amount of energy left for this critter to
		/// the result of a GET_ENERGY request.
		/// </summary>
		/// <param name="message">The message containig the results of the GET_ENERGY request.</param>
		private void UpdateEnergy(string message)
		{

		}

		/// <summary>
		/// Updates the velocity of this critter to
		/// the result of a GET_VELOCITY request.
		/// </summary>
		/// <param name="message">The message containig the results of the GET_VELOCITY request.</param>
		private void UpdateVelocity(string message)
		{

		}

		/// <summary>
		/// Updates the location of this critter to
		/// the result of a GET_LOCATION request.
		/// </summary>
		/// <param name="message">The message containig the results of the GET_LOCATION request.</param>
		private void UpdateLocation(string message)
		{

		}

		/// <summary>
		/// Short-range scan that is called automatically
		/// by the CritterWorld environmnent continuously.
		/// </summary>
		protected virtual void OnSee(string[] sightElements)
		{

		}

		#endregion

		#region Message senders

		/// <summary>
		/// Runs a SCAN operation.
		/// </summary>
		protected void Scan()
		{
			var scanRequester = CreateRequest<ScanRequest>();
			scanRequester.OnResolved += (sender, message) => MessageScan(message);
			scanRequester.Submit(Responder);
		}

		/// <summary>
		/// Updates the stored speed value to the current critter's speed.
		/// </summary>
		protected void CheckSpeed()
		{
			var speedRequester = CreateRequest<SpeedRequest>();
			speedRequester.OnResolved += (sender, message) => UpdateVelocity(message);
			speedRequester.Submit(Responder);
		}

		/// <summary>
		/// Updates the stored location to the current critter's position.
		/// </summary>
		protected void CheckLocation()
		{
			var locationRequester = CreateRequest<LocationRequest>();
			locationRequester.OnResolved += (sender, message) => UpdateLocation(message);
			locationRequester.Submit(Responder);
		}

		/// <summary>
		/// Updates the stored elapsed time to the current level's timer.
		/// </summary>
		protected void CheckElapsedTime()
		{
			var elapsedTimeRequester = CreateRequest<LevelDurationRequest>();
			elapsedTimeRequester.OnResolved += (sender, message) => UpdateTimeElapsed(message);
			elapsedTimeRequester.Submit(Responder);
		}

		/// <summary>
		/// Updates the remaining time to the current level's timer.
		/// </summary>
		protected void CheckRemainingTime()
		{
			var remainingTimeRequester = CreateRequest<TimeRemainingRequest>();
			remainingTimeRequester.OnResolved += (sender, message) => UpdateTimeRemaining(message);
			remainingTimeRequester.Submit(Responder);
		}

		/// <summary>
		/// Updates the current local health to the critter's health value.
		/// </summary>
		protected void CheckHealth()
		{
			var healthChecker = CreateRequest<HealthRequest>();
			healthChecker.OnResolved += (sender, message) => UpdateHealth(message);
			healthChecker.Submit(Responder);
		}

		/// <summary>
		/// Updates the current local energy to the critter's energy value.
		/// </summary>
		protected void CheckEnergy()
		{
			var energyChecker = CreateRequest<EnergyRequest>();
			energyChecker.OnResolved += (sender, message) => UpdateEnergy(message);
			energyChecker.Submit(Responder);
		}

		#endregion
	}
}
