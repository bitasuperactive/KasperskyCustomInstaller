using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

            if (MainForm.KavGuid != null && Properties.Settings.Default.AutoInstall.Equals(true))
            {
                OutputLabel.Text = $"Iniciando reinicio del equipo en 5 segundos";
                await Task.Delay(5000);

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
            if (((MainForm)Owner).KavInstalled())
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
                else
                {
                    throw new WebException($"No es posible continuar la instalación de {Properties.Settings.Default.KavEditionToInstall} sin haber descargado el asistente de instalación. Vuelve a intentarlo.");
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
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs n)
                    {
                        labelObject.Text = $"Descargando licencias de evaluación ({n.ProgressPercentage}%)";
                    };
                    await client.DownloadFileTaskAsync(Properties.Settings.Default.KavLicenseURL, Path.GetTempPath() + "kavlicenses.txt");
                    await Task.Delay(1000);
                }

                using (var reader = new StreamReader(Path.GetTempPath() + "kavlicenses.txt")) // Meh...
                {
                    if (Properties.Settings.Default.KavEditionToInstall == "Kaspersky Anti-Virus")
                    {
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        Properties.Settings.Default.KavLicense = reader.ReadLine();
                    }
                    else if (Properties.Settings.Default.KavEditionToInstall == "Kaspersky Internet Security")
                    {
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        Properties.Settings.Default.KavLicense = reader.ReadLine();
                    }
                    else if (Properties.Settings.Default.KavEditionToInstall == "Kaspersky Total Security")
                    {
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        reader.ReadLine();
                        Properties.Settings.Default.KavLicense = reader.ReadLine();
                    }
                }
            }
            catch (WebException)
            {
                //MessageBox.Show(this, "No ha sido posible descargar ninguna licencia de evaluación para esta edición de Kaspersky Lab. Puedes activar la versión de evaluación del producto manualmente.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                //if (MainForm.KavEditionInstalled.Equals("Kaspersky Anti-Virus") || Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Anti-Virus"))
                //{
                //    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                //    {
                //        text.WriteLine("YZWEN-98MCX-Z9FV1-9TUSW");
                //    }
                //}
                //else if (MainForm.KavEditionInstalled.Equals("Kaspersky Internet Security") || Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Internet Security"))
                //{
                //    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                //    {
                //        text.WriteLine("YRCJ8-NCRTD-4XKCN-HXZ2K");
                //    }
                //}
                //else if (MainForm.KavEditionInstalled.Equals("Kaspersky Total Security") || Properties.Settings.Default.KavEditionToInstall.Equals("Kaspersky Total Security"))
                //{
                //    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                //    {
                //        text.WriteLine("YQG37-CW4MK-HGJBZ-FG9CH");
                //    }
                //}
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
                        $"/s /mybirthdate=1986‑12‑23 /pAGREETOEULA=1 /pAGREETOPRIVACYPOLICY=1 /pJOINKSN_ENHANCE_PROTECTION=0 /pJOINKSN_MARKETING=0 /pALLOWREBOOT=0");
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
            catch (Win32Exception) // ???
            {
                if (MessageBox.Show(this, $"No ha sido posible iniciar el asistente de instalación. ¿Quieres volver a intentarlo?", "KCI", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry)
                {
                    SecondThread();
                    return;
                }
                else
                {
                    MessageBox.Show(this, $"No es posible continuar la instalación sin el asistente de instalación para {Properties.Settings.Default.KavEditionToInstall}. Vuelve a intentarlo.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    Properties.Settings.Default.Reset();

                    Hide();

                    this.CenterToScreen();

                    return;
                }
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
                OutputLabel.Text = $"Iniciando reinicio del equipo en 5 segundos";
                await Task.Delay(5000);
                await HiddenProcess("shutdown.exe", "/R /T 00");
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
                    Application.Exit(); // Throw user closing exit ?
                }
            }


            GIF.Visible = true; TopMost = true;


            if (Properties.Settings.Default.AutoInstall.Equals(true)) // ???
            {
                OutputLabel.Text = "Actualizando bases de datos del producto"; GIF.Enabled = true;
                //await Task.Delay(2000);
                await HiddenProcess($@"{AVProot}\avp.com", "UPDATE"); GIF.Enabled = false;
            }
            else
            {
                HiddenProcess($@"{AVProot}\avp.com", "UPDATE"); GIF.Enabled = false;
            }


            OutputLabel.Text = "Activando licencia de evaluación del producto"; GIF.Enabled = true;
            await Task.Delay(2000);

            await HiddenProcess("cmd.exe", $"/C echo & \"{AVProot}\\avp.com\" LICENSE /ADD {Properties.Settings.Default.KavLicense}"); GIF.Enabled = false;

            if (MainForm.LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab\WmiHlp").GetValueNames().Contains("IsReportedExpired"))
            {
                MessageBox.Show(this, $"No ha sido posible realizar la activación de {Properties.Settings.Default.KavEditionToInstall}. Intenta [Activar la versión de evaluación de la aplicación] desde el apartado [Introducir código de activación].", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            if (Properties.Settings.Default.KavSecureConnection.Equals(false)) // Sometimes KSDE Registry Key does not exist
            {
                OutputLabel.Text = "Desinstalando Kaspersky Secure Connection"; GIF.Enabled = true;
                await Task.Delay(2000);

                if (ksdeGUID != null)
                {
                    foreach (Process process in Process.GetProcessesByName("ksde")) { process.Kill(); }
                    foreach (Process process in Process.GetProcessesByName("ksdeui")) { process.Kill(); }

                    await HiddenProcess("msiexec.exe", $"/x {ksdeGUID} /quiet"); GIF.Enabled = false;
                }
                else
                {
                    GIF.Enabled = false;
                    MessageBox.Show(this, $"No ha sido posible realizar la desinstalación de Kaspersky Secure Connection. Deberás gestionarlo manualmente.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                    
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

        private void OutputForm_FormClosing(object sender, FormClosingEventArgs close) // Pause thread ?
        {
            if (Properties.Settings.Default.MainThreadDone.Equals(true) && close.CloseReason.Equals(CloseReason.WindowsShutDown))
            {
                MainForm.LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).SetValue("KciExtension", System.Reflection.Assembly.GetEntryAssembly().Location, RegistryValueKind.String);
            }
            else if (close.CloseReason.Equals(CloseReason.UserClosing))
            {
                if (MessageBox.Show(this, "La instalación no ha finalizado." + Environment.NewLine + "¿Deseas cerrar la aplicación de todas formas?", "KCI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Properties.Settings.Default.Reset();

                    try
                    {
                        foreach (Process process in Process.GetProcessesByName("msiexec")) { process.Kill(); }
                        foreach (Process process in Process.GetProcessesByName("startup")) { process.Kill(); }

                        if (File.Exists(Path.GetTempPath() + "kavsetup.exe"))
                            File.Delete(Path.GetTempPath() + "kavsetup.exe");
                        if (File.Exists(Path.GetTempPath() + "kavlicense.txt"))
                            File.Delete(Path.GetTempPath() + "kavlicense.txt");
                        if (Directory.Exists($@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\Kaspersky Lab Setup Files"))
                            Directory.Delete($@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\Kaspersky Lab Setup Files", true);
                    }
                    catch (Exception) { }
                }
                else
                {
                    close.Cancel = true;
                }
            }
        }
    }
}
