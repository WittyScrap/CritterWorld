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
		public Size Extents { get; private set; }

		/// <summary>
		/// Indicates whether or not this map's size is known.
		/// </summary>
		public bool SizeKnown {
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
		/// List containig all detected terrain. This list is immutable.
		/// </summary>
		private static List<Point> LocatedTerrain { get; } = new List<Point>();

		/// <summary>
		/// Updates the list of located threats through what the
		/// SEE message provided has detected.
		/// </summary>
		/// <param name="message">The SEE message.</param>
		public void OnSee(SeeMessage message)
		{

		}

		/// <summary>
		/// Updates the list of located food and gifts through
		/// what the SCAN message provided has detected.
		/// </summary>
		/// <param name="message">The SCAN message.</param>
		public void OnScan(ScanMessage message)
		{

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
		public void ReportSize(Size arenaSize)
		{
			if (Extents == Size.Empty)
			{
				Extents = arenaSize;
			}
		}
	}
}
