using MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Critters
{
	/// <summary>
	/// Represents a critter that holds a neural network.
	/// </summary>
	public interface INetworkHolder
	{
		/// <summary>
		/// The neural network used for handling the critter's
		/// behaviour.
		/// </summary>
		NeuralNetwork CritterBrain { get; set; }
	}
}
