﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning.Neurons
{
	/// <summary>
	/// Abstract representation of a single
	/// perceptron.
	/// </summary>
	[DataContract]
	public class Neuron<TActivationFunction> : INeuron where TActivationFunction : IActivationFunction, new()
	{
		/// <summary>
		/// A dictionary containing all the connection weights to this neuron.
		/// </summary>
		[DataMember]
		private ConcurrentDictionary<INeuron, decimal> InternalConnections { get; set; } = new ConcurrentDictionary<INeuron, decimal>();
		
		/// <summary>
		/// The connections to this neurons from any neuron in the previous layer and
		/// their respective weights.
		/// </summary>
		public IReadOnlyDictionary<INeuron, decimal> Connections {
			get
			{
				return InternalConnections;
			}
		}

		/// <summary>
		/// Updates the weight of a connection.
		/// </summary>
		private void UpdateConnectionWeight(INeuron key, decimal value)
		{
			if (!InternalConnections.ContainsKey(key))
			{
				return;
			}

			if (value > 1m)
			{
				value = 1m;
			}
			if (value < -1m)
			{
				value = -1m;
			}

			InternalConnections[key] = value;
		}

		/// <summary>
		/// Randomly mutates connections within this neuron.
		/// </summary>
		public void Mutate(int mutationIntensity)
		{
			foreach (var connection in Connections)
			{
				decimal intensity = mutationIntensity / 10m;
				decimal delta = Randomizer.NextDecimal(-intensity, intensity);
				decimal updatedWeight = connection.Value + delta;

				UpdateConnectionWeight(connection.Key, updatedWeight);
			}
		}

		/// <summary>
		/// The output value of this neuron.
		/// </summary>
		[DataMember]
		public virtual decimal Output { get; set; } = 0.0m;

		/// <summary>
		/// Indicates whether or not this neuron has been deleted.
		/// </summary>
		public bool IsDestroyed { get; private set; } = false;

		/// <summary>
		/// Calculates the output value for this neuron.
		/// </summary>
		public void Calculate()
		{
			TActivationFunction activationFunction = new TActivationFunction();
			decimal weighedSum = GetWeighedSum();

			Output = activationFunction.Calculate(weighedSum);
		}

		/// <summary>
		/// Calculates a weighed sum of all incoming connections.
		/// </summary>
		/// <returns>The sum of all incoming connections.</returns>
		private decimal GetWeighedSum()
		{
			decimal weighedSum = 0.0m;
			foreach (var connection in InternalConnections)
			{
				INeuron connectionNeuron = connection.Key;
				decimal connectionWeight = connection.Value;

				weighedSum += connectionNeuron.Output * connectionWeight;
			}
			return weighedSum;
		}

		/// <summary>
		/// Clears all connections.
		/// </summary>
		public void Disconnect()
		{
			InternalConnections.Clear();
		}

		/// <summary>
		/// Adds an incoming connection from a different
		/// neuron with a specific weight.
		/// </summary>
		/// <param name="source">The source neuron.</param>
		/// <param name="weight">The connection weight.</param>
		public virtual void AddConnection(INeuron source, decimal weight)
		{
			InternalConnections[source] = weight;
		}

		/// <summary>
		/// Connects this neuron to a different neuron using
		/// a specific weight.
		/// </summary>
		/// <param name="target">The target neuron.</param>
		/// <param name="weight">The connection weight.</param>
		public void Connect(INeuron target, decimal weight)
		{
			target.AddConnection(this, weight);
		}

		/// <summary>
		/// Connects this neuron to every neuron in the target layer.
		/// </summary>
		/// <param name="target">The target layer.</param>
		/// <param name="weight">The weight to use for every connection.</param>
		public void Connect(ILayer<INeuron> target, decimal weight)
		{
			foreach (INeuron neuron in target)
			{
				Connect(neuron, weight);
			}
		}

		/// <summary>
		/// Removes dangling connections to nonexisting neurons.
		/// </summary>
		public void Clean()
		{
			Stack<INeuron> destroyedNeurons = new Stack<INeuron>();
			foreach (var connection in InternalConnections)
			{
				if (connection.Key.IsDestroyed)
				{
					destroyedNeurons.Push(connection.Key);
				}
			}

			while (destroyedNeurons.Count > 0)
			{
				INeuron toRemove = destroyedNeurons.Pop();
				while (InternalConnections.TryRemove(toRemove, out decimal discardedWeight));
			}
		}

		/// <summary>
		/// Flags this neuron as deleted.
		/// </summary>
		public void Destroy()
		{
			IsDestroyed = true;
		}
	}
}
