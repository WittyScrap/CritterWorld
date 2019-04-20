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
	public class TrainableNetwork : INeuralNetwork<NamedLayer<InputNeuron>, NamedLayer<Neuron<SigmoidFunction>>>
	{
		/// <summary>
		/// The network's own randomizer.
		/// </summary>
		private static Random Randomizer { get; } = new Random();
		
		/// <summary>
		/// Internal hidden neurons.
		/// </summary>
		private LinkedList<Layer<Neuron<SigmoidFunction>>> HiddenLayers { get; } = new LinkedList<Layer<Neuron<SigmoidFunction>>>();

		/// <summary>
		/// The input layer for this network.
		/// </summary>
		public NamedLayer<InputNeuron> NetworkInput { get; } = new NamedLayer<InputNeuron>();

		/// <summary>
		/// Represents the output layer for this network.
		/// </summary>
		public NamedLayer<Neuron<SigmoidFunction>> NetworkOutput { get; } = new NamedLayer<Neuron<SigmoidFunction>>();

		/// <summary>
		/// Sequencially returns every layer in this network in reverse order,
		/// starting from the last layer in the hidden layers, going through all the
		/// hidden layers and ending on the input layer.
		/// </summary>
		private IEnumerable<ILayer<INeuron>> GetWorkingLayers()
		{
			for (var layerNode = HiddenLayers.Last; layerNode != null; layerNode = layerNode.Previous)
			{
				yield return layerNode.Value;
			}
			yield return NetworkInput;
		}

		/// <summary>
		/// Walks through every layer in the network, starting from the
		/// Input layer and ending on the Output layer.
		/// </summary>
		private IEnumerable<ILayer<INeuron>> GetAllLayers()
		{
			yield return NetworkInput;
			foreach (var layer in HiddenLayers)
			{
				yield return layer;
			}
			yield return NetworkOutput;
		}

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

		/// <summary>
		/// Randomly fills and strictly connects this network
		/// based on given parameters.
		/// </summary>
		/// <param name="minLayers">The minimum number of hidden layers. Note that the input and output layers will be left untouched.</param>
		/// <param name="maxLayers">The maximum number of hidden layers. Note that the input and output layers will be left untouched.</param>
		/// <param name="minNeuronsPerLayer">The minimum amount of neurons per layer. This is NOT the minimum number of neurons for each layer, it's the minimum number of neurons each layer can have.</param>
		/// <param name="maxNeuronsPerLayer">The maximum amount of neurons per layer. This is NOT the maximum number of neurons for each layer, it's the maximum number of neurons each layer can have.</param>
		public void Randomize(int minLayers, int maxLayers, int minNeuronsPerLayer, int maxNeuronsPerLayer)
		{
			int hiddenLayerCount = Randomizer.Next(minLayers, maxLayers + 1);
			for (int layerID = 0; layerID < hiddenLayerCount; ++layerID)
			{
				var lastLayer = new Layer<Neuron<SigmoidFunction>>();
				HiddenLayers.AddLast(lastLayer);
				lastLayer.FillRandom(minNeuronsPerLayer, maxNeuronsPerLayer);
			}
			Connect();
		}

		/// <summary>
		/// Connects the entire network together.
		/// </summary>
		public void Connect()
		{
			ILayer<IWorkingNeuron> nextLayer = NetworkOutput;
			foreach (var networkLayer in GetWorkingLayers())
			{
				networkLayer.Connect(nextLayer);
				if (networkLayer is ILayer<IWorkingNeuron> workingLayer)
				{
					nextLayer = workingLayer;
				}
			}
		}

		/// <summary>
		/// Walks through the network layers and returns a string based
		/// map of the network.
		/// </summary>
		/// <returns>A string based map of the network.</returns>
		public string Walk()
		{
			StringBuilder mapBuilder = new StringBuilder();
			foreach (var layer in GetAllLayers().Reverse())
			{
				foreach (var neuron in layer.GetNeurons())
				{
					mapBuilder.Append(" {" + neuron.Output + "} ");
				}
				mapBuilder.AppendLine();
			}
			return mapBuilder.ToString();
		}
	}
}
