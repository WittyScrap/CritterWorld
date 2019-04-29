using CritterRobots.Critters;
using System.Drawing;

namespace CritterRobots.AI
{
	/// <summary>
	/// Represents a series of raycast that are able to detect
	/// objects of a certain type.
	/// </summary>
	public partial class CritterEye
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
	}
}
