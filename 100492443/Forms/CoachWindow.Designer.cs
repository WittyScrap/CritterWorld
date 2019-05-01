namespace CritterRobots.Forms
{
	partial class CoachWindow
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
			this.uselessInfo = new System.Windows.Forms.Label();
			this.intriguingLabel = new System.Windows.Forms.Label();
			this.resolutionButton = new System.Windows.Forms.Button();
			this.checkEscapedCritter = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// uselessInfo
			// 
			this.uselessInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.uselessInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uselessInfo.Location = new System.Drawing.Point(0, 0);
			this.uselessInfo.Name = "uselessInfo";
			this.uselessInfo.Size = new System.Drawing.Size(331, 33);
			this.uselessInfo.TabIndex = 0;
			this.uselessInfo.Text = "... Waiting for the level to finish ...";
			this.uselessInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// intriguingLabel
			// 
			this.intriguingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.intriguingLabel.Location = new System.Drawing.Point(1, 33);
			this.intriguingLabel.Name = "intriguingLabel";
			this.intriguingLabel.Size = new System.Drawing.Size(301, 42);
			this.intriguingLabel.TabIndex = 1;
			this.intriguingLabel.Text = "Or maybe you could, you know... speed it up!";
			this.intriguingLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// resolutionButton
			// 
			this.resolutionButton.Location = new System.Drawing.Point(12, 78);
			this.resolutionButton.Name = "resolutionButton";
			this.resolutionButton.Size = new System.Drawing.Size(307, 41);
			this.resolutionButton.TabIndex = 2;
			this.resolutionButton.Text = "Yes! Speed it up please!";
			this.resolutionButton.UseVisualStyleBackColor = true;
			this.resolutionButton.Click += new System.EventHandler(this.resolutionButton_Click);
			// 
			// checkEscapedCritter
			// 
			this.checkEscapedCritter.Location = new System.Drawing.Point(12, 125);
			this.checkEscapedCritter.Name = "checkEscapedCritter";
			this.checkEscapedCritter.Size = new System.Drawing.Size(307, 24);
			this.checkEscapedCritter.TabIndex = 3;
			this.checkEscapedCritter.Text = "Hey! One of my critters escaped and you didn\'t even know!";
			this.checkEscapedCritter.UseVisualStyleBackColor = true;
			this.checkEscapedCritter.Click += new System.EventHandler(this.checkEscapedCritter_Click);
			// 
			// CoachWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 161);
			this.Controls.Add(this.checkEscapedCritter);
			this.Controls.Add(this.resolutionButton);
			this.Controls.Add(this.intriguingLabel);
			this.Controls.Add(this.uselessInfo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CoachWindow";
			this.Text = "CoachWindow";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label uselessInfo;
		private System.Windows.Forms.Label intriguingLabel;
		private System.Windows.Forms.Button resolutionButton;
		private System.Windows.Forms.Button checkEscapedCritter;
	}
}