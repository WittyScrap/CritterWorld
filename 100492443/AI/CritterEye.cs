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
		/// The maximum distance after which the eye stops seeing anything.
		/// </summary>
		public int MaximumDistance { get; set; } = 1000;

		/// <summary>
		/// Returns the distance from this critter to
		/// the specified boundary.
		/// </summary>
		private double GetBoundary(BoundaryCheck check)
		{
			if (Map.SizeKnown)
			{
				return check(Map.Extents, ReferenceCritter.Location);
			}
			else
			{
				return 0.0;
			}
		}

		/// <summary>
		/// Returns the nearest items for every item type.
		/// </summary>
		public EyeResult GetNearestItems()
		{
			Point? nearestGift	  = ReferenceCritter.DetectedMap.GetClosestGift(ReferenceCritter.Location);
			Point? nearestFood	  = ReferenceCritter.DetectedMap.GetClosestFood(ReferenceCritter.Location);
			Point? nearestThreat  = ReferenceCritter.DetectedMap.GetClosestThreat(ReferenceCritter.Location);
			Point? nearestTerrain = ReferenceCritter.DetectedMap.GetClosestTerrain(ReferenceCritter.Location);
			Point? escapeHatch	  = Map.LocatedEscapeHatch;

			bool invalidGift	= nearestGift    == null;
			bool invalidFood	= nearestFood    == null;
			bool invalidThreat	= nearestThreat  == null;
			bool invalidTerrain = nearestTerrain == null;
			bool invalidEscape  = escapeHatch    == null;

			Vector gift		= invalidGift    ? Vector.Zero : (Vector)nearestGift    - ReferenceCritter.Location;
			Vector food		= invalidFood    ? Vector.Zero : (Vector)nearestFood    - ReferenceCritter.Location;
			Vector threat	= invalidThreat  ? Vector.Zero : (Vector)nearestThreat  - ReferenceCritter.Location;
			Vector terrain	= invalidTerrain ? Vector.Zero : (Vector)nearestTerrain - ReferenceCritter.Location;
			Vector escape	= invalidEscape  ? Vector.Zero : (Vector)escapeHatch    - ReferenceCritter.Location;

			double foodDistance		= invalidFood    ? 1.0 : Math.Min(food.Magnitude, MaximumDistance)    / MaximumDistance;
			double giftDistance		= invalidGift    ? 1.0 : Math.Min(gift.Magnitude, MaximumDistance)    / MaximumDistance;
			double terrainDistance	= invalidTerrain ? 1.0 : Math.Min(terrain.Magnitude, MaximumDistance) / MaximumDistance;
			double threatDistance	= invalidThreat  ? 1.0 : Math.Min(threat.Magnitude, MaximumDistance)  / MaximumDistance;
			double escapeDistance	= invalidEscape  ? 1.0 : Math.Min(escape.Magnitude, MaximumDistance)  / MaximumDistance;

			double twoPI = Math.PI * 2;

			return new EyeResult()
			{
				NearestFood		= new Item(foodDistance,	invalidFood    ? 0.0 : Vector.FullAngle(Vector.Up, food)	/ twoPI),
				NearestGift		= new Item(giftDistance,	invalidGift    ? 0.0 : Vector.FullAngle(Vector.Up, gift)	/ twoPI),
				NearestTerrain	= new Item(terrainDistance, invalidTerrain ? 0.0 : Vector.FullAngle(Vector.Up, terrain) / twoPI),
				NearestThreat	= new Item(threatDistance,  invalidThreat  ? 0.0 : Vector.FullAngle(Vector.Up, threat)  / twoPI),
				EscapeHatch		= new Item(escapeDistance,	invalidEscape  ? 0.0 : Vector.FullAngle(Vector.Up, escape)  / twoPI)
			};
		}

		/// <summary>
		/// Asks the eye to look north and report the distance to the north boundary.
		/// </summary>
		public double LookNorth()
		{
			return GetBoundary((size, location) => location.Y / (double)size.Height);
		}

		/// <summary>
		/// Asks the eye to look east and report the distance to the east boundary.
		/// </summary>
		public double LookEast()
		{
			return GetBoundary((size, location) => location.X / (double)size.Width);
		}

		/// <summary>
		/// Asks the eye to look south and report the closest items of each type.
		/// </summary>
		public double LookSouth()
		{
			return GetBoundary((size, location) => 1 - location.Y / (double)size.Height);
		}

		/// <summary>
		/// Asks the eye to look west and report the closest items of each type.
		/// </summary>
		public double LookWest()
		{
			return GetBoundary((size, location) => 1 - location.X / (double)size.Width);
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
