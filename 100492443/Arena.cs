using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace _100492443
{
	/// <summary>
	/// Represents a simple 2-dimensional map of the terrain.
	/// </summary>
	class Arena
	{
		private class 

		/// <summary>
		/// Enumerator to represent what type
		/// of contents are present within a given
		/// tile.
		/// </summary>
		[Flags]
		public enum TileContents
		{
			None = 0,
			Wall = 1 << 0,
			Gift = 1 << 1,
			Food = 1 << 2,
			Bomb = 1 << 3
		}
		
		/// <summary>
		/// Accesses the internal terrain map.
		/// </summary>
		private TileContents[,] TerrainMap { get; set; }

		/// <summary>
		/// Creates an arena of a specific pixel size.
		/// </summary>
		/// <param name="pixelWidth">The width of the arena in pixels.</param>
		/// <param name="pixelHeight">The height of the arena in pixels.</param>
		/// <param name="pixelSize">How many pixels compose one tile.</param>
		public Arena(int pixelWidth, int pixelHeight, int pixelSize)
		{
			TerrainMap = new TileContents[pixelWidth / pixelSize, pixelHeight / pixelSize];
		}

		/// <summary>
		/// Updates the local map through a series of detected
		/// entities stored in a string using the standard SCAN
		/// or SEE output format (entities spearated by \t).
		/// </summary>
		/// <param name="detectedEntities">String containing all the detected entities.</param>
		/// <remarks>
		/// This method clears cell data for those cells for 
		/// which a new element was detected exclusively.
		/// </remarks>
		public void UpdateMap(string detectedEntities)
		{

		}
	}
}
