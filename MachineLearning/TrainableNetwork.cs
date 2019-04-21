using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning
{
	/// <summary>
	/// Represents a standard Feed-forward neural network.
	/// </summary>
	public class TrainableNetwork : NeuralNetwork<NamedLayer<InputNeuron>, NamedLayer<Neuron<SigmoidFunction>>>
	{
		/// <summary>
		/// Creates an output neuron and binds it to
		/// the given name.
		/// </summary>
		/// <param name="outputName">The name of the output neuron.</param>
		public void CreateOutput(string outputName)
		{
			NetworkOutput[outputName] = new Neuron<SigmoidFunction>();
		}

		/// <summary>
		/// Creates an output layer from the given list of
		/// names.
		/// </summary>
		/// <param name="outputNames">The list of names.</param>
		public void CreateOutput(params string[] outputNames)
		{
			foreach (string outputName in outputNames)
			{
				CreateOutput(outputName);
			}
		}
	}
}
