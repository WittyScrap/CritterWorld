using CritterRobots.Critters.Controllers;
using CritterRobots.Forms;
using CritterRobots.Messages;
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
	public class CritterCoach : Critter
	{
		/// <summary>
		/// The active teacher critter.
		/// </summary>
		public static CritterCoach Coach { get; private set; }

		/// <summary>
		/// The amount of students registered.
		/// </summary>
		public int StudentsCount {
			get
			{
				return CritterStudents.Count;
			}
		}

		/// <summary>
		/// Container for all the critter students.
		/// </summary>
		private List<CritterStudent> CritterStudents { get; } = new List<CritterStudent>();

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
		/// Informs that the selected student has successfully
		/// escaped.
		/// </summary>
		/// <param name="studentID">The ID of the critter that escaped.</param>
		public void SetStudentEscaped(int studentID)
		{
			if (studentID >= 0 && studentID < CritterStudents.Count)
			{
				CritterStudents[studentID].HasEscaped = true;
				CritterStudent.AnyEscaped = true;
			}
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
			CoachWindow coachWindow = new CoachWindow(this);
			coachWindow.Show();
			coachWindow.Focus();
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
		/// Returns the best student in this generation based on a combination of whether or
		/// not the critter escaped, is still alive, and has a high enough score.
		/// </summary>
		/// <returns>The best critter in this generation.</returns>
		private CritterStudent GetBestStudent()
		{
			CritterStudent bestStudent = GetBestStudent(critter => critter.HasEscaped && critter.Score > 0, critter => critter.Score);

			if (bestStudent == null)
			{
				bestStudent = GetBestStudent(critter => critter.IsAlive && critter.Score > 0, critter => critter.Score);
			}
			
			if (bestStudent == null)
			{
				bestStudent = GetBestStudent(critter => critter.Score > 0, critter => critter.Score);
			}

			return bestStudent;
		}

		/// <summary>
		/// Picks the winner for this generation and stops working.
		/// </summary>
		public void FinishGeneration()
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
			else
			{
				MessageBox.Show("Round complete. Nobody wins.\nYou guys SUCK.", "Coach!", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			TimeChecker.Stop();
			HasFinished = true;
		}

		/// <summary>
		/// Get information on the best performing critter and
		/// save its brain.
		/// </summary>
		/// <param name="stopReason"></param>
		protected override void OnTimeRemainingUpdate(double timeRemaining)
		{
			if (timeRemaining < 2.0 && !HasFinished)
			{
				FinishGeneration();
			}
		}
	}
}
