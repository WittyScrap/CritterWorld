using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;
using MachineLearning.Neurons;

namespace MachineLearning.Layers
{
	/// <summary>
	/// Represents a single layer of perceptrons.
	/// </summary>
	[DataContract]
	public class Layer<TNeuron> : ILayer<TNeuron> where TNeuron : INeuron
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
					neuron.Connect(targetNeuron, Randomizer.Nextdecimal(-1.0m, 1.0m));
				}
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
		/// Adds a new neuron to this network.
		/// </summary>
		/// <param name="neuron">The neuron to be added.</param>
		public void AddNeuron(TNeuron neuron)
		{
			InternalNeurons.Add(neuron);
		}
	}
}
