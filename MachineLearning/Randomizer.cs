using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
	/// <summary>
	/// Provides random functionality to external
	/// classes.
	/// </summary>
	public static class Randomizer
	{
		/// <summary>
		/// The random number generator.
		/// </summary>
		private static Random Generator { get; } = new Random();

		/// <summary>
		/// Thread locker.
		/// </summary>
		private static object Locker { get; } = new object();

		/// <summary>
		/// Returns a non-negative random integer.
		/// </summary>
		/// <returns>A non-negative random integer.</returns>
		public static int NextInteger()
		{
			lock (Locker)
			{
				return Generator.Next();
			}
		}

		/// <summary>
		/// Returns a random integer that is within a specified
		/// range.
		/// </summary>
		/// <param name="minimum">The inclusive lower bound.</param>
		/// <param name="maximum">The exclusive upper bound.</param>
		/// <returns>A random integer that is within a specified
		/// range.</returns>
		public static int NextInteger(int minimum, int maximum)
		{
			lock (Locker)
			{
				return Generator.Next(minimum, maximum);
			}
		}

		/// <summary>
		/// Returns a random decimal number that is more than 0.0 and less than 1.0.
		/// </summary>
		/// <returns>A non-negative random decimal number.</returns>
		public static decimal NextDecimal()
		{
			lock (Locker)
			{
				return (decimal)Generator.NextDouble();
			}
		}

		/// <summary>
		/// Returns a random decimal number within the specified range.
		/// </summary>
		/// <param name="minimum">The lower bound.</param>
		/// <param name="maximum">The upper bound.</param>
		/// <returns>A random decimal number within the specified range.</returns>
		public static decimal NextDecimal(decimal minimum, decimal maximum)
		{
			lock (Locker)
			{
				return NextDecimal() * (maximum - minimum) + minimum;
			}
		}
	}
}
