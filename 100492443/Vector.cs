using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Critters
{
	public struct Vector
	{
		/// <summary>
		/// (0, 1).
		/// </summary>
		public static Vector Up => new Vector(0, 1);

		/// <summary>
		/// (0, -1).
		/// </summary>
		public static Vector Down => -Up;

		/// <summary>
		/// (1, 0).
		/// </summary>
		public static Vector Right => new Vector(1, 0);

		/// <summary>
		/// (-1, 0).
		/// </summary>
		public static Vector Left => -Right;

		/// <summary>
		/// A vector with all of its components
		/// set to 0.
		/// </summary>
		public static Vector Zero => new Vector(0, 0);

		/// <summary>
		/// A vector with all of its components
		/// set to 1.
		/// </summary>
		public static Vector One => new Vector(1, 1);

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

		/// <summary>
		/// A normalized copy of this vector.
		/// </summary>
		public Vector Normalized {
			get
			{
				Vector copy = new Vector(X, Y);
				copy.Normalize();
				return copy;
			}
		}

		/// <summary>
		/// Returns a copy of this vector, rotated by the given radians angle.
		/// </summary>
		/// <param name="radiansAngle">The angle to rotate the copy of this vector for.</param>
		public Vector Rotated(double radiansAngle)
		{
			Vector rotated = new Vector();
			rotated.X = X * Math.Cos(radiansAngle) - Y * Math.Sin(radiansAngle);
			rotated.Y = X * Math.Sin(radiansAngle) + Y * Math.Cos(radiansAngle);

			return rotated;
		}

		/// <summary>
		/// Turns this vector into a unit vector.
		/// </summary>
		public void Normalize()
		{
			double magnitude = Magnitude;

			X /= magnitude;
			Y /= magnitude;
		}

		/// <summary>
		/// Returns the dot product between the
		/// two vectors.
		/// </summary>
		public static double Dot(Vector lhs, Vector rhs)
		{
			return lhs.X * rhs.X + lhs.Y * rhs.Y;
		}

		/// <summary>
		/// Returns the angle in radians between
		/// two vectors.
		/// </summary>
		public static double Angle(Vector lhs, Vector rhs)
		{
			double angleCosine = Dot(lhs, rhs) / (lhs.Magnitude * rhs.Magnitude);
			return Math.Acos(angleCosine);
		}

		/// <summary>
		/// Returns the full angle (reaches past 180 degrees) 
		/// in radians between two vectors.
		/// </summary>
		public static double FullAngle(Vector lhs, Vector rhs)
		{
			double angle = Math.Atan2(lhs.Y, lhs.X) - Math.Atan2(rhs.Y, rhs.X);
			if (angle < 0)
			{
				angle += 2 * Math.PI;
			}
			return angle;
		}

		/// <summary>
		/// Negates the vector.
		/// </summary>
		public static Vector operator-(Vector vector)
		{
			return vector * -1;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		public static Vector operator+(Vector lhs, Vector rhs)
		{
			return new Vector(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		public static Vector operator+(Point lhs, Vector rhs)
		{
			return new Vector(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		public static Vector operator+(Vector lhs, Point rhs)
		{
			return new Vector(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		public static Vector operator-(Vector lhs, Vector rhs)
		{
			return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		public static Vector operator-(Point lhs, Vector rhs)
		{
			return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		public static Vector operator-(Vector lhs, Point rhs)
		{
			return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		/// <summary>
		/// Multiplies a vector by a scalar.
		/// </summary>
		public static Vector operator*(Vector lhs, float constant)
		{
			return new Vector(lhs.X * constant, lhs.Y * constant);
		}

		/// <summary>
		/// Multiplies a vector by another vector.
		/// </summary>
		public static Vector operator*(Vector lhs, Vector rhs)
		{
			return new Vector(lhs.X * rhs.X, lhs.Y * rhs.Y);
		}

		/// <summary>
		/// Converts this vector into an equivalent point.
		/// </summary>
		public static explicit operator Point(Vector source)
		{
			return new Point((int)source.X, (int)source.Y);
		}

		/// <summary>
		/// Converts a point into an equivalent vector.
		/// </summary>
		public static explicit operator Vector(Point source)
		{
			return new Vector(source.X, source.Y);
		}

		/// <summary>
		/// Converts this vector to the {X=<x-coord>,Y=<y-coord>} format.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "{X=" + X + ",Y=" + Y + "}";
		}
	}
}
