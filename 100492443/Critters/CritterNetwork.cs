using MachineLearning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Critters
{

	/// <summary>
	/// Represents a critter that holds a neural network.
	/// </summary>
	class CritterNetwork : NeuralCritter
	{
		public CritterNetwork(int critterID) : base("Networky bug #" + critterID)
		{ }

		/// <summary>
		/// Find any .crbn file and load it.
		/// </summary>
		protected override void LoadNetwork()
		{
			if (File.Exists(Filepath + "critter_brain.crbn"))
			{
				using (StreamReader brainReader = new StreamReader("critter_brain.crbn"))
				{
					string serializedBrain = brainReader.ReadToEnd();
					CritterBrain = NeuralNetwork.Deserialize(serializedBrain);
					CritterBrain.Mutate(1); // Add a little bit of variance.
				}
			}
		}
	}
}
