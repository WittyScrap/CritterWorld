using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning
{
	/// <summary>
	/// Represents a simple working perceptron.
	/// </summary>
	public class Neuron<TActivationFunction> : IWorkingNeuron where TActivationFunction : IActivationFunction, new()
	{
		/// <summary>
		/// Weight randomizer.
		/// </summary>
		private static Random Randomizer { get; } = new Random();

		/// <summary>
		/// The neuron's output.
		/// </summary>
		public float Output { get => CalculateNeuronOutput(); }

		/// <summary>
		/// The weighed sum of all incoming connections.
		/// </summary>
		private float WeighedSum {
			get
			{
				float sum = 0.0f;
				foreach (Connection connection in Connections)
				{
					sum += connection.Source.Output * connection.Weight;
				}
				return sum;
			}
		}

		/// <summary>
		/// This neuron's activation function.
		/// </summary>
		private TActivationFunction ActivationFunction { get; } = new TActivationFunction();

		/// <summary>
		/// List of connections.
		/// </summary>
		private LinkedList<Connection> Connections { get; } = new LinkedList<Connection>();

		/// <summary>
		/// Calculates the weighed sum of all incoming connections
		/// and passes it through the activation function.
		/// </summary>
		/// <returns></returns>
		private float CalculateNeuronOutput()
		{
			return ActivationFunction.Calculate(WeighedSum);
		}

		/// <summary>
		/// Creates a connection with the <paramref name="source"/> neuron.
		/// </summary>
		/// <param name="source">The neuron to connect from.</param>
		/// <param name="weight">The weight of the new connection.</param>
		public void AddConnection(INeuron source, float weight)
		{
			Connections.AddLast(new Connection(source, weight));
		}

		/// <summary>
		/// Creates a connection between this neuron and
		/// the <paramref name="target"/> neuron. The weight
		/// will be calculated randomly.
		/// </summary>
		/// <param name="target">The neuron to be connected to.</param>
		public void Connect(IWorkingNeuron target)
		{
			// NextDouble() yields a result in the range of 0 / 1, multiplying
			// by 2 and subtracting 1 changes the range to a more weight appropriate
			// -1 / 1
			Connect(target, (float)(Randomizer.NextDouble() * 2 - 1));
		}

		/// <summary>
		/// Creates a connection between this neuron and
		/// the <paramref name="target"/> neuron, with a
		/// specified connection weight.
		/// </summary>
		/// <param name="target">The target neuron.</param>
		/// <param name="weight">The connection weight.</param>
		public void Connect(IWorkingNeuron target, float weight)
		{
			target.AddConnection(this, weight);
		}
	}
}
