using MachineLearning.ActivationFunctions;
using MachineLearning.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Neurons
{
	/// <summary>
	/// Neuron that always outputs 1.
	/// </summary>
	[DataContract]
	class BiasNeuron : Neuron<SigmoidFunction>
	{
		/// <summary>
		/// A constant value of 1.
		/// </summary>
		[DataMember]
		public override decimal Output { get => 1; set { } }

		/// <summary>
		/// Ignore connection.
		/// </summary>
		public override void AddConnection(INeuron source, decimal weight)
		{ }
	}
}
