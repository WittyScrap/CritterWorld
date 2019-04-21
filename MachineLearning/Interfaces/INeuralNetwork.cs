using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Defines an interface for an autonomous
	/// neural network.
	/// </summary>
	public interface INeuralNetwork<TInput, TOutput> 
		where TInput : ILayer<INeuron>
		where TOutput : ILayer<IWorkingNeuron>
	{
		/// <summary>
		/// Represents the network input.
		/// </summary>
		TInput NetworkInput { get; }

		/// <summary>
		/// Represents the network output.
		/// </summary>
		TOutput NetworkOutput { get; }
	}
}
