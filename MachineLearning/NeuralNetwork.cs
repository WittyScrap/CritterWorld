using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MachineLearning.Interfaces;
using System.IO;
using MachineLearning.Layers;
using MachineLearning.Neurons;
using MachineLearning.ActivationFunctions;
using ISerializable = MachineLearning.Interfaces.ISerializable;

namespace MachineLearning
{
	/// <summary>
	/// Generic neural network model.
	/// </summary>
	[DataContract, KnownType(typeof(Layer<Neuron<SigmoidFunction>>)), KnownType(typeof(Neuron<SigmoidFunction>)), KnownType(typeof(SigmoidFunction))]
	public class NeuralNetwork : INeuralNetwork, ISerializable
	{
		/// <summary>
		/// Event called when the network has updated its output
		/// neurons (aka after a feed-forward).
		/// </summary>
		public event EventHandler NetworkUpdated;

		/// <summary>
		/// The input layer of this neural network.
		/// </summary>
		[DataMember]
		public Layer<Neuron<SigmoidFunction>> InputNeurons { get; private set; } = new Layer<Neuron<SigmoidFunction>>();

		/// <summary>
		/// The various hidden layers of the neural network.
		/// </summary>
		[DataMember]
		public List<Layer<Neuron<SigmoidFunction>>> HiddenNeurons { get; private set; } = new List<Layer<Neuron<SigmoidFunction>>>();

		/// <summary>
		/// The output layer of this neural network.
		/// </summary>
		public Layer<Neuron<SigmoidFunction>> OutputNeurons {
			get
			{
				return HiddenNeurons.Last();
			}
		}

		/// <summary>
		/// Creates a new neural network with a specified configuration.
		/// The configuration defines how many layers and how many neurons
		/// per each layer will exist in this network, for instance using
		/// the format: 3, 5, 5, 4, 7, 1 will create a network with 3 input
		/// neurons, 1 output neuron, and 4 hidden layers containing 5, 5, 4 and
		/// 7 neurons respectively.
		/// </summary>
		/// <param name="networkConfiguration">The network configuration.</param>
		public NeuralNetwork(params int[] networkConfiguration)
		{
			// Create input neurons
			for (int i = 0; i < networkConfiguration[0]; ++i)
			{
				InputNeurons.AddNeuron(new Neuron<SigmoidFunction>());
			}
			// Create hidden and output neurons
			for (int i = 1; i < networkConfiguration.Length; ++i)
			{
				var lastLayer = new Layer<Neuron<SigmoidFunction>>();
				HiddenNeurons.Add(lastLayer);
				for (int j = 0; j < networkConfiguration[i]; ++j)
				{
					lastLayer.AddNeuron(new Neuron<SigmoidFunction>());
				}
			}
			FullyConnect();
		}

		/// <summary>
		/// Fully connects every layer of this neural network.
		/// </summary>
		private void FullyConnect()
		{
			if (HiddenNeurons.Count == 0)
			{
				InputNeurons.Connect(OutputNeurons);
			}
			else
			{
				InputNeurons.Connect(HiddenNeurons[0]);
				for (int i = 0; i < HiddenNeurons.Count - 1; ++i)
				{
					HiddenNeurons[i].Connect(HiddenNeurons[i + 1]);
				}
				HiddenNeurons.Last().Connect(OutputNeurons);
			}
		}

		/// <summary>
		/// Restricts the value to the range 0 - 1.
		/// </summary>
		private decimal Normalize(decimal value)
		{
			return Math.Max(Math.Min(value, 1m), 0m);
		}

		/// <summary>
		/// Feeds the input values into the input layer
		/// and forwards them through the rest of the network.
		/// </summary>
		/// <param name="inputValues">The input values to feed through the network.</param>
		public void Feedforward(params decimal[] inputValues)
		{
			if (inputValues.Length != InputNeurons.Count)
			{
				throw new ArgumentOutOfRangeException("inputValues");
			}

			for (int neuron = 0; neuron < InputNeurons.Count; ++neuron)
			{
				InputNeurons[neuron].Output = Normalize(inputValues[neuron]);
			}

			if (HiddenNeurons.Count != 0)
			{
				foreach (var layer in HiddenNeurons)
				{
					layer.Feedforward();
				}
			}
			OutputNeurons.Feedforward();
			NetworkUpdated?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Returns the output of this network from its output
		/// layer.
		/// </summary>
		/// <returns>The output of this network from its output
		/// layer.</returns>
		public decimal[] GetNetworkOutput()
		{
			decimal[] networkOutput = new decimal[OutputNeurons.Count];
			for (int neuron = 0; neuron < OutputNeurons.Count; ++neuron)
			{
				networkOutput[neuron] = OutputNeurons[neuron].Output;
			}
			return networkOutput;
		}

		/// <summary>
		/// Applies random small mutations to this network
		/// </summary>
		public void Mutate(int mutationIntensity = 2)
		{
			int layersDelta = Randomizer.NextInteger(-mutationIntensity, mutationIntensity + 1);

			while (layersDelta < 0)
			{
				int neuronCount = Randomizer.NextInteger(1, mutationIntensity + 1);
				int location = Randomizer.NextInteger(0, HiddenNeurons.Count - 1);

				RemoveNeurons(location, neuronCount);

				++layersDelta;
			}

			while (layersDelta > 0)
			{
				int neuronCount = Randomizer.NextInteger(1, mutationIntensity + 1);
				int location = Randomizer.NextInteger(0, HiddenNeurons.Count - 1);

				AddNeurons(location, neuronCount, 30);

				--layersDelta;
			}

			MutateConnectionWeights(mutationIntensity);
		}

		/// <summary>
		/// Mutates the weights of all the connectons in the network.
		/// </summary>
		/// <param name="mutationIntensity">The intensity of the mutation.</param>
		private void MutateConnectionWeights(int mutationIntensity)
		{
			foreach (var layer in HiddenNeurons)
			{
				layer.Mutate(mutationIntensity);
			}
		}

		/// <summary>
		/// Adds a specified number of neurons to the specified layer index.
		/// </summary>
		/// <param name="positionIndex">The layer to add neurons to.</param>
		/// <param name="neuronCount">The amunt of neurons to add.</param>
		/// <param name="threshold">The total amount of neurons the layer can carry before overflowing to a new layer.</param>
		private void AddNeurons(int positionIndex, int neuronCount, int threshold)
		{
			if (positionIndex >= 0 && positionIndex < HiddenNeurons.Count - 1)
			{
				if (HiddenNeurons[positionIndex].Count + neuronCount < threshold)
				{
					var previousLayer = positionIndex == 0 ? InputNeurons : HiddenNeurons[positionIndex - 1];
					var nextLayer = HiddenNeurons[positionIndex + 1];
					foreach (var addedNeuron in HiddenNeurons[positionIndex].FillIterative(neuronCount))
					{
						foreach (var targetNeuron in nextLayer)
						{
							addedNeuron.Connect(targetNeuron, Randomizer.NextDecimal(-1m, 1m));
						}
						foreach (var sourceNeuron in previousLayer)
						{
							sourceNeuron.Connect(addedNeuron, Randomizer.NextDecimal(-1m, 1m));
						}
					}
				}
				else
				{
					AddLayer(positionIndex + 1, neuronCount);
				}
			}
		}

		/// <summary>
		/// Adds one additional hidden layer at the specified position.
		/// </summary>
		private void AddLayer(int positionIndex, int neuronCount)
		{
			if (positionIndex >= 0 && positionIndex < HiddenNeurons.Count)
			{
				var previousLayer = positionIndex == 0 ? InputNeurons : HiddenNeurons[positionIndex - 1];
				var currentLayer = HiddenNeurons[positionIndex];
				currentLayer.Disconnect();

				var generatedLayer = new Layer<Neuron<SigmoidFunction>>();
				generatedLayer.Fill(neuronCount);
				HiddenNeurons.Insert(positionIndex, generatedLayer);
				previousLayer.Connect(generatedLayer);
				generatedLayer.Connect(currentLayer);
			}
		}

		/// <summary>
		/// Removes a specified amount of neurons from the specified layer.
		/// </summary>
		/// <param name="positionIndex">The layer from which to remove neurons.</param>
		/// <param name="neuronCount">The amount of neurons to remove.</param>
		private void RemoveNeurons(int positionIndex, int neuronCount)
		{
			if (positionIndex >= 0 && positionIndex < HiddenNeurons.Count - 1)
			{
				var selectedLayer = HiddenNeurons[positionIndex];
				if (selectedLayer.Count > neuronCount)
				{
					selectedLayer.RemoveNeurons(neuronCount);
					HiddenNeurons[positionIndex + 1].Clean();
				}
				else
				{
					RemoveLayer(positionIndex);
				}
			}
		}

		/// <summary>
		/// Removes a layer at the specified index.
		/// </summary>
		/// <param name="positionIndex">The position at which to remove a layer.</param>
		private void RemoveLayer(int positionIndex)
		{
			if (positionIndex >= 0 && positionIndex < HiddenNeurons.Count - 1)
			{
				var nextLayer = HiddenNeurons[positionIndex + 1];
				nextLayer.Disconnect();

				HiddenNeurons.RemoveAt(positionIndex);
				var previousLayer = positionIndex > 0 ? HiddenNeurons[positionIndex - 1] : InputNeurons;
				previousLayer.Connect(nextLayer);
			}
		}

		/// <summary>
		/// Removes the last layer in the hidden layers list.
		/// </summary>
		private void RemoveLast()
		{
			if (HiddenNeurons.Count > 1)
			{
				RemoveLayer(HiddenNeurons.Count - 2);
			}
		}

		/// <summary>
		/// Removes a defined amount of layers from the Hidden layers section.
		/// </summary>
		/// <param name="count">The amount of layers to remove.</param>
		private void RemoveLayers(int count)
		{
			if (count < HiddenNeurons.Count - 1 && count > 0)
			{
				RemoveLast();
				RemoveLayers(count - 1);
			}
		}

		/// <summary>
		/// Serializes this neural network to a string.
		/// </summary>
		/// <returns>A serialized version of this neural network.</returns>
		public string Serialize()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				DataContractSerializer serializer = new DataContractSerializer(GetType(), null, 0x7FFF, false, true, null);
				serializer.WriteObject(memoryStream, this);
				memoryStream.Seek(0, SeekOrigin.Begin);

				using (StreamReader streamReader = new StreamReader(memoryStream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Deserializes a raw formatted neural network into an object.
		/// </summary>
		/// <param name="raw">The XML serialized neural network.</param>
		/// <returns>A deserialized neural network.</returns>
		public static NeuralNetwork Deserialize(string raw)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] data = Encoding.UTF8.GetBytes(raw);
				memoryStream.Write(data, 0, data.Length);
				memoryStream.Position = 0;
				DataContractSerializer deserializer = new DataContractSerializer(typeof(NeuralNetwork));
				return (NeuralNetwork)deserializer.ReadObject(memoryStream);
			}
		}

		/// <summary>
		/// Creates a random neural network with a fixed
		/// number of input and output neurons.
		/// </summary>
		/// <returns>A random neural network.</returns>
		public static NeuralNetwork RandomNetwork(int inputNeurons, int outputNeurons, int minLayers = 0, int maxLayers = 10, int minNeurons = 1, int maxNeurons = 50)
		{
			int networkSize = Randomizer.NextInteger(minLayers, maxLayers);
			List<int> randomNetwork = new List<int>(networkSize);
			for (int i = 0; i < networkSize; ++i)
			{
				randomNetwork.Add(Randomizer.NextInteger(minNeurons, maxNeurons));
			}
			randomNetwork.Insert(0, inputNeurons);
			randomNetwork.Add(outputNeurons);

			return new NeuralNetwork(randomNetwork.ToArray());
		}

		/// <summary>
		/// Creates a random neural network with a fixed
		/// number of output neurons.
		/// </summary>
		/// <returns>A random neural network.</returns>
		public static NeuralNetwork RandomNetwork(int outputNeurons, int minLayers = 0, int maxLayers = 10, int minNeurons = 1, int maxNeurons = 50)
		{
			return RandomNetwork(Randomizer.NextInteger(minNeurons, maxNeurons), outputNeurons, minLayers, maxLayers, minNeurons, maxNeurons);
		}
	}
}
