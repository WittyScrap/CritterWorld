﻿using MachineLearning;
using CritterRobots.Forms;
using System.Drawing;
using System.IO;
using CritterRobots.AI;
using CritterRobots.Messages;
using System;
using System.Timers;
using System.Windows.Forms;

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
		/// Indicates whether any of the student critters in this round successfully escaped.
		/// </summary>
		public static bool AnyEscaped { get; private set; }

		/// <summary>
		/// Constructs a new critter.
		/// </summary>
		/// <param name="critterID">A unique representative ID for the critter.</param>
		public CritterStudent(int critterID) : base("Student that will totally not drop out #" + critterID, 10)
		{
			AnyEscaped = false;
			CritterCoach.Coach?.AddStudent(this);
		}

		/// <summary>
		/// Initialize alive timer.
		/// </summary>
		protected override void OnInitialize()
		{
			base.OnInitialize();
		}

		/// <summary>
		/// Handles loading the neural network.
		/// </summary>
		protected override void LoadNetwork(int networkInput, int networkOutput)
		{
			if (!File.Exists(Filepath + "best_brain_snapshot.crbn"))
			{
				// 5 outputs:
				// "How much do I want to walk north?"
				// "How much do I want to walk east?"
				// "How much do I want to walk south?"
				// "How much do I want to walk west?"
				// "At what speed do I want to walk?"
				//
				// The inputs are split between:
				// 10 eye cells to detect food
				// 10 eye cells to detect gifts
				// 10 eye cells to detect critters, bombs and terrain
				// 10 eye cells to detect the exit
				// 1 input to determine the health
				// 1 input to determine the energy
				// 1 input to determine the remaining level time
				CritterBrain = NeuralNetwork.RandomNetwork(
					networkInput, 
					networkOutput, 
					0,
					10, 
					1, 
					50
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
				AnyEscaped = true;
			}
		}
	}
}
