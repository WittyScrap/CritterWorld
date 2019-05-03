using CritterRobots.AI;
using MachineLearning;
using System.Drawing;
using System.IO;
using System.Linq;

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
		public bool IsAlive { get; private set; } = true;

		/// <summary>
		/// Map of all the previously visited sectors.
		/// </summary>
		private bool[,] VisitedSectorsMap { get; } = new bool[Map.Sectors.Width, Map.Sectors.Height];

		/// <summary>
		/// The amount of sectors that this critter visited.
		/// </summary>
		public int VisitedSectors {
			get
			{
				return (from bool wasVisited in VisitedSectorsMap
					    where wasVisited
					    select wasVisited).Count();
			}
		}

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
		/// Constructs a new student critter.
		/// </summary>
		/// <param name="critterID">A unique representative ID for the critter.</param>
		public CritterStudent(int critterID) : base("Helpless Slave #" + critterID)
		{ }

		/// <summary>
		/// Handles loading the neural network.
		/// </summary>
		protected override void LoadNetwork()
		{
			CritterCoach.Coach?.AddStudent(this);
		}

		/// <summary>
		/// Records the current visited sectors into the visited sectors map.
		/// </summary>
		protected override void OnLocationUpdate(int requestID, Point location)
		{
			Point locationOnSectorGrid = Map.GetSector(location);
			VisitedSectorsMap[locationOnSectorGrid.X, locationOnSectorGrid.Y] = true;
		}

		/// <summary>
		/// Indicates that the critter has scored.
		/// </summary>
		protected override void OnScored(Point location)
		{
			Score++;
		}

		/// <summary>
		/// Indicates that the critter has eaten.
		/// </summary>
		protected override void OnAte(Point location)
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
			else
			{
				IsAlive = false;
			}
		}
	}
}
