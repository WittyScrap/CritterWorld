namespace CritterRobots.Forms
{
	partial class EscapeInformer
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
			this.label1 = new System.Windows.Forms.Label();
			this.critterID = new System.Windows.Forms.NumericUpDown();
			this.OK = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.critterID)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(170, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Okay, fine, which one...";
			// 
			// critterID
			// 
			this.critterID.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.critterID.Location = new System.Drawing.Point(16, 32);
			this.critterID.Name = "critterID";
			this.critterID.Size = new System.Drawing.Size(75, 38);
			this.critterID.TabIndex = 1;
			// 
			// OK
			// 
			this.OK.Location = new System.Drawing.Point(16, 76);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(75, 42);
			this.OK.TabIndex = 2;
			this.OK.Text = "Nice.";
			this.OK.UseVisualStyleBackColor = true;
			this.OK.Click += new System.EventHandler(this.OK_Click);
			// 
			// cancel
			// 
			this.cancel.Location = new System.Drawing.Point(97, 32);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(100, 86);
			this.cancel.TabIndex = 3;
			this.cancel.Text = "Actually you know what? This isn\'t fine, I don\'t like your attitude you little pi" +
    "ece of irrelevant ...";
			this.cancel.UseVisualStyleBackColor = true;
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// EscapeInformer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(209, 130);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.OK);
			this.Controls.Add(this.critterID);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "EscapeInformer";
			this.Text = "EscapeInformer";
			((System.ComponentModel.ISupportInitialize)(this.critterID)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown critterID;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button cancel;
	}
}