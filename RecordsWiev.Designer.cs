namespace Lines
{
	partial class RecordsWiev
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
			this.flInfo = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// flInfo
			// 
			this.flInfo.AutoSize = true;
			this.flInfo.Location = new System.Drawing.Point(32, 33);
			this.flInfo.Name = "flInfo";
			this.flInfo.Size = new System.Drawing.Size(46, 17);
			this.flInfo.TabIndex = 0;
			this.flInfo.Text = "label1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(169, 435);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(191, 49);
			this.button1.TabIndex = 1;
			this.button1.Text = "Подтвердить";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// RecordsWiev
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(551, 496);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.flInfo);
			this.Name = "RecordsWiev";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Рекорды";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label flInfo;
		private System.Windows.Forms.Button button1;
	}
}