namespace PowerFlag
{
	partial class EditForm
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
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.newFeedBTN = new System.Windows.Forms.Button();
			this.feedsLB = new System.Windows.Forms.ListBox();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.rulesLB = new System.Windows.Forms.ListBox();
			this.urlTB = new System.Windows.Forms.TextBox();
			this.saveFeedBTN = new System.Windows.Forms.Button();
			this.deleteFeedBTN = new System.Windows.Forms.Button();
			this.deleteRuleBTN = new System.Windows.Forms.Button();
			this.saveRuleBTN = new System.Windows.Forms.Button();
			this.ruleTB = new System.Windows.Forms.TextBox();
			this.newRuleBTN = new System.Windows.Forms.Button();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer1.Size = new System.Drawing.Size(834, 654);
			this.splitContainer1.SplitterDistance = 400;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.deleteFeedBTN);
			this.splitContainer2.Panel1.Controls.Add(this.saveFeedBTN);
			this.splitContainer2.Panel1.Controls.Add(this.urlTB);
			this.splitContainer2.Panel1.Controls.Add(this.newFeedBTN);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.feedsLB);
			this.splitContainer2.Size = new System.Drawing.Size(400, 654);
			this.splitContainer2.SplitterDistance = 100;
			this.splitContainer2.TabIndex = 0;
			// 
			// newFeedBTN
			// 
			this.newFeedBTN.Location = new System.Drawing.Point(13, 13);
			this.newFeedBTN.Name = "newFeedBTN";
			this.newFeedBTN.Size = new System.Drawing.Size(75, 23);
			this.newFeedBTN.TabIndex = 0;
			this.newFeedBTN.Text = "New";
			this.newFeedBTN.UseVisualStyleBackColor = true;
			this.newFeedBTN.Click += new System.EventHandler(this.newFeedBTN_Click);
			// 
			// feedsLB
			// 
			this.feedsLB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.feedsLB.FormattingEnabled = true;
			this.feedsLB.Location = new System.Drawing.Point(0, 0);
			this.feedsLB.Name = "feedsLB";
			this.feedsLB.Size = new System.Drawing.Size(400, 550);
			this.feedsLB.TabIndex = 0;
			this.feedsLB.SelectedIndexChanged += new System.EventHandler(this.feedsLB_SelectedIndexChanged);
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.deleteRuleBTN);
			this.splitContainer3.Panel1.Controls.Add(this.saveRuleBTN);
			this.splitContainer3.Panel1.Controls.Add(this.newRuleBTN);
			this.splitContainer3.Panel1.Controls.Add(this.ruleTB);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.rulesLB);
			this.splitContainer3.Size = new System.Drawing.Size(430, 654);
			this.splitContainer3.SplitterDistance = 100;
			this.splitContainer3.TabIndex = 0;
			// 
			// rulesLB
			// 
			this.rulesLB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rulesLB.FormattingEnabled = true;
			this.rulesLB.Location = new System.Drawing.Point(0, 0);
			this.rulesLB.Name = "rulesLB";
			this.rulesLB.Size = new System.Drawing.Size(430, 550);
			this.rulesLB.TabIndex = 0;
			this.rulesLB.SelectedIndexChanged += new System.EventHandler(this.rulesLB_SelectedIndexChanged);
			// 
			// urlTB
			// 
			this.urlTB.Location = new System.Drawing.Point(13, 43);
			this.urlTB.Name = "urlTB";
			this.urlTB.Size = new System.Drawing.Size(375, 20);
			this.urlTB.TabIndex = 1;
			// 
			// saveFeedBTN
			// 
			this.saveFeedBTN.Location = new System.Drawing.Point(312, 70);
			this.saveFeedBTN.Name = "saveFeedBTN";
			this.saveFeedBTN.Size = new System.Drawing.Size(75, 23);
			this.saveFeedBTN.TabIndex = 2;
			this.saveFeedBTN.Text = "Save";
			this.saveFeedBTN.UseVisualStyleBackColor = true;
			this.saveFeedBTN.Click += new System.EventHandler(this.saveFeedBTN_Click);
			// 
			// deleteFeedBTN
			// 
			this.deleteFeedBTN.Location = new System.Drawing.Point(231, 70);
			this.deleteFeedBTN.Name = "deleteFeedBTN";
			this.deleteFeedBTN.Size = new System.Drawing.Size(75, 23);
			this.deleteFeedBTN.TabIndex = 3;
			this.deleteFeedBTN.Text = "Delete";
			this.deleteFeedBTN.UseVisualStyleBackColor = true;
			this.deleteFeedBTN.Click += new System.EventHandler(this.deleteFeedBTN_Click);
			// 
			// deleteRuleBTN
			// 
			this.deleteRuleBTN.Location = new System.Drawing.Point(233, 70);
			this.deleteRuleBTN.Name = "deleteRuleBTN";
			this.deleteRuleBTN.Size = new System.Drawing.Size(75, 23);
			this.deleteRuleBTN.TabIndex = 7;
			this.deleteRuleBTN.Text = "Delete";
			this.deleteRuleBTN.UseVisualStyleBackColor = true;
			this.deleteRuleBTN.Click += new System.EventHandler(this.deleteRuleBTN_Click);
			// 
			// saveRuleBTN
			// 
			this.saveRuleBTN.Location = new System.Drawing.Point(314, 70);
			this.saveRuleBTN.Name = "saveRuleBTN";
			this.saveRuleBTN.Size = new System.Drawing.Size(75, 23);
			this.saveRuleBTN.TabIndex = 6;
			this.saveRuleBTN.Text = "Save";
			this.saveRuleBTN.UseVisualStyleBackColor = true;
			this.saveRuleBTN.Click += new System.EventHandler(this.saveRuleBTN_Click);
			// 
			// ruleTB
			// 
			this.ruleTB.Location = new System.Drawing.Point(15, 43);
			this.ruleTB.Name = "ruleTB";
			this.ruleTB.Size = new System.Drawing.Size(375, 20);
			this.ruleTB.TabIndex = 5;
			// 
			// newRuleBTN
			// 
			this.newRuleBTN.Location = new System.Drawing.Point(15, 13);
			this.newRuleBTN.Name = "newRuleBTN";
			this.newRuleBTN.Size = new System.Drawing.Size(75, 23);
			this.newRuleBTN.TabIndex = 4;
			this.newRuleBTN.Text = "New";
			this.newRuleBTN.UseVisualStyleBackColor = true;
			this.newRuleBTN.Click += new System.EventHandler(this.newRuleBTN_Click);
			// 
			// EditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(834, 654);
			this.Controls.Add(this.splitContainer1);
			this.Name = "EditForm";
			this.Text = "EditForm";
			this.Load += new System.EventHandler(this.EditForm_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel1.PerformLayout();
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Button newFeedBTN;
		private System.Windows.Forms.ListBox feedsLB;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.ListBox rulesLB;
		private System.Windows.Forms.Button deleteFeedBTN;
		private System.Windows.Forms.Button saveFeedBTN;
		private System.Windows.Forms.TextBox urlTB;
		private System.Windows.Forms.Button deleteRuleBTN;
		private System.Windows.Forms.Button saveRuleBTN;
		private System.Windows.Forms.Button newRuleBTN;
		private System.Windows.Forms.TextBox ruleTB;
	}
}