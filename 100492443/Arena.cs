using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace _100492443.Critters
{
	/// <summary>
	/// Represents a simple 2-dimensional map of the terrain.
	/// </summary>
	public sealed class Arena
	{
		/// <summary>
		/// Enumerator to represent what type
		/// of contents are present within a given
		/// tile.
		/// </summary>
		[Flags]
		public enum TileContents
		{
			Empty		= 0,
			Terrain		= 1 << 0,
			Gift		= 1 << 1,
			Food		= 1 << 2,
			Bomb		= 1 << 3,
			EscapeHatch = 1 << 4
		}

		/// <summary>
		/// Defines weights for the desirability factors of each tile entity.
		/// </summary>
		private Dictionary<TileContents, float> DesirabilityWeights { get; set; } = new Dictionary<TileContents, float>();

		/// <summary>
		/// The location of the escape hatch, if it was detected.
		/// </summary>
		public Point EscapeHatch { get; private set; }

		/// <summary>
		/// Thread lock for handling changes with the terrain.
		/// </summary>
		private object TerrainHandlerLock { get; }

		/// <summary>
		/// Accesses the internal terrain map.
		/// </summary>
		public TileContents[,] TerrainMap { get; private set; }

		/// <summary>
		/// Accesses the internal desirability map.
		/// </summary>
		public float[,] DesirabilityMap { get; private set; }

		/// <summary>
		/// The size of a cell in pixels.
		/// </summary>
		private readonly int m_pixelSize;

		/// <summary>
		/// Creates an arena of a specific pixel size.
		/// </summary>
		/// <param name="pixelWidth">The width of the arena in pixels.</param>
		/// <param name="pixelHeight">The height of the arena in pixels.</param>
		/// <param name="pixelSize">How many pixels compose one tile.</param>
		public Arena(int pixelWidth, int pixelHeight, int pixelSize)
		{
			TerrainMap = new TileContents
			[
				pixelWidth / pixelSize,
				pixelHeight / pixelSize
			];

			m_pixelSize = pixelSize;
		}

		/// <summary>
		/// Defines how desirable each type of object should make a tile.
		/// </summary>
		public IDictionary<TileContents, float> Desirability
		{
			get
			{
				return DesirabilityWeights;
			}
		}

		/// <summary>
		/// Converts a pixel coordinate to a cell coordinate.
		/// </summary>
		/// <param name="pixelCoordinate">The coordinates in pixels.</param>
		/// <returns>The coordinate's equivalent in cell space.</returns>
		public Point CellCoordinate(Point pixelCoordinate)
		{
			return new Point
			(
				pixelCoordinate.X / m_pixelSize,
				pixelCoordinate.Y / m_pixelSize
			);
		}

        /// <summary>
        /// Checks if the <paramref name="check"/> bit is set to
        /// 1 in the <paramref name="mask"/>.
        /// </summary>
        /// <param name="check">The single bit to check.</param>
        /// <param name="mask">The mask to check against.</param>
        /// <returns>1 if the bit is set, 0 if it is not.</returns>
        private int IsSet(TileContents check, TileContents mask)
        {
            return ((check & mask) == check) ? 1 : 0;
        }

		/// <summary>
		/// Returns a floating point value for the current cell's desirability.
		/// </summary>
		/// <param name="cell">The contents of the cell.</param>
		/// <returns>A desirability value for the specified cell.</returns>
		private float CalculateDesirability(TileContents cell)
		{
			float desirabilitySum = 0.0f;
            var tileKeys = Enum.GetValues(cell.GetType()).Cast<TileContents>();
			
			foreach (TileContents tileKey in tileKeys)
            {
                desirabilitySum += DesirabilityWeights[tileKey] * IsSet(tileKey, cell);
            }

            return desirabilitySum / tileKeys.Count();
		}

		/// <summary>
		/// Parses a single entity from its string block notation.
		/// </summary>
		/// <param name="entityBlock">The string block containing the entity's data.</param>
		private void ParseEntity(string entityBlock)
		{
			string[] blockParts = entityBlock.Split(':');

			string entityName = blockParts[0];
			string entityPoint = blockParts[1];

			if (Enum.TryParse(entityName, out TileContents tileContents))
			{
				Point entityLocation = ParseCoordinate(entityPoint);
				Point entityCell = CellCoordinate(entityLocation);

				lock (TerrainHandlerLock)
				{
					TerrainMap[entityCell.X, entityCell.Y] |= tileContents;
                    DesirabilityMap[entityCell.X, entityCell.Y] = CalculateDesirability(TerrainMap[entityCell.X, entityCell.Y]);

					if (tileContents == TileContents.EscapeHatch)
					{
						EscapeHatch = entityLocation;
					}
				}
			}
		}

        /// <summary>
        /// Clears a cell's contents (won't clear walls and exits).
        /// </summary>
        /// <param name="cellIndex">The index of the cell.</param>
        private void ClearCell(int cellIndex)
        {
            int mapWidth = TerrainMap.GetLength(0);
            TerrainMap[cellIndex % mapWidth, cellIndex / mapWidth] &= TileContents.Terrain | TileContents.EscapeHatch;
            DesirabilityMap[cellIndex % mapWidth, cellIndex / mapWidth] = CalculateDesirability(TerrainMap[cellIndex % mapWidth, cellIndex / mapWidth]);
        }

		/// <summary>
		/// Updates the local map through a series of detected
		/// entities stored in a string using the standard SCAN
		/// output format (entities spearated by \t).
		/// </summary>
		/// <param name="detectedEntities">String containing all the detected entities.</param>
		/// <remarks>
		/// This method clears cell data for those cells for 
		/// which a new element was detected exclusively.
		/// </remarks>
		public void Update(string[] detectedEntities)
		{
            Parallel.For(0, TerrainMap.Length, ClearCell);
			Parallel.ForEach(detectedEntities, ParseEntity);
		}

		/// <summary>
		/// Sets the correct tile to be flagged as containing a wall.
		/// </summary>
		/// <param name="wallLocation">The pixel location of the wall.</param>
		public void SetWall(Point wallLocation)
		{
			Point cellCoordinate = CellCoordinate(wallLocation);

			if (cellCoordinate.X < TerrainMap.GetLength(0) &&
				cellCoordinate.Y < TerrainMap.GetLength(1))
			{
				TerrainMap[cellCoordinate.X, cellCoordinate.Y] = TileContents.Terrain;
			}
            else
            {
                throw new CritterException("Invalid wall location for point: " + wallLocation);
            }
		}

		/// <summary>
		/// Marks the correct tile to be flagged as containing a bomb.
		/// </summary>
		/// <param name="bombLocation">The pixel location of the bomb.</param>
		public void SetBomb(Point bombLocation)
		{
			Point cellCoordinate = CellCoordinate(bombLocation);

			if (cellCoordinate.X < TerrainMap.GetLength(0) &&
				cellCoordinate.Y < TerrainMap.GetLength(1))
			{
				TerrainMap[cellCoordinate.X, cellCoordinate.Y] |= TileContents.Bomb;
            }
            else
            {
                throw new CritterException("Invalid bomb location for point: " + bombLocation);
            }
        }
		
		/// <summary>
		/// Parses a formatted coordinate string into
		/// a <see cref="Point"/>.
		/// </summary>
		/// <param name="coordinateFormat">The formatted point string.</param>
		/// <returns>A point parsed from the string, <seealso cref="Point.Empty"/> if the parsing is unsuccessful.</returns>
		/// <example>
		/// <code>
		/// Point parsedCoordinate = ParseCoordinate("{X=24,Y=765}");
		/// </code>
		/// </example>
		public static Point ParseCoordinate(string coordinateFormat)
		{
			coordinateFormat = coordinateFormat.Replace('{', ' ').Replace('}', ' ');
			string[] components = coordinateFormat.Split(',');

			if (components.Length != 2)
			{
				throw new CritterException("Coordinate string was formatted incorrectly, number of components detected was not exactly 2: " + coordinateFormat);
			}

			string xFormat = components[0].Split('=')[1];
			string yFormat = components[1].Split('=')[1];

			if (int.TryParse(xFormat, out int x) && int.TryParse(yFormat, out int y))
			{
				return new Point(x, y);
			}
			else
			{
				throw new CritterException("Coordinate string was formatted incorrectly and could not be parsed: " + coordinateFormat);
			}
		}
	}
}
