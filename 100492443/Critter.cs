using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CritterController;

/// <summary>
/// Project bounds namespace.
/// </summary>
namespace _100492443.Critters.AI
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
		/// List containing every object detected in the scene.
		/// </summary>
		private HashSet<ReadonlyObject> AllDetectedObjects { get; set; } = new HashSet<ReadonlyObject>();

		/// <summary>
		/// The logger obejct to show debugging messages.
		/// </summary>
		protected Debug Debugger { get; private set; }

		/// <summary>
		/// The location of the escape hatch, if it was detected.
		/// </summary>
		protected Point EscapeHatch { get; private set; }

		/// <summary>
		/// Returns all detected objects one by one.
		/// </summary>
		protected IEnumerable<ReadonlyObject> AllObjects {
			get
			{
				foreach (ReadonlyObject readonlyObject in AllDetectedObjects)
				{
					yield return readonlyObject;
				}
			}
		}

		/// <summary>
		/// Returns all detected critters one by one.
		/// </summary>
		protected IEnumerable<DetectedCritter> Critters {
			get
			{
				foreach (ReadonlyObject readonlyObject in AllDetectedObjects)
				{
					if (readonlyObject is DetectedCritter)
					{
						yield return (DetectedCritter)readonlyObject;
					}
				}
			}
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
			case "ERROR":
				Debugger.LogError(body);
				break;
			case "SEE":
				MessageSee(body);
				break;
			case "SCAN":
				MessageScan(body);
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
		/// </summary>
		/// <param name="messageBody"></param>
		private void MessageScan(string messageBody)
		{
			string[] components = messageBody.Split('\n');

			if (int.TryParse(components[0], out int requestID))
			{
				string[] sightElements = components[1].Split('\t');

				OnScan(sightElements, requestID);
			}
			else
			{
				Debugger.LogWarning("A new SCAN message was received, but the requestID could be parsed: " + components[0]);
			}
		}

		/// <summary>
		/// Converts a semi parsed detected object into a
		/// <see cref="ReadonlyObject"/> instance.
		/// </summary>
		/// <param name="objectName">The name of the object.</param>
		/// <param name="coordinate">The location of the object.</param>
		/// <param name="data">Generic data about the object.</param>
		/// <returns>An instance of <see cref="ReadonlyObject"/> to match the object specification.</returns>
		private ReadonlyObject GetDetectedObject(string objectName, Point coordinate, params string[] data)
		{
			ReadonlyObject generatedObject = ReadonlyObject.Create(objectName, coordinate);

			if (generatedObject != null)
			{
				generatedObject.ParseObjectData(data);
				return generatedObject;
			}
			else
			{
				Debugger.LogError("There was an error parsing a detected object with name: " + objectName);
				return null;
			}
		}

		/// <summary>
		/// Parses a formatted coordinate string into
		/// a <see cref="Point"/>.
		/// </summary>
		/// <param name="coordinateFormat">The formatted point string.</param>
		/// <returns>A point parsed from the string, <seealso cref="Point.Empty"/> if the parsing is unsuccessful.</returns>
		/// <example>
		/// <code>
		/// Point parsedCoordinate = ParseCoordinate("{X=24,Y=765}");
		/// </code>
		/// </example>
		protected Point ParseCoordinate(string coordinateFormat)
		{
			coordinateFormat = coordinateFormat.Replace('{', ' ').Replace('}', ' ');
			string[] components = coordinateFormat.Split(',');

			if (components.Length != 2)
			{
				Debugger.LogError("Coordinate string was formatted incorrectly, number of components detected was not exactly 2: " + coordinateFormat);
				return Point.Empty;
			}

			string xFormat = components[0].Split('=')[1];
			string yFormat = components[1].Split('=')[1];
			
			if (int.TryParse(xFormat, out int x) && int.TryParse(yFormat, out int y))
			{
				return new Point(x, y);
			}
			else
			{
				Debugger.LogError("Coordinate string was formatted incorrectly and could not be parsed: " + coordinateFormat);
				return Point.Empty;
			}
		}

		/// <summary>
		/// Parses a detected element into an instace of a
		/// <see cref="ReadonlyObject"/> and saves it into the
		/// list of known elements.
		/// </summary>
		/// <param name="element">The element to parse.</param>
		private void ParseDetectedObject(string element)
		{
			string[] elementProperties = element.Split(':');
			if (elementProperties.Length <= 1)
			{
				Debugger.LogError("Invalid element string, coordinate block could not be detected: " + element);
				return;
			}
			string objectName = elementProperties[0];
			Point coordinate = ParseCoordinate(elementProperties[1]);
			string[] objectData = elementProperties.Skip(2).ToArray();
			ReadonlyObject detectedObject = GetDetectedObject(objectName, coordinate, objectData);
			if (detectedObject != null)
			{
				AllDetectedObjects.Remove(detectedObject);
				AllDetectedObjects.Add(detectedObject);
			}
		}

		/// <summary>
		/// Updates map surroundings after a SCAN or SEE call.
		/// </summary>
		/// <param name="scannedElements">List of scanned elements.</param>
		private void UpdateSurroundings(ICollection<string> scannedElements)
		{
			Parallel.ForEach(scannedElements, ParseDetectedObject);
		}

		/// <summary>
		/// Short-range scan that is called automatically
		/// by the CritterWorld environmnent continuously.
		/// </summary>
		protected virtual void OnSee(ICollection<string> sightElements)
		{

		}

		/// <summary>
		/// Long-range scan that is called as a response
		/// from a SCAN request that as to be sent in advance.
		/// </summary>
		protected virtual void OnScan(ICollection<string> sightElements, int requestID)
		{

		}

		#endregion
	}
}
