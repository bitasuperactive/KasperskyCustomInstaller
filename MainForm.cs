using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KCIBasic
{
    public partial class MainForm : Form
    {
        public static RegistryKey LocalMachine32View { get; set; } = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        public static RegistryKey LocalMachine64View { get; set; } = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        public static string AVPversion { get; set; }
        public static string KavEditionInstalled { get; set; } = "Kaspersky";
        public static string AVProot { get; set; }
        public static string KavGuid { get; set; }

        public MainForm()
        {
            InitializeComponent(); //Properties.Settings.Default.Reset();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await CheckRequirements();

            SetOutputFormEvents();

            if (Properties.Settings.Default.MainThreadDone.Equals(false))
            {
                Properties.Settings.Default.Reset();
                await CheckKavData();
                MainPanel.Visible = true;
            }
            else
            {
                outputForm.Show(this);
            }
        }


        #region OutputForm
        public OutputForm outputForm { get; set; } = new OutputForm();

        private void SetOutputFormEvents()
        {
            outputForm.FormClosed += OutputForm_FormClosed;
            outputForm.VisibleChanged += OutputForm_VisibleChanged;
        }

        private void OutputForm_VisibleChanged(object sender, EventArgs e)
        {
            if (outputForm.Visible) Hide();
            if (!outputForm.Visible) Show();
        }

        public void OutputForm_FormClosed(object sender, FormClosedEventArgs close)
        {
            //outputForm = null;
            Application.Exit();
        }
        #endregion


        #region Checking
        public async Task CheckRequirements()
        {
            await Task.Delay(500);

            if (Process.GetProcesses().Count(process => process.ProcessName == Process.GetCurrentProcess().ProcessName) > 1)
            {
                MessageBox.Show("Kaspersky Custom Installer no responde.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }

            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Permisos de Administrador requeridos.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }
        }
        public bool KavInstalled()
        {
            if (MainForm.LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
            {
                foreach (string SubKeys in MainForm.LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                {
                    if (SubKeys.Contains("AVP")) return true;
                }
            }
            return false;
        }

        private async Task CheckKavData()
        {
            await Task.Delay(500);

            if (KavInstalled())
            {
                foreach (string AVP in LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                {
                    if (AVP.Contains("AVP"))
                    {
                        AVPversion = AVP;
                        AVProot = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVP}\environment").GetValue("ProductRoot").ToString();
                        KavEditionInstalled = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVP}\environment").GetValue("ProductName").ToString();
                        KavGuid = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVP}\environment").GetValue("ProductCode").ToString();
                        break;
                    }
                }

                if (LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab\WmiHlp").GetValueNames().Contains("IsReportedExpired"))
                {
                    if (KavEditionInstalled == "Kaspersky Anti-Virus")
                    {
                        Properties.Settings.Default.KavLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
                    }

                    else if (KavEditionInstalled == "Kaspersky Internet Security")
                    {
                        Properties.Settings.Default.KavLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
                    }

                    else if (KavEditionInstalled == "Kaspersky Total Security")
                    {
                        Properties.Settings.Default.KavLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
                    }

                    ActivateButton.Enabled = true;
                }
                else
                {
                    ActivateButton.Text = "Producto Activado";
                }

                //if (LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVProot}\Data\PatchRollbackDetector") == null)
                //{
                //    StartingLabel.Text = "Actualizando bases de datos de Kaspersky Lab";
                //    await outputForm.HiddenProcess($@"{AVProot}\avp.com", "UPDATE");
                //}
            }
            else
            {
                ActivateButton.Enabled = false;
                BackupButton.Enabled = false;
            }
        }
        #endregion


        #region Buttons
        private void StartButton_Click(object sender, EventArgs e) => StartButtonMethod();
        private void StartAutoButton_Click(object sender, EventArgs e) => StartAutoButtonMethod();
        private void KAVRadioButton_CheckedChanged(object sender, EventArgs e) => RadioButtonMethod();
        private void KISRadioButton_CheckedChanged(object sender, EventArgs e) => RadioButtonMethod();
        private void KTSRadioButton_CheckedChanged(object sender, EventArgs e) => RadioButtonMethod();
        private void SecureConnectionCheckBox_CheckedChanged(object sender, EventArgs e) => SecureConnectionCheckBoxMethod();
        private void ActivateButton_Click(object sender, EventArgs e) => ActivateButtonMethod();
        private void BackupButton_Click(object sender, EventArgs e) => BackupButtonMethod();

        #region CheckBoxes
        private void RadioButtonMethod()
        {
            if (KAVRadioButton.Checked)
            {
                KISRadioButton.Font = new Font(KISRadioButton.Font, FontStyle.Regular);
                KTSRadioButton.Font = new Font(KTSRadioButton.Font, FontStyle.Regular);
                KAVRadioButton.Font = new Font(KAVRadioButton.Font, FontStyle.Bold);
            }
            else if (KISRadioButton.Checked)
            {
                KAVRadioButton.Font = new Font(KAVRadioButton.Font, FontStyle.Regular);
                KTSRadioButton.Font = new Font(KTSRadioButton.Font, FontStyle.Regular);
                KISRadioButton.Font = new Font(KISRadioButton.Font, FontStyle.Bold);
            }
            else if (KTSRadioButton.Checked)
            {
                KAVRadioButton.Font = new Font(KAVRadioButton.Font, FontStyle.Regular);
                KISRadioButton.Font = new Font(KISRadioButton.Font, FontStyle.Regular);
                KTSRadioButton.Font = new Font(KTSRadioButton.Font, FontStyle.Bold);
            }
        }

        private void StartButtonMethod()
        {
            Properties.Settings.Default.AutoInstall = false;

            outputForm.Show(this);
        }

        private void StartAutoButtonMethod()
        {
            try
            {
                if (LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPversion}\Data\FlushedMsiCriticalSettings").GetValue("EnablePswrdProtect").ToString() == "1")
                {
                    MessageBox.Show(this, $"Debes desactivar Kaspersky Password Protect (protección por contraseña del Antivirus) y cerrar {KavEditionInstalled} manualmente para continuar.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            catch (NullReferenceException) { }

            if (Process.GetProcessesByName("avp").Length > 0)
            {
                MessageBox.Show(this, $"Debes cerrar {KavEditionInstalled} manualmente para continuar.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Properties.Settings.Default.AutoInstall = true;

            outputForm.Show(this);
        }

        private void OfflineSetupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (OfflineSetupCheckBox.Checked) StartAutoButton.Enabled = true;

            //else StartAutoButton.Enabled = false;
        }

        private void SecureConnectionCheckBoxMethod()
        {
            if (SecureConnectionCheckBox.Checked) Properties.Settings.Default.KavSecureConnection = true;
            
            else Properties.Settings.Default.KavSecureConnection = false;
        }
        #endregion

        private async void ActivateButtonMethod()
        {
            MainPanel.Visible = false; TopMost = true;

            StartingLabel.Text = "Actualizando producto"; GIF.Visible = true;
            //await Task.Delay(2000);
            await outputForm.HiddenProcess($@"{AVProot}\avp.com", "UPDATE"); GIF.Visible = false; // Needed to update "IsReportedExpired" value.

            await outputForm.License(StartingLabel);

            StartingLabel.Text = "Activando licencia de evaluación del producto";
            await Task.Delay(2000);

            string LicenseKey1;
            using (var reader = new StreamReader(Path.GetTempPath() + "kavlicense.txt"))
            {
                LicenseKey1 = reader.ReadLine();
            }
            await outputForm.HiddenProcess("cmd.exe", $"/C echo & \"{MainForm.AVProot}\\avp.com\" LICENSE /ADD {LicenseKey1}");

            if (MainForm.LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab\WmiHlp").GetValueNames().Contains("IsReportedExpired"))
            {
                MessageBox.Show(this, $"No ha sido posible realizar la activación de {KavEditionInstalled}. Intenta [Activar la versión de evaluación de la aplicación] desde el apartado [Introducir código de activación].", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Opacity = 0;

                Process.Start($@"{AVProot}\avpui.exe");

                MessageBox.Show(this, $"{KavEditionInstalled} ha sido activado con éxito.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ActivateButton.Enabled = false; ActivateButton.Text = "Producto Activado";
                
                this.Opacity = 100;
            }

            MainPanel.Visible = true; TopMost = false;
        }

        private async void BackupButtonMethod()
        {
            MainPanel.Visible = false; TopMost = true;

            StartingLabel.Text = "Exportando configuración del producto"; GIF.Visible = true;
            await Task.Delay(2000);

            await outputForm.HiddenProcess($@"{AVProot}\avp.com", $"EXPORT \"{Directory.GetCurrentDirectory()}\\{KavEditionInstalled} Configuration.cfg\""); GIF.Visible = false;

            if (File.Exists($@"{Directory.GetCurrentDirectory()}\{KavEditionInstalled} Configuration.cfg"))
                MessageBox.Show(this, $"Configuración de {KavEditionInstalled} exportada con éxito.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this, $"No ha sido posible exportar la configuración de {KavEditionInstalled}. Debes exportarla manualmente desde el apartado [Exportar Configuración] dentro de ajustes de la aplicación.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);

            MainPanel.Visible = true; TopMost = false;
        }
        #endregion
    }
}
