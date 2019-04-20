using MachineLearning.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
	/// <summary>
	/// Represents a group of perceptrons that can be addressed by name.
	/// </summary>
	public class NamedLayer<TNeuron> : ConcurrentDictionary<string, TNeuron>, ILayer<TNeuron> where TNeuron : INeuron, new()
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
			foreach (INeuron neuron in GetNeurons())
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
		public void Fill(params string[] names)
		{
			for (int i = 0; i < names.Length; ++i)
			{
				base[names[i]] = new TNeuron();
			}
		}

		/// <summary>
		/// Iteratively reutrns all neurons in this layer.
		/// </summary>
		public IEnumerable<TNeuron> GetNeurons()
		{
			foreach (var element in this)
			{
				yield return element.Value;
			}
		}
	}
}