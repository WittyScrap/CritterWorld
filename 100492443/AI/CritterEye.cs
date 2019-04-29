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
		public enum TileContents
		{
			Empty = 0,
			Terrain = 1 << 0,
			Gift = 1 << 1,
			Food = 1 << 2,
			Bomb = 1 << 3,
			EscapeHatch = 1 << 4
		}

		/// <summary>
		/// The precision (or "resolution") of the eye
		/// is the amount of rays fired outwards from the
		/// center of the critter.
		/// </summary>
		public int Precision { get; }

		/// <summary>
		/// Returns a sequence of rays given this eye's accuracy.
		/// </summary>
		/// <returns>This eye's "retina".</returns>
		public IEnumerable<Ray> GetRays(Point critterLocation, Vector critterForward)
		{
			for (double angle = 0; angle < 2 * Math.PI; angle += Math.PI * 2 / Precision)
			{
				yield return new Ray(critterLocation, critterForward.Rotated(angle));
			}
		}

		/// <summary>
		/// Returns one specific ray.
		/// </summary>
		public Ray GetRay(Point critterLocation, Vector critterForward, int rayID)
		{
			if (rayID < 0 || rayID >= Precision)
			{
				throw new ArgumentOutOfRangeException("rayID");
			}
			double angle = (Math.PI * 2 / Precision) * rayID;
			return new Ray(critterLocation, critterForward.Rotated(angle));
		}

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
