namespace KCIBasic
{
    partial class KCI
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KCI));
            this.OutputLabel = new System.Windows.Forms.Label();
            this.GIF = new System.Windows.Forms.PictureBox();
            this.StartPanel = new System.Windows.Forms.Panel();
            this.StartButton = new System.Windows.Forms.Button();
            this.KTSCheckBox = new System.Windows.Forms.CheckBox();
            this.KISCheckBox = new System.Windows.Forms.CheckBox();
            this.KAVCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.GIF)).BeginInit();
            this.StartPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OutputLabel
            // 
            this.OutputLabel.BackColor = System.Drawing.Color.Transparent;
            this.OutputLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutputLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputLabel.Location = new System.Drawing.Point(0, 0);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(386, 76);
            this.OutputLabel.TabIndex = 0;
            this.OutputLabel.Text = "Iniciando Kaspersky Custom Installer";
            this.OutputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OutputLabel.UseMnemonic = false;
            // 
            // GIF
            // 
            this.GIF.BackColor = System.Drawing.Color.Transparent;
            this.GIF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GIF.ErrorImage = null;
            this.GIF.Image = ((System.Drawing.Image)(resources.GetObject("GIF.Image")));
            this.GIF.InitialImage = null;
            this.GIF.Location = new System.Drawing.Point(65, 57);
            this.GIF.Name = "GIF";
            this.GIF.Size = new System.Drawing.Size(256, 10);
            this.GIF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.GIF.TabIndex = 1;
            this.GIF.TabStop = false;
            this.GIF.Visible = false;
            // 
            // StartPanel
            // 
            this.StartPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StartPanel.Controls.Add(this.StartButton);
            this.StartPanel.Controls.Add(this.KTSCheckBox);
            this.StartPanel.Controls.Add(this.KISCheckBox);
            this.StartPanel.Controls.Add(this.KAVCheckBox);
            this.StartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartPanel.Location = new System.Drawing.Point(0, 0);
            this.StartPanel.Name = "StartPanel";
            this.StartPanel.Size = new System.Drawing.Size(386, 76);
            this.StartPanel.TabIndex = 2;
            this.StartPanel.Visible = false;
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.SystemColors.Control;
            this.StartButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartButton.Enabled = false;
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(226, 11);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(147, 52);
            this.StartButton.TabIndex = 3;
            this.StartButton.Text = "Iniciar instalación";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // KTSCheckBox
            // 
            this.KTSCheckBox.AutoSize = true;
            this.KTSCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KTSCheckBox.Location = new System.Drawing.Point(11, 47);
            this.KTSCheckBox.Name = "KTSCheckBox";
            this.KTSCheckBox.Size = new System.Drawing.Size(175, 22);
            this.KTSCheckBox.TabIndex = 2;
            this.KTSCheckBox.Text = "Kaspersky Total Security";
            this.KTSCheckBox.UseVisualStyleBackColor = true;
            this.KTSCheckBox.CheckedChanged += new System.EventHandler(this.KTSCheckBox_CheckedChanged);
            // 
            // KISCheckBox
            // 
            this.KISCheckBox.AutoSize = true;
            this.KISCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KISCheckBox.Location = new System.Drawing.Point(11, 26);
            this.KISCheckBox.Name = "KISCheckBox";
            this.KISCheckBox.Size = new System.Drawing.Size(196, 22);
            this.KISCheckBox.TabIndex = 1;
            this.KISCheckBox.Text = "Kaspersky Internet Security";
            this.KISCheckBox.UseVisualStyleBackColor = true;
            this.KISCheckBox.CheckedChanged += new System.EventHandler(this.KISCheckBox_CheckedChanged);
            // 
            // KAVCheckBox
            // 
            this.KAVCheckBox.AutoSize = true;
            this.KAVCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KAVCheckBox.Location = new System.Drawing.Point(11, 5);
            this.KAVCheckBox.Name = "KAVCheckBox";
            this.KAVCheckBox.Size = new System.Drawing.Size(147, 22);
            this.KAVCheckBox.TabIndex = 0;
            this.KAVCheckBox.Text = "Kaspersky Antivirus";
            this.KAVCheckBox.UseVisualStyleBackColor = true;
            this.KAVCheckBox.CheckedChanged += new System.EventHandler(this.KAVCheckBox_CheckedChanged);
            // 
            // KCI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(386, 76);
            this.Controls.Add(this.StartPanel);
            this.Controls.Add(this.GIF);
            this.Controls.Add(this.OutputLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KCI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KCI";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Shown += new System.EventHandler(this.KCI_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.GIF)).EndInit();
            this.StartPanel.ResumeLayout(false);
            this.StartPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.PictureBox GIF;
        private System.Windows.Forms.Panel StartPanel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.CheckBox KTSCheckBox;
        private System.Windows.Forms.CheckBox KISCheckBox;
        private System.Windows.Forms.CheckBox KAVCheckBox;
    }
}

