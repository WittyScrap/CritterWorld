using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.ActivationFunctions;
using MachineLearning.Interfaces;
using MachineLearning.Neurons;

namespace MachineLearning.Layers
{
	/// <summary>
	/// Represents a single layer of perceptrons.
	/// </summary>
	[DataContract]
	public class Layer<TNeuron> : ILayer<TNeuron> where TNeuron : INeuron, new()
	{
		/// <summary>
		/// Container for every neuron in this layer.
		/// </summary>
		[DataMember]
		private List<TNeuron> InternalNeurons { get; set; } = new List<TNeuron>();

		/// <summary>
		/// Retrieves a neuron with a given index.
		/// </summary>
		/// <param name="neuronID">The index of the neuron.</param>
		/// <returns>A neuron from the given index.</returns>
		public TNeuron this[int neuronID] {
			get
			{
				if (neuronID < 0 || neuronID >= Count)
				{
					throw new ArgumentOutOfRangeException("neuronID");
				}
				return InternalNeurons[neuronID];
			}
		}

		/// <summary>
		/// The number of neurons in this layer.
		/// </summary>
		public int Count {
			get
			{
				return InternalNeurons.Count;
			}
		}

		public bool IsEmpty {
			get
			{
				return Count == 0;
			}
		}

		/// <summary>
		/// Connects every neuron in this layer to every neuron
		/// in the target layer. The connection weights will be
		/// randomised.
		/// </summary>
		/// <param name="other">The target layer.</param>
		public void Connect(ILayer<TNeuron> other)
		{
			foreach (TNeuron neuron in this)
			{
				foreach (TNeuron targetNeuron in other)
				{
					neuron.Connect(targetNeuron, Randomizer.NextDecimal(-1.0m, 1.0m));
				}
			}
		}

		/// <summary>
		/// Clears the list of connections for each neuron.
		/// </summary>
		public void Disconnect()
		{
			foreach (TNeuron neuron in this)
			{
				neuron.Disconnect();
			}
		}

		/// <summary>
		/// Runs the calculations on all the neurons in this layer.
		/// </summary>
		public void Feedforward()
		{
			foreach (TNeuron neuron in this)
			{
				neuron.Calculate();
			}
		}

		/// <summary>
		/// Iteratively returns every neuron in this layer.
		/// </summary>
		public IEnumerator<TNeuron> GetEnumerator()
		{
			foreach (TNeuron neuron in InternalNeurons)
			{
				yield return neuron;
			}
		}

		/// <summary>
		/// Iteratively returns every neuron in this layer.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Connects every neuron in this layer to those in the
		/// target layer.
		/// </summary>
		/// <param name="target">The target layer.</param>
		/// <param name="weight">The weight of each connection.</param>
		public void Connect(ILayer<TNeuron> target, decimal weight)
		{
			foreach (IConnectable<ILayer<TNeuron>> connectableNeuron in this)
			{
				connectableNeuron.Connect(target, weight);
			}
		}

		/// <summary>
		/// Randomly mutates connections within the neurons
		/// in this layer.
		/// </summary>
		public void Mutate(int mutationIntensity)
		{
			foreach (var neuron in this)
			{
				neuron.Mutate(mutationIntensity);
			}
		}

		/// <summary>
		/// Adds a new neuron to this network.
		/// </summary>
		/// <param name="neuron">The neuron to be added.</param>
		public void AddNeuron(TNeuron neuron)
		{
			InternalNeurons.Add(neuron);
		}

		/// <summary>
		/// Removes a specified amount of neurons from this layer.
		/// </summary>
		/// <param name="neuronCount">The amount of neurons to remove.</param>
		public void RemoveNeurons(int neuronCount)
		{
			if (neuronCount > 0 && neuronCount <= Count)
			{
				InternalNeurons[InternalNeurons.Count - 1].Destroy();
				InternalNeurons.RemoveAt(InternalNeurons.Count - 1);
				RemoveNeurons(neuronCount - 1);
			}
		}

		/// <summary>
		/// Removes dangling connections to neurons that no longer
		/// exist.
		/// </summary>
		public void Clean()
		{
			foreach (TNeuron neuron in this)
			{
				neuron.Clean();
			}
		}

		/// <summary>
		/// Adds the specified amount of neurons to this layer.
		/// </summary>
		/// <param name="neuronCount">The amount of neurons to add.</param>
		public void Fill(int neuronCount)
		{
			while (neuronCount > 0)
			{
				AddNeuron(new TNeuron());
				--neuronCount;
			}
		}

		/// <summary>
		/// Adds the specified amount of neurons to this layer.
		/// </summary>
		/// <param name="neuronCount">The amount of neurons to add.</param>
		public IEnumerable<TNeuron> FillIterative(int neuronCount)
		{
			while (neuronCount > 0)
			{
				TNeuron newNeuron = new TNeuron();
				AddNeuron(newNeuron);
				--neuronCount;

				yield return newNeuron;
			}
		}
	}
}
