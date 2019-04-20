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
	public interface ILayer<out TNeuron> where TNeuron : INeuron
	{
		/// <summary>
		/// Iteratively returns every neuron in this layer.
		/// </summary>
		IEnumerable<TNeuron> GetNeurons();

		/// <summary>
		/// Connects this layer to a different layer.
		/// </summary>
		/// <param name="other">The target layer</param>
		void Connect(ILayer<IWorkingNeuron> other);
	}
}
