using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Critters
{
	public struct Vector
	{
		/// <summary>
		/// A zero sized vector.
		/// </summary>
		public static Vector Zero {
			get
			{
				return new Vector(0, 0);
			}
		}

		/// <summary>
		/// A vector with all of its components
		/// set to 1.
		/// </summary>
		public static Vector One {
			get
			{
				return new Vector(1, 1);
			}
		}

		/// <summary>
		/// Creates a new vector from two components.
		/// </summary>
		/// <param name="x">The X component of the vector.</param>
		/// <param name="y">The Y component of the vector.</param>
		public Vector(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// The X component of this vector.
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// The Y component of this vector.
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// The magnitude of this vector, squared.
		/// </summary>
		public double SqrMagnitude
		{
			get
			{
				return X * X + Y * Y;
			}
		}

		/// <summary>
		/// The magnitude of this vector.
		/// </summary>
		public double Magnitude 
		{
			get
			{
				return Math.Sqrt(SqrMagnitude);
			}
		}
	}
}
