using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Represents a perceptron's activation function.
	/// </summary>
	public interface IActivationFunction
	{
		/// <summary>
		/// Runs the function's calculations over the
		/// weighed sum.
		/// </summary>
		/// <param name="weighedSum">The weighed sum of all previous connections.</param>
		/// <returns>The result passed through the activation function.</returns>
		decimal Calculate(decimal weighedSum);
	}
}
