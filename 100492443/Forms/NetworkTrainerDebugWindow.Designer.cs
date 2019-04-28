namespace CritterRobots.Forms
{
	partial class NetworkTrainerDebugWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.neuronIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.inputsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.hiddenToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.outputsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.connectionLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.networkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.recenterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.seekSalvationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nothingFoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.networkDrawer = new CritterRobots.Components.NetworkDrawer();
			this.fieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mutateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fieToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.networkToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(1068, 24);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "menuStrip1";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuronIconsToolStripMenuItem,
            this.connectionLinesToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.viewToolStripMenuItem.Text = "Edit";
			// 
			// neuronIconsToolStripMenuItem
			// 
			this.neuronIconsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputsToolStripMenuItem1,
            this.hiddenToolStripMenuItem2,
            this.outputsToolStripMenuItem});
			this.neuronIconsToolStripMenuItem.Name = "neuronIconsToolStripMenuItem";
			this.neuronIconsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.neuronIconsToolStripMenuItem.Text = "Show Neuron icons...";
			// 
			// inputsToolStripMenuItem1
			// 
			this.inputsToolStripMenuItem1.Checked = true;
			this.inputsToolStripMenuItem1.CheckOnClick = true;
			this.inputsToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.inputsToolStripMenuItem1.Name = "inputsToolStripMenuItem1";
			this.inputsToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
			this.inputsToolStripMenuItem1.Text = "Inputs";
			this.inputsToolStripMenuItem1.Click += new System.EventHandler(this.inputsToolStripMenuItem1_Click);
			// 
			// hiddenToolStripMenuItem2
			// 
			this.hiddenToolStripMenuItem2.Checked = true;
			this.hiddenToolStripMenuItem2.CheckOnClick = true;
			this.hiddenToolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.hiddenToolStripMenuItem2.Name = "hiddenToolStripMenuItem2";
			this.hiddenToolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
			this.hiddenToolStripMenuItem2.Text = "Hidden";
			this.hiddenToolStripMenuItem2.Click += new System.EventHandler(this.hiddenToolStripMenuItem2_Click);
			// 
			// outputsToolStripMenuItem
			// 
			this.outputsToolStripMenuItem.Checked = true;
			this.outputsToolStripMenuItem.CheckOnClick = true;
			this.outputsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.outputsToolStripMenuItem.Name = "outputsToolStripMenuItem";
			this.outputsToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.outputsToolStripMenuItem.Text = "Outputs";
			this.outputsToolStripMenuItem.Click += new System.EventHandler(this.outputsToolStripMenuItem_Click);
			// 
			// connectionLinesToolStripMenuItem
			// 
			this.connectionLinesToolStripMenuItem.Checked = true;
			this.connectionLinesToolStripMenuItem.CheckOnClick = true;
			this.connectionLinesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.connectionLinesToolStripMenuItem.Name = "connectionLinesToolStripMenuItem";
			this.connectionLinesToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.connectionLinesToolStripMenuItem.Text = "Show Connection lines";
			this.connectionLinesToolStripMenuItem.Click += new System.EventHandler(this.connectionLinesToolStripMenuItem_Click);
			// 
			// networkToolStripMenuItem
			// 
			this.networkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuItem,
            this.recenterToolStripMenuItem});
			this.networkToolStripMenuItem.Name = "networkToolStripMenuItem";
			this.networkToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.networkToolStripMenuItem.Text = "View";
			// 
			// zoomToolStripMenuItem
			// 
			this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem});
			this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
			this.zoomToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
			this.zoomToolStripMenuItem.Text = "Zoom...";
			// 
			// zoomInToolStripMenuItem
			// 
			this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
			this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.zoomInToolStripMenuItem.Text = "Zoom in";
			this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
			// 
			// zoomOutToolStripMenuItem
			// 
			this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
			this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.zoomOutToolStripMenuItem.Text = "Zoom out";
			this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
			// 
			// recenterToolStripMenuItem
			// 
			this.recenterToolStripMenuItem.Name = "recenterToolStripMenuItem";
			this.recenterToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
			this.recenterToolStripMenuItem.Text = "Recenter";
			this.recenterToolStripMenuItem.Click += new System.EventHandler(this.recenterToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.seekSalvationToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// seekSalvationToolStripMenuItem
			// 
			this.seekSalvationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nothingFoundToolStripMenuItem});
			this.seekSalvationToolStripMenuItem.Name = "seekSalvationToolStripMenuItem";
			this.seekSalvationToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
			this.seekSalvationToolStripMenuItem.Text = "Seek salvation...";
			// 
			// nothingFoundToolStripMenuItem
			// 
			this.nothingFoundToolStripMenuItem.Name = "nothingFoundToolStripMenuItem";
			this.nothingFoundToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.nothingFoundToolStripMenuItem.Text = "Nothing found";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// networkDrawer
			// 
			this.networkDrawer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.networkDrawer.BackColor = System.Drawing.Color.White;
			this.networkDrawer.CanPan = true;
			this.networkDrawer.CanZoom = true;
			this.networkDrawer.Cursor = System.Windows.Forms.Cursors.SizeAll;
			this.networkDrawer.Location = new System.Drawing.Point(0, 27);
			this.networkDrawer.Name = "networkDrawer";
			this.networkDrawer.NeuronSize = new System.Drawing.Size(10, 10);
			this.networkDrawer.ReferenceBrain = null;
			this.networkDrawer.ShowConnections = true;
			this.networkDrawer.ShowHiddenNeurons = true;
			this.networkDrawer.ShowInputNeurons = true;
			this.networkDrawer.ShowOutputNeurons = true;
			this.networkDrawer.Size = new System.Drawing.Size(1068, 691);
			this.networkDrawer.TabIndex = 1;
			this.networkDrawer.Load += new System.EventHandler(this.NetworkDrawer_Load);
			// 
			// fieToolStripMenuItem
			// 
			this.fieToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mutateToolStripMenuItem,
            this.restoreToolStripMenuItem});
			this.fieToolStripMenuItem.Name = "fieToolStripMenuItem";
			this.fieToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fieToolStripMenuItem.Text = "File";
			// 
			// mutateToolStripMenuItem
			// 
			this.mutateToolStripMenuItem.Name = "mutateToolStripMenuItem";
			this.mutateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.mutateToolStripMenuItem.Text = "Mutate...";
			this.mutateToolStripMenuItem.Click += new System.EventHandler(this.mutateToolStripMenuItem_Click);
			// 
			// restoreToolStripMenuItem
			// 
			this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
			this.restoreToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.restoreToolStripMenuItem.Text = "Restore";
			// 
			// NetworkTrainerDebugWindow
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1068, 719);
			this.Controls.Add(this.networkDrawer);
			this.Controls.Add(this.mainMenu);
			this.MainMenuStrip = this.mainMenu;
			this.Name = "NetworkTrainerDebugWindow";
			this.Text = "Neural Network Diagram";
			this.Resize += new System.EventHandler(this.NetworkTrainerDebugWindow_Resize);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem networkToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private Components.NetworkDrawer networkDrawer;
		private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recenterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem neuronIconsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inputsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem hiddenToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem outputsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem connectionLinesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem seekSalvationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nothingFoundToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fieToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mutateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
	}
}