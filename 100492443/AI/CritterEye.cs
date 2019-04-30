using CritterRobots.Critters;
using CritterRobots.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CritterRobots.AI
{
	/// <summary>
	/// Represents a series of raycast that are able to detect
	/// objects of a certain type.
	/// </summary>
	public class CritterEye
	{
		/// <summary>
		/// Delegate used to check for boundary distances.
		/// </summary>
		/// <returns></returns>
		private delegate double BoundaryCheck(Size mapSize, Point critterLocation);
		
		/// <summary>
		/// The reference critter.
		/// </summary>
		public ILocatableCritter ReferenceCritter { get; }

		/// <summary>
		/// Returns the distance from this critter to
		/// the specified boundary.
		/// </summary>
		private double GetBoundary(BoundaryCheck check)
		{
			if (ReferenceCritter.DetectedMap.SizeKnown)
			{
				return check(ReferenceCritter.DetectedMap.Extents, ReferenceCritter.Location);
			}
			else
			{
				return 0.0;
			}
		}

		/// <summary>
		/// Returns the distace to a wall directly near this
		/// critter or the distance to the boundary.
		/// </summary>
		/// <param name="check"></param>
		/// <returns></returns>
		private double GetWallOrBoundary(BoundaryCheck check)
		{
			return GetBoundary(check);
		}

		/// <summary>
		/// Asks the eye to look north and report the closest items of each type.
		/// </summary>
		/// <param name="nearestFood">The nearest detected piece of food.</param>
		/// <param name="nearestGift">The nearest detected gift.</param>
		/// <param name="nearestThreat">The nearest detected threat.</param>
		/// <param name="northTerrain">The distance from any terrain located directly north, or to the north boundary.</param>
		public EyeResult LookNorth()
		{
			EyeResult results = new EyeResult
			{
				NearestFood = 0.0,
				NearestGift = 0.0,
				NearestThreat = 0.0,
				NearestTerrain = GetWallOrBoundary((size, location) => 1 - location.Y / (double)size.Height)
			};

			return results;
		}

		/// <summary>
		/// Asks the eye to look east and report the closest items of each type.
		/// </summary>
		/// <param name="nearestFood">The nearest detected piece of food.</param>
		/// <param name="nearestGift">The nearest detected gift.</param>
		/// <param name="nearestThreat">The nearest detected threat.</param>
		/// <param name="northTerrain">The distance from any terrain located directly east, or to the east boundary.</param>
		public EyeResult LookEast()
		{
			EyeResult results = new EyeResult
			{
				NearestFood = 0.0,
				NearestGift = 0.0,
				NearestThreat = 0.0,
				NearestTerrain = GetWallOrBoundary((size, location) => 1 - location.X / (double)size.Width)
			};

			return results;
		}

		/// <summary>
		/// Asks the eye to look south and report the closest items of each type.
		/// </summary>
		/// <param name="nearestFood">The nearest detected piece of food.</param>
		/// <param name="nearestGift">The nearest detected gift.</param>
		/// <param name="nearestThreat">The nearest detected threat.</param>
		/// <param name="northTerrain">The distance from any terrain located directly south, or to the south boundary.</param>
		public EyeResult LookSouth()
		{
			EyeResult results = new EyeResult
			{
				NearestFood = 0.0,
				NearestGift = 0.0,
				NearestThreat = 0.0,
				NearestTerrain = GetWallOrBoundary((size, location) => location.Y / (double)size.Height)
			};

			return results;
		}

		/// <summary>
		/// Asks the eye to look west and report the closest items of each type.
		/// </summary>
		/// <param name="nearestFood">The nearest detected piece of food.</param>
		/// <param name="nearestGift">The nearest detected gift.</param>
		/// <param name="nearestThreat">The nearest detected threat.</param>
		/// <param name="northTerrain">The distance from any terrain located directly west, or to the west boundary.</param>
		public EyeResult LookWest()
		{
			EyeResult results = new EyeResult
			{
				NearestFood = 0.0,
				NearestGift = 0.0,
				NearestThreat = 0.0,
				NearestTerrain = GetWallOrBoundary((size, location) => location.X / (double)size.Width)
			};

			return results;
		}

		/// <summary>
		/// Creates a new critter eye.
		/// </summary>
		/// <param name="precision">The density of the eye's "retina".</param>
		public CritterEye(ILocatableCritter referenceCritter)
		{
			ReferenceCritter = referenceCritter;
		}
	}
}
