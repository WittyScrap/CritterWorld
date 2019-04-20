using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning
{
	/// <summary>
	/// Collection of neurons
	/// </summary>
	class Layer<TNeuron> : HashSet<TNeuron>, ILayer<TNeuron> where TNeuron : INeuron, new()
	{
		/// <summary>
		/// Neuron count randomizer.
		/// </summary>
		private static Random Randomizer { get; } = new Random();

		/// <summary>
		/// Connects this layer to a different layer.
		/// </summary>
		/// <param name="other">The target layer.</param>
		public void Connect(ILayer<IWorkingNeuron> other)
		{
			foreach (INeuron neuron in this)
			{
				foreach (IWorkingNeuron workingNeuron in other.GetNeurons())
				{
					neuron.Connect(workingNeuron);
				}
			}
		}

		/// <summary>
		/// Fills this collection with neurons.
		/// </summary>
		/// <param name="neuronCount">The number of neurons to create.</param>
		public void Fill(int neuronCount)
		{
			for (int i = 0; i < neuronCount; ++i)
			{
				Add(new TNeuron());
			}
		}

		/// <summary>
		/// Fills this collection with a random
		/// number of neurons;
		/// </summary>
		public void FillRandom(int minNeurons, int maxNeurons)
		{
			Fill(Randomizer.Next(minNeurons, maxNeurons + 1));
		}

		/// <summary>
		/// Iteratively reutrns all neurons in this layer.
		/// </summary>
		public IEnumerable<TNeuron> GetNeurons()
		{
			foreach (var element in this)
			{
				yield return element;
			}
		}
	}
}
