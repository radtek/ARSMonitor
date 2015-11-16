namespace ARSMonitor
{
    partial class serverControl
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
            this.objLabel = new System.Windows.Forms.Label();
            this.statusImage = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.statusImage)).BeginInit();
            this.SuspendLayout();
            // 
            // objLabel
            // 
            this.objLabel.AutoSize = true;
            this.objLabel.Location = new System.Drawing.Point(79, 11);
            this.objLabel.Name = "objLabel";
            this.objLabel.Size = new System.Drawing.Size(47, 13);
            this.objLabel.TabIndex = 1;
            this.objLabel.Text = "objLabel";
            // 
            // statusImage
            // 
            this.statusImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusImage.Image = global::ARSMonitor.Properties.Resources.OFF;
            this.statusImage.InitialImage = global::ARSMonitor.Properties.Resources.OFF;
            this.statusImage.Location = new System.Drawing.Point(-1, -1);
            this.statusImage.Margin = new System.Windows.Forms.Padding(4);
            this.statusImage.Name = "statusImage";
            this.statusImage.Padding = new System.Windows.Forms.Padding(2);
            this.statusImage.Size = new System.Drawing.Size(57, 46);
            this.statusImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.statusImage.TabIndex = 0;
            this.statusImage.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(73, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(102, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Visible = false;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(100, 22);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 3;
            this.ok.Text = "button1";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Visible = false;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // serverControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ok);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.objLabel);
            this.Controls.Add(this.statusImage);
            this.Name = "serverControl";
            this.Size = new System.Drawing.Size(174, 44);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.serverControl_MouseDown);
            this.MouseLeave += new System.EventHandler(this.serverControl_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.statusImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox statusImage;
        private System.Windows.Forms.Label objLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button ok;
    }
}
