namespace 
{
	partial class 
	{
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{

			this.Button4 = new System.Windows.Forms.Button();
			this.Button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// Button4 
			//
			this.Button4.Location = new System.Drawing.Point(85, 70);
			this.Button4.Name = "Button4";
			this.Button4.Size = new System.Drawing.Size(75, 23);
			this.Button4.Text = "Button5";
			this.Button4.BackColor = System.Drawing.Color.FromArgb(-6250336);
			this.Button4.ForeColor = System.Drawing.Color.FromArgb(-16777216);
			// 
			// Button2 
			//
			this.Button2.Location = new System.Drawing.Point(107, 154);
			this.Button2.Name = "Button2";
			this.Button2.Size = new System.Drawing.Size(75, 23);
			this.Button2.Text = "Button2";
			this.Button2.BackColor = System.Drawing.Color.FromArgb(-6250336);
			this.Button2.ForeColor = System.Drawing.Color.FromArgb(-16777216);
			// 
			//  
			//
			 this.AutoScaleDimensions = new System.Drawing.SizeF(6, 13);
			 this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(796, 612);


			this.ResumeLayout(false);
			this.PerformLayout(); 
			this.Controls.Add(Button4);
			this.Controls.Add(Button2);
			Subscriptions();
		}
		private void Subscriptions()
		{ }
		private System.Windows.Forms.Button Button4;
		private System.Windows.Forms.Button Button2;
	}
}
