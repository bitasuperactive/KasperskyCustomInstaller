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

namespace KCIBasic
{
    public partial class KCI : Form
    {
        private RegistryKey LocalMachine32View { get; set; } = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        private RegistryKey LocalMachine64View { get; set; } = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        public KCI()
        {
            if (Process.GetProcesses().Count(process => process.ProcessName == Process.GetCurrentProcess().ProcessName) > 1)
            {
                MessageBox.Show("Kaspersky Custom Installer no responde.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }
            InitializeComponent();
        }

        private void KCI_Shown(object sender, EventArgs e)
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Permisos de Administrador requeridos.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }
            if (Properties.Settings.Default.MainThreadDone.Equals(false)) StartPanel.Visible = true; else SecondThread();
        }

        #region Buttons
        private void StartButton_Click(object sender, EventArgs e)
        {
            StartPanel.Visible = false;
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

        private async void MainThread(string kavEditionToInstall = null, string kavEdition = null, string kavGUID = null, string kavDownloadUrl = null, string kavLicenseUrl = null)
        {
            if (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
            {
                foreach (string AVPkey in LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                {
                    if (AVPkey.Contains("AVP"))
                    {
                        kavEdition = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductName").ToString();
                        kavGUID = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductCode").ToString();
                        break;
                    }
                }
            }

            if (KAVCheckBox.Checked)
            {
                kavEditionToInstall = "Kaspersky Antivirus";
                kavDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";
                kavLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
                Properties.Settings.Default.KavWebsite = "https://www.kaspersky.com/downloads/thank-you/antivirus"; Properties.Settings.Default.Save();
            }
            else if (KISCheckBox.Checked)
            {
                kavEditionToInstall = "Kaspersky Internet Security";
                kavDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";
                kavLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
                Properties.Settings.Default.KavWebsite = "https://www.kaspersky.com/downloads/thank-you/internet-security"; Properties.Settings.Default.Save();
            }
            else if (KTSCheckBox.Checked)
            {
                kavEditionToInstall = "Kaspersky Total Security";
                kavDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe";
                kavLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
                Properties.Settings.Default.KavWebsite = "https://www.kaspersky.com/downloads/thank-you/total-security"; Properties.Settings.Default.Save();
            }

            try
            {
                await Task.Delay(4000);
                await Uninstall(kavEdition, kavGUID);
                await Registry();
                await DownloadSetup(kavEditionToInstall, kavDownloadUrl);
                await License(kavLicenseUrl, kavEditionToInstall);
                Properties.Settings.Default.MainThreadDone = true; Properties.Settings.Default.Save();
            }
            catch (Exception exception) { MessageBox.Show(this, $"Error: {exception.Message}", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);  Application.Exit(); return; }

            if (kavGUID != null)
            {
                OutputLabel.Text = "Esperando reinicio del equipo";
                MessageBox.Show(this, "Debes reiniciar el equipo para continuar la instalación.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.TopMost = false;
            }
            else SecondThread();
        }

        private async Task Uninstall(string kavEdition, string kavGUID)
        {
            if (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
            {
                if (Process.GetProcessesByName("avp").Length > 0)
                {
                    MessageBox.Show(this, $"Debes cerrar manualmente {kavEdition} para continuar." + Environment.NewLine + Environment.NewLine + "ATENCIÓN: Si Kaspersky se encuentra protegido por contraseña, adicionalmente deberás deshabilitar esta protección, o realizar la desinstalación manualmente.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    OutputLabel.Text = "Esperando cierre de Kaspersky Lab";
                    while (Process.GetProcessesByName("avp").Length > 0) await Task.Delay(500);
                }

                if (kavGUID != null)
                {
                    //If KAV is protected by password cannot uninstall.
                    OutputLabel.Text = $"Desinstalando {kavEdition}"; GIF.Visible = true;
                    await Task.Run(() => Process.Start("msiexec.exe", $"/x {kavGUID} /quiet").WaitForExit());
                }
                else
                {
                    MessageBox.Show(this, $"No es posible realizar la desinstalación automática de {kavEdition}. Debes gestionarlo manualmente utilizando el panel de control.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    OutputLabel.Text = $"Esperando desinstalación manual de {kavEdition}";
                    this.TopMost = false;
                    Process.Start(Path.Combine(Environment.SystemDirectory, "control.exe"));
                }

                while (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null) await Task.Delay(1000);
                GIF.Visible = false; this.TopMost = true;
            }
        }

        private async Task Registry()
        {
            OutputLabel.Text = $"Editando registro de Windows";
            await Task.Delay(2000);

            try
            {
                LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(this, "No ha sido posible acceder al registro de Windows." + Environment.NewLine + "Comprueba tus privilegios como Administrador, reinicia el equipo y cierra cualquier anti-malware abierto antes de volver a ejecutar Kaspersky Custom Installer.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            catch (ArgumentException) { }

            try { LocalMachine32View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab"); } catch (Exception) { /*No need to catch*/ }
            try { LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab"); } catch (Exception) { /*No need to catch*/ }
            try { LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Cryptography\RNG"); } catch (Exception) { /*No need to catch*/ }
        }

        private async Task DownloadSetup(string kavEdition, string kavDownloadUrl)
        {
            OutputLabel.Text = $"Descargando {kavEdition}";
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
                    await client.DownloadFileTaskAsync(kavDownloadUrl, Path.GetTempPath() + "kavsetup.exe");
                    await Task.Delay(1000);
                    OutputLabel.Text = outputText;
                }
            }
            catch (WebException)
            {
                if (MessageBox.Show(this, $"No ha sido posible descargar el instalador para {kavEdition}. Comprueba tu conexión a internet.", "KCI", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry) goto Retry;
            }
        }

        private async Task License(string kavLicenseUrl, string kavEditionToInstall)
        {
            OutputLabel.Text = $"Generando licencias";
            await Task.Delay(2000);

            try
            {
                using (WebClient client = new WebClient()) { client.DownloadFile(kavLicenseUrl, Path.GetTempPath() + "kavlicense.txt"); }
            }
            catch (WebException)
            {
                if (kavEditionToInstall == "Kaspersky Antivirus")
                {
                    //using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    //{
                    //    text.Write("null");
                    //}
                }
                else if (kavEditionToInstall == "Kaspersky Internet Security")
                {
                    using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    {
                        text.Write("4CH4C-PPFDT-NFK4B-45R69");
                    }
                }
                else if (kavEditionToInstall == "Kaspersky Total Security")
                {
                    //using (StreamWriter text = File.CreateText(Path.GetTempPath() + "kavlicense.txt"))
                    //{
                    //    text.Write("null");
                    //}
                }
            }
            catch (Exception) { }
        }

        private async void SecondThread(string AVProot = null, string ksdeGUID = null)
        {
            OutputLabel.Text = "Iniciando instalación";

            try
            {
                Process kavsetup = new Process();
                kavsetup.StartInfo.FileName = Path.GetTempPath() + "kavsetup.exe";
                kavsetup.Start();

                this.Location = new Point(this.Location.X, 1); GIF.Visible = true;
                await Task.Run(() => kavsetup.WaitForExit()); GIF.Visible = false;
                try { File.Delete(Path.GetTempPath() + "kavsetup.exe"); } catch { }
            }
            catch (Exception)
            {
                MessageBox.Show(this, $"No ha sido posible iniciar el instalador de Kaspersky Lab." + Environment.NewLine +"Al cerrar este dialogo, serás dirigido a la web oficial para realizar la descarga manualmente.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.TopMost = false;
                Process.Start(Properties.Settings.Default.KavWebsite);
            }

            OutputLabel.Text = "Esperando instalación manual"; this.Refresh();

            this.TopMost = false;
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
                ProcessStartInfo avp = new ProcessStartInfo();
                avp.FileName = $@"{AVProot}\avp.com";
                avp.Arguments = $"license /add {LicenseKey}";
                avp.WindowStyle = ProcessWindowStyle.Hidden;
                avp.CreateNoWindow = true;
                await Task.Run(() => Process.Start(avp).WaitForExit());
                try { File.Delete(Path.GetTempPath() + "kavlicense.txt"); } catch { }
            }
            else MessageBox.Show(this, $"No existen licencias de activación disponibles. Activa la versión de evaluación de la aplicación desde el apartado [Introducir código de activación].", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            foreach (Process process in Process.GetProcessesByName("ksde")) { process.Kill(); }
            foreach (Process process in Process.GetProcessesByName("ksdeui")) { process.Kill(); }

            await Task.Run(() => Process.Start("msiexec.exe", $"/x {ksdeGUID} /quiet").WaitForExit()); GIF.Visible = false;

            Properties.Settings.Default.MainThreadDone = false; Properties.Settings.Default.Save();

            this.Opacity = 0; this.ShowInTaskbar = false;
            MessageBox.Show(this, "¡Todo listo! Gracias por utilizar Kaspersky Custom Installer.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        private void Form_Closing(object sender, FormClosingEventArgs close)
        {
            if (close.CloseReason.Equals(CloseReason.UserClosing))
            {
                if (MessageBox.Show(this, "La instalación no ha finalizado. ¿Deseas cerrar la aplicación de todas formas?", "KCI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) close.Cancel = true;
            }
            else if (close.CloseReason.Equals(CloseReason.WindowsShutDown))
            {
                LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).SetValue("KciExtension", System.Reflection.Assembly.GetEntryAssembly().Location, RegistryValueKind.String);
                return;
            }
            try { LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).DeleteValue("KciExtension"); } catch (ArgumentException) { }
            Properties.Settings.Default.MainThreadDone = false; Properties.Settings.Default.Save();
        }
    }
}
