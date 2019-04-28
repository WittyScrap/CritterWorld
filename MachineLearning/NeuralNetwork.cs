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
				InputNeurons[neuron].Output = inputValues[neuron];
			}

			if (HiddenNeurons.Count != 0)
			{
				foreach (var layer in HiddenNeurons)
				{
					layer.Feedforward();
				}
			}
			OutputNeurons.Feedforward();
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
		/// Mutates randomly the amount of layers in the
		/// network.
		/// </summary>
		private void MutateLayerNumber(int maxNewNeurons, int mutationIntensity)
		{
			int layerNumberMutation = Randomizer.NextInteger(-mutationIntensity, mutationIntensity);
			if (layerNumberMutation < 0)
			{
				bool networkChanged = false;
				for (int i = 0; i < -layerNumberMutation && HiddenNeurons.Count > 1; ++i)
				{
					HiddenNeurons.RemoveAt(0);
					networkChanged = true;
				}
				if (networkChanged)
				{
					InputNeurons.Connect(HiddenNeurons[0]);
				}
			}
			else if (layerNumberMutation > 0)
			{
				var outputLayer = OutputNeurons;
				outputLayer.Disconnect();

				// Remove output layer...
				HiddenNeurons.RemoveAt(HiddenNeurons.Count - 1);
				int newLayerSize = Randomizer.NextInteger(1, maxNewNeurons);

				var lastLayer = HiddenNeurons.Count > 0 ? HiddenNeurons[HiddenNeurons.Count - 1] : InputNeurons;

				for (int i = 0; i < layerNumberMutation; ++i)
				{
					var newLayer = new Layer<Neuron<SigmoidFunction>>();
					HiddenNeurons.Add(newLayer);
					for (int j = 0; j < newLayerSize; ++j)
					{
						newLayer.AddNeuron(new Neuron<SigmoidFunction>());
					}
					lastLayer.Connect(newLayer);
					lastLayer = newLayer;
				}

				// Add back output layer...
				HiddenNeurons.Add(outputLayer);
				lastLayer.Connect(outputLayer);
			}
		}

		/// <summary>
		/// Mutates the existing layers (except for the output layer) without
		/// changing the number of layers.
		/// </summary>
		private void MutateInternalNetwork(int mutationIntensity)
		{
			var previousLayer = InputNeurons;
			for (int hiddenLayerID = 0; hiddenLayerID < HiddenNeurons.Count; ++hiddenLayerID)
			{
				var currentLayer = HiddenNeurons[hiddenLayerID];
				for (int previousLayerNeuronID = 0; previousLayerNeuronID < previousLayer.Count; ++previousLayerNeuronID)
				{
					if (Randomizer.NextDecimal() > 0.5m)
					{
						for (int currentLayerNeuronID = 0; currentLayerNeuronID < currentLayer.Count; ++currentLayerNeuronID)
						{
							var previousLayerNeuron = previousLayer[previousLayerNeuronID];
							var currentLayerNeuron = currentLayer[currentLayerNeuronID];

							decimal weightMutationIntensity = mutationIntensity / 10m;
							decimal weightDelta = Randomizer.NextDecimal(-weightMutationIntensity, weightMutationIntensity);

							decimal weight = currentLayerNeuron.Connections[previousLayerNeuron];
							decimal updatedWeight = weight + weightDelta;

							currentLayerNeuron.UpdateConnectionWeight(previousLayerNeuron, updatedWeight);
						}
					}
				}
				previousLayer = currentLayer;
			}
		}

		/// <summary>
		/// Applies random small mutations to this network
		/// </summary>
		public void Mutate(int maxNewNeurons = 50, int mutationIntensity = 2)
		{
			MutateLayerNumber(maxNewNeurons, mutationIntensity);
			MutateInternalNetwork(mutationIntensity);
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
