using CritterRobots.AI;
using CritterRobots.Critters.Controllers;
using CritterRobots.Forms;
using CritterRobots.Messages;
using MachineLearning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;

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
		/// The distance in pixels that the critter can cover
		/// through a single SET_DESTINATION request if one of its
		/// output neurons indicates a 1.
		/// </summary>
		public int MaximumWalkingDistance { get; set; } = 50;

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
		/// This timer will call the <see cref="ProcessNetwork"/> method
		/// on every tick.
		/// </summary>
		private Timer NetworkProcessor { get; }

		/// <summary>
		/// This timer will call <see cref="RetrieveStats"/>.
		/// </summary>
		private Timer Retriever { get; }

		/// <summary>
		/// This timer will periodically send a SCAN message.
		/// </summary>
		private Timer Scanner { get; }

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
			decimal[] networkInput = new decimal[19];

			networkInput[0] = (decimal)Health;
			networkInput[1] = (decimal)Energy;
			networkInput[2] = MaximumTimeSet ? ((decimal)RemainingTime / MaximumTime) : 1.0m;

			Eye.LookNorth(out double northFood, out double northGift, out double northThreat, out double northTerrain);
			Eye.LookEast (out double eastFood,  out double eastGift,  out double eastThreat,  out double eastTerrain);
			Eye.LookSouth(out double southFood, out double southGift, out double southThreat, out double southTerrain);
			Eye.LookWest (out double westFood,  out double westGift,  out double westThreat,  out double westTerrain);

			networkInput[3] = (decimal)northFood;
			networkInput[4] = (decimal)northGift;
			networkInput[5] = (decimal)northThreat;
			networkInput[6] = (decimal)northTerrain;

			networkInput[7] = (decimal)eastFood;
			networkInput[8] = (decimal)eastGift;
			networkInput[9] = (decimal)eastThreat;
			networkInput[10] = (decimal)eastTerrain;

			networkInput[11] = (decimal)southFood;
			networkInput[12] = (decimal)southGift;
			networkInput[13] = (decimal)southThreat;
			networkInput[14] = (decimal)southTerrain;

			networkInput[15] = (decimal)westFood;
			networkInput[16] = (decimal)westGift;
			networkInput[17] = (decimal)westThreat;
			networkInput[18] = (decimal)westTerrain;

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
			try
			{
				decimal[] networkInput = GatherNetworkInput();
				CritterBrain.Feedforward(networkInput);
				InterpretNetworkOutput();
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show("Uncaught exception during network processing for " + Name + " (" + e.Message + "):\n" + e.StackTrace);
			}
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

			decimal wantsToWalkNorth = Clamp01(networkOutput[0]);
			decimal wantsToWalkEast = Clamp01(networkOutput[1]);
			decimal wantsToWalkSouth = Clamp01(networkOutput[2]);
			decimal wantsToWalkWest = Clamp01(networkOutput[3]);
			decimal movementSpeed = Clamp01(networkOutput[4]);

			Debugger.LogMessage(" Wants to walk N: " + wantsToWalkNorth +
								" Wants to walk E: " + wantsToWalkEast + 
								" Wants to walk S: " + wantsToWalkSouth +
								" Wants to walk W: " + wantsToWalkWest + 
								" At this speed: " + movementSpeed);

			decimal verticalDelta = wantsToWalkNorth * MaximumWalkingDistance - wantsToWalkSouth * MaximumWalkingDistance;
			decimal horizontalDelta = wantsToWalkEast * MaximumWalkingDistance - wantsToWalkWest * MaximumWalkingDistance;

			Direction = new Vector((double)horizontalDelta, (double)verticalDelta);
			LastRequestedSpeed = movementSpeed;

			OnDestinationReached(Location);
		}

		/// <summary>
		/// When the critter reaches its desintaion, simply keep it moving the same direction.
		/// </summary>
		protected override void OnDestinationReached(Point location)
		{
			Vector destination = new Vector(Location.X + Direction.X, Location.Y + Direction.Y);
			Responder("SET_DESTINATION:" + (int)destination.X + ":" + (int)destination.Y + ":" + (int)(LastRequestedSpeed * 5));
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
		protected abstract void LoadNetwork(int inputNeurons, int outputNeurons);

		/// <summary>
		/// Checks that the network contains the correct number of input and
		/// output neurons.
		/// </summary>
		/// <returns>True if the network is configured correctly, false otherwise.</returns>
		private bool CheckNetworkStructure(int inputNeurons, int outputNeurons)
		{
			if (CritterBrain == null || CritterBrain.InputNeurons.Count != inputNeurons || CritterBrain.OutputNeurons.Count != outputNeurons)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Creates a new neural network based critter.
		/// </summary>
		public NeuralCritter(string critterName, int retinaDensity) : base(critterName)
		{
			Eye = new CritterEye(this);

			int networkInput = 19;
			int networkOutput = 5;

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
			LoadNetwork(networkInput, networkOutput);

			if (!CheckNetworkStructure(networkInput, networkOutput))
			{
				Debugger.LogError("Invalid network structure!");

				throw new FormatException("Unexpected network structure, detected input count was " + CritterBrain.InputNeurons + 
										  " instead of " + networkInput + ", and the outputs were " + CritterBrain.OutputNeurons + 
										  " instead of " + networkOutput);
			}

			NetworkProcessor = new Timer(50);
			Retriever = new Timer(25);
			Scanner = new Timer(1000);

			NetworkProcessor.Elapsed += (sender, args) => ProcessNetwork();
			Retriever.Elapsed += (sender, args) => RetrieveStats();
			Scanner.Elapsed += (sender, args) => Responder("SCAN:0");
		}

		/// <summary>
		/// Set off the timers.
		/// </summary>
		protected override void OnInitialize()
		{
			NetworkProcessor.Start();
			Retriever.Start();
			Scanner.Start();
		}
	}
}
