namespace CritterRobots.Components
{
	partial class NetworkDrawer
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.debugLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// debugLabel
			// 
			this.debugLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.debugLabel.AutoSize = true;
			this.debugLabel.Location = new System.Drawing.Point(4, 134);
			this.debugLabel.Name = "debugLabel";
			this.debugLabel.Size = new System.Drawing.Size(47, 13);
			this.debugLabel.TabIndex = 0;
			this.debugLabel.Text = "Ready...";
			// 
			// NetworkDrawer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.debugLabel);
			this.Name = "NetworkDrawer";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.NetworkDrawer_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NetworkDrawer_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NetworkDrawer_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NetworkDrawer_MouseUp);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label debugLabel;
	}
}
