using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KCIBasic
{
    public partial class OutputForm : Form
    {
        public OutputForm()
        {
            InitializeComponent();
        }

        private void OutputForm_VisibleChanged(object sender, EventArgs e)
        {
            if (((MainForm)Owner).outputForm.Visible)
                if (Properties.Settings.Default.MainThreadDone.Equals(false)) MainThread(); else SecondThread();
        }


        #region MainThread
        private async void MainThread()
        {
            if (((MainForm)Owner).KAVRadioButton.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Anti-Virus";

                if (((MainForm)Owner).OfflineSetupCheckBox.Checked)
                    Properties.Settings.Default.KavSetupURL = "https://box.kaspersky.com/f/0203108bc5eb4806a22d/?dl=1";
                else
                    Properties.Settings.Default.KavSetupURL = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";

                Properties.Settings.Default.KavLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
            }

            else if (((MainForm)Owner).KISRadioButton.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Internet Security";

                if (((MainForm)Owner).OfflineSetupCheckBox.Checked)
                    Properties.Settings.Default.KavSetupURL = "https://box.kaspersky.com/f/a16a247db28a48039342/?dl=1";
                else
                    Properties.Settings.Default.KavSetupURL = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";

                Properties.Settings.Default.KavLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
            }

            else if (((MainForm)Owner).KTSRadioButton.Checked)
            {
                Properties.Settings.Default.KavEditionToInstall = "Kaspersky Total Security";

                if (((MainForm)Owner).OfflineSetupCheckBox.Checked)
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
                await License(OutputLabel);
                Properties.Settings.Default.MainThreadDone = true;
                Properties.Settings.Default.Save();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Error: {exception.Message}", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Hide();

                this.CenterToScreen();

                return;
            }

            if (MainForm.KavGuid != null && Properties.Settings.Default.AutoInstall)
            {
                OutputLabel.Text = $"Reiniciando equipo";
                await Task.Delay(2000);

                await HiddenProcess("shutdown.exe", "/R /T 00");
            }

            else if (MainForm.KavGuid != null)
            {
                OutputLabel.Text = "Esperando reinicio del equipo";

                MessageBox.Show(this, "Debes reiniciar el equipo para continuar la instalación.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                TopMost = false;
            }

            else SecondThread();
        }

        private async Task Uninstall()
        {
            if (((MainForm)Owner).KavInstalled()) // MainForm.KavGuid != null ?
            {
                if (Properties.Settings.Default.AutoInstall.Equals(false))
                {
                    OutputLabel.Text = $"Iniciando asistente de desinstalación";
                    await Task.Delay(2000);

                    this.Location = new Point(this.Location.X, 1);
                    OutputLabel.Text = "Esperando desinstalación manual de" + Environment.NewLine + MainForm.KavEditionInstalled;

                    await HiddenProcess("msiexec.exe", $"/i {MainForm.KavGuid} /norestart REMOVE=ALL");
                }
                else
                {
                    OutputLabel.Text = $"Desinstalando {MainForm.KavEditionInstalled}"; GIF.Visible = true;

                    await HiddenProcess("msiexec.exe", $"/x {MainForm.KavGuid} /quiet"); GIF.Visible = false;
                }

                if (((MainForm)Owner).KavInstalled())
                {
                    throw new UnauthorizedAccessException($"La desinstalación de {MainForm.KavEditionInstalled} no ha sido realizada correctamente. Inténtalo de nuevo." + Environment.NewLine + "Si el error persiste, reinicia tu equipo para completar los cambios realizados en el sistema.");
                }

                this.CenterToScreen();
            }
        }

        private async Task Registry()
        {
            OutputLabel.Text = $"Configurando registro de Windows";
            await Task.Delay(2000);

            try
            {
                if (MainForm.LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
                    MainForm.LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");

                if (MainForm.LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
                    MainForm.LocalMachine32View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");

                if (MainForm.LocalMachine64View.OpenSubKey(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates") != null)
                    MainForm.LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates");

                if (MainForm.LocalMachine64View.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography\RNG") != null)
                    MainForm.LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Cryptography\RNG");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("No ha sido posible acceder al registro de Windows." + Environment.NewLine + $"Reinicia el equipo para finalizar la desinstalación de {MainForm.KavEditionInstalled}, y cierra cualquier otro anti - malware abierto antes de volver a ejecutar Kaspersky Custom Installer.");
            }
        }

        public async Task DownloadSetup()
        {
            OutputLabel.Text = $"Descargando asistente de instalación (0%)";
            await Task.Delay(1000);

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs n)
                    {
                        OutputLabel.Text = $"Descargando asistente de instalación ({n.ProgressPercentage}%)";
                    };
                    await client.DownloadFileTaskAsync(Properties.Settings.Default.KavSetupURL, Path.GetTempPath() + "kavsetup.exe");
                    await Task.Delay(1000);
                }
            }
            catch (WebException)
            {
                if (MessageBox.Show(this, $"No ha sido posible descargar el asistente de instalación. Comprueba tu conexión a internet e inténtalo de nuevo.", "KCI", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry)
                {
                    await DownloadSetup();
                }
            }
        }

        public async Task License(Label labelObject)
        {
            labelObject.Text = $"Descargando licencias de evaluación (0%)";
            await Task.Delay(1000);

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs n)
                    {
                        labelObject.Text = $"Descargando licencias de evaluación ({n.ProgressPercentage}%)";
                    };
                    await client.DownloadFileTaskAsync(Properties.Settings.Default.KavLicenseUrl, Path.GetTempPath() + "kavlicense.txt");
                    await Task.Delay(1000);
                }
            }
            catch (WebException)
            {
                //labelObject.Text = $"Generando licencias de evaluación";
                //await Task.Delay(2000);

                if (MainForm.KavEditionInstalled.Equals("Kaspersky Anti-Virus") || Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Anti-Virus"))
                {
                    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    {
                        text.WriteLine("YZWEN-98MCX-Z9FV1-9TUSW");
                    }
                }

                else if (MainForm.KavEditionInstalled.Equals("Kaspersky Internet Security") || Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Internet Security"))
                {
                    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    {
                        text.WriteLine("YRCJ8-NCRTD-4XKCN-HXZ2K");
                    }
                }

                else if (MainForm.KavEditionInstalled.Equals("Kaspersky Total Security") || Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Total Security"))
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
        private async void SecondThread(string AVProot = null, string ksdeGUID = null)
        {
            try
            {
                if (Properties.Settings.Default.AutoInstall.Equals(true))
                {
                    OutputLabel.Text = $"Instalando {Properties.Settings.Default.KavEditionToInstall}"; GIF.Visible = true;

                    await HiddenProcess(Path.GetTempPath() + "kavsetup.exe",
                        $"/s /mybirthdate=1990‑01‑01 /pAGREETOEULA=1 /pAGREETOPRIVACYPOLICY=1 /pJOINKSN_ENHANCE_PROTECTION=1 /pJOINKSN_MARKETING=0 /pSELFPROTECTION=1 /pALLOWREBOOT=0");
                    GIF.Visible = false;
                }
                else
                {
                    this.Location = new Point(this.Location.X, 1);

                    OutputLabel.Text = "Iniciando asistente de instalación";
                    await Task.Delay(2000);

                    await Task.Run(() => Process.Start(Path.GetTempPath() + "kavsetup.exe"));

                    OutputLabel.Text = $"Esperando instalación manual de" + Environment.NewLine + $"{Properties.Settings.Default.KavEditionToInstall}";

                    while (Process.GetProcessesByName("kavsetup").Length > 0 || Process.GetProcessesByName("startup").Length > 0) await Task.Delay(500);
                }
            }
            catch (Win32Exception)
            {
                MessageBox.Show(this, $"No ha sido posible iniciar el asistente de instalación." + Environment.NewLine + "Al cerrar este dialogo, serás dirigido a la web oficial de Kaspersky Lab para realizar la descarga e instalación manual.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                OutputLabel.Text = "Esperando instalación manual";

                this.Location = new Point(this.Location.X, 1);

                Process.Start(Properties.Settings.Default.KavSetupURL);
            }


            if (((MainForm)Owner).KavInstalled())
            {
                while (AVProot == null)
                    foreach (string subkey in MainForm.LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                    {
                        if (subkey.Contains("AVP")) AVProot = MainForm.LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{subkey}\environment").GetValue("ProductRoot").ToString();
                        if (subkey.Contains("KSDE")) ksdeGUID = MainForm.LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{subkey}\environment").GetValue("ProductCode").ToString();
                    }
            }
            else if (Properties.Settings.Default.AutoInstall.Equals(true))
            {
                OutputLabel.Text = $"Reiniciando equipo";
                await Task.Delay(2000);
                await HiddenProcess("shutdown.exe", "/R /T 00"); // await?
                return;
            }
            else if (Properties.Settings.Default.AutoInstall.Equals(false))
            {
                DialogResult messageBox = MessageBox.Show(this, $"El asistente de instalación no ha finalizado correctamente. ¿Deseas reintentar la instalación?", "KCI", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                if (messageBox == DialogResult.Retry)
                {
                    SecondThread();
                    return;
                }
                else
                {
                    Properties.Settings.Default.Reset();
                    Application.Exit();
                }
            }


            GIF.Visible = true; TopMost = true;

            if (Properties.Settings.Default.KavSecureConnection.Equals(false))
            {
                OutputLabel.Text = "Desinstalando Kaspersky Secure Connection"; GIF.Enabled = true;
                await Task.Delay(2000);

                foreach (Process process in Process.GetProcessesByName("ksde")) { process.Kill(); }
                foreach (Process process in Process.GetProcessesByName("ksdeui")) { process.Kill(); }

                await HiddenProcess("msiexec.exe", $"/x {ksdeGUID} /quiet"); GIF.Enabled = false;

                if (ksdeGUID == null) // Check if KSDE was uninstalled successfully
                    MessageBox.Show(this, $"No ha sido posible realizar la desinstalación de Kaspersky Secure Connection. Deberás gestionarlo manualmente.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            OutputLabel.Text = "Actualizando producto"; GIF.Enabled = true;
            //await Task.Delay(2000);
            await HiddenProcess($@"{AVProot}\avp.com", "UPDATE"); GIF.Enabled = false; ; // await? Needed to update "IsReportedExpired" value.


            OutputLabel.Text = "Activando licencia de evaluación del producto"; GIF.Enabled = true;
            await Task.Delay(2000);

            string LicenseKey1;
            using (var reader = new StreamReader(Path.GetTempPath() + "kavlicense.txt"))
            {
                LicenseKey1 = reader.ReadLine();
            }
            await HiddenProcess("cmd.exe", $"/C echo & \"{AVProot}\\avp.com\" LICENSE /ADD {LicenseKey1}"); GIF.Enabled = false;

            if (MainForm.LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab\WmiHlp").GetValueNames().Contains("IsReportedExpired"))
            {
                MessageBox.Show(this, $"No ha sido posible realizar la activación de {Properties.Settings.Default.KavEditionToInstall}. Intenta [Activar la versión de evaluación de la aplicación] desde el apartado [Introducir código de activación].", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // annexed = Environment.NewLine + "Debes activar la versión de evaluación de la aplicación desde el apartado [Introducir código de activación].";
            }


            OutputLabel.Text = "Finalizando instalación"; GIF.Enabled = true;

            try
            {
                File.Delete(Path.GetTempPath() + "kavsetup.exe");
                File.Delete(Path.GetTempPath() + "kavlicense.txt");
                Directory.Delete($@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\Kaspersky Lab Setup Files", true);
            }
            catch { }

            this.Opacity = 0; //this.ShowInTaskbar = false;

            Process.Start($@"{AVProot}\avpui.exe");

            OutputLabel.Text = "¡Todo listo!"; GIF.Visible = false;

            MessageBox.Show(this, "¡Todo listo! Gracias por utilizar Kaspersky Custom Installer.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Properties.Settings.Default.Reset();

            Application.Exit();
        }
        #endregion


        public async Task HiddenProcess(string processFile, string processArguments)
        {
            ProcessStartInfo hiddenProcess = new ProcessStartInfo();
            hiddenProcess.FileName = processFile;
            hiddenProcess.Arguments = processArguments;
            hiddenProcess.WindowStyle = ProcessWindowStyle.Hidden;
            hiddenProcess.CreateNoWindow = true;
            await Task.Run(() => Process.Start(hiddenProcess).WaitForExit());
        }

        private void OutputForm_FormClosing(object sender, FormClosingEventArgs close)
        {
            if (close.CloseReason.Equals(CloseReason.WindowsShutDown)) // Do not restart MainForm...
            {
                MainForm.LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).SetValue("KciExtension", System.Reflection.Assembly.GetEntryAssembly().Location, RegistryValueKind.String);
            }
            else
            {
                if (close.CloseReason.Equals(CloseReason.UserClosing))
                {
                    if (MessageBox.Show(this, "La instalación no ha finalizado." + Environment.NewLine + "¿Deseas cerrar la aplicación de todas formas?", "KCI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        close.Cancel = true;
                    else
                        Properties.Settings.Default.Reset(); // Meh...
                }

                try
                {
                    foreach (Process process in Process.GetProcessesByName("msiexec")) { process.Kill(); }
                    foreach (Process process in Process.GetProcessesByName("startup")) { process.Kill(); }
                }
                catch { }
            }
        }
    }
}
