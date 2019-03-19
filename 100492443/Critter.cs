using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
	abstract class Critter : ICritterController
	{
		private Send m_loggerObject;
        private static int m_nextRequestID = 0;

        #region ICritterController interface components
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
		/// List containing every object detected in the scene.
		/// </summary>
		private HashSet<ReadonlyObject> AllDetectedObjects { get; set; } = new HashSet<ReadonlyObject>();

        /// <summary>
        /// Container for every submitted message awaiting a response.
        /// </summary>
        private Dictionary<int, Request> AllSubmittedRequests { get; set; } = new Dictionary<int, Request>();
        
		/// <summary>
		/// The logger obejct to show debugging messages.
		/// </summary>
		protected Debug Debugger { get; private set; }

		/// <summary>
		/// The location of the escape hatch, if it was detected.
		/// </summary>
		protected Point EscapeHatch { get; private set; }

        /// <summary>
        /// Returns true if the CritterController can send messages.
        /// </summary>
        protected bool HasLaunched { get; private set; }

        /// <summary>
        /// The last known position of the critter.
        /// </summary>
        protected Point Position { get; private set; }

        /// <summary>
        /// The last known speed.
        /// </summary>
        protected static Point Speed { get; private set; }

        /// <summary>
        /// The last known health value.
        /// </summary>
        protected float Health { get; private set; }

        /// <summary>
        /// The last known energy amount.
        /// </summary>
        protected float Energy { get; private set; }

        /// <summary>
        /// The current known arena size.
        /// </summary>
        protected static Size ArenaSize { get; private set; }

        /// <summary>
        /// Navigational map data. Ensure the map's resolution
        /// is large enough to avoid losing too much detail.
        /// </summary>
        protected static bool[,] NavmapData { get; private set; }

        /// <summary>
        /// The last known level duration.
        /// </summary>
        protected static int LevelDuration { get; private set; }

        /// <summary>
        /// The remaining level time.
        /// </summary>
        protected static int TimeRemaining { get; private set; }

        /// <summary>
        /// Returns all detected objects one by one.
        /// </summary>
        protected IEnumerable<ReadonlyObject> GetAllObjects()
        {
			foreach (ReadonlyObject readonlyObject in AllDetectedObjects)
			{
				yield return readonlyObject;
			}
		}

		/// <summary>
		/// Returns all detected critters one by one.
		/// </summary>
		protected IEnumerable<DetectedCritter> GetDetectedCritters()
        {
			foreach (ReadonlyObject readonlyObject in AllDetectedObjects.Where(obj => obj is DetectedCritter))
			{
				yield return (DetectedCritter)readonlyObject;
			}
		}

        /// <summary>
        /// The next available unique request ID.
        /// </summary>
        protected static int NextRequestID
        {
            get => m_nextRequestID++;
        }

        /// <summary>
        /// Submits a message to the CritterWorld environment.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="callback">The callback method that will be called when a response is received.</param>
        /// <param name="requestID">The ID of the request, it MUST match the one in the message string.</param>
        protected void SubmitRequest(string message, int requestID, Request request)
        {
            Responder(message);
            AllSubmittedRequests[requestID] = request;
        }

        /// <summary>
        /// Cancel a previously submitted request.
        /// </summary>
        /// <param name="requestID">The ID of the request to cancel.</param>
        protected void CancelRequest(int requestID)
        {
            if (AllSubmittedRequests.ContainsKey(requestID))
            {
                AllSubmittedRequests.Remove(requestID);
            }
            else
            {
                Debugger.LogWarning("Invalid request ID specified when asking to cancel a request: " + requestID);
            }
        }

        /// <summary>
        /// Resolves a submitted request and removes it from the
        /// requests list.
        /// </summary>
        /// <param name="requestID">The request to be resolved.</param>
        private void ResolveRequest(int requestID, EventArgs args)
        {
            if (AllSubmittedRequests.ContainsKey(requestID))
            {
                AllSubmittedRequests[requestID].OnRequestResolved(args);
                AllSubmittedRequests.Remove(requestID);
            }
        }

        /// <summary>
        /// Sets the critter as running and
        /// updates the last known point.
        /// </summary>
        /// <param name="coordinates">The received coordinates.</param>
        private void Start(string coordinates)
        {
            HasLaunched = true;
            Position = ParseCoordinate(coordinates);
        }

        /// <summary>
        /// Sets the critter as not running and
        /// updates the last known point.
        /// </summary>
        /// <param name="coordinates">The received coordinates.</param>
        private void Stop(string coordinates)
        {
            HasLaunched = false;
            Position = ParseCoordinate(coordinates);
        }

        /// <summary>
        /// Crash event detected from the CritterWorld environment.
        /// </summary>
        /// <param name="crashReport">The body of the CRASHED message.</param>
        protected virtual void OnCrashed(string crashReport)
        {
            string[] components = crashReport.Split(':');
            Debugger.LogError(components[1]);
            Stop(components[0]);
        }

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

		#region Event driven methods

		/// <summary>
		/// Decodes an incoming message and calls the appropriate events.
		/// </summary>
		/// <param name="header">The message header.</param>
		/// <param name="body">The message sub-sections.</param>
		private void MessageDecoder(string header, string body)
		{
			switch (header)
            {
            case "FATALITY":
            case "STARVED":
            case "BOMBED":
            case "REACHED_DESTINATION":
                Stop(body);
                break;

            case "ERROR":
				Debugger.LogError(body);
				break;

            case "CRASHED":
                OnCrashed(body);
                break;

            case "LAUNCH":
                Start(body);
                break;

			case "SEE":
				ParseSee(body);
				break;

			case "SCAN":
				ParseScan(body);
				break;

            case "LEVEL_DURATION":
                OnTimeElapsed(body);
                break;

            case "LEVEL_TIME_REMAINING":
                OnTimeRemaining(body);
                break;
			}
		}

        /// <summary>
        /// Updates the known level duration from
        /// a raw string message.
        /// </summary>
        /// <param name="message">The message that contains both the request number and the elapsed time.</param>
        protected virtual void OnTimeElapsed(string message)
        {
            string[] messageParts = message.Split(':');
            
            if (int.TryParse(messageParts[0], out int requestID) && int.TryParse(messageParts[1], out int elapsedTime))
            {
                ResolveRequest(requestID, new TimeElapsedRequest.Args() { ElapsedTime = elapsedTime });
                LevelDuration = elapsedTime;
            }
            else
            {
                Debugger.LogWarning("A new LEVEL_DURATION message was received but it could not be parsed: " + message);
            }
        }

        /// <summary>
        /// Updates the level time duration from
        /// a raw string message.
        /// </summary>
        /// <param name="message">The message that contains both the request number and the remaining time.</param>
        protected virtual void OnTimeRemaining(string message)
        {
            string[] messageParts = message.Split(':');

            if (int.TryParse(messageParts[0], out int requestID) && int.TryParse(messageParts[1], out int remainingTime))
            {
                ResolveRequest(requestID, remainingTime);
                LevelDuration = remainingTime;
            }
            else
            {
                Debugger.LogWarning("A new LEVEL_TIME_REMAINING message was received but it could not be parsed: " + message);
            }
        }

        /// <summary>
        /// Parses a coordinate block in the format of
        /// {X=&lt;x-coord&gt;,Y=&lt;y-coord&gt;} into a Point object.
        /// </summary>
        /// <param name="coordinateMessage">The coordinate block to parse.</param>
        /// <returns>A parsed point from the passed coordinates.</returns>
        protected Point ParseCoordinate(string coordinateMessage)
        {
            Point result = new Point();
            
            string removedBrackets = coordinateMessage.Replace('{', '\0').Replace('}', '\0');
            string[] blocks = removedBrackets.Split(',');
            string[] xBlock = blocks[0].Split('=');
            string[] yBlock = blocks[1].Split('=');
            string xCoordinate = xBlock[1];
            string yCoordinate = yBlock[1];

            if (int.TryParse(xCoordinate, out int parsedX) && int.TryParse(yCoordinate, out int parsedY))
            {
                result.X = parsedX;
                result.Y = parsedY;
            }
            else
            {
                Debugger.LogError("Unable to parse X or Y component from received coordinate: " + coordinateMessage);
            }

            return result;
        }

		/// <summary>
		/// Decodes the SEE request into different sections
		/// and feeds the results to <seealso cref="OnSee(string[], int)"/>.
		/// </summary>
		/// <param name="messageBody">The body of the message.</param>
		private void ParseSee(string messageBody)
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
		/// and feeds the results to <seealso cref="OnScan(string[])"/>.
		/// </summary>
		/// <param name="messageBody"></param>
		private void ParseScan(string messageBody)
		{
			string[] components = messageBody.Split('\n');

			if (int.TryParse(components[0], out int requestID))
			{
				string[] sightElements = components[1].Split('\t');

				OnScan(sightElements, requestID);
			}
			else
			{
				Debugger.LogWarning("A new SCAN message was received, but the requestID cannot be parsed: " + messageBody);
			}
		}

		/// <summary>
		/// Short-range scan that is called automatically
		/// by the CritterWorld environmnent continuously.
		/// </summary>
		protected virtual void OnSee(string[] sightElements)
		{
            /// TODO: Actually implement this!!!
            throw new NotImplementedException();
		}

		/// <summary>
		/// Long-range scan that is called as a response
		/// from a SCAN request that as to be sent in advance.
		/// </summary>
		protected virtual void OnScan(string[] sightElements, int requestID)
		{
            ResolveRequest(requestID, sightElements);

            /// TODO: Actually implement this!!!
            throw new NotImplementedException();
        }


		#endregion
	}
}
