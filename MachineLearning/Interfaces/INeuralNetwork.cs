using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Represents a network of perceptrons.
	/// </summary>
	interface INeuralNetwork
	{
		/// <summary>
		/// Feeds the input values into the input layer
		/// and forwards them through the rest of the network.
		/// </summary>
		/// <param name="inputValues">The input values to feed through the network.</param>
		void Feedforward(params decimal[] inputValues);
		
		/// <summary>
		/// Returns the output of this network from its output
		/// layer.
		/// </summary>
		/// <returns>The output of this network from its output
		/// layer.</returns>
		decimal[] GetNetworkOutput();
	}
}
