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
	public interface INeuron : IConnectable<INeuron>, IConnectable<ILayer<INeuron>>, IDisconnectable, IConnectionTarget<INeuron>, IDestroyable
	{
		/// <summary>
		/// The output value of this neuron.
		/// </summary>
		decimal Output { get; }

		/// <summary>
		/// Calculates the new value for this neuron.
		/// </summary>
		void Calculate();

		/// <summary>
		/// Mutates the connections for this neuron.
		/// </summary>
		void Mutate(int mutationIntensity);
	}
}
