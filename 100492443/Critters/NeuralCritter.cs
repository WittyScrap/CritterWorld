using CritterRobots.AI;
using CritterRobots.Critters.Controllers;
using CritterRobots.Forms;
using MachineLearning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CritterRobots.Critters
{
	/// <summary>
	/// Represents a critter that uses a neural network to operate.
	/// </summary>
	public abstract class NeuralCritter : Critter, INetworkHolder
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
		/// Determines whether or not the maximum amount of time for
		/// this level has been set.
		/// </summary>
		private bool MaximumTimeSet { get; set; } = false;

		/// <summary>
		/// The maximum amount of time in this level.
		/// </summary>
		private decimal MaximumTime { get; set; }

		/// <summary>
		/// The network debug window.
		/// </summary>
		private NetworkTrainerDebugWindow DebugWindow { get; set; }

		/// <summary>
		/// The current walking direction.
		/// </summary>
		protected Vector Direction { get; set; } = Vector.Up;

		/// <summary>
		/// The current destination.
		/// </summary>
		protected Point Destination { get; set; }

		/// <summary>
		/// This timer will call the <see cref="ProcessNetwork"/> method
		/// on every tick.
		/// </summary>
		private System.Timers.Timer NetworkProcessor { get; }

		/// <summary>
		/// This timer will call 
		/// </summary>
		private System.Timers.Timer Retriever { get; }

		/// <summary>
		/// The last requested speed by the network.
		/// </summary>
		private decimal LastRequestedSpeed { get; set; }

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
		/// Creates a network input map.
		/// </summary>
		/// <returns></returns>
		private decimal[] GatherNetworkInput()
		{
			decimal[] networkInput = new decimal[3 + Eye.Precision * 4];

			networkInput[0] = (decimal)Health;
			networkInput[1] = (decimal)Energy;
			networkInput[2] = MaximumTimeSet ? ((decimal)RemainingTime / MaximumTime) : 1.0m;
			
			// DO IT DO IT DO IT
			// DO NOT FORGET IT
			// DO. NOT. FORGET. IT.
			// Handle retina input...
			// For serious, this doesn't do anything!!
			// The critter is blind for god's sake
			// Don't be so cruel
			// He has no sense of touch, smell, sound or sight
			// All he knows is how well he is and how long he has before his demise
			// That's it
			// Please give him working eyes!!!!!

			for (int i = 3; i < networkInput.Length; ++i)
			{
				int eyeID = (i - 3) % 10;
				networkInput[i] = 0m;
			}

			return networkInput;
		}

		/// <summary>
		/// Sets the maximum amount of time.
		/// </summary>
		protected override void OnTimeRemainingUpdate(double timeRemaining)
		{
			if (!MaximumTimeSet)
			{
				MaximumTime = (decimal)timeRemaining;
				MaximumTimeSet = true;
			}
		}

		/// <summary>
		/// Runs the required data through the network, parses its
		/// output and updates the current destination and direction.
		/// </summary>
		protected void ProcessNetwork()
		{
			decimal[] networkInput = GatherNetworkInput();
			CritterBrain.Feedforward(networkInput);
			InterpretNetworkOutput();
		}

		/// <summary>
		/// Clamps the value to the range 0-1.
		/// </summary>
		private decimal Clamp01(decimal value)
		{
			return Math.Min(Math.Max(value, 0.0m), 1.0m);
		}

		/// <summary>
		/// Interprets the output provided by the neural network.
		/// </summary>
		private void InterpretNetworkOutput()
		{
			decimal[] networkOutput = CritterBrain.GetNetworkOutput();

			decimal wantsToTurnLeft = Clamp01(networkOutput[0]);
			decimal wantsToTurnRight = Clamp01(networkOutput[1]);
			decimal turnAmount = Clamp01(networkOutput[2]);
			decimal movementSpeed = Clamp01(networkOutput[3]);
			
			double turningAngle = (double)turnAmount * Math.PI * ((wantsToTurnRight < wantsToTurnLeft) ? -1 : 1);

			Debugger.LogMessage("Wants to turn left: " + wantsToTurnLeft + "\nWants to turn right: " + wantsToTurnRight);
			
			if (turningAngle < 0)
			{
				turningAngle = Math.PI + (Math.PI + turningAngle);
			}

			Direction = Direction.Rotated(turningAngle);
			Destination = (Point)(Location + Direction * 100);
			LastRequestedSpeed = movementSpeed;
			
			Responder("SET_DESTINATION:" + Destination.X + ":" + Destination.Y + ":" + (int)(LastRequestedSpeed * 5));
		}

		/// <summary>
		/// Called when the critter reaches its destination.
		/// </summary>
		protected override void OnDestinationReached(Point location)
		{
			Destination = (Point)(Location + Direction * 100);
			Responder("SET_DESTINATION:" + Destination.X + ":" + Destination.Y + ":" + (int)(LastRequestedSpeed * 5));
		}

		/// <summary>
		/// Sends a request to the CritterWorld environment to
		/// retrieve the current location, the remaining level time,
		/// the remaining health and the remaining energy.
		/// </summary>
		private void RetrieveStats()
		{
			Responder("GET_LOCATION:0");
			Responder("GET_LEVEL_TIME_REMAINING:0");
			Responder("GET_HEALTH:0");
			Responder("GET_ENERGY:0");
		}

		/// <summary>
		/// Loads the neural network through any means necessary.
		/// </summary>
		protected abstract void LoadNetwork();

		/// <summary>
		/// Creates a new neural network based critter.
		/// </summary>
		public NeuralCritter(string critterName) : base(critterName)
		{
			LoadNetwork();

			// One input neuron per "cone cell" in the eye, with three
			// input neurons reserved for:
			//		- Remaining health (0.0 - 1.0)
			//		- Remaining energy (0.0 - 1.0)
			//		- Remaining time (0.0 - 1.0)
			// The remaining neurons are split in groups of 4 of variable length, each
			// will represent one purpose specific eye to detect:
			//		- Food
			//		- Gifts
			//		- Bombs, Critters, Walls
			//		- Exit
			Eye = new CritterEye((CritterBrain.InputNeurons.Count - 3) / 4);

			NetworkProcessor = new System.Timers.Timer(50);
			Retriever = new System.Timers.Timer(50);

			NetworkProcessor.Elapsed += (sender, args) => ProcessNetwork();
			Retriever.Elapsed += (sender, args) => RetrieveStats();
		}

		/// <summary>
		/// Set off the timers.
		/// </summary>
		protected override void OnInitialize()
		{
			NetworkProcessor.Start();
			Retriever.Start();
		}
	}
}
