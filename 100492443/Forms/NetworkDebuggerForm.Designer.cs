namespace CritterRobots.Forms
{
	partial class NetworkDebuggerForm
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
			this.components = new System.ComponentModel.Container();
			this.titleLabel = new System.Windows.Forms.Label();
			this.dataDump = new System.Windows.Forms.Label();
			this.Extractor = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// titleLabel
			// 
			this.titleLabel.AutoSize = true;
			this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleLabel.Location = new System.Drawing.Point(12, 9);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(276, 24);
			this.titleLabel.TabIndex = 0;
			this.titleLabel.Text = "Network data dump for {0} layer:";
			// 
			// dataDump
			// 
			this.dataDump.BackColor = System.Drawing.Color.White;
			this.dataDump.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dataDump.Location = new System.Drawing.Point(12, 47);
			this.dataDump.Name = "dataDump";
			this.dataDump.Size = new System.Drawing.Size(384, 289);
			this.dataDump.TabIndex = 1;
			this.dataDump.Text = "Please wait...";
			// 
			// Extractor
			// 
			this.Extractor.Enabled = true;
			// 
			// NetworkDebuggerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(408, 345);
			this.Controls.Add(this.dataDump);
			this.Controls.Add(this.titleLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "NetworkDebuggerForm";
			this.Text = "NetworkDebuggerForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label dataDump;
		private System.Windows.Forms.Timer Extractor;
	}
}