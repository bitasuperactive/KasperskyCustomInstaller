using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Principal;
using Squirrel;
using System.Drawing.Text;
using System.ComponentModel;

namespace KCIBasic
{
    public partial class KCI : Form //Try out shutdown /G & GIF is NOT visible
    {
        private RegistryKey LocalMachine32View { get; set; } = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        private RegistryKey LocalMachine64View { get; set; } = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        private string AVPkey { get; set; }
        private string AVProot { get; set; }
        private string KavEditionInstalled { get; set; }
        private string KavGUID { get; set; }
        private bool BackupConfig { get; set; }

    public KCI()
        {
            using (var manager = new UpdateManager(string.Empty))
            {
                SquirrelAwareApp.HandleEvents(
                    onInitialInstall: v => manager.RemoveShortcutForThisExe(),
                    onAppUpdate: v => manager.UpdateApp(),
                    onFirstRun: () => manager.RemoveUninstallerRegistryEntry(),
                    onAppUninstall: v => manager.FullUninstall());
            }

            InitializeComponent();
        }


        private async void KCI_Shown(object sender, EventArgs e)
        {
            await CheckRequirements();
            if (Properties.Settings.Default.MainThreadDone.Equals(false))
            {
                Properties.Settings.Default.Reset();
                StartPanel.Visible = true;
            }
            else SecondThread();
        }

        private async Task CheckRequirements()
        {
            await Task.Delay(1000);

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

            if (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null) //Check if installed key
            {
                foreach (string AVPkey in LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                {
                    if (AVPkey.Contains("AVP"))
                    {
                        this.AVPkey = AVPkey;
                        AVProot = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductRoot").ToString();
                        KavEditionInstalled = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductName").ToString();
                        KavGUID = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductCode").ToString();
                        break;
                    }
                }
            }
            else BackupCheckBox.Visible = false;

            //using (var manager = new UpdateManager(string.Empty))
            //{
            //    await manager.UpdateApp();
            //}
        }

        //private bool GetKavPswrdProtect()
        //{
        //    if (kavEditionInstalled != null)
        //    {
        //        if (LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\Data\FlushedMsiCriticalSettings") != null)
        //        {
        //            if (LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\Data\FlushedMsiCriticalSettings").GetValue("EnablePswrdProtect").ToString() == "1") return true;
        //        }
        //    }
        //    return false;
        //}

        #region Buttons
        private void StartButton_Click(object sender, EventArgs e)
        {
            StartPanel.Visible = false;
            Properties.Settings.Default.AutoInstall = false;
            MainThread();
        }

        private void StartAutoButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\Data\FlushedMsiCriticalSettings").GetValue("EnablePswrdProtect").ToString() == "1")
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

            StartPanel.Visible = false;
            Properties.Settings.Default.AutoInstall = true;
            MainThread();
        }

        private void KAVCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (KAVCheckBox.Checked)
            {
                KISCheckBox.Checked = false; KISCheckBox.Font = new Font(KISCheckBox.Font, FontStyle.Regular);
                KTSCheckBox.Checked = false; KTSCheckBox.Font = new Font(KTSCheckBox.Font, FontStyle.Regular);
                KAVCheckBox.Font = new Font(KAVCheckBox.Font, FontStyle.Bold);
            }
            CheckBoxMethod();
        }

        private void KISCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (KISCheckBox.Checked)
            {
                KAVCheckBox.Checked = false; KAVCheckBox.Font = new Font(KAVCheckBox.Font, FontStyle.Regular);
                KTSCheckBox.Checked = false; KTSCheckBox.Font = new Font(KTSCheckBox.Font, FontStyle.Regular);
                KISCheckBox.Font = new Font(KISCheckBox.Font, FontStyle.Bold);
            }
            CheckBoxMethod();
        }

        private void KTSCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (KTSCheckBox.Checked)
            {
                KAVCheckBox.Checked = false; KAVCheckBox.Font = new Font(KAVCheckBox.Font, FontStyle.Regular);
                KISCheckBox.Checked = false; KISCheckBox.Font = new Font(KISCheckBox.Font, FontStyle.Regular);
                KTSCheckBox.Font = new Font(KTSCheckBox.Font, FontStyle.Bold);
            }
            CheckBoxMethod();
        }

        private void BackupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (BackupCheckBox.Checked) { BackupConfig = true; BackupCheckBox.Font = new Font(BackupCheckBox.Font, FontStyle.Bold); }
            else
            {
                BackupConfig = false;
                BackupCheckBox.Font = new Font(BackupCheckBox.Font, FontStyle.Regular);
            }
        }

        private void CheckBoxMethod()
        {
            if (KAVCheckBox.Checked || KISCheckBox.Checked || KTSCheckBox.Checked)
            {
                StartButton.Enabled = true;
            }
            else
            {
                KAVCheckBox.Font = new Font(KAVCheckBox.Font, FontStyle.Regular);
                KISCheckBox.Font = new Font(KISCheckBox.Font, FontStyle.Regular);
                KTSCheckBox.Font = new Font(KTSCheckBox.Font, FontStyle.Regular);
                StartButton.Enabled = false;
            }
        }
        #endregion

        private async void MainThread()
        {
            if (KAVCheckBox.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Antivirus";

                if (Properties.Settings.Default.AutoInstall)
                    Properties.Settings.Default.KavSetupURL = "https://arc-products.s.kaspersky-labs.com/homeuser/kav2019/19.0.0.1088abcdefg/spanish-ES-0.7524.0/3233313438337c44454c7c35/KAV19.0.0.1088_es-ES_full.exe";
                else
                    Properties.Settings.Default.KavSetupURL = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";

                Properties.Settings.Default.kavLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
            }
            else if (KISCheckBox.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Internet Security";

                if (Properties.Settings.Default.AutoInstall)
                    Properties.Settings.Default.KavSetupURL = "https://box.kaspersky.com/seafhttp/files/d4c03521-cb81-49dc-980d-5a8d036c3010/kis20.0.14.1085_es-es_full.exe";
                else
                    Properties.Settings.Default.KavSetupURL = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";

                Properties.Settings.Default.kavLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
            }
            else if (KTSCheckBox.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Total Security";

                if (Properties.Settings.Default.AutoInstall) 
                    Properties.Settings.Default.KavSetupURL = "https://arc-products.s.kaspersky-labs.com/homeuser/kts2019/19.0.0.1088abcdefg/spanish-ES-0.7524.0/3233323636367c44454c7c35/KTS19.0.0.1088_es-ES_full.exe";
                else
                    Properties.Settings.Default.KavSetupURL = "https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe";

                Properties.Settings.Default.kavLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
            }

            try
            {
                if (BackupConfig) await BackupKavConfiguration();
                await Uninstall();
                await Registry();
                await DownloadSetup();
                await License();
                Properties.Settings.Default.MainThreadDone = true;
                Properties.Settings.Default.Save();
            }
            catch (Exception) { /*MessageBox.Show(this, $"Error: {exception.Message}", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);*/ Application.Exit(); }

            if (KavEditionInstalled != null && Properties.Settings.Default.AutoInstall)
            {
                OutputLabel.Text = $"Reiniciando equipo";
                await Task.Delay(2000);
                Process.Start("ShutDown", "/R /T 00");
            }

            else if (KavEditionInstalled != null)
            {
                OutputLabel.Text = "Esperando reinicio del equipo";
                MessageBox.Show(this, "Debes reiniciar el equipo para continuar la instalación.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.TopMost = false;
            }

            else SecondThread();
        }

        private async Task BackupKavConfiguration()
        {
            OutputLabel.Text = $"Exportando configuración de Kaspersky";
            await Task.Delay(2000);

            ProcessStartInfo backupConfig = new ProcessStartInfo();
            backupConfig.FileName = $@"{AVProot}\avp.com";
            backupConfig.Arguments = $"EXPORT \"{Directory.GetCurrentDirectory()}\\{KavEditionInstalled} Configuration.cfg\"";
            backupConfig.WindowStyle = ProcessWindowStyle.Hidden;
            backupConfig.CreateNoWindow = true;
            await Task.Run(() => Process.Start(backupConfig).WaitForExit());
        }

        private async Task Uninstall()
        {
            if (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
            {
                if (!Properties.Settings.Default.AutoInstall.Equals(true))
                {
                    OutputLabel.Text = $"Iniciando desinstalación manual de {KavEditionInstalled}";

                    await Task.Delay(2000);

                    this.Location = new Point(this.Location.X, 1);

                    OutputLabel.Text = $"Esperando desinstalación manual";

                    await Task.Run(() => Process.Start("msiexec.exe", $"/i {KavGUID} /norestart REMOVE=ALL").WaitForExit());

                    this.CenterToScreen();
                }
                else
                {
                    OutputLabel.Text = $"Desinstalando {KavEditionInstalled}"; GIF.Visible = true;

                    await Task.Run(() => Process.Start("msiexec.exe", $"/x {KavGUID} /quiet").WaitForExit()); GIF.Visible = false;

                    //else
                    //{
                    //    MessageBox.Show(this, $"No es posible realizar la desinstalación automática de {kavEditionInstalled}. Debes gestionarlo manualmente utilizando el asistente de desinstalación que se abrirá a continuación.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    OutputLabel.Text = $"Esperando desinstalación manual de {kavEditionInstalled}";
                    //    this.TopMost = false;
                    //    await Task.Run(() => Process.Start("msiexec.exe", $"/i {kavGUID} /norestart REMOVE=ALL").WaitForExit());
                    //    this.TopMost = true;
                    //}
                }

                //while (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null) await Task.Delay(1000);

                //Check if uninstallation was successfull 
                if (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
                {
                    MessageBox.Show(this, $"La desinstalación de {KavEditionInstalled} no ha sido realizada correctamente. Si el error persiste, por favor reinicia tu equipo para finalizar el proceso.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return?
                    Application.Exit();
                }
            }
        }

        private async Task Registry()
        {
            OutputLabel.Text = $"Editando registro de Windows";
            await Task.Delay(2000);

            try
            {
                LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates");
                //LocalMachine32View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");
                //LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(this, "No ha sido posible acceder al registro de Windows." + Environment.NewLine + $"Reinicia el equipo para finalizar la desinstalación de Kaspersky, y cierra cualquier otro anti-malware abierto antes de volver a ejecutar Kaspersky Custom Installer.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return?
                Application.Exit();
            }
            catch (ArgumentException) { }

            //try { LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Cryptography\RNG"); } catch (Exception) { /*No need to catch*/ }
        }

        private async Task DownloadSetup()
        {
            OutputLabel.Text = $"Descargando {Properties.Settings.Default.KavEditionToInstall}";
            await Task.Delay(1000);

            Retry:
            try
            {
                using (WebClient client = new WebClient())
                {
                    string outputText = OutputLabel.Text;
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs n)
                    {
                        OutputLabel.Text = $"{outputText} ({n.ProgressPercentage}%)";
                    };
                    await client.DownloadFileTaskAsync(Properties.Settings.Default.KavSetupURL, Path.GetTempPath() + "kavsetup.exe");
                    await Task.Delay(1000);
                    OutputLabel.Text = outputText;
                }
            }
            catch (WebException)
            {
                if (MessageBox.Show(this, $"No ha sido posible descargar el instalador para {Properties.Settings.Default.KavEditionToInstall}. Comprueba tu conexión a internet.", "KCI", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry) goto Retry;
            }
        }

        private async Task License()
        {
            OutputLabel.Text = $"Generando licencias";
            await Task.Delay(2000);

            try
            {
                using (WebClient client = new WebClient()) { client.DownloadFile(Properties.Settings.Default.kavLicenseUrl, Path.GetTempPath() + "kavlicense.txt"); }
            }
            catch (WebException)
            {
                if (Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Antivirus"))
                {
                    //using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    //{
                    //    text.Write("null");
                    //}
                }
                else if (Properties.Settings.Default.KavEditionToInstall .Equals("Kaspersky Internet Security"))
                {
                    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    {
                        text.Write("4CH4C-PPFDT-NFK4B-45R69");
                    }
                }
                else if (Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Total Security"))
                {
                    //using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    //{
                    //    text.Write("null");
                    //}
                }
            }
            catch (Exception) { }
        }

        private async void SecondThread(string AVProot = null, string ksdeGUID = null, string annexed = null)
        {
            OutputLabel.Text = "Iniciando instalación";

            if (Properties.Settings.Default.AutoInstall.Equals(false))
            {
                try
                {
                    Process kavsetup = new Process();
                    kavsetup.StartInfo.FileName = Path.GetTempPath() + "kavsetup.exe";
                    kavsetup.Start();

                    this.Location = new Point(this.Location.X, 1); GIF.Visible = true;
                    await Task.Run(() => kavsetup.WaitForExit()); GIF.Visible = false;
                }
                catch (Win32Exception)
                {
                    MessageBox.Show(this, $"No ha sido posible iniciar el instalador de Kaspersky Lab." + Environment.NewLine + "Al cerrar este dialogo, serás dirigido a la web oficial para realizar la descarga manualmente.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Location = new Point(this.Location.X, 1); this.TopMost = false;
                    Process.Start(Properties.Settings.Default.KavSetupURL);
                }

                OutputLabel.Text = "Esperando instalación manual"; this.Refresh();
                this.TopMost = false;
            }
            else
            {
                OutputLabel.Text = $"Instalando {Properties.Settings.Default.KavEditionToInstall}"; GIF.Visible = true;
                await Task.Run(() => Process.Start(Path.GetTempPath() + "kavsetup.exe", $"/s /mybirthdate=1990‑01‑01 /pAGREETOEULA=1 /pAGREETOPRIVACYPOLICY=1 /pJOINKSN_ENHANCE_PROTECTION=0 /pJOINKSN_MARKETING=0 /pSELFPROTECTION=1 /pALLOWREBOOT=0").WaitForExit());
            }


            while (AVProot == null || ksdeGUID == null)
            {
                try
                {
                    foreach (string subkey in LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                    {
                        if (subkey.Contains("AVP")) AVProot = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{subkey}\environment").GetValue("ProductRoot").ToString();
                        if (subkey.Contains("KSDE")) ksdeGUID = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{subkey}\environment").GetValue("ProductCode").ToString();
                    }
                }
                catch (NullReferenceException) { }
                await Task.Delay(1000);
            }

            this.TopMost = true;

            OutputLabel.Text = "Finalizando instalación"; GIF.Visible = true;

            if (File.Exists(Path.GetTempPath() + "kavlicense.txt"))
            {
                string LicenseKey = string.Empty;
                using (var reader = new StreamReader(Path.GetTempPath() + "kavlicense.txt"))
                {
                    LicenseKey = reader.ReadLine();
                }
                ProcessStartInfo activation = new ProcessStartInfo();
                activation.FileName = $@"{AVProot}\avp.com";
                activation.Arguments = $"license /add {LicenseKey}";
                activation.WindowStyle = ProcessWindowStyle.Hidden;
                activation.CreateNoWindow = true;
                await Task.Run(() => Process.Start(activation).WaitForExit());
            }
            else annexed = Environment.NewLine + "Debes activar la versión de evaluación de la aplicación desde el apartado [Introducir código de activación].";

            //ProcessStartInfo update = new ProcessStartInfo();
            //update.FileName = $@"{AVProot}\avp.com";
            //update.Arguments = "UPDATE";
            //update.WindowStyle = ProcessWindowStyle.Hidden;
            //update.CreateNoWindow = true;
            //await Task.Run(() => Process.Start(update).WaitForExit());

            foreach (Process process in Process.GetProcessesByName("ksde")) { process.Kill(); }
            foreach (Process process in Process.GetProcessesByName("ksdeui")) { process.Kill(); }

            await Task.Run(() => Process.Start("msiexec.exe", $"/x {ksdeGUID} /quiet").WaitForExit()); GIF.Visible = false;

            Properties.Settings.Default.MainThreadDone = false; Properties.Settings.Default.Save();

            try
            {
                Process.Start($@"{AVProot}\avpui.exe");
                File.Delete(Path.GetTempPath() + "kavsetup.exe");
                File.Delete(Path.GetTempPath() + "kavlicense.txt");
            }
            catch (Exception) { }

            //if (Properties.Settings.Default.AutoInstall) Process.Start("shutdown.exe", "/R /T 03");

            this.Opacity = 0; this.ShowInTaskbar = false;
            MessageBox.Show(this, "¡Todo listo! Gracias por utilizar Kaspersky Custom Installer." + annexed, "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        private void Form_Closing(object sender, FormClosingEventArgs close)
        {
            try
            {
                if (close.CloseReason.Equals(CloseReason.WindowsShutDown))
                {
                    LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).SetValue("KciExtension", System.Reflection.Assembly.GetEntryAssembly().Location, RegistryValueKind.String);
                    return;
                }
                else if (close.CloseReason.Equals(CloseReason.UserClosing))
                {
                    if (MessageBox.Show(this, "La instalación no ha finalizado. ¿Deseas cerrar la aplicación de todas formas?", "KCI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) close.Cancel = true;
                }
                //Process.Start($@"{Directory.GetCurrentDirectory()}\Update.exe", "--uninstall -s");
                Properties.Settings.Default.MainThreadDone = false; Properties.Settings.Default.Save();
                foreach (Process process in Process.GetProcessesByName("msiexec")) { process.Kill(); }
                foreach (Process process in Process.GetProcessesByName("startup")) { process.Kill(); }
                LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).DeleteValue("KciExtension");
            }
            catch (Exception) { }
        }
    }
}
