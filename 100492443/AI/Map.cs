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
		/// List containig all detected terrain. This list won't be cleared.
		/// </summary>
		private static HashSet<Point> LocatedTerrain { get; } = new HashSet<Point>();

		/// <summary>
		/// The location of the escape hatch.
		/// </summary>
		private static Point LocatedEscapeHatch { get; set; }

		/// <summary>
		/// Every discovered terrain tile.
		/// </summary>
		public static IReadOnlyCollection<Point> Terrain => LocatedTerrain;

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
					LocatedTerrain.Add(detectedEntity.Location);
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
					LocatedEscapeHatch = detectedEntity.Location;
					break;
				}
			}
		}

		/// <summary>
		/// Clears all known shared terrain data.
		/// </summary>
		public static void Reset()
		{
			LocatedTerrain.Clear();
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
