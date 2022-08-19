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
            this.OfflineSetupCheckBox = new System.Windows.Forms.CheckBox();
            this.Title = new System.Windows.Forms.Label();
            this.ActivateButton = new System.Windows.Forms.Button();
            this.BackupButton = new System.Windows.Forms.Button();
            this.SecureConnectionCheckBox = new System.Windows.Forms.CheckBox();
            this.StartAutoButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.KTSCheckBox = new System.Windows.Forms.CheckBox();
            this.KISCheckBox = new System.Windows.Forms.CheckBox();
            this.KAVCheckBox = new System.Windows.Forms.CheckBox();
            this.StartingLabel = new System.Windows.Forms.Label();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.OfflineSetupCheckBox);
            this.MainPanel.Controls.Add(this.Title);
            this.MainPanel.Controls.Add(this.ActivateButton);
            this.MainPanel.Controls.Add(this.BackupButton);
            this.MainPanel.Controls.Add(this.SecureConnectionCheckBox);
            this.MainPanel.Controls.Add(this.StartAutoButton);
            this.MainPanel.Controls.Add(this.StartButton);
            this.MainPanel.Controls.Add(this.KTSCheckBox);
            this.MainPanel.Controls.Add(this.KISCheckBox);
            this.MainPanel.Controls.Add(this.KAVCheckBox);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(459, 167);
            this.MainPanel.TabIndex = 2;
            this.MainPanel.Visible = false;
            // 
            // OfflineSetupCheckBox
            // 
            this.OfflineSetupCheckBox.AutoSize = true;
            this.OfflineSetupCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OfflineSetupCheckBox.Location = new System.Drawing.Point(24, 105);
            this.OfflineSetupCheckBox.Name = "OfflineSetupCheckBox";
            this.OfflineSetupCheckBox.Size = new System.Drawing.Size(108, 22);
            this.OfflineSetupCheckBox.TabIndex = 11;
            this.OfflineSetupCheckBox.Text = "Offline Setup";
            this.OfflineSetupCheckBox.UseVisualStyleBackColor = true;
            this.OfflineSetupCheckBox.CheckedChanged += new System.EventHandler(this.OfflineSetupCheckBox_CheckedChanged);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(32, 4);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(219, 23);
            this.Title.TabIndex = 9;
            this.Title.Text = "Kaspersky Custom Installer";
            // 
            // ActivateButton
            // 
            this.ActivateButton.BackColor = System.Drawing.SystemColors.Control;
            this.ActivateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActivateButton.Enabled = false;
            this.ActivateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ActivateButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivateButton.Location = new System.Drawing.Point(277, 107);
            this.ActivateButton.Name = "ActivateButton";
            this.ActivateButton.Size = new System.Drawing.Size(169, 22);
            this.ActivateButton.TabIndex = 7;
            this.ActivateButton.Text = "Activar Producto";
            this.ActivateButton.UseVisualStyleBackColor = false;
            this.ActivateButton.Click += new System.EventHandler(this.ActivateButton_Click);
            // 
            // BackupButton
            // 
            this.BackupButton.BackColor = System.Drawing.SystemColors.Control;
            this.BackupButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BackupButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BackupButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackupButton.Location = new System.Drawing.Point(277, 133);
            this.BackupButton.Name = "BackupButton";
            this.BackupButton.Size = new System.Drawing.Size(169, 22);
            this.BackupButton.TabIndex = 5;
            this.BackupButton.Text = "Exportar Configuración";
            this.BackupButton.UseVisualStyleBackColor = false;
            this.BackupButton.Click += new System.EventHandler(this.BackupButton_Click);
            // 
            // SecureConnectionCheckBox
            // 
            this.SecureConnectionCheckBox.AutoSize = true;
            this.SecureConnectionCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecureConnectionCheckBox.Location = new System.Drawing.Point(24, 125);
            this.SecureConnectionCheckBox.Name = "SecureConnectionCheckBox";
            this.SecureConnectionCheckBox.Size = new System.Drawing.Size(247, 22);
            this.SecureConnectionCheckBox.TabIndex = 6;
            this.SecureConnectionCheckBox.Text = "Kaspersky Secure Connection (VPN)";
            this.SecureConnectionCheckBox.UseVisualStyleBackColor = true;
            this.SecureConnectionCheckBox.CheckedChanged += new System.EventHandler(this.SecureConnectionCheckBox_CheckedChanged);
            // 
            // StartAutoButton
            // 
            this.StartAutoButton.BackColor = System.Drawing.SystemColors.Control;
            this.StartAutoButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartAutoButton.Enabled = false;
            this.StartAutoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartAutoButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartAutoButton.Location = new System.Drawing.Point(277, 64);
            this.StartAutoButton.Name = "StartAutoButton";
            this.StartAutoButton.Size = new System.Drawing.Size(169, 37);
            this.StartAutoButton.TabIndex = 4;
            this.StartAutoButton.Text = "Instalación Automática";
            this.StartAutoButton.UseVisualStyleBackColor = false;
            this.StartAutoButton.Click += new System.EventHandler(this.StartAutoButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.SystemColors.Control;
            this.StartButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(277, 9);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(169, 49);
            this.StartButton.TabIndex = 3;
            this.StartButton.Tag = "";
            this.StartButton.Text = "Instalación Habitual";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // KTSCheckBox
            // 
            this.KTSCheckBox.AutoSize = true;
            this.KTSCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KTSCheckBox.Location = new System.Drawing.Point(11, 83);
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
            this.KISCheckBox.Checked = true;
            this.KISCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.KISCheckBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KISCheckBox.Location = new System.Drawing.Point(11, 60);
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
            this.KAVCheckBox.Location = new System.Drawing.Point(11, 37);
            this.KAVCheckBox.Name = "KAVCheckBox";
            this.KAVCheckBox.Size = new System.Drawing.Size(147, 22);
            this.KAVCheckBox.TabIndex = 0;
            this.KAVCheckBox.Text = "Kaspersky Antivirus";
            this.KAVCheckBox.UseVisualStyleBackColor = true;
            this.KAVCheckBox.CheckedChanged += new System.EventHandler(this.KAVCheckBox_CheckedChanged);
            // 
            // StartingLabel
            // 
            this.StartingLabel.BackColor = System.Drawing.Color.Transparent;
            this.StartingLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StartingLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartingLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartingLabel.Location = new System.Drawing.Point(0, 0);
            this.StartingLabel.Name = "StartingLabel";
            this.StartingLabel.Size = new System.Drawing.Size(459, 167);
            this.StartingLabel.TabIndex = 0;
            this.StartingLabel.Text = "Iniciando Kaspersky Custom Installer";
            this.StartingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StartingLabel.UseMnemonic = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(459, 167);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.StartingLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KCI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Shown += new System.EventHandler(this.KCI_Shown);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.CheckBox KTSCheckBox;
        private System.Windows.Forms.CheckBox KISCheckBox;
        private System.Windows.Forms.CheckBox KAVCheckBox;
        private System.Windows.Forms.Button StartAutoButton;
        private System.Windows.Forms.Button BackupButton;
        private System.Windows.Forms.CheckBox SecureConnectionCheckBox;
        private System.Windows.Forms.Button ActivateButton;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.CheckBox OfflineSetupCheckBox;
        private System.Windows.Forms.Label StartingLabel;
    }
}

