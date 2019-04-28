using MachineLearning;
using CritterRobots.Forms;
using System.Drawing;
using System.IO;
using CritterRobots.AI;
using CritterRobots.Messages;
using System;

namespace CritterRobots.Critters.Controllers
{
	/// <summary>
	/// This critter will handle training the
	/// internal Neural Network.
	/// </summary>
	public class CritterStudent : Critter, INetworkHolder
	{
		/// <summary>
		/// The neural network used for handling the critter's
		/// behaviour.
		/// </summary>
		public NeuralNetwork CritterBrain { get; set; }

		/// <summary>
		/// This critter's eye.
		/// </summary>
		public CritterEye Eye { get; private set; }

		/// <summary>
		/// The current critter's score.
		/// </summary>
		public int Score { get; private set; }

		/// <summary>
		/// Indicates whether or not this critter reached the escape hatch alive.
		/// </summary>
		public bool HasEscaped { get; private set; }

		/// <summary>
		/// Indicates whether any of the student critters in this round successfully escaped.
		/// </summary>
		public static bool AnyEscaped { get; private set; }

		/// <summary>
		/// The network debug window.
		/// </summary>
		private NetworkTrainerDebugWindow DebugWindow { get; set; }

		/// <summary>
		/// The current walking direction.
		/// </summary>
		private Vector Direction { get; set; }

		/// <summary>
		/// The current destination.
		/// </summary>
		private Point Destination { get; set; }

		/// <summary>
		/// Constructs a new critter.
		/// </summary>
		/// <param name="critterID">A unique representative ID for the critter.</param>
		public CritterStudent(int critterID) : base("Mortal slave #" + critterID)
		{
			AnyEscaped = false;
			if (!File.Exists(Filepath + "best_brain_snapshot.crbn"))
			{
				// 4 outputs:
				// "How convinced am I that I want to turn left?"
				// "How convinced am I that I want to turn right?"
				// "How much, between 0 and 180 degrees, should I turn that direction?"
				// "At what speed should I be walking?"
				CritterBrain = NeuralNetwork.RandomNetwork(4);
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
			Eye = new CritterEye(CritterBrain.InputNeurons.Count);
		}

		/// <summary>
		/// Launches a UI that displays the neural network's status
		/// and allows to make changes.
		/// </summary>
		public override void LaunchUI()
		{
			DebugWindow = new NetworkTrainerDebugWindow(this);
			DebugWindow.Show();
			DebugWindow.Focus();
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
				AnyEscaped = true;
			}
		}

		/// <summary>
		/// Process the network and set the first destination.
		/// </summary>
		protected override void OnSee(SeeMessage message)
		{
			ProcessNetwork();
		}

		/// <summary>
		/// Runs the required data through the network, parses its
		/// output and updates the current destination and direction.
		/// </summary>
		private void ProcessNetwork()
		{
			decimal[] inputValues = new decimal[Eye.Precision];
			for (int i = 0; i < inputValues.Length; ++i)
			{
				inputValues[i] = Randomizer.NextDecimal() > .5m ? 1 : 0;
			}
			CritterBrain.Feedforward(inputValues);
			decimal[] output = CritterBrain.GetNetworkOutput();

			decimal wantsToTurnLeft = output[0];
			decimal wantsToTurnRight = output[1];

			decimal turnAmount = output[2];
			decimal movementSpeed = output[3];

			double turningAngle = Math.PI / (double)turnAmount;

			if (wantsToTurnRight < wantsToTurnLeft)
			{
				Direction = new Vector(Math.Cos(turningAngle), Math.Sin(turningAngle));
			}
			else
			{
				Direction = new Vector(Math.Cos(turningAngle), -Math.Sin(turningAngle));
			}

			Destination = new Point((int)(Location.X + Direction.X * 1000), (int)(Location.Y + Direction.Y * 1000));
			Responder("SET_DESTINATION:" + Destination.X + ":" + Destination.Y + ":" + (int)(movementSpeed * 5));
		}
	}
}
