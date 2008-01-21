namespace PowerFlag
{
	partial class PowerFlag
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.settingsBTN = new System.Windows.Forms.Button();
			this.flagBTN = new System.Windows.Forms.Button();
			this.outputRTB = new System.Windows.Forms.RichTextBox();
			this.importBTN = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.importBTN);
			this.splitContainer1.Panel1.Controls.Add(this.settingsBTN);
			this.splitContainer1.Panel1.Controls.Add(this.flagBTN);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.outputRTB);
			this.splitContainer1.Size = new System.Drawing.Size(637, 547);
			this.splitContainer1.SplitterDistance = 51;
			this.splitContainer1.TabIndex = 0;
			// 
			// settingsBTN
			// 
			this.settingsBTN.Location = new System.Drawing.Point(95, 12);
			this.settingsBTN.Name = "settingsBTN";
			this.settingsBTN.Size = new System.Drawing.Size(75, 23);
			this.settingsBTN.TabIndex = 1;
			this.settingsBTN.Text = "Settings";
			this.settingsBTN.UseVisualStyleBackColor = true;
			this.settingsBTN.Click += new System.EventHandler(this.settingsBTN_Click);
			// 
			// flagBTN
			// 
			this.flagBTN.Location = new System.Drawing.Point(13, 13);
			this.flagBTN.Name = "flagBTN";
			this.flagBTN.Size = new System.Drawing.Size(75, 23);
			this.flagBTN.TabIndex = 0;
			this.flagBTN.Text = "Flag!";
			this.flagBTN.UseVisualStyleBackColor = true;
			this.flagBTN.Click += new System.EventHandler(this.flagBTN_Click);
			// 
			// outputRTB
			// 
			this.outputRTB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outputRTB.Location = new System.Drawing.Point(0, 0);
			this.outputRTB.Name = "outputRTB";
			this.outputRTB.Size = new System.Drawing.Size(637, 492);
			this.outputRTB.TabIndex = 0;
			this.outputRTB.Text = "";
			// 
			// importBTN
			// 
			this.importBTN.Location = new System.Drawing.Point(177, 11);
			this.importBTN.Name = "importBTN";
			this.importBTN.Size = new System.Drawing.Size(75, 23);
			this.importBTN.TabIndex = 2;
			this.importBTN.Text = "Import";
			this.importBTN.UseVisualStyleBackColor = true;
			this.importBTN.Click += new System.EventHandler(this.importBTN_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// PowerFlag
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(637, 547);
			this.Controls.Add(this.splitContainer1);
			this.Name = "PowerFlag";
			this.Text = "PowerFlag";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button flagBTN;
		private System.Windows.Forms.RichTextBox outputRTB;
		private System.Windows.Forms.Button settingsBTN;
		private System.Windows.Forms.Button importBTN;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}