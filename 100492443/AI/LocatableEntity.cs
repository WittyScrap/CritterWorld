using CritterRobots.Critters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.AI
{
	/// <summary>
	/// Represents an entity that can be located.
	/// </summary>
	public struct LocatableEntity
	{
		/// <summary>
		/// Enumerator to represent what type
		/// of contents are present within a given
		/// tile.
		/// </summary>
		[Flags]
		public enum Entity
		{
			Empty = 0,
			Terrain = 1 << 0,
			Gift = 1 << 1,
			Food = 1 << 2,
			Bomb = 1 << 3,
			EscapeHatch = 1 << 4,
			Critter = 1 << 5,
			All = Terrain | Gift | Food | Bomb | EscapeHatch,
			Threats = Critter | Bomb | Terrain
		}

		/// <summary>
		/// What entity is this.
		/// </summary>
		public Entity Type { get; }

		/// <summary>
		/// The location of this entity.
		/// </summary>
		public Point Location { get; }

		/// <summary>
		/// Creates a locatable entity.
		/// </summary>
		/// <param name="type">The type of the located entity.</param>
		/// <param name="location">Where it is located.</param>
		public LocatableEntity(Entity type, Point location)
		{
			Type = type;
			Location = location;
		}
	}
}
