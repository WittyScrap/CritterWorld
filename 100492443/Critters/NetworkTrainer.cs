using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning;

namespace CritterRobots.Critters.Controllers
{
	/// <summary>
	/// This critter will handle training the
	/// internal Neural Network.
	/// </summary>
	class NetworkTrainer : Critter
	{
		/// <summary>
		/// The internal neural network.
		/// </summary>
		private TrainableNetwork InternalNeuralNetwork { get; }

		/// <summary>
		/// Constructs a new critter.
		/// </summary>
		/// <param name="critterID">A unique representative ID for the critter.</param>
		public NetworkTrainer(int critterID) : base("Mortal slave #" + critterID)
		{
		}
	}
}
