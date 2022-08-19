using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KCIBasic
{
    public partial class MainForm : Form
    {
        private static RegistryKey LocalMachine32View { get; set; } = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        private static RegistryKey LocalMachine64View { get; set; } = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        private static string AVPKey { get; set; }
        public static string KavEditionInstalled { get; set; } = "Kaspersky";
        private static string AVProot { get; set; }
        private static string KavGuid { get; set; }
        public bool Working { get; set; }

        public MainForm()
        {
            InitializeComponent(); //Properties.Settings.Default.Reset();
        }

        private async void KCI_Shown(object sender, EventArgs e)
        {
            await CheckRequirements();
            if (Properties.Settings.Default.MainThreadDone.Equals(false))
            {
                Properties.Settings.Default.Reset();
                await CheckKavData();
                DefaultWindow();
            }
            else
            {
                WorkingWindow();
                SecondThread();
            }
        }

        #region Checks
        private async Task CheckRequirements()
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

        private bool KavInstalled()
        {
            if (LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
            {
                foreach (string SubKeys in LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                {
                    if (SubKeys.Contains("AVP")) return true;
                }
            }

            return false;
        }
        //CHECK INTEGRITY OF KAV SOFTWARE
        private async Task CheckKavData()
        {
            await Task.Delay(500);

            if (KavInstalled())
            {
                foreach (string AVPkey in LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                {
                    if (AVPkey.Contains("AVP"))
                    {
                        MainForm.AVPKey = AVPkey;
                        AVProot = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductRoot").ToString();
                        KavEditionInstalled = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductName").ToString();
                        KavGuid = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductCode").ToString();
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
            }
            else
            {
                BackupButton.Enabled = false;
                ActivateButton.Enabled = false;
            }
        }

        private void WorkingWindow()
        {
            Working = true;

            this.FormBorderStyle = FormBorderStyle.None;

            this.Size = new Size(410, 110);

            this.CenterToScreen();

            MainPanel.Visible = false;

            TopMost = true;
        }

        private void DefaultWindow()
        {
            Working = false;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.Size = new Size(475, 206);

            this.CenterToScreen();

            MainPanel.Visible = true;

            TopMost = false;
        }
        #endregion


        #region Buttons
        private void StartButton_Click(object sender, EventArgs e) => StartButtonMethod();
        private void StartAutoButton_Click(object sender, EventArgs e) => StartAutoButtonMethod();
        private void KAVCheckBox_CheckedChanged(object sender, EventArgs e) => CheckBoxMethod();
        private void KISCheckBox_CheckedChanged(object sender, EventArgs e) => CheckBoxMethod();
        private void KTSCheckBox_CheckedChanged(object sender, EventArgs e) => CheckBoxMethod();
        private void SecureConnectionCheckBox_CheckedChanged(object sender, EventArgs e) => SecureConnectionCheckBoxMethod();
        private void ActivateButton_Click(object sender, EventArgs e) => ActivateProduct();
        private void BackupButton_Click(object sender, EventArgs e) => BackupKavConfiguration();

        private void CheckBoxMethod()
        {
            if (KAVCheckBox.Checked || KISCheckBox.Checked || KTSCheckBox.Checked)
            {
                if (KAVCheckBox.Focused)
                {
                    KISCheckBox.Checked = false; KISCheckBox.Font = new Font(KISCheckBox.Font, FontStyle.Regular);
                    KTSCheckBox.Checked = false; KTSCheckBox.Font = new Font(KTSCheckBox.Font, FontStyle.Regular);
                    KAVCheckBox.Font = new Font(KAVCheckBox.Font, FontStyle.Bold);
                }
                else if (KISCheckBox.Focused)
                {
                    KAVCheckBox.Checked = false; KAVCheckBox.Font = new Font(KAVCheckBox.Font, FontStyle.Regular);
                    KTSCheckBox.Checked = false; KTSCheckBox.Font = new Font(KTSCheckBox.Font, FontStyle.Regular);
                    KISCheckBox.Font = new Font(KISCheckBox.Font, FontStyle.Bold);
                }
                else if (KTSCheckBox.Focused)
                {
                    KAVCheckBox.Checked = false; KAVCheckBox.Font = new Font(KAVCheckBox.Font, FontStyle.Regular);
                    KISCheckBox.Checked = false; KISCheckBox.Font = new Font(KISCheckBox.Font, FontStyle.Regular);
                    KTSCheckBox.Font = new Font(KTSCheckBox.Font, FontStyle.Bold);
                }
                StartButton.Enabled = true;
                if (OfflineSetupCheckBox.Checked) StartAutoButton.Enabled = true;
            }
            else
            {
                KAVCheckBox.Font = new Font(KAVCheckBox.Font, FontStyle.Regular);
                KISCheckBox.Font = new Font(KISCheckBox.Font, FontStyle.Regular);
                KTSCheckBox.Font = new Font(KTSCheckBox.Font, FontStyle.Regular);
                StartButton.Enabled = false;
                StartAutoButton.Enabled = false;
            }
        }

        private void StartButtonMethod()
        {
            Properties.Settings.Default.AutoInstall = false;

            WorkingWindow();

            MainThread();
        }

        private void StartAutoButtonMethod()
        {
            if (KavInstalled())
            {
                if (LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPKey}\Data\FlushedMsiCriticalSettings").GetValue("EnablePswrdProtect").ToString() == "1")
                {
                    MessageBox.Show(this, $"Debes desactivar Kaspersky Password Protect (protección por contraseña del Antivirus) y cerrar {KavEditionInstalled} manualmente para continuar.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (Process.GetProcessesByName("avp").Length > 0)
            {
                MessageBox.Show(this, $"Debes cerrar {KavEditionInstalled} manualmente para continuar.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Properties.Settings.Default.AutoInstall = true;

            WorkingWindow();

            MainThread();
        }

        private void OfflineSetupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OfflineSetupCheckBox.Checked) StartAutoButton.Enabled = true;

            else StartAutoButton.Enabled = false;
        }

        private void SecureConnectionCheckBoxMethod()
        {
            if (SecureConnectionCheckBox.Checked) Properties.Settings.Default.KavSecureConnection = true;

            else Properties.Settings.Default.KavSecureConnection = false;
        }

        private async Task BackupKavConfiguration()
        {
            MainPanel.Visible = false; TopMost = true;

            StartingLabel.Text = $"Exportando configuración del producto";
            await Task.Delay(2000);

            await HiddenProcess($@"{AVProot}\avp.com", $"EXPORT \"{Directory.GetCurrentDirectory()}\\{KavEditionInstalled} Configuration.cfg\"");

            if (File.Exists($@"{Directory.GetCurrentDirectory()}\{KavEditionInstalled} Configuration.cfg"))
                MessageBox.Show(this, $"Configuración de {KavEditionInstalled} exportada con éxito.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this, $"No ha sido posible exportar la configuración de {KavEditionInstalled}. Debes exportarla manualmente desde el apartado [Exportar Configuración] dentro de ajustes de la aplicación.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);

            MainPanel.Visible = true; TopMost = false;
        }

        private async Task ActivateProduct()
        {
            MainPanel.Visible = false; TopMost = true;

            await License();

            StartingLabel.Text = $"Activando versión de evaluación del producto";
            await Task.Delay(2000);

            string LicenseKey = string.Empty;
            using (var reader = new StreamReader(Path.GetTempPath() + "kavlicense.txt"))
            {
                LicenseKey = reader.ReadLine();
            }

            await HiddenProcess($@"{AVProot}\avp.com", $"license /add {LicenseKey}");

            if (LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab\WmiHlp").GetValueNames().Contains("IsReportedExpired"))
            {
                using (var reader = new StreamReader(Path.GetTempPath() + "kavlicense.txt"))
                {
                    reader.ReadLine();
                    LicenseKey = reader.ReadLine();
                }

                await HiddenProcess($@"{AVProot}\avp.com", $"license /add {LicenseKey}");

                if (LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab\WmiHlp").GetValueNames().Contains("IsReportedExpired"))
                {
                    MessageBox.Show(this, $"No ha sido posible realizar la activación de {KavEditionInstalled}.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show(this, $"{KavEditionInstalled} ha sido activado con éxito.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show(this, $"{KavEditionInstalled} ha sido activado con éxito.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Process.Start($@"{AVProot}\avpui.exe");

            ActivateButton.Enabled = false; ActivateButton.Text = "Producto Activado";

            MainPanel.Visible = true; TopMost = false;
        }
        #endregion


        #region MainThread
        private async void MainThread()
        {
            if (KAVCheckBox.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Anti-Virus";

                if (OfflineSetupCheckBox.Checked)
                    Properties.Settings.Default.KavSetupURL = "https://box.kaspersky.com/f/0203108bc5eb4806a22d/?dl=1";
                else
                    Properties.Settings.Default.KavSetupURL = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";

                Properties.Settings.Default.KavLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
            }
            else if (KISCheckBox.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Internet Security";

                if (OfflineSetupCheckBox.Checked)
                    Properties.Settings.Default.KavSetupURL = "https://box.kaspersky.com/f/a16a247db28a48039342/?dl=1";
                else
                    Properties.Settings.Default.KavSetupURL = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";

                Properties.Settings.Default.KavLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
            }
            else if (KTSCheckBox.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Total Security";

                if (OfflineSetupCheckBox.Checked)
                    Properties.Settings.Default.KavSetupURL = "https://box.kaspersky.com/f/9046188e18eb401c8219/?dl=1";
                else
                    Properties.Settings.Default.KavSetupURL = "https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe";

                Properties.Settings.Default.KavLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
            }

            try
            {
                await Uninstall();
                await Registry();
                await DownloadSetup();
                await License();
                Properties.Settings.Default.MainThreadDone = true;
                Properties.Settings.Default.Save();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Error: {exception.Message}", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);

                DefaultWindow();

                return;
            }

            if (KavEditionInstalled != null && Properties.Settings.Default.AutoInstall)
            {
                StartingLabel.Text = $"Reiniciando equipo";
                await Task.Delay(2000);

                await HiddenProcess("shutdown.exe", "/R /T 00");
            }

            else if (KavEditionInstalled != null)
            {
                StartingLabel.Text = "Esperando reinicio del equipo";

                MessageBox.Show(this, "Debes reiniciar el equipo para continuar la instalación.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                TopMost = false;
            }

            else SecondThread();
        }

        private async Task Uninstall()
        {
            if (KavInstalled())
            {
                if (!Properties.Settings.Default.AutoInstall.Equals(true))
                {
                    StartingLabel.Text = $"Iniciando asistente de desinstalación";
                    await Task.Delay(2000);

                    this.Location = new Point(this.Location.X, 1);
                    StartingLabel.Text = $"Esperando desinstalación de {KavEditionInstalled}";

                    await HiddenProcess("msiexec.exe", $"/i {KavGuid} /norestart REMOVE=ALL");
                }
                else
                {
                    StartingLabel.Text = $"Desinstalando {KavEditionInstalled}"; GIF.Visible = true;

                    await HiddenProcess("msiexec.exe", $"/x {KavGuid} /quiet"); GIF.Visible = false;
                }

                if (KavInstalled())
                {
                    throw new UnauthorizedAccessException($"La desinstalación de {KavEditionInstalled} no ha sido realizada correctamente. Inténtalo de nuevo." + Environment.NewLine + "Si el error persiste, por favor reinicia tu equipo para completar los cambios.");
                }

                this.CenterToScreen();
            }
        }

        private async Task Registry()
        {
            StartingLabel.Text = $"Configurando registro de Windows";
            await Task.Delay(2000);

            try
            {
                LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates");
                LocalMachine32View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");
                LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");
                LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Cryptography\RNG");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("No ha sido posible acceder al registro de Windows." + Environment.NewLine + $"Reinicia el equipo para finalizar la desinstalación de { KavEditionInstalled }, y cierra cualquier otro anti - malware abierto antes de volver a ejecutar Kaspersky Custom Installer.");
            }
            catch (ArgumentException) { }

            try
            {
                Directory.Delete($@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\Kaspersky Lab", true);
                Directory.Delete($@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\Kaspersky Lab Setup Files", true);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("No ha sido posible acceder a los archivos residuales de Kaspersky Lab." + Environment.NewLine + $"Reinicia el equipo para finalizar la desinstalación de {KavEditionInstalled}, y cierra cualquier otro anti-malware abierto antes de volver a ejecutar Kaspersky Custom Installer.");
            }
            catch (DirectoryNotFoundException) { }
        }

        private async Task DownloadSetup()
        {
            StartingLabel.Text = $"Descargando asistente de instalación";
            await Task.Delay(1000);

            try
            {
                using (WebClient client = new WebClient())
                {
                    string output = StartingLabel.Text;
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs n)
                    {
                        StartingLabel.Text = $"{output} ({n.ProgressPercentage}%)";
                    };
                    await client.DownloadFileTaskAsync(Properties.Settings.Default.KavSetupURL, Path.GetTempPath() + "kavsetup.exe");
                    await Task.Delay(1000);
                    StartingLabel.Text = output;
                }
            }
            catch (WebException)
            {
                if (MessageBox.Show(this, $"No ha sido posible descargar el instalador para {Properties.Settings.Default.KavEditionToInstall}. Comprueba tu conexión a internet.", "KCI", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry)
                {
                    await DownloadSetup();
                    return;
                }
            }
        }

        private async Task License()
        {
            StartingLabel.Text = $"Descargando licencias de evaluación";
            await Task.Delay(1000);

            try
            {
                //using (WebClient client = new WebClient()) { await client.DownloadFileTaskAsync(Properties.Settings.Default.KavLicenseUrl, Path.GetTempPath() + "kavlicense.txt"); }
                using (WebClient client = new WebClient())
                {
                    string output = StartingLabel.Text;
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs n)
                    {
                        StartingLabel.Text = $"{output} ({n.ProgressPercentage}%)";
                    };
                    await client.DownloadFileTaskAsync(Properties.Settings.Default.KavLicenseUrl, Path.GetTempPath() + "kavlicense.txt");
                    await Task.Delay(1000);
                    StartingLabel.Text = output;
                }
            }
            catch (WebException)
            {
                if (Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Anti-Virus"))
                {
                    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    {
                        text.WriteLine("YZWEN-98MCX-Z9FV1-9TUSW");
                    }
                }
                else if (Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Internet Security"))
                {
                    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    {
                        text.WriteLine("4CH4C-PPFDT-NFK4B-45R69");
                        text.WriteLine("YRCJ8-NCRTD-4XKCN-HXZ2K");
                    }
                }
                else if (Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Total Security"))
                {
                    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    {
                        text.WriteLine("YQG37-CW4MK-HGJBZ-FG9CH");
                    }
                }
            }
        }
        #endregion


        #region SecondThread
        private async void SecondThread(string AVProot = null, string ksdeGUID = null, string annexed = null)
        {
            StartingLabel.Text = "Iniciando instalación";

            try
            {
                if (Properties.Settings.Default.AutoInstall.Equals(true))
                {
                    StartingLabel.Text = $"Instalando {Properties.Settings.Default.KavEditionToInstall}"; GIF.Visible = true;

                    await HiddenProcess(Path.GetTempPath() + "kavsetup.exe", $"/s /mybirthdate=1990‑01‑01 /pAGREETOEULA=1 /pAGREETOPRIVACYPOLICY=1 /pJOINKSN_ENHANCE_PROTECTION=1 /pJOINKSN_MARKETING=0 /pSELFPROTECTION=1 /pALLOWREBOOT=0");
                    
                    if (!KavInstalled())
                    {
                        StartingLabel.Text = $"Reiniciando equipo";
                        await Task.Delay(2000);

                        await HiddenProcess("shutdown.exe", "/R /T 00");
                    }
                }
                else
                {
                    Process kavsetup = new Process();
                    kavsetup.StartInfo.FileName = Path.GetTempPath() + "kavsetup.exe";
                    kavsetup.Start();

                    this.Location = new Point(this.Location.X, 1); GIF.Visible = true;
                    await Task.Run(() => kavsetup.WaitForExit()); GIF.Visible = false;

                    StartingLabel.Text = "Esperando instalación manual";
                    TopMost = false;
                }
            }
            catch (Win32Exception)
            {
                MessageBox.Show(this, $"No ha sido posible iniciar el asistente de instalación para {Properties.Settings.Default.KavEditionToInstall}." + Environment.NewLine + "Al cerrar este dialogo, serás dirigido a la web oficial para realizar la descarga e instalación manual.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                StartingLabel.Text = "Esperando instalación manual";
                
                TopMost = false;
                
                this.Location = new Point(this.Location.X, 1);
                
                Process.Start(Properties.Settings.Default.KavSetupURL);
            }

            while (AVProot == null || ksdeGUID == null)
            {
                await Task.Delay(1000);

                if (KavInstalled())
                {
                    foreach (string subkey in LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                    {
                        if (subkey.Contains("AVP")) AVProot = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{subkey}\environment").GetValue("ProductRoot").ToString();
                        if (subkey.Contains("KSDE")) ksdeGUID = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{subkey}\environment").GetValue("ProductCode").ToString();
                    }
                }
            }

            StartingLabel.Text = "Finalizando instalación"; GIF.Visible = true;
            TopMost = true;

            if (Properties.Settings.Default.KavSecureConnection.Equals(false))
            {
                StartingLabel.Text = "Desinstalando Kaspersky Secure Connection";

                foreach (Process process in Process.GetProcessesByName("ksde")) { process.Kill(); }
                foreach (Process process in Process.GetProcessesByName("ksdeui")) { process.Kill(); }

                await HiddenProcess("msiexec.exe", $"/x {ksdeGUID} /quiet"); GIF.Visible = false;
            }

            if (File.Exists(Path.GetTempPath() + "kavlicense.txt"))
            {
                StartingLabel.Text = "Activando licencia de evaluación";

                string LicenseKey = string.Empty;
                using (var reader = new StreamReader(Path.GetTempPath() + "kavlicense.txt"))
                {
                    LicenseKey = reader.ReadLine();
                }

                await HiddenProcess($@"{AVProot}\avp.com", $"license /add {LicenseKey}");
            }
            else annexed = Environment.NewLine + "Debes activar la versión de evaluación de la aplicación desde el apartado [Introducir código de activación].";

            HiddenProcess($@"{AVProot}\avp.com", "UPDATE");

            StartingLabel.Text = "Finalizando instalación";

            try
            {
                File.Delete(Path.GetTempPath() + "kavsetup.exe");
                File.Delete(Path.GetTempPath() + "kavlicense.txt");
            }
            catch (Exception) { }

            if (Properties.Settings.Default.AutoInstall.Equals(true)) Process.Start($@"{AVProot}\avpui.exe");

            Properties.Settings.Default.Reset();

            Working = false;

            this.Opacity = 0; this.ShowInTaskbar = false;

            MessageBox.Show(this, "¡Todo listo! Gracias por utilizar Kaspersky Custom Installer." + annexed, "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.Exit();
        }
        #endregion


        private async Task HiddenProcess(string processFile, string processArguments)
        {
            ProcessStartInfo hiddenProcess = new ProcessStartInfo();
            hiddenProcess.FileName = processFile;
            hiddenProcess.Arguments = processArguments;
            hiddenProcess.WindowStyle = ProcessWindowStyle.Hidden;
            hiddenProcess.CreateNoWindow = true;
            await Task.Run(() => Process.Start(hiddenProcess).WaitForExit());
        }


        private void Form_Closing(object sender, FormClosingEventArgs close)
        {
            try
            {
                if (close.CloseReason.Equals(CloseReason.WindowsShutDown))
                {
                    LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).SetValue("KciExtension", System.Reflection.Assembly.GetEntryAssembly().Location, RegistryValueKind.String);
                }
                else
                {
                    if (Working && close.CloseReason.Equals(CloseReason.UserClosing))
                        if (MessageBox.Show(this, "La instalación no ha finalizado. ¿Deseas cerrar la aplicación de todas formas?", "KCI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            close.Cancel = true;

                    Properties.Settings.Default.Reset();
                    foreach (Process process in Process.GetProcessesByName("msiexec")) { process.Kill(); }
                    foreach (Process process in Process.GetProcessesByName("startup")) { process.Kill(); }
                    LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).DeleteValue("KciExtension");
                }
            }
            catch (Exception) { }
        }
    }
}
