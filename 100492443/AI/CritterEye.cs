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
	public class CritterEye
	{
		/// <summary>
		/// Represents a single eye ray.
		/// </summary>
		public struct Ray
		{
			/// <summary>
			/// The origin of the ray.
			/// </summary>
			public Point Origin { get; set; }

			/// <summary>
			/// The direction of the ray.
			/// </summary>
			public Vector Direction { get; set; }

			/// <summary>
			/// Constructs a new ray.
			/// </summary>
			/// <param name="origin">The origin point of the ray.</param>
			/// <param name="direction">The ray's direction.</param>
			public Ray(Point origin, Vector direction)
			{
				Origin = origin;
				Direction = direction;
			}
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
		public IEnumerable<Ray> GetRays(Point critterLocation)
		{
			for (double angle = 0; angle < 2 * Math.PI; angle += Math.PI * 2 / Precision)
			{
				yield return new Ray(critterLocation, new Vector(Math.Cos(angle), Math.Sin(angle)));
			}
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
