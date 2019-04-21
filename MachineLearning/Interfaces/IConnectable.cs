using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Represents an item that can be connected to and from.
	/// </summary>
	public interface IConnectable<TConnectionTarget>
	{
		/// <summary>
		/// Creates a connection between this neuron and
		/// the <paramref name="target"/> neuron, with a
		/// specified connection weight.
		/// </summary>
		/// <param name="target">The target neuron.</param>
		/// <param name="weight">The connection weight.</param>
		void Connect(TConnectionTarget target, decimal weight);
	}
}
