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
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.networkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.networkDrawer1 = new CritterRobots.Components.NetworkDrawer();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.networkToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.injectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.inputValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.inputWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.recenterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.neuronValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.inputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hiddenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.connectionWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.inputsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hiddenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.noneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.outputToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.neuronIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.inputsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.hiddenToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.outputsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.connectionLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.seekSalvationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nothingFoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.networkToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(832, 24);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.shutdownToolStripMenuItem,
            this.injectToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// networkToolStripMenuItem
			// 
			this.networkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuItem,
            this.recenterToolStripMenuItem});
			this.networkToolStripMenuItem.Name = "networkToolStripMenuItem";
			this.networkToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.networkToolStripMenuItem.Text = "Edit";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuronValuesToolStripMenuItem,
            this.connectionWeightsToolStripMenuItem,
            this.neuronIconsToolStripMenuItem,
            this.connectionLinesToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
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
			// networkDrawer1
			// 
			this.networkDrawer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.networkDrawer1.BackColor = System.Drawing.Color.White;
			this.networkDrawer1.Location = new System.Drawing.Point(0, 27);
			this.networkDrawer1.Name = "networkDrawer1";
			this.networkDrawer1.Size = new System.Drawing.Size(832, 477);
			this.networkDrawer1.TabIndex = 1;
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.networkToolStripMenuItem1});
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.newToolStripMenuItem.Text = "New...";
			// 
			// networkToolStripMenuItem1
			// 
			this.networkToolStripMenuItem1.Name = "networkToolStripMenuItem1";
			this.networkToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.networkToolStripMenuItem1.Text = "Network";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromFileToolStripMenuItem});
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openToolStripMenuItem.Text = "Open...";
			// 
			// fromFileToolStripMenuItem
			// 
			this.fromFileToolStripMenuItem.Name = "fromFileToolStripMenuItem";
			this.fromFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.fromFileToolStripMenuItem.Text = "From file";
			// 
			// resetToolStripMenuItem
			// 
			this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			this.resetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.resetToolStripMenuItem.Text = "Reset";
			// 
			// shutdownToolStripMenuItem
			// 
			this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
			this.shutdownToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.shutdownToolStripMenuItem.Text = "Shutdown";
			// 
			// injectToolStripMenuItem
			// 
			this.injectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputValuesToolStripMenuItem,
            this.inputWeightsToolStripMenuItem});
			this.injectToolStripMenuItem.Name = "injectToolStripMenuItem";
			this.injectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.injectToolStripMenuItem.Text = "Inject...";
			// 
			// inputValuesToolStripMenuItem
			// 
			this.inputValuesToolStripMenuItem.Name = "inputValuesToolStripMenuItem";
			this.inputValuesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.inputValuesToolStripMenuItem.Text = "Input values";
			// 
			// inputWeightsToolStripMenuItem
			// 
			this.inputWeightsToolStripMenuItem.Name = "inputWeightsToolStripMenuItem";
			this.inputWeightsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.inputWeightsToolStripMenuItem.Text = "Input weights";
			// 
			// zoomToolStripMenuItem
			// 
			this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem});
			this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
			this.zoomToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.zoomToolStripMenuItem.Text = "Zoom...";
			// 
			// zoomInToolStripMenuItem
			// 
			this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
			this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.zoomInToolStripMenuItem.Text = "Zoom in";
			// 
			// zoomOutToolStripMenuItem
			// 
			this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
			this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.zoomOutToolStripMenuItem.Text = "Zoom out";
			// 
			// recenterToolStripMenuItem
			// 
			this.recenterToolStripMenuItem.Name = "recenterToolStripMenuItem";
			this.recenterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.recenterToolStripMenuItem.Text = "Recenter";
			// 
			// neuronValuesToolStripMenuItem
			// 
			this.neuronValuesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputToolStripMenuItem,
            this.hiddenToolStripMenuItem,
            this.outputToolStripMenuItem});
			this.neuronValuesToolStripMenuItem.Name = "neuronValuesToolStripMenuItem";
			this.neuronValuesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.neuronValuesToolStripMenuItem.Text = "Neuron values...";
			// 
			// inputToolStripMenuItem
			// 
			this.inputToolStripMenuItem.Name = "inputToolStripMenuItem";
			this.inputToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.inputToolStripMenuItem.Text = "Input";
			// 
			// hiddenToolStripMenuItem
			// 
			this.hiddenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.allToolStripMenuItem,
            this.selectedToolStripMenuItem});
			this.hiddenToolStripMenuItem.Name = "hiddenToolStripMenuItem";
			this.hiddenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.hiddenToolStripMenuItem.Text = "Hidden...";
			// 
			// noneToolStripMenuItem
			// 
			this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
			this.noneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.noneToolStripMenuItem.Text = "None";
			// 
			// allToolStripMenuItem
			// 
			this.allToolStripMenuItem.Name = "allToolStripMenuItem";
			this.allToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.allToolStripMenuItem.Text = "All";
			// 
			// selectedToolStripMenuItem
			// 
			this.selectedToolStripMenuItem.Name = "selectedToolStripMenuItem";
			this.selectedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.selectedToolStripMenuItem.Text = "Selected...";
			// 
			// outputToolStripMenuItem
			// 
			this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
			this.outputToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.outputToolStripMenuItem.Text = "Output";
			// 
			// connectionWeightsToolStripMenuItem
			// 
			this.connectionWeightsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputsToolStripMenuItem,
            this.hiddenToolStripMenuItem1,
            this.outputToolStripMenuItem1});
			this.connectionWeightsToolStripMenuItem.Name = "connectionWeightsToolStripMenuItem";
			this.connectionWeightsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.connectionWeightsToolStripMenuItem.Text = "Connection weights...";
			// 
			// inputsToolStripMenuItem
			// 
			this.inputsToolStripMenuItem.Name = "inputsToolStripMenuItem";
			this.inputsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.inputsToolStripMenuItem.Text = "Inputs";
			// 
			// hiddenToolStripMenuItem1
			// 
			this.hiddenToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem1,
            this.selectToolStripMenuItem,
            this.allToolStripMenuItem1});
			this.hiddenToolStripMenuItem1.Name = "hiddenToolStripMenuItem1";
			this.hiddenToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.hiddenToolStripMenuItem1.Text = "Hidden...";
			// 
			// noneToolStripMenuItem1
			// 
			this.noneToolStripMenuItem1.Name = "noneToolStripMenuItem1";
			this.noneToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.noneToolStripMenuItem1.Text = "None";
			// 
			// selectToolStripMenuItem
			// 
			this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
			this.selectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.selectToolStripMenuItem.Text = "Select...";
			// 
			// allToolStripMenuItem1
			// 
			this.allToolStripMenuItem1.Name = "allToolStripMenuItem1";
			this.allToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.allToolStripMenuItem1.Text = "All";
			// 
			// outputToolStripMenuItem1
			// 
			this.outputToolStripMenuItem1.Name = "outputToolStripMenuItem1";
			this.outputToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.outputToolStripMenuItem1.Text = "Output";
			// 
			// neuronIconsToolStripMenuItem
			// 
			this.neuronIconsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputsToolStripMenuItem1,
            this.hiddenToolStripMenuItem2,
            this.outputsToolStripMenuItem});
			this.neuronIconsToolStripMenuItem.Name = "neuronIconsToolStripMenuItem";
			this.neuronIconsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.neuronIconsToolStripMenuItem.Text = "Neuron icons...";
			// 
			// inputsToolStripMenuItem1
			// 
			this.inputsToolStripMenuItem1.Name = "inputsToolStripMenuItem1";
			this.inputsToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.inputsToolStripMenuItem1.Text = "Inputs";
			// 
			// hiddenToolStripMenuItem2
			// 
			this.hiddenToolStripMenuItem2.Name = "hiddenToolStripMenuItem2";
			this.hiddenToolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
			this.hiddenToolStripMenuItem2.Text = "Hidden";
			// 
			// outputsToolStripMenuItem
			// 
			this.outputsToolStripMenuItem.Name = "outputsToolStripMenuItem";
			this.outputsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.outputsToolStripMenuItem.Text = "Outputs";
			// 
			// connectionLinesToolStripMenuItem
			// 
			this.connectionLinesToolStripMenuItem.Name = "connectionLinesToolStripMenuItem";
			this.connectionLinesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.connectionLinesToolStripMenuItem.Text = "Connection lines";
			// 
			// seekSalvationToolStripMenuItem
			// 
			this.seekSalvationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nothingFoundToolStripMenuItem});
			this.seekSalvationToolStripMenuItem.Name = "seekSalvationToolStripMenuItem";
			this.seekSalvationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.seekSalvationToolStripMenuItem.Text = "Seek salvation...";
			// 
			// nothingFoundToolStripMenuItem
			// 
			this.nothingFoundToolStripMenuItem.Name = "nothingFoundToolStripMenuItem";
			this.nothingFoundToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.nothingFoundToolStripMenuItem.Text = "Nothing found";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// NetworkTrainerDebugWindow
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(832, 505);
			this.Controls.Add(this.networkDrawer1);
			this.Controls.Add(this.mainMenu);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MainMenuStrip = this.mainMenu;
			this.Name = "NetworkTrainerDebugWindow";
			this.Text = "NetworkTrainerDebugWindow";
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem networkToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private Components.NetworkDrawer networkDrawer1;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem networkToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fromFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem injectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inputValuesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inputWeightsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recenterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem neuronValuesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inputToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hiddenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem connectionWeightsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inputsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hiddenToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem neuronIconsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem inputsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem hiddenToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem outputsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem connectionLinesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem seekSalvationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nothingFoundToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
	}
}