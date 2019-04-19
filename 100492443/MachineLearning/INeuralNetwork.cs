using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.MachineLearning
{
	/// <summary>
	/// Defines an interface for an autonomous
	/// neural network.
	/// </summary>
	interface INeuralNetwork<TInput, TOutput>
		where TInput : ILayer
		where TOutput : IReadOnlyLayer
	{
		/// <summary>
		/// The network inputs.
		/// </summary>
		TInput NetworkInput { get; }

		/// <summary>
		/// The network output.
		/// </summary>
		TOutput NetworkOutput { get; }

		/// <summary>
		/// Runs the current input values through
		/// the networks and updates the network output.
		/// </summary>
		void Feedforward();
	}
}
