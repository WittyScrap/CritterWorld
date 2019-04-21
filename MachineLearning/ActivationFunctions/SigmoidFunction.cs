using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning.ActivationFunctions
{
	/// <summary>
	/// Activation function: sigmoid;
	/// 
	///     1
	/// ---------
	///       -x
	///  1 + e
	/// 
	/// </summary>
	public class SigmoidFunction : IActivationFunction
	{
		/// <summary>
		/// Returns the result of the function.
		/// </summary>
		/// <param name="weighedSum">The weighed sum input.</param>
		/// <returns>The function's result.</returns>
		public decimal Calculate(decimal weighedSum)
		{
			return 1 / (1 + (decimal)Math.Pow(Math.E, (double)-weighedSum));
		}
	}
}
