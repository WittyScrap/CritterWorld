using CritterRobots.Critters;
using CritterRobots.Messages;
using System;
using System.Collections.Concurrent;
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
		/// The number of sectors for this map.
		/// </summary>
		public static Size Sectors { get; set; } = new Size(10, 10);

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
		/// Located gifts and food.
		/// </summary>
		private ConcurrentBag<Point> LocatedCollectables { get; set; } = new ConcurrentBag<Point>();

		/// <summary>
		/// Located critters and bombs.
		/// </summary>
		private ConcurrentBag<Point> LocatedThreats { get; set; } = new ConcurrentBag<Point>();

		/// <summary>
		/// Thread lock for making changes to the escape hatch's location.
		/// </summary>
		private static object EscapeLock { get; } = new object();

		/// <summary>
		/// The location of the escape hatch.
		/// </summary>
		public static Point? LocatedEscapeHatch { get; private set; } = null;

		/// <summary>
		/// Returns the relative sector given a set of pixels coordinate.
		/// </summary>
		public static Point GetSector(Point pixelCoordinate)
		{
			return new Point((int)Math.Floor(pixelCoordinate.X / (float)Extents.Width * Sectors.Width), (int)Math.Floor(pixelCoordinate.Y / (float)Extents.Height * Sectors.Height));
		}

		/// <summary>
		/// Every discovered gift and food object.
		/// </summary>
		public IReadOnlyCollection<Point> Collectables => LocatedCollectables;
		
		/// <summary>
		/// Every located threat.
		/// </summary>
		public IReadOnlyCollection<Point> Threat => LocatedThreats;

		/// <summary>
		/// Returns the closest item of each given type.
		/// </summary>
		/// <param name="critterLocation">The current critter's location.</param>
		/// <param name="closestGift">The gift closest to the location.</param>
		/// <param name="closestFood">The food item closest to the location.</param>
		/// <param name="closestTheat">The threat closest to the location.</param>
		/// <param name="closestTerrain">The terrain closest to the location.</param>
		public void GetClosest(Point critterLocation, out Point closestCollectable, out Point closestThreat)
		{
			GetClosest(LocatedCollectables,	critterLocation, out closestCollectable);
			GetClosest(LocatedThreats,		critterLocation, out closestThreat);
		}

		/// <summary>
		/// Gets the closest item from a given collection.
		/// </summary>
		public bool GetClosest(IReadOnlyCollection<Point> source, Point critterLocation, out Point closest)
		{
			if (source.Count == 0)
			{
				closest = new Point(-1, -1);
				return false;
			}
			
			try
			{
				closest = source.First();
			}
			catch
			{
				closest = new Point(-1, -1);
				return false;
			}

			foreach (Point element in source)
			{
				if (((Vector)closest - critterLocation).SqrMagnitude >
					((Vector)element - critterLocation).SqrMagnitude)
				{
					closest = element;
				}
			}
			return true;
		}
		
		/// <summary>
		/// Updates the list of located threats through what the
		/// SEE message provided has detected.
		/// </summary>
		/// <param name="message">The SEE message.</param>
		public void OnSee(SeeMessage message)
		{
			LocatedThreats = new ConcurrentBag<Point>();

			foreach (LocatableEntity detectedEntity in message.GetEntities())
			{
				switch (detectedEntity.Type)
				{
				case LocatableEntity.Entity.Bomb:
//				case LocatableEntity.Entity.Critter:
				case LocatableEntity.Entity.Terrain:
					LocatedThreats.Add(detectedEntity.Location);
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
			LocatedCollectables = new ConcurrentBag<Point>();

			foreach (LocatableEntity detectedEntity in message.GetEntities())
			{
				switch (detectedEntity.Type)
				{
				case LocatableEntity.Entity.Food:
				case LocatableEntity.Entity.Gift:
					LocatedCollectables.Add(detectedEntity.Location);
					break;
				case LocatableEntity.Entity.EscapeHatch:
					lock (EscapeLock)
					{
						if (LocatedEscapeHatch == null)
						{
							LocatedEscapeHatch = detectedEntity.Location;
						}
					}
					break;
				}
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
