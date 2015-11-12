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
            this.statusImage = new System.Windows.Forms.PictureBox();
            this.objLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.statusImage)).BeginInit();
            this.SuspendLayout();
            // 
            // statusImage
            // 
            this.statusImage.Image = global::ARSMonitor.Properties.Resources.OFF;
            this.statusImage.InitialImage = global::ARSMonitor.Properties.Resources.OFF;
            this.statusImage.Location = new System.Drawing.Point(0, 0);
            this.statusImage.Name = "statusImage";
            this.statusImage.Size = new System.Drawing.Size(31, 46);
            this.statusImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.statusImage.TabIndex = 0;
            this.statusImage.TabStop = false;
            // 
            // objLabel
            // 
            this.objLabel.AutoSize = true;
            this.objLabel.Location = new System.Drawing.Point(37, 11);
            this.objLabel.Name = "objLabel";
            this.objLabel.Size = new System.Drawing.Size(47, 13);
            this.objLabel.TabIndex = 1;
            this.objLabel.Text = "objLabel";
            // 
            // serverControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objLabel);
            this.Controls.Add(this.statusImage);
            this.Name = "serverControl";
            this.Size = new System.Drawing.Size(176, 46);
            ((System.ComponentModel.ISupportInitialize)(this.statusImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox statusImage;
        private System.Windows.Forms.Label objLabel;
    }
}
