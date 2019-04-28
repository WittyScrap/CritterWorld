using CritterRobots.Critters;
using CritterRobots.Critters.Controllers;
using MachineLearning;
using MachineLearning.ActivationFunctions;
using MachineLearning.Layers;
using MachineLearning.Neurons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CritterRobots.Forms
{
	public partial class NetworkTrainerDebugWindow : Form
	{
		/// <summary>
		/// The trainer critter that will hold the reference neural network.
		/// </summary>
		private INetworkHolder ReferenceBrain { get; }

		/// <summary>
		/// Creates a new debug window with a reference critter.
		/// </summary>
		/// <param name="critterTrainer">The trainer critter to be used as a reference.</param>
		public NetworkTrainerDebugWindow(INetworkHolder critterTrainer) : this()
		{
			ReferenceBrain = critterTrainer;
		}

		/// <summary>
		/// Event method called when the network drawer component is loaded.
		/// </summary>
		private void NetworkDrawer_Load(object sender, EventArgs e)
		{
			networkDrawer.ReferenceBrain = ReferenceBrain.CritterBrain;
		}

		public NetworkTrainerDebugWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Recenters the network diagram.
		/// </summary>
		private void recenterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			networkDrawer.Recenter();
		}

		/// <summary>
		/// Zooms into the diagram.
		/// </summary>
		private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
		{
			networkDrawer.Zoom(0.1f);
		}

		/// <summary>
		/// Zooms out from the diagram.
		/// </summary>
		private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			networkDrawer.Zoom(-0.1f);
		}

		/// <summary>
		/// Toggles whether or not the connection lines should be shown.
		/// </summary>
		private void connectionLinesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			networkDrawer.ShowConnections = !networkDrawer.ShowConnections;
		}

		/// <summary>
		/// Toggles whether or not the input neurons should be shown.
		/// </summary>
		private void inputsToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			networkDrawer.ShowInputNeurons = !networkDrawer.ShowInputNeurons;
		}

		/// <summary>
		/// Toggles whether or not the hidden neurons should be shown.
		/// </summary>
		private void hiddenToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			networkDrawer.ShowHiddenNeurons = !networkDrawer.ShowHiddenNeurons;
		}


		/// <summary>
		/// Toggles whether or not the output neurons should be shown.
		/// </summary>
		private void outputsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			networkDrawer.ShowOutputNeurons = !networkDrawer.ShowOutputNeurons;
		}

		/// <summary>
		/// Refreshes the network drawer.
		/// </summary>
		private void NetworkTrainerDebugWindow_Resize(object sender, EventArgs e)
		{
			networkDrawer.Refresh();
		}

		/// <summary>
		/// Randomly mutates the held neural network.
		/// </summary>
		private void mutateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ReferenceBrain.CritterBrain.Mutate();
			networkDrawer.Refresh();
		}
	}
}
