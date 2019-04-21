using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	public interface IConnectionTarget<in TConnectionTarget>
	{
		/// <summary>
		/// Creates a connection with the <paramref name="source"/> neuron.
		/// </summary>
		/// <param name="source">The neuron to connect from.</param>
		/// <param name="weight">The weight of the new connection.</param>
		void AddConnection(TConnectionTarget source, decimal weight);
	}
}
