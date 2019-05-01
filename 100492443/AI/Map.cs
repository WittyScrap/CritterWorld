using CritterRobots.Critters;
using CritterRobots.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.AI
{
	/// <summary>
	/// Represents the critter arena's map.
	/// </summary>
	public class Map
	{
		/// <summary>
		/// The dimensions of this map.
		/// </summary>
		public static Size Extents { get; private set; }

		/// <summary>
		/// Indicates whether or not this map's size is known.
		/// </summary>
		public static bool SizeKnown {
			get
			{
				return !Extents.IsEmpty;
			}
		}

		/// <summary>
		/// Located gifts.
		/// </summary>
		private List<Point> LocatedGifts { get; } = new List<Point>();

		/// <summary>
		/// Located food items.
		/// </summary>
		private List<Point> LocatedFood { get; } = new List<Point>();

		/// <summary>
		/// Located critters and bombs.
		/// </summary>
		private List<Point> LocatedThreats { get; } = new List<Point>();

		/// <summary>
		/// Thread lock for making changes to the map.
		/// </summary>
		private static object MapLock { get; } = new object();

		/// <summary>
		/// Thread lock for making changes to the escape hatch's location.
		/// </summary>
		private static object EscapeLock { get; } = new object();

		/// <summary>
		/// List containig all detected terrain. This list won't be cleared.
		/// </summary>
		private static HashSet<Point> LocatedTerrain { get; } = new HashSet<Point>();

		/// <summary>
		/// The location of the escape hatch.
		/// </summary>
		public static Point? LocatedEscapeHatch { get; private set; } = null;

		/// <summary>
		/// Every discovered terrain tile.
		/// </summary>
		public static IReadOnlyCollection<Point> Terrain => LocatedTerrain;

		/// <summary>
		/// Gets the closest item from a given collection.
		/// </summary>
		private Point? GetClosest(ICollection<Point> source, Point critterLocation)
		{
			if (source.Count == 0)
			{
				return null;
			}

			Point closest = source.First();
			foreach (Point element in LocatedGifts)
			{
				if (((Vector)closest - critterLocation).SqrMagnitude >
					((Vector)element - critterLocation).SqrMagnitude)
				{
					closest = element;
				}
			}
			return closest;
		}

		/// <summary>
		/// Returns the closest located gift.
		/// </summary>
		public Point? GetClosestGift(Point critterLocation)
		{
			return GetClosest(LocatedGifts, critterLocation);
		}

		/// <summary>
		/// Returns the closest located piece of food.
		/// </summary>
		public Point? GetClosestFood(Point critterLocation)
		{
			return GetClosest(LocatedFood, critterLocation);
		}

		/// <summary>
		/// Returns the closest located threat (bomb or critter).
		/// </summary>
		public Point? GetClosestThreat(Point critterLocation)
		{
			return GetClosest(LocatedThreats, critterLocation);
		}

		/// <summary>
		/// Returns the closest located terrain. Will not return boundaries.
		/// </summary>
		public Point? GetClosestTerrain(Point critterLocation)
		{
			return GetClosest(LocatedTerrain, critterLocation);
		}

		/// <summary>
		/// Updates the list of located threats through what the
		/// SEE message provided has detected.
		/// </summary>
		/// <param name="message">The SEE message.</param>
		public void OnSee(SeeMessage message)
		{
			LocatedThreats.Clear();

			foreach (LocatableEntity detectedEntity in message.GetEntities(entity => LocatableEntity.Entity.SeeComponents.HasFlag(entity.Type)))
			{
				switch (detectedEntity.Type)
				{
				case LocatableEntity.Entity.Bomb:
				case LocatableEntity.Entity.Critter:
					LocatedThreats.Add(detectedEntity.Location);
					break;
				case LocatableEntity.Entity.Terrain:
					lock (MapLock)
					{
						LocatedTerrain.Add(detectedEntity.Location);
					}
					break;
				}
			}
		}

		/// <summary>
		/// Updates the list of located food and gifts through
		/// what the SCAN message provided has detected.
		/// </summary>
		/// <param name="message">The SCAN message.</param>
		public void OnScan(ScanMessage message)
		{
			LocatedFood.Clear();
			LocatedGifts.Clear();

			foreach (LocatableEntity detectedEntity in message.GetEntities(entity => LocatableEntity.Entity.ScanComponents.HasFlag(entity.Type)))
			{
				switch (detectedEntity.Type)
				{
				case LocatableEntity.Entity.Food:
					LocatedFood.Add(detectedEntity.Location);
					break;
				case LocatableEntity.Entity.Gift:
					LocatedGifts.Add(detectedEntity.Location);
					break;
				case LocatableEntity.Entity.EscapeHatch:
					lock (EscapeLock)
					{
						if (EscapeLock == null)
						{
							LocatedEscapeHatch = detectedEntity.Location;
						}
					}
					break;
				}
			}
		}

		/// <summary>
		/// Clears all known shared terrain data.
		/// </summary>
		public static void Reset()
		{
			lock (MapLock)
			{
				LocatedTerrain.Clear();
			}
		}

		/// <summary>
		/// Reports the arena size after receiving an
		/// ARENA_SIZE message.
		/// </summary>
		/// <param name="arenaSize">The reported arena size.</param>
		public static void ReportSize(Size arenaSize)
		{
			if (Extents == Size.Empty)
			{
				Extents = arenaSize;
			}
		}
	}
}
