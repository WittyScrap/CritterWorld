﻿using CritterRobots.AI;
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
		/// The amount of input neurons for this critter's neural network.
		/// </summary>
		public static int NetworkInput { get; } = 9;

		/// <summary>
		/// The amount of output neurons for this critter's neural network.
		/// </summary>
		public static int NetworkOutput { get; } = 2;

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
		public int MaximumMovementSpeed { get; set; } = 5;

		/// <summary>
		/// The minimum speed at which critters can move.
		/// </summary>
		public int MinimumMovementSpeed { get; set; } = 1;

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
		protected Vector Direction { get; set; } = Vector.Down;

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
		/// Keeps the value <paramref name="t"/> within the range 0 to <paramref name="range"/>.
		/// </summary>
		private double Repeat(double t, double range)
		{
			return t % range;
		}

		/// <summary>
		/// Rotates the angles in the eye results by the angle
		/// formed between the current critter's facing direction and
		/// the UP vector.
		/// </summary>
		/// <param name="results"></param>
		private void RotateEyeResults(ref EyeResult results)
		{
			Vector nearestCollectable	= results.NearestCollectable.Direction;
			Vector nearestThreat		= results.NearestCollectable.Direction;
			Vector escapeHatch			= results.EscapeHatch.Direction;

			double collectableAngle		= Repeat(Vector.FullAngle(Direction.Normalized, nearestCollectable.Normalized)	+ Math.PI, Math.PI * 2) / (Math.PI * 2);
			double threatAngle			= Repeat(Vector.FullAngle(Direction.Normalized, nearestThreat.Normalized)		+ Math.PI, Math.PI * 2) / (Math.PI * 2);
			double escapeAngle			= Repeat(Vector.FullAngle(Direction.Normalized, escapeHatch.Normalized)			+ Math.PI, Math.PI * 2) / (Math.PI * 2);

			results.NearestCollectable	= new Item(results.NearestCollectable.Distance, collectableAngle, nearestCollectable);
			results.NearestThreat		= new Item(results.NearestThreat.Distance,		threatAngle,	  nearestThreat);
			results.EscapeHatch			= new Item(results.EscapeHatch.Distance,		escapeAngle,	  escapeHatch);
		}

		/// <summary>
		/// Creates a network input map.
		/// </summary>
		private decimal[] GatherNetworkInput()
		{
			decimal[] networkInput = new decimal[9];

			/* Health */ networkInput[0] = (decimal)Health;
			/* Energy */ networkInput[1] = (decimal)Energy;
			/*  Time  */ networkInput[2] = (decimal)RemainingTime / MaximumTime;

			/*
			double northBoundaryDistance = Eye.LookNorth();
			double eastBoundaryDistance  = Eye.LookEast();
			double southBoundaryDistance = Eye.LookSouth();
			double westBoundaryDistance  = Eye.LookWest();
			*/

			EyeResult closestItems = Eye.GetClosestItems();
			

//			/* Distance of north boundary      */ networkInput[3] = (decimal)northBoundaryDistance;
//			/* Distance of east boundary       */ networkInput[4] = (decimal)eastBoundaryDistance;
//			/* Distance of south boundary      */ networkInput[5] = (decimal)southBoundaryDistance;
//			/* Distance of west boundary       */ networkInput[6] = (decimal)westBoundaryDistance;
			
			/* Angle of closest collectable    */ networkInput[3] = (decimal)closestItems.NearestCollectable.Angle;
			/* Angle of closest threat	       */ networkInput[4] = (decimal)closestItems.NearestThreat.Angle;
			/* Angle of escape hatch	       */ networkInput[5] = (decimal)closestItems.EscapeHatch.Angle;
			
			/* Distance to closest collectable */ networkInput[6] = 1 - (decimal)closestItems.NearestCollectable.Distance;
			/* Distance to closest threat      */ networkInput[7] = 1 - (decimal)closestItems.NearestThreat.Distance;
			/* Distance to escape hatch        */ networkInput[8] = 1 - (decimal)closestItems.EscapeHatch.Distance;

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

			
			decimal wantsToRotateBy = (decimal)Repeat((double)networkOutput[0] * (Math.PI * 0.05) - (Math.PI * 0.025), Math.PI * 2);
			decimal movementSpeed = Math.Max(Clamp01(networkOutput[1]) * MaximumMovementSpeed, MinimumMovementSpeed);

			if (wantsToRotateBy < 0)
			{
				wantsToRotateBy = 2 * (decimal)Math.PI - Math.Abs(wantsToRotateBy);
			}

			Debugger.LogMessage("Wants to rotate by: " + wantsToRotateBy + " rad " +
								"At this speed: " + movementSpeed);
			
			Direction = Direction.Normalized.Rotated((double)wantsToRotateBy) * 1000;
			/*LastRequestedSpeed = (int)movementSpeed;
			
			-- OLD CODE ARCHIVES --
			-- DO NOT TOUCH --
			-- Or do, you can do whatever you want --

			decimal wantsToRotateTo = Clamp01(networkOutput[0]) * 2 * (decimal)Math.PI;
			decimal movementSpeed = Clamp01(networkOutput[1]) * MaximumMovementSpeed;

			Debugger.LogMessage(" Wants to rotate to: " + wantsToRotateTo + " rad" +
								" At this speed: " + movementSpeed);

			Direction = Vector.Up.Rotated((double)wantsToRotateTo) * 100;*/
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
		protected abstract void LoadNetwork();

		/// <summary>
		/// Checks that the network contains the correct number of input and
		/// output neurons.
		/// </summary>
		/// <returns>True if the network is configured correctly, false otherwise.</returns>
		private bool CheckNetworkStructure()
		{
			return CritterBrain != null && CritterBrain.InputNeurons.Count - 1 == NetworkInput && CritterBrain.OutputNeurons.Count == NetworkOutput;
		}

		/// <summary>
		/// Creates a new neural network based critter.
		/// </summary>
		public NeuralCritter(string critterName) : base(critterName)
		{
			Eye = new CritterEye(this);
			
			/*
			 * Input layout is done as follows:
			 *		[0] -> The current critter's health
			 *		[1] -> The current critter's energy
			 *		[2] -> The total time remaining in the level
			 *		[3] -> The angle (normalized between 0 and 1) of the closest gift or food item
			 *		[4] -> The angle (normalized between 0 and 1) of the closest critter, wall or bomb
			 *		[5] -> The angle (normalized between 0 and 1) of the escape hatch
			 *		[6] -> The distance to the closest gift or food item
			 *		[7] -> The distance to the closest critter, wall or bomb
			 *		[8] -> The distance to the escape hatch.
			 *		
			 *	Ouput layout is done as follows:
			 *		[0] -> The angle delta that the critter wants to rotate by (normalized between 0 and 1, with 0 being -180 degrees and 1 being 180 degrees, and 0.5 being no rotation).
			 *		[1] -> The desired walking speed.
			 */
			LoadNetwork();

			if (!CheckNetworkStructure())
			{
				Debugger.LogError("Invalid network structure!");

				throw new FormatException("Unexpected network structure, detected input count was " + CritterBrain.InputNeurons + 
										  " instead of " + NetworkInput + ", and the outputs were " + CritterBrain.OutputNeurons + 
										  " instead of " + NetworkOutput);
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
