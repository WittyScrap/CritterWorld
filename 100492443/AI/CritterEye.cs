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
	public partial class CritterEye
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
		/// The precision (or "resolution") of the eye
		/// is the amount of rays fired outwards from the
		/// center of the critter.
		/// </summary>
		public int Precision { get; }

		/// <summary>
		/// Creates a new critter eye.
		/// </summary>
		/// <param name="precision">The density of the eye's "retina".</param>
		public CritterEye(int precision)
		{
			Precision = precision;
		}
	}
}
