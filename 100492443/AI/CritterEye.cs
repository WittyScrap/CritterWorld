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
		/// Item filter used during searches.
		/// </summary>
		private enum ItemType
		{
			Food,
			Gift,
			Threat,
			Terrain
		}

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
		/// The map stored inside the reference critter.
		/// </summary>
		private Map CritterMap {
			get => ReferenceCritter.DetectedMap;
		}

		/// <summary>
		/// The location of this reference critter.
		/// </summary>
		private Point CritterLocation {
			get => ReferenceCritter.Location;
		}

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
		/// Returns a direction from this critter to
		/// the given item location.
		/// </summary>
		private Vector CritterItemDirection(Point itemLocation)
		{
			// Invalid location.
			if (itemLocation.X < 0 || itemLocation.Y < 0)
			{
				return Vector.Zero;
			}

			return (Vector)itemLocation - CritterLocation;
		}

		/// <summary>
		/// Returns an item consisting of a distance and an angle
		/// from a directional non-normalized vector.
		/// </summary>
		/// <param name="critterItemDirection">The direction from this critter to the item.</param>
		private Item GetItemFromVector(Vector critterItemDirection)
		{
			if (critterItemDirection.SqrMagnitude < 0.01f)
			{
				return new Item(1.0, 0.0);
			}

			double distance = Math.Min(critterItemDirection.Magnitude, MaximumDistance) / MaximumDistance;
			double angle = Vector.FullAngle(Vector.Up, critterItemDirection.Normalized);

			return new Item(distance, angle);
		}

		/// <summary>
		/// Returns the nearest items for every item type.
		/// </summary>
		public EyeResult GetNearestItems()
		{
			CritterMap.GetClosest(CritterLocation, out Point closestGift, out Point closestFood, out Point closestThreat, out Point closestTerrain);

			Vector giftDirection    = CritterItemDirection(closestGift);
			Vector foodDirection    = CritterItemDirection(closestFood);
			Vector threatDirection  = CritterItemDirection(closestThreat);
			Vector terrainDirection = CritterItemDirection(closestTerrain);

			return new EyeResult()
			{
				NearestFood    = GetItemFromVector(foodDirection),
				NearestGift    = GetItemFromVector(giftDirection),
				NearestThreat  = GetItemFromVector(threatDirection),
				NearestTerrain = GetItemFromVector(terrainDirection)
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
			return GetBoundary((size, location) => 1 - location.X / (double)size.Width);
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
			return GetBoundary((size, location) => location.X / (double)size.Width);
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
