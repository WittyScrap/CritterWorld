using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MachineLearning.Interfaces;
using System.Xml;
using System.IO;

namespace MachineLearning
{
	/// <summary>
	/// Generic neural network model.
	/// </summary>
	public abstract class NeuralNetwork<TInput, TOutput> : INeuralNetwork<TInput, TOutput>
		where TInput : ILayer<INeuron>, new()
		where TOutput : ILayer<IWorkingNeuron>, new()
	{
		/// <summary>
		/// The network's own randomizer.
		/// </summary>
		private static Random Randomizer { get; } = new Random();

		/// <summary>
		/// Internal hidden neurons.
		/// </summary>
		private List<Layer<Neuron<SigmoidFunction>>> HiddenLayers { get; } = new List<Layer<Neuron<SigmoidFunction>>>();

		/// <summary>
		/// The input layer for this network.
		/// </summary>
		public TInput NetworkInput { get; } = new TInput();

		/// <summary>
		/// Represents the output layer for this network.
		/// </summary>
		public TOutput NetworkOutput { get; } = new TOutput();

		/// <summary>
		/// Sequencially returns every layer in this network in reverse order,
		/// starting from the last layer in the hidden layers, going through all the
		/// hidden layers and ending on the input layer.
		/// </summary>
		private IEnumerable<ILayer<INeuron>> GetWorkingLayers()
		{
			for (int index = HiddenLayers.Count - 1; index >= 0; --index)
			{
				yield return HiddenLayers[index];
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
				HiddenLayers.Add(lastLayer);
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

		/// <summary>
		/// Serializes this class and returns a string
		/// equivalent of it.
		/// </summary>
		/// <returns>The serialized version of this class.</returns>
		public virtual string Serialize()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				var serializer = new DataContractSerializer(GetType(), null, 0x7FFF, false, true, null);
				serializer.WriteObject(stream, this);
				stream.Seek(0, SeekOrigin.Begin);

				using (StreamReader reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Loads the contents of this class from
		/// a serialized string.
		/// </summary>
		/// <param name="raw">The source serialized string.</param>
		/// <returns>A deserialized version of the <paramref name="raw"/> source.</returns>
		public static NeuralNetwork<TInput, TOutput> Deserialize(string raw)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				byte[] data = Encoding.UTF8.GetBytes(raw);
				stream.Write(data, 0, data.Length);
				stream.Position = 0;
				DataContractSerializer deserializer = new DataContractSerializer(typeof(NeuralNetwork<TInput, TOutput>));
				return (NeuralNetwork<TInput, TOutput>)deserializer.ReadObject(stream);
			}
		}
	}
}
