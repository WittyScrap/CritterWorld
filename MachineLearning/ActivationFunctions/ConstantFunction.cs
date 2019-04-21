using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning.ActivationFunctions
{
	/// <summary>
	/// Always returns 1.
	/// </summary>
	public class ConstantFunction : IActivationFunction
	{
		/// <summary>
		/// Always returns 1.
		/// </summary>
		/// <returns>1.</returns>
		public decimal Calculate(decimal weighedSum)
		{
			return 1;
		}
	}
}
