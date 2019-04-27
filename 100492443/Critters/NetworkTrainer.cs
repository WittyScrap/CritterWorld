using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearning;
using CritterRobots.Forms;

namespace CritterRobots.Critters.Controllers
{
	/// <summary>
	/// This critter will handle training the
	/// internal Neural Network.
	/// </summary>
	public class NetworkTrainer : Critter, INetworkHolder
	{
		/// <summary>
		/// The neural network used for handling the critter's
		/// behaviour.
		/// </summary>
		public NeuralNetwork CritterBrain { get; set; }

		/// <summary>
		/// The network debug window.
		/// </summary>
		private NetworkTrainerDebugWindow DebugWindow { get; set; }

		/// <summary>
		/// Constructs a new critter.
		/// </summary>
		/// <param name="critterID">A unique representative ID for the critter.</param>
		public NetworkTrainer(int critterID) : base("Mortal slave #" + critterID)
		{
			CritterBrain = NeuralNetwork.RandomNetwork(5, 2);
		}

		/// <summary>
		/// Launches a UI that displays the neural network's status
		/// and allows to make changes.
		/// </summary>
		public override void LaunchUI()
		{
			DebugWindow = new NetworkTrainerDebugWindow(this);
			DebugWindow.Show();
			DebugWindow.Focus();
		}
	}
}
