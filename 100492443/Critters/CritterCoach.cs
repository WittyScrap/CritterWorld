using CritterRobots.Critters.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace CritterRobots.Critters
{
	/// <summary>
	/// This critter handles picking the best
	/// critter from a generation.
	/// </summary>
	class CritterCoach : Critter
	{
		/// <summary>
		/// The active teacher critter.
		/// </summary>
		public static CritterCoach Coach { get; private set; }

		/// <summary>
		/// Container for all the critter students.
		/// </summary>
		private ConcurrentBag<CritterStudent> CritterStudents { get; } = new ConcurrentBag<CritterStudent>();

		/// <summary>
		/// Represents a method that selects which
		/// component of the student should be used as a discriminator
		/// to determine which critter student is more fit in the set.
		/// </summary>
		private delegate float CritterFitnessDiscriminator(CritterStudent student);

		/// <summary>
		/// Sends out periodical time check requests.
		/// </summary>
		private System.Timers.Timer TimeChecker { get; set; }

		/// <summary>
		/// Indicates whether or not this teacher has already picked a winner.
		/// </summary>
		private bool HasFinished { get; set; } = false;

		/// <summary>
		/// Logs a student critter to this teacher's attention.
		/// </summary>
		/// <param name="student">The student to be logged.</param>
		public void AddStudent(CritterStudent student)
		{
			CritterStudents.Add(student);
		}

		/// <summary>
		/// Creates a teacher critter.
		/// </summary>
		public CritterCoach() : base("Teacher Critter")
		{
			Coach = this;
		}

		/// <summary>
		/// No specific UI.
		/// </summary>
		public override void LaunchUI()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Set up timer.
		/// </summary>
		protected override void OnInitialize()
		{
			TimeChecker = new System.Timers.Timer(1000);
			TimeChecker.Elapsed += (sender, e) => Responder("GET_LEVEL_TIME_REMAINING:0");
			TimeChecker.Start();
		}

		/// <summary>
		/// Returns the best student given a filter and a discriminator.
		/// </summary>
		/// <param name="filter">Filtering rules to only select critters that match this criteria.</param>
		/// <param name="discriminator">The discriminator by which the best critter is picked.</param>
		private CritterStudent GetBestStudent(Predicate<CritterStudent> filter, CritterFitnessDiscriminator discriminator)
		{
			float bestFitness = 0.0f;
			CritterStudent bestCritter = null;

			foreach (var studentCritter in CritterStudents)
			{
				float currentFitness = discriminator(studentCritter);
				if (filter(studentCritter) && currentFitness > bestFitness)
				{
					bestFitness = currentFitness;
					bestCritter = studentCritter;
				}
			}

			return bestCritter;
		}

		/// <summary>
		/// Checks if all critters are alive.
		/// </summary>
		private bool AllCrittersAlive()
		{
			foreach (var critter in CritterStudents)
			{
				if (!critter.IsAlive)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns the best student based on the following
		/// criteria:
		/// If any of the critters managed to escape, pick among the ones
		/// that did the one with the highest score.
		/// 
		/// If none of the critters managed to escape but all survived, pick
		/// the one with the highest score.
		/// 
		/// If none of the critters managed to escape and only some or none of
		/// them survived, pick the one that survived for the longest.
		/// </summary>
		/// <returns>The best critter in this generation.</returns>
		private CritterStudent GetBestStudent()
		{
			if (CritterStudent.AnyEscaped)
			{
				return GetBestStudent(critter => critter.HasEscaped, critter => critter.Score);
			}
			
			return GetBestStudent(critter => critter.IsAlive, critter => critter.Score);
		}

		/// <summary>
		/// Get information on the best performing critter and
		/// save its brain.
		/// </summary>
		/// <param name="stopReason"></param>
		protected override void OnTimeRemainingUpdate(double timeRemaining)
		{
			if (timeRemaining < 5.0 && !HasFinished)
			{
				CritterStudent bestCritter = GetBestStudent();

				if (bestCritter != null)
				{
					string serializedBrain = bestCritter.CritterBrain.Serialize();

					MessageBox.Show("Round complete, " + bestCritter.Name +
									" is the best critter this time, with a score of " + bestCritter.Score + 
									" Alive state of: " + bestCritter.IsAlive + 
									" and Escape state of: " + bestCritter.HasEscaped
									, "Coach!", MessageBoxButtons.OK, MessageBoxIcon.Information);

					using (StreamWriter brainWriter = new StreamWriter(Filepath + "best_brain_snapshot.crbn"))
					{
						brainWriter.Write(serializedBrain);
					}
				}

				TimeChecker.Stop();
				HasFinished = true;
			}
		}
	}
}
