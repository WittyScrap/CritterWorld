﻿using System;
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
		private List<ReadonlyObject> AllDetectedObjects { get; set; } = new List<ReadonlyObject>();

		/// <summary>
		/// Represents a terrain entity/tile.
		/// </summary>
		protected enum TerrainEntity
		{
			None = 0,
			Wall = 1 << 0,
			Bomb = 1 << 1,
			Gift = 1 << 2,
			Food = 1 << 3
		}

		/// <summary>
		/// The logger obejct to show debugging messages.
		/// </summary>
		protected Debug Debugger { get; private set; }

		/// <summary>
		/// Collection of all terrain entities.
		/// If the array is of size 0, then the arena size has not been
		/// returned yet.
		/// </summary>
		protected TerrainEntity[,] TerrainEntities { get; private set; }

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
		/// and feeds the results to <seealso cref="OnSee(string[], int)"/>.
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
		/// and feeds the results to <seealso cref="OnScan(string[])"/>.
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
				Debugger.LogWarning("A new SCAN message was received, but the requestID cannot be parsed: " + components[0]);
			}
		}

		/// <summary>
		/// Short-range scan that is called automatically
		/// by the CritterWorld environmnent continuously.
		/// </summary>
		protected virtual void OnSee(string[] sightElements)
		{

		}

		/// <summary>
		/// Long-range scan that is called as a response
		/// from a SCAN request that as to be sent in advance.
		/// </summary>
		protected virtual void OnScan(string[] sightElements, int requestID)
		{

		}

		#endregion
	}
}
