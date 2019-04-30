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
	/// Functions as a ray with a "thickness".
	/// </summary>
	struct Beam
	{
		/// <summary>
		/// The minimum values inside which an object is considered
		/// within the beam.
		/// </summary>
		public Point Minimum { get; set; }

		/// <summary>
		/// The maximum values under which an object is considered
		/// within the beam.
		/// </summary>
		public Point Maximum { get; set; }

		/// <summary>
		/// Creates a beam from a minimum point and
		/// a maximum point.
		/// </summary>
		/// <param name="minimum">The beam's minimum point.</param>
		/// <param name="maximum">The beam's maximum point.</param>
		public Beam(Point minimum, Point maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		/// <summary>
		/// Checks whether or not a given point is inside this beam.
		/// </summary>
		/// <param name="point">The point to check for.</param>
		/// <returns>True if the point is inside the beam, false otherwise.</returns>
		public bool Contains(Point point)
		{
			return	point.X > Minimum.X &&
					point.Y > Minimum.Y &&
					point.X < Maximum.X &&
					point.Y < Maximum.Y;
		}
	}
}
