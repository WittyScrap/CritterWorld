using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning.Interfaces;

namespace MachineLearning
{
	/// <summary>
	/// Represents a connection between two neurons.
	/// </summary>
	struct Connection
	{
		/// <summary>
		/// The "left side" neuron.
		/// </summary>
		public INeuron Source { get; }

		/// <summary>
		/// The weight of the connection;
		/// </summary>
		public float Weight { get; set; }

		/// <summary>
		/// Creates a connection between two neurons.
		/// </summary>
		/// <param name="leftSide"></param>
		/// <param name="rightSide"></param>
		/// <param name="weight"></param>
		public Connection(INeuron source, float weight)
		{
			Source = source;
			Weight = weight;
		}
	}
}
