using MachineLearning;
using CritterRobots.Forms;
using System.Drawing;
using System.IO;
using CritterRobots.AI;
using CritterRobots.Messages;
using System;
using System.Timers;

namespace CritterRobots.Critters.Controllers
{
	/// <summary>
	/// This critter will handle training the
	/// internal Neural Network.
	/// </summary>
	public class CritterStudent : NeuralCritter
	{
		/// <summary>
		/// The current critter's score.
		/// </summary>
		public int Score { get; private set; }

		/// <summary>
		/// Indicates whether or not this critter reached the escape hatch alive.
		/// </summary>
		public bool HasEscaped { get; set; }

		/// <summary>
		/// Indicates whether or not this critter is alive.
		/// </summary>
		public bool IsAlive {
			get => Health > .01f;
		}

		/// <summary>
		/// Indicates whether any of the student critters in this round successfully escaped.
		/// </summary>
		public static bool AnyEscaped { get; set; }

		/// <summary>
		/// The starting location of this critter.
		/// </summary>
		private Point? StartingLocation { get; set; } = null;

		/// <summary>
		/// This critter's distance from the spawn point.
		/// </summary>
		public Vector DistanceFromSpawn {
			get
			{
				if (StartingLocation == null)
				{
					return (Vector)Location;
				}

				return (Vector)StartingLocation - Location;
			}
		}

		/// <summary>
		/// If this flag is set, the critter will walk downwards at maximum speed
		/// until it dies.
		/// </summary>
		public bool HasFailed {
			get => Failed;
			set
			{
				if (!Failed && value)
				{
					Failed = value;

					Score = 0;
					Direction = Vector.Up * 10;
					RequestedSpeed = MaximumMovementSpeed;

					Debugger.LogMessage(Name + " failed to meet the criteria for the next generation and will now die.");
				}
			}
		}

		/// <summary>
		/// Indicates whether or not this critter failed.
		/// </summary>
		private bool Failed { get; set; }

		/// <summary>
		/// Saves the initial location if it is not set.
		/// </summary>
		protected override void OnLocationUpdate(int requestID, Point location)
		{
			base.OnLocationUpdate(requestID, location);

			if (StartingLocation == null)
			{
				StartingLocation = location;
			}
		}

		/// <summary>
		/// Constructs a new critter.
		/// </summary>
		/// <param name="critterID">A unique representative ID for the critter.</param>
		public CritterStudent(int critterID) : base("Helpless Slave #" + critterID, 10)
		{
			AnyEscaped = false;
			CritterCoach.Coach?.AddStudent(this);
		}

		/// <summary>
		/// Only execute if the critter hasn't failed.
		/// </summary>
		protected override void ProcessNetwork()
		{
			if (!HasFailed)
			{
				base.ProcessNetwork();
			}
		}

		/// <summary>
		/// Handles loading the neural network.
		/// </summary>
		protected override void LoadNetwork(int networkInput, int networkOutput)
		{
			if (!File.Exists(Filepath + "best_brain_snapshot.crbn"))
			{
				CritterBrain = NeuralNetwork.RandomNetwork(
					inputNeurons: networkInput, 
					outputNeurons: networkOutput,
					minLayers: 0,
					maxLayers: 2,
					minNeurons: 1,
					maxNeurons: 3
				);
			}
			else
			{
				using (StreamReader brainReader = new StreamReader(Filepath + "best_brain_snapshot.crbn"))
				{
					string serializedBrain = brainReader.ReadToEnd();
					CritterBrain = NeuralNetwork.Deserialize(serializedBrain);
					CritterBrain.Mutate();
				}
			}
		}

		/// <summary>
		/// Indicates that the critter has scored.
		/// </summary>
		protected override void OnScored(Point location)
		{
			if (!Failed)
			{
				Score++;
			}
		}

		/// <summary>
		/// Indicates that this critter has stopped.
		/// If this is because the critter has escaped, we'll flag
		/// this critter as ESCAPED.
		/// </summary>
		protected override void OnStop(string stopReason)
		{
			if (stopReason == "ESCAPE")
			{
				HasEscaped = true;
				AnyEscaped = true;
			}
		}
	}
}
