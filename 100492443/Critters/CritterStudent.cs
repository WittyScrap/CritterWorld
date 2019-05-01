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
		public bool HasEscaped { get; private set; }

		/// <summary>
		/// Indicates whether or not this critter is alive.
		/// </summary>
		public bool IsAlive {
			get => Health > .01f;
		}

		/// <summary>
		/// The total amount of distance that this critter covered up until
		/// this point.
		/// </summary>
		public double DistanceCovered { get; private set; }

		/// <summary>
		/// The distance from this critter's location to the
		/// goalpost.
		/// </summary>
		public double DistanceFromGoal {
			get
			{
				if (Map.LocatedEscapeHatch != null)
				{
					return ((Vector)Map.LocatedEscapeHatch - Location).Magnitude;
				}

				return 0.0;
			}
		}

		/// <summary>
		/// Adds the delta between the new position and the last recorded
		/// position to the total amount walked.
		/// </summary>
		protected override void OnLocationUpdate(int requestID, Point location)
		{
			DistanceCovered += ((Vector)location - Location).Magnitude;
		}

		/// <summary>
		/// Constructs a new critter.
		/// </summary>
		/// <param name="critterID">A unique representative ID for the critter.</param>
		public CritterStudent(int critterID) : base("Helpless Slave #" + critterID, 10)
		{
			CritterCoach.Coach?.AddStudent(this);
		}

		/// <summary>
		/// Only execute if the critter hasn't failed.
		/// </summary>
		protected override void ProcessNetwork()
		{
			base.ProcessNetwork();
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
			Score++;
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
			}
		}
	}
}
