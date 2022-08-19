namespace KCIBasic
{
    partial class OutputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputForm));
            this.OutputPanel = new System.Windows.Forms.Panel();
            this.GIF = new System.Windows.Forms.PictureBox();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.OutputPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GIF)).BeginInit();
            this.SuspendLayout();
            // 
            // OutputPanel
            // 
            this.OutputPanel.Controls.Add(this.GIF);
            this.OutputPanel.Controls.Add(this.OutputLabel);
            this.OutputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputPanel.Location = new System.Drawing.Point(0, 0);
            this.OutputPanel.Name = "OutputPanel";
            this.OutputPanel.Size = new System.Drawing.Size(450, 81);
            this.OutputPanel.TabIndex = 0;
            // 
            // GIF
            // 
            this.GIF.BackColor = System.Drawing.Color.Transparent;
            this.GIF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GIF.ErrorImage = null;
            this.GIF.Image = ((System.Drawing.Image)(resources.GetObject("GIF.Image")));
            this.GIF.InitialImage = null;
            this.GIF.Location = new System.Drawing.Point(12, 59);
            this.GIF.Name = "GIF";
            this.GIF.Size = new System.Drawing.Size(426, 10);
            this.GIF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.GIF.TabIndex = 4;
            this.GIF.TabStop = false;
            this.GIF.Visible = false;
            // 
            // OutputLabel
            // 
            this.OutputLabel.BackColor = System.Drawing.Color.Transparent;
            this.OutputLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutputLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputLabel.Location = new System.Drawing.Point(0, 0);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(450, 81);
            this.OutputLabel.TabIndex = 1;
            this.OutputLabel.Text = "Iniciando instalación";
            this.OutputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OutputLabel.UseMnemonic = false;
            // 
            // OutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 81);
            this.Controls.Add(this.OutputPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OutputForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KCI";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutputForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.OutputForm_VisibleChanged);
            this.OutputPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GIF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel OutputPanel;
        private System.Windows.Forms.PictureBox GIF;
        private System.Windows.Forms.Label OutputLabel;
    }
}