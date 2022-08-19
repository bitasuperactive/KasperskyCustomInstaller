namespace KCIBasic
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainPanel = new System.Windows.Forms.Panel();
            this.KTSRadioButton = new System.Windows.Forms.RadioButton();
            this.KISRadioButton = new System.Windows.Forms.RadioButton();
            this.KAVRadioButton = new System.Windows.Forms.RadioButton();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.StartButton = new System.Windows.Forms.Button();
            this.BackupButton = new System.Windows.Forms.Button();
            this.StartAutoButton = new System.Windows.Forms.Button();
            this.ActivateButton = new System.Windows.Forms.Button();
            this.SecureConnectionCheckBox = new System.Windows.Forms.CheckBox();
            this.OfflineSetupCheckBox = new System.Windows.Forms.CheckBox();
            this.StartingLabel = new System.Windows.Forms.Label();
            this.GIF = new System.Windows.Forms.PictureBox();
            this.MainPanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GIF)).BeginInit();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.KTSRadioButton);
            this.MainPanel.Controls.Add(this.KISRadioButton);
            this.MainPanel.Controls.Add(this.KAVRadioButton);
            this.MainPanel.Controls.Add(this.ButtonsPanel);
            this.MainPanel.Controls.Add(this.SecureConnectionCheckBox);
            this.MainPanel.Controls.Add(this.OfflineSetupCheckBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(434, 167);
            this.MainPanel.TabIndex = 2;
            this.MainPanel.Visible = false;
            // 
            // KTSRadioButton
            // 
            this.KTSRadioButton.AutoSize = true;
            this.KTSRadioButton.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.KTSRadioButton.Location = new System.Drawing.Point(11, 59);
            this.KTSRadioButton.Name = "KTSRadioButton";
            this.KTSRadioButton.Size = new System.Drawing.Size(174, 22);
            this.KTSRadioButton.TabIndex = 14;
            this.KTSRadioButton.Text = "Kaspersky Total Security";
            this.KTSRadioButton.UseVisualStyleBackColor = true;
            this.KTSRadioButton.CheckedChanged += new System.EventHandler(this.KTSRadioButton_CheckedChanged);
            // 
            // KISRadioButton
            // 
            this.KISRadioButton.AutoSize = true;
            this.KISRadioButton.Checked = true;
            this.KISRadioButton.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KISRadioButton.Location = new System.Drawing.Point(11, 36);
            this.KISRadioButton.Name = "KISRadioButton";
            this.KISRadioButton.Size = new System.Drawing.Size(195, 22);
            this.KISRadioButton.TabIndex = 13;
            this.KISRadioButton.TabStop = true;
            this.KISRadioButton.Text = "Kaspersky Internet Security";
            this.KISRadioButton.UseVisualStyleBackColor = true;
            this.KISRadioButton.CheckedChanged += new System.EventHandler(this.KISRadioButton_CheckedChanged);
            // 
            // KAVRadioButton
            // 
            this.KAVRadioButton.AutoSize = true;
            this.KAVRadioButton.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.KAVRadioButton.Location = new System.Drawing.Point(11, 13);
            this.KAVRadioButton.Name = "KAVRadioButton";
            this.KAVRadioButton.Size = new System.Drawing.Size(153, 22);
            this.KAVRadioButton.TabIndex = 8;
            this.KAVRadioButton.Text = "Kaspersky Anti-Virus";
            this.KAVRadioButton.UseVisualStyleBackColor = true;
            this.KAVRadioButton.CheckedChanged += new System.EventHandler(this.KAVRadioButton_CheckedChanged);
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ButtonsPanel.Controls.Add(this.StartButton);
            this.ButtonsPanel.Controls.Add(this.BackupButton);
            this.ButtonsPanel.Controls.Add(this.StartAutoButton);
            this.ButtonsPanel.Controls.Add(this.ActivateButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonsPanel.Location = new System.Drawing.Point(242, 0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(190, 165);
            this.ButtonsPanel.TabIndex = 12;
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.SystemColors.Control;
            this.StartButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(11, 9);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(169, 40);
            this.StartButton.TabIndex = 3;
            this.StartButton.Tag = "";
            this.StartButton.Text = "Instalación Habitual";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // BackupButton
            // 
            this.BackupButton.BackColor = System.Drawing.SystemColors.Control;
            this.BackupButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BackupButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BackupButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackupButton.Location = new System.Drawing.Point(11, 129);
            this.BackupButton.Name = "BackupButton";
            this.BackupButton.Size = new System.Drawing.Size(169, 22);
            this.BackupButton.TabIndex = 5;
            this.BackupButton.Text = "Exportar Configuración";
            this.BackupButton.UseVisualStyleBackColor = false;
            this.BackupButton.Click += new System.EventHandler(this.BackupButton_Click);
            // 
            // StartAutoButton
            // 
            this.StartAutoButton.BackColor = System.Drawing.SystemColors.Control;
            this.StartAutoButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartAutoButton.Enabled = false;
            this.StartAutoButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartAutoButton.Location = new System.Drawing.Point(11, 55);
            this.StartAutoButton.Name = "StartAutoButton";
            this.StartAutoButton.Size = new System.Drawing.Size(169, 40);
            this.StartAutoButton.TabIndex = 4;
            this.StartAutoButton.Text = "Instalación Automática";
            this.StartAutoButton.UseVisualStyleBackColor = false;
            this.StartAutoButton.Click += new System.EventHandler(this.StartAutoButton_Click);
            // 
            // ActivateButton
            // 
            this.ActivateButton.BackColor = System.Drawing.SystemColors.Control;
            this.ActivateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActivateButton.Enabled = false;
            this.ActivateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ActivateButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivateButton.Location = new System.Drawing.Point(11, 101);
            this.ActivateButton.Name = "ActivateButton";
            this.ActivateButton.Size = new System.Drawing.Size(169, 22);
            this.ActivateButton.TabIndex = 7;
            this.ActivateButton.Text = "Activar Producto";
            this.ActivateButton.UseVisualStyleBackColor = false;
            this.ActivateButton.Click += new System.EventHandler(this.ActivateButton_Click);
            // 
            // SecureConnectionCheckBox
            // 
            this.SecureConnectionCheckBox.AutoSize = true;
            this.SecureConnectionCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecureConnectionCheckBox.Location = new System.Drawing.Point(24, 106);
            this.SecureConnectionCheckBox.Name = "SecureConnectionCheckBox";
            this.SecureConnectionCheckBox.Size = new System.Drawing.Size(207, 22);
            this.SecureConnectionCheckBox.TabIndex = 6;
            this.SecureConnectionCheckBox.Text = "Kaspersky Secure Connection";
            this.SecureConnectionCheckBox.UseVisualStyleBackColor = true;
            this.SecureConnectionCheckBox.CheckedChanged += new System.EventHandler(this.SecureConnectionCheckBox_CheckedChanged);
            // 
            // OfflineSetupCheckBox
            // 
            this.OfflineSetupCheckBox.AutoSize = true;
            this.OfflineSetupCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OfflineSetupCheckBox.Location = new System.Drawing.Point(24, 84);
            this.OfflineSetupCheckBox.Name = "OfflineSetupCheckBox";
            this.OfflineSetupCheckBox.Size = new System.Drawing.Size(108, 22);
            this.OfflineSetupCheckBox.TabIndex = 11;
            this.OfflineSetupCheckBox.Text = "Offline Setup";
            this.OfflineSetupCheckBox.UseVisualStyleBackColor = true;
            this.OfflineSetupCheckBox.CheckedChanged += new System.EventHandler(this.OfflineSetupCheckBox_CheckedChanged);
            // 
            // StartingLabel
            // 
            this.StartingLabel.BackColor = System.Drawing.Color.Transparent;
            this.StartingLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StartingLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartingLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartingLabel.Location = new System.Drawing.Point(0, 0);
            this.StartingLabel.Name = "StartingLabel";
            this.StartingLabel.Size = new System.Drawing.Size(434, 167);
            this.StartingLabel.TabIndex = 0;
            this.StartingLabel.Text = "Iniciando Kaspersky Custom Installer";
            this.StartingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StartingLabel.UseMnemonic = false;
            // 
            // GIF
            // 
            this.GIF.BackColor = System.Drawing.Color.Transparent;
            this.GIF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GIF.ErrorImage = null;
            this.GIF.Image = ((System.Drawing.Image)(resources.GetObject("GIF.Image")));
            this.GIF.InitialImage = null;
            this.GIF.Location = new System.Drawing.Point(4, 102);
            this.GIF.Name = "GIF";
            this.GIF.Size = new System.Drawing.Size(426, 10);
            this.GIF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.GIF.TabIndex = 16;
            this.GIF.TabStop = false;
            this.GIF.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(434, 167);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.GIF);
            this.Controls.Add(this.StartingLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KCI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GIF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel MainPanel;
        public System.Windows.Forms.Button StartButton;
        public System.Windows.Forms.Button StartAutoButton;
        public System.Windows.Forms.Button BackupButton;
        public System.Windows.Forms.CheckBox SecureConnectionCheckBox;
        public System.Windows.Forms.Button ActivateButton;
        public System.Windows.Forms.CheckBox OfflineSetupCheckBox;
        public System.Windows.Forms.Label StartingLabel;
        private System.Windows.Forms.Panel ButtonsPanel;
        public System.Windows.Forms.RadioButton KAVRadioButton;
        public System.Windows.Forms.RadioButton KTSRadioButton;
        public System.Windows.Forms.RadioButton KISRadioButton;
        private System.Windows.Forms.PictureBox GIF;
    }
}

