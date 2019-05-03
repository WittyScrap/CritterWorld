using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MachineLearning;
using MachineLearning.Layers;
using MachineLearning.Neurons;
using MachineLearning.ActivationFunctions;
using System.Threading.Tasks;
using CritterRobots.Critters;
using System;

namespace CritterRobots.Components
{
	public partial class NetworkDrawer : UserControl
	{
		/// <summary>
		/// The reference neural network.
		/// </summary>
		private NeuralNetwork TargetNetwork { get; set; }

		/// <summary>
		/// Sets the reference neural network.
		/// </summary>
		public void SetTargetNetwork(NeuralNetwork value)
		{
			TargetNetwork = value;
		}

		/// <summary>
		/// The zoom level for the canvas.
		/// </summary>
		private float m_canvasZoom = 1.0f;
		private bool m_showConnections = true;
		private bool m_showInputNeurons = true;
		private bool m_showHiddenNeurons = true;
		private bool m_showOutputNeurons = true;

		/// <summary>
		/// If true, the connections for this network will be
		/// drawn.
		/// </summary>
		public bool ShowConnections {
			get => m_showConnections;
			set
			{
				m_showConnections = value;
				Refresh();
			}
		}

		/// <summary>
		/// If true, the input neurons for this network will be
		/// drawn.
		/// </summary>
		public bool ShowInputNeurons {
			get => m_showInputNeurons;
			set
			{
				m_showInputNeurons = value;
				Refresh();
			}
		}

		/// <summary>
		/// If true, the hidden neurons for this network will be drawn.
		/// </summary>
		public bool ShowHiddenNeurons {
			get => m_showHiddenNeurons;
			set
			{
				m_showHiddenNeurons = value;
				Refresh();
			}
		}

		/// <summary>
		/// If true, the output neurons for this network will be drawn.
		/// </summary>
		public bool ShowOutputNeurons {
			get => m_showOutputNeurons;
			set
			{
				m_showOutputNeurons = value;
				Refresh();
			}
		}

		/// <summary>
		/// The canvas offset.
		/// </summary>
		private Point CanvasOffset { get; set; }

		/// <summary>
		/// The canvas zoom.
		/// </summary>
		private float CanvasZoom {
			get => m_canvasZoom;
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				m_canvasZoom = value;
			}
		}

		/// <summary>
		/// The internal drawable canvas.
		/// </summary>
		private Rectangle Canvas {
			get
			{
				Size size = new Size((int)(Size.Width * CanvasZoom), (int)(Size.Height * CanvasZoom));
				Rectangle canvas = new Rectangle(CanvasOffset, size);

				return canvas;
			}
		}

		/// <summary>
		/// The size of each neuron on the graph.
		/// </summary>
		public Size NeuronSize { get; set; } = new Size(10, 10);

		/// <summary>
		/// Sets and gets whether or not the graph can be zoomed in and out.
		/// </summary>
		public bool CanZoom { get; set; } = true;

		/// <summary>
		/// Gets and sets whether or not the graph can be panned around.
		/// </summary>
		public bool CanPan { get; set; } = true;

		/// <summary>
		/// The color when a neuron's value is 0.0.
		/// </summary>
		public Color OffColor { get; set; }

		/// <summary>
		/// The color when a neuron's value is 1.0.
		/// </summary>
		public Color OnColor { get; set; }

		/// <summary>
		/// Checks whether or not the cursor is currently panning.
		/// </summary>
		private bool IsPanning { get; set; }

		/// <summary>
		/// Checks whether or not the cursor is within the control's boundaries.
		/// </summary>
		private bool IsMouseInside { get; set; }

		/// <summary>
		/// Records the previous mouse position.
		/// </summary>
		private Point PreviousMousePosition { get; set; }

		/// <summary>
		/// Creates a new Network Drawer.
		/// </summary>
		public NetworkDrawer()
		{
			InitializeComponent();

			DoubleBuffered = true;
			if (CanPan)
			{
				Cursor = Cursors.SizeAll;
			}
			networkUpdateCheck.Tick += (sender, e) => Refresh();
		}

		/// <summary>
		/// Returns the vertical interval at
		/// which to display neurons within a layer.
		/// </summary>
		/// <param name="neuronCount">The number of neurons.</param>
		/// <returns>The vertical interval at which to display neurons within a layer.</returns>
		int GetVerticalInterval(int neuronCount)
		{
			return Canvas.Height / neuronCount;
		}

		/// <summary>
		/// Returns the interval at which to display
		/// the neural network.
		/// </summary>
		/// <param name="layerCount">The number of layers in the neural network.</param>
		/// <returns>The interval at which to display the network.</returns>
		int GetHorizontalInteval()
		{
			return Canvas.Width / (TargetNetwork.HiddenNeurons.Count + 1);
		}

		/// <summary>
		/// Draws connections between two layers.
		/// </summary>
		/// <param name="layerLeft">The layer to connect on the left.</param>
		/// <param name="layerRight">The layer to connect on the right.</param>
		void DrawConnectionLayers(Layer<Neuron<SigmoidFunction>> layerLeft, Layer<Neuron<SigmoidFunction>> layerRight, Graphics targetGraphics, int layerNumber)
		{
			Pen pen = new Pen(Color.Black);

			int horizontalInterval = GetHorizontalInteval();
			int leftInterval = GetVerticalInterval(layerLeft.Count);
			int rightInterval = GetVerticalInterval(layerRight.Count);

			int horizontalOffset = Canvas.Location.X + horizontalInterval / 2;

			int leftX = horizontalOffset + horizontalInterval * layerNumber;
			int rightX = leftX + horizontalInterval;
			
			int leftOffset = Canvas.Location.Y + leftInterval / 2;
			int rightOffset = Canvas.Location.Y + rightInterval / 2;
			for (int leftNeuronID = 0; leftNeuronID < layerLeft.Count; ++leftNeuronID)
			{
				int leftNeuronY = leftOffset + leftInterval * leftNeuronID;
				for (int rightNeuronID = 0; rightNeuronID < layerRight.Count; ++rightNeuronID)
				{
					int rightNeuronY = rightOffset + rightInterval * rightNeuronID;

					Point neuronLeft = new Point(leftX, leftNeuronY);
					Point neuronRight = new Point(rightX, rightNeuronY);

					var leftNeuron = layerLeft[leftNeuronID];
					var rightNeuron = layerRight[rightNeuronID];

					if (rightNeuron.Connections.ContainsKey(leftNeuron))
					{
						pen.Width = (float)rightNeuron.Connections[leftNeuron] * 2;
						targetGraphics.DrawLine(pen, neuronLeft, neuronRight);
					}
				}
			}
		}

		/// <summary>
		/// Draws all the connections in the neural network.
		/// </summary>
		private void DrawConnections(Graphics targetGraphics)
		{
			if (!ShowConnections)
			{
				return;
			}
			var previousLayer = TargetNetwork.InputNeurons;
			for (int i = 0; i < TargetNetwork.HiddenNeurons.Count; ++i)
			{
				var currentLayer = TargetNetwork.HiddenNeurons[i];
				DrawConnectionLayers(previousLayer, currentLayer, targetGraphics, i);
				previousLayer = currentLayer;
			}
		}

		/// <summary>
		/// Linear interpolation.
		/// </summary>
		double Lerp(double v0, double v1, double t)
		{
			return (1 - t) * v0 + t * v1;
		}

		/// <summary>
		/// Interpolates between the two given colors
		/// through a given alpha gradient value.
		/// </summary>
		private Color LerpColor(Color fromColor, Color toColor, double alpha)
		{
			return Color.FromArgb((int)Lerp(fromColor.R, toColor.R, alpha), (int)Lerp(fromColor.G, toColor.G, alpha), (int)Lerp(fromColor.B, toColor.B, alpha));
		}

		/// <summary>
		/// Draws a layer of neurons.
		/// </summary>
		/// <param name="neuronCount">The number of neurons in the layer that needs to be drawn.</param>
		/// <param name="targetGraphics">The target canvas to darw on.</param>
		/// <param name="layerNumber">The layer ID (or its offset).</param>
		private void DrawNeuronsLayer(Layer<Neuron<SigmoidFunction>> layer, Graphics targetGraphics, int layerNumber, Size neuronSize)
		{
			SolidBrush brush = new SolidBrush(Color.Black);
			
			int horizontalInterval = GetHorizontalInteval();
			int verticalInterval = GetVerticalInterval(layer.Count);

			int horizontalOffset = Canvas.X + horizontalInterval / 2;
			int verticalOffset = Canvas.Y + verticalInterval / 2;

			int drawX = horizontalOffset + horizontalInterval * layerNumber;
			for (int neuron = 0; neuron < layer.Count; ++neuron)
			{
				Color neuronColor = LerpColor(OffColor, OnColor, (double)layer[neuron].Output);

				int neuronY = verticalOffset + verticalInterval * neuron;
				Point neuronPosition = new Point(drawX - neuronSize.Width / 2, neuronY - neuronSize.Height / 2);
				Rectangle neuronRect = new Rectangle(neuronPosition, neuronSize);

				brush.Color = neuronColor;
				targetGraphics.FillRectangle(brush, neuronRect);
			}
		}

		/// <summary>
		/// Draws neurons as squares.
		/// </summary>
		/// <param name="targetGraphics">The target canvas.</param>
		private void DrawNeurons(Graphics targetGraphics, Size neuronSize)
		{
			if (ShowInputNeurons)
			{
				DrawNeuronsLayer(TargetNetwork.InputNeurons, targetGraphics, 0, neuronSize);
			}
			if (ShowHiddenNeurons)
			{
				for (int i = 0; i < TargetNetwork.HiddenNeurons.Count - 1; ++i)
				{
					DrawNeuronsLayer(TargetNetwork.HiddenNeurons[i], targetGraphics, i + 1, neuronSize);
				}
			}
			if (ShowOutputNeurons)
			{
				DrawNeuronsLayer(TargetNetwork.OutputNeurons, targetGraphics, TargetNetwork.HiddenNeurons.Count, neuronSize);
			}
		}

		/// <summary>
		/// Window paint event.
		/// </summary>
		private void NetworkDrawer_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.White);
			if (TargetNetwork != null)
			{
				DrawConnections(e.Graphics);
				DrawNeurons(e.Graphics, NeuronSize);
			}
		}

		/// <summary>
		/// Returns the mouse delta.
		/// </summary>
		private Vector GetMouseDelta(Point mouseLocation)
		{
			return new Vector(mouseLocation.X - PreviousMousePosition.X, mouseLocation.Y - PreviousMousePosition.Y);
		}

		/// <summary>
		/// Event called when the left mouse button has been
		/// clicked within this control.
		/// </summary>
		private void NetworkDrawer_MouseDown(object sender, MouseEventArgs e)
		{
			IsPanning = CanPan;
			PreviousMousePosition = e.Location;
		}

		/// <summary>
		/// Event called when the left mouse button has been
		/// releaed.
		/// </summary>
		private void NetworkDrawer_MouseUp(object sender, MouseEventArgs e)
		{
			IsPanning = false;
		}

		/// <summary>
		/// Event called when the mouse was moved within this control's boundaries.
		/// </summary>
		private void NetworkDrawer_MouseMove(object sender, MouseEventArgs e)
		{
			if (IsPanning)
			{
				Vector mouseDelta = GetMouseDelta(e.Location);
				Point canvasOffset = CanvasOffset;

				canvasOffset.X += (int)mouseDelta.X;
				canvasOffset.Y += (int)mouseDelta.Y;

				CanvasOffset = canvasOffset;
				PreviousMousePosition = e.Location;

				debugLabel.Text = "Panning (X: " + mouseDelta.X + ", Y: " + mouseDelta.Y + ")";

				Refresh();
			}
		}

		/// <summary>
		/// Captures the mouse wheel event.
		/// </summary>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (CanZoom)
			{
				float zoomAmount = e.Delta / 1000.0f;
				CanvasZoom += zoomAmount;
				Point canvasOffset = CanvasOffset;

				canvasOffset.X -= (int)(Width * zoomAmount / 2);
				canvasOffset.Y -= (int)(Height * zoomAmount / 2);

				CanvasOffset = canvasOffset;

				debugLabel.Text = "Zooming (" + (CanvasZoom * 100) + "%)";

				Refresh();
			}
		}

		/// <summary>
		/// Recenters this diagram.
		/// </summary>
		public void Recenter()
		{
			CanvasOffset = Point.Empty;
			CanvasZoom = 1.0f;

			Refresh();
		}

		/// <summary>
		/// Zooms by the selected amount.
		/// </summary>
		/// <param name="zoomAmount">How far to zoom in. If this value is negative, the network will zoom out instead.</param>
		public void Zoom(float zoomAmount)
		{
			CanvasZoom += zoomAmount;

			Refresh();
		}
	}
}
