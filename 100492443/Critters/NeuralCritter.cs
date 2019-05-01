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
		/// The maximum speed the critter can request.
		/// </summary>
		public int MaximumMovementSpeed { get; set; } = 10;

		/// <summary>
		/// The maximum amount of time in this level.
		/// </summary>
		private decimal MaximumTime { get; } = 60 * 3;

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
		/// The last requested speed by the network.
		/// </summary>
		protected int RequestedSpeed { get; set; }

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
		private decimal[] GatherNetworkInput()
		{
			decimal[] networkInput = new decimal[14];

			/* Health */ networkInput[0] = (decimal)Health;
			/* Energy */ networkInput[1] = (decimal)Energy;
			/*  Time  */ networkInput[2] = (decimal)RemainingTime / MaximumTime;

			double northBoundaryDistance = Eye.LookNorth();
			double eastBoundaryDistance  = Eye.LookEast();
			double southBoundaryDistance = Eye.LookSouth();
			double westBoundaryDistance  = Eye.LookWest();

			EyeResult closestItems = Eye.GetNearestItems();
			
			/* Distance of north boundary      */ networkInput[3]  = 1 - (decimal)northBoundaryDistance;
			/* Distance of east boundary       */ networkInput[4]  = 1 - (decimal)eastBoundaryDistance;
			/* Distance of south boundary      */ networkInput[5]  = 1 - (decimal)southBoundaryDistance;
			/* Distance of west boundary       */ networkInput[6]  = 1 - (decimal)westBoundaryDistance;
			
			/* Angle of closest collectable    */ networkInput[8]  = (decimal)closestItems.NearestCollectable.Angle;
			/* Angle of closest threat	       */ networkInput[9]  = (decimal)closestItems.NearestThreat.Angle;
			/* Angle of escape hatch	       */ networkInput[10] = (decimal)closestItems.EscapeHatch.Angle;
			
			/* Distance to closest collectable */ networkInput[11] = 1 - (decimal)closestItems.NearestCollectable.Distance;
			/* Distance to closest threat      */ networkInput[12] = 1 - (decimal)closestItems.NearestThreat.Distance;
			/* Distance to escape hatch        */ networkInput[13] = 1 - (decimal)closestItems.EscapeHatch.Distance;

			return networkInput;
		}

		/// <summary>
		/// Runs the required data through the network, parses its
		/// output and updates the current destination and direction.
		/// </summary>
		protected virtual void ProcessNetwork()
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

			/*
			decimal wantsToRotateBy = -(decimal)Math.PI + Clamp01(networkOutput[0]) * 2 * (decimal)Math.PI;
			decimal movementSpeed = Clamp01(networkOutput[1]) * (decimal)MaximumMovementSpeed;

			if (wantsToRotateBy < 0)
			{
				wantsToRotateBy = 2 * (decimal)Math.PI - Math.Abs(wantsToRotateBy);
			}

			Debugger.LogMessage(" Wants to rotate by: " + wantsToRotateBy + " rad" +
								" At this speed: " + movementSpeed);
			
			Direction = Direction.Rotated((double)wantsToRotateBy);
			LastRequestedSpeed = (int)movementSpeed;
			*/

			decimal wantsToRotateTo = Clamp01(networkOutput[0]) * 2 * (decimal)Math.PI;
			decimal movementSpeed = Clamp01(networkOutput[1]) * MaximumMovementSpeed;

			Debugger.LogMessage(" Wants to rotate to: " + wantsToRotateTo + " rad" +
								" At this speed: " + movementSpeed);

			Direction = Vector.Up.Rotated((double)wantsToRotateTo) * 100;
			RequestedSpeed = (int)movementSpeed;

			OnDestinationReached(Location);
		}

		/// <summary>
		/// When the critter reaches its desintaion, simply keep it moving the same direction.
		/// </summary>
		protected override void OnDestinationReached(Point location)
		{
			Vector destination = new Vector(Math.Max(Location.X + Direction.X, 0), Math.Max(Location.Y + Direction.Y, 0));
			Responder("SET_DESTINATION:" + (int)destination.X + ":" + (int)destination.Y + ":" + RequestedSpeed);
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
			Responder("SCAN:0");
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

			int networkInput = 14;
			int networkOutput = 2;

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
			Retriever = new Timer(50);

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
