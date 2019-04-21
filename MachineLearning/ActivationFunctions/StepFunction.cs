using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning.ActivationFunctions
{
	/// <summary>
	/// Returns either -1 or 1 depending on whether the
	/// input is past a defined threshold.
	///            __________
	///           |
	/// ----------|----------
	/// __________|
	/// </summary>
	public class StepFunction : IActivationFunction
	{
		/// <summary>
		/// The threshold.
		/// </summary>
		public decimal Threshold { get; set; }

		/// <summary>
		/// Returns the result of the step function.
		/// </summary>
		public decimal Calculate(decimal weighedSum)
		{
			return weighedSum > Threshold ? 1 : -1;
		}

		/// <summary>
		/// Creates a new StepFunction calculator.
		/// </summary>
		public StepFunction(decimal threshold)
		{
			Threshold = threshold;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public StepFunction() : this(0) { }
	}
}
