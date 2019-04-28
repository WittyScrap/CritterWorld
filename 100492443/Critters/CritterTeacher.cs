using CritterRobots.Critters.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Critters
{
	/// <summary>
	/// This critter handles picking the best
	/// critter from a generation.
	/// </summary>
	class CritterTeacher : Critter
	{
		/// <summary>
		/// The active teacher critter.
		/// </summary>
		public static CritterTeacher Teacher { get; private set; }

		/// <summary>
		/// Container for all the critter students.
		/// </summary>
		private ConcurrentBag<CritterStudent> CritterStudents { get; } = new ConcurrentBag<CritterStudent>();

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
		public CritterTeacher() : base("Teacher Critter")
		{
			Teacher = this;
		}

		/// <summary>
		/// No specific UI.
		/// </summary>
		public override void LaunchUI()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the best student based on the following
		/// criteria:
		/// If none of the critters have managed to escape,
		/// pick the critter with the highest score as the best one.
		/// 
		/// If any of the critters managed to escape alive, pick the one between
		/// those that managed to escape with the best score as the best option.
		/// </summary>
		/// <returns>The best critter in this generation.</returns>
		private CritterStudent GetBestStudent()
		{
			if (!CritterStudent.AnyEscaped)
			{
				float bestScore = 0.0f;
				CritterStudent bestCritter = null;
				foreach (var studentCritter in CritterStudents)
				{
					if (studentCritter.Score > bestScore)
					{
						bestScore = studentCritter.Score;
						bestCritter = studentCritter;
					}
				}
				return bestCritter;
			}
			else
			{
				float bestScore = 0.0f;
				CritterStudent bestCritter = null;
				foreach (var studentCritter in CritterStudents)
				{
					if (studentCritter.HasEscaped && studentCritter.Score > bestScore)
					{
						bestScore = studentCritter.Score;
						bestCritter = studentCritter;
					}
				}
				return bestCritter;
			}
		}

		/// <summary>
		/// Get information on the best performing critter and
		/// save its brain.
		/// </summary>
		/// <param name="stopReason"></param>
		protected override void OnStop(string stopReason)
		{
			if (stopReason == "SHUTDOWN")
			{
				CritterStudent bestCritter = GetBestStudent();
				string serializedBrain = bestCritter.CritterBrain.Serialize();

				using (StreamWriter brainWriter = new StreamWriter(Filepath + "best_brain_snapshot.crbn"))
				{
					brainWriter.Write(serializedBrain);
				}
			}
		}
	}
}
