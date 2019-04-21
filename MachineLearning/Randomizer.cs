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
	internal static class Randomizer
	{
		/// <summary>
		/// The random number generator.
		/// </summary>
		private static Random Generator { get; } = new Random();

		/// <summary>
		/// Returns a non-negative random integer.
		/// </summary>
		/// <returns>A non-negative random integer.</returns>
		public static int NextInteger()
		{
			return Generator.Next();
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
			return Generator.Next(minimum, maximum);
		}

		/// <summary>
		/// Returns a random decimaling point number that is more than 0.0 and less than 1.0.
		/// </summary>
		/// <returns>A non-negative random decimaling point number.</returns>
		public static decimal Nextdecimal()
		{
			return (decimal)Generator.NextDouble();
		}

		/// <summary>
		/// Returns a random decimaling point number within the specified range.
		/// </summary>
		/// <param name="minimum">The lower bound.</param>
		/// <param name="maximum">The upper bound.</param>
		/// <returns>A random decimaling point number within the specified range.</returns>
		public static decimal Nextdecimal(decimal minimum, decimal maximum)
		{
			return Nextdecimal() * (maximum - minimum) + minimum;
		}
	}
}
