using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Represents a group of perceptrons.
	/// </summary>
	public interface ILayer<TNeuron> : IReadOnlyCollection<TNeuron>, IConnectable<ILayer<TNeuron>> where TNeuron : INeuron
	{
		/// <summary>
		/// Runs the values of any neuron connected to the ones
		/// in this layer through their respective activation
		/// functions.
		/// </summary>
		void Feedforward();

		/// <summary>
		/// Retrieves the neuron at the specified location.
		/// </summary>
		/// <param name="neuronID">The neuron index.</param>
		/// <returns>The neuron at the specified location.</returns>
		TNeuron this[int neuronID] { get; }

		/// <summary>
		/// Adds a new neuron to this network.
		/// </summary>
		/// <param name="neuron">The neuron to be added.</param>
		void AddNeuron(TNeuron neuron);
	}
}
