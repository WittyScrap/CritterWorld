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
	public partial class NetworkDebuggerForm : Form
	{
		/// <summary>
		/// Delegate that indicates which method to call in order
		/// to read the requested layer's contents.
		/// </summary>
		public delegate decimal[] NetworkExtractor();

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NetworkDebuggerForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Updates the window's contents through the provided neuron values.
		/// </summary>
		/// <param name="neuronValues">The list of neuron values.</param>
		private void UpdateWindow(decimal[] neuronValues)
		{
			StringBuilder stringBuilder = new StringBuilder();
			dataDump.Text = "";

			for (int i = 0; i < neuronValues.Length; ++i)
			{
				stringBuilder.AppendLine("Neuron Index [" + i + "] = " + neuronValues[i]);
			}

			dataDump.Text = stringBuilder.ToString();
			Refresh();
		}

		/// <summary>
		/// Initializes a network debugging form.
		/// </summary>
		/// <param name="layerName">The friendly name of the layer that will be examined.</param>
		/// <param name="networkExtractor">The extraction routine.</param>
		public NetworkDebuggerForm(string layerName, NetworkExtractor networkExtractor) : this()
		{
			titleLabel.Text = titleLabel.Text.Replace("{0}", layerName);
			Extractor.Tick += (sender, e) => UpdateWindow(networkExtractor());
			Extractor.Start();
		}
	}
}
