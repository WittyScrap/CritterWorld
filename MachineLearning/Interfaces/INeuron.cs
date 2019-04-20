using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Represents a single perceptron.
	/// </summary>
	public interface INeuron
	{
		/// <summary>
		/// The output value of this neuron.
		/// </summary>
		float Output { get; }

		/// <summary>
		/// Creates a connection between this neuron and
		/// the <paramref name="target"/> neuron. The weight
		/// will be calculated randomly.
		/// </summary>
		/// <param name="target">The neuron to be connected to.</param>
		void Connect(IWorkingNeuron target);

		/// <summary>
		/// Creates a connection between this neuron and
		/// the <paramref name="target"/> neuron, with a
		/// specified connection weight.
		/// </summary>
		/// <param name="target">The target neuron.</param>
		/// <param name="weight">The connection weight.</param>
		void Connect(IWorkingNeuron target, float weight);
	}
}
