using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning
{
	/// <summary>
	/// Represents an input-only perceptron.
	/// </summary>
	public class InputNeuron : INeuron
	{
		/// <summary>
		/// Internal randomizer for random weights.
		/// </summary>
		private static Random Randomizer { get; } = new Random();

		/// <summary>
		/// The neuron's output value.
		/// </summary>
		public float Output { get; set; }

		/// <summary>
		/// Creates a connection between this neuron and
		/// the <paramref name="target"/> neuron. The weight
		/// will be calculated randomly.
		/// </summary>
		/// <param name="target">The neuron to be connected to.</param>
		public void Connect(IWorkingNeuron target)
		{
			// NextDouble() yields a result in the range of 0 / 1, multiplying
			// by 2 and subtracting 1 changes the range to a more weight appropriate
			// -1 / 1
			Connect(target, (float)(Randomizer.NextDouble() * 2 - 1));
		}
		
		/// <summary>
		/// Creates a connection between this neuron and
		/// the <paramref name="target"/> neuron, with a
		/// specified connection weight.
		/// </summary>
		/// <param name="target">The target neuron.</param>
		/// <param name="weight">The connection weight.</param>
		public void Connect(IWorkingNeuron target, float weight)
		{
			target.AddConnection(this, weight);
		}

		/// <summary>
		/// Creates an input neuron with a default initial value.
		/// </summary>
		/// <param name="initialValue">The initial value of this input neuron.</param>
		public InputNeuron(float initialValue)
		{
			Output = initialValue;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public InputNeuron() : this(0.0f)
		{ }
	}
}
