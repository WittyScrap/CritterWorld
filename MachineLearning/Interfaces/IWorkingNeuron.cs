using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Represents a perceptron that has to exist within either a
	/// hidden or output layer.
	/// </summary>
	public interface IWorkingNeuron : INeuron
	{
		/// <summary>
		/// Creates a connection with the <paramref name="source"/> neuron.
		/// </summary>
		/// <param name="source">The neuron to connect from.</param>
		/// <param name="weight">The weight of the new connection.</param>
		void AddConnection(INeuron source, float weight);
	}
}
