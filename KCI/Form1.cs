using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;
using System.Management;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace KCI
{
    #region Main
    public partial class KCI : Form
    {
        public KCI()
        //Try async task so that main thread does not block.
        //Kaspersky Installed: SOFTWARE\KasperskyLab\WmiHlp\IsInstalled
        //Kaspersky Edition: SOFTWARE\KasperskyLab\AVP20.0\environment\ProductName
        //Uninstall string: SOFTWARE\KasperskyLab\AVP20.0\environment\Ins_UninstallString
        //Get GUID: SOFTWARE\KasperskyLab\AVP20.0\environment\ProductCode
        {
            if (Process.GetProcesses().Count(process => process.ProcessName == Process.GetCurrentProcess().ProcessName) > 1) { MessageBox.Show("Kaspersky Custom Installer no responde.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); Environment.Exit(0); }
            InitializeComponent();
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                StartButtonDisable();
                UninstallButton.Enabled = false; UninstallButton.Image = Properties.Resources.UninstallButtonDisabled;
                RegistryButton.Enabled = false; RegistryButton.Image = Properties.Resources.RegistryButtonDisabled;
                ErrorLabel.Visible = true;
            }
        }
        #endregion

        #region MainButtons
        private void StartButtonMethod()
        {
            //if (!StartButton.Enabled) return;
            StartButtonDisable(); CustomizeButtonDisable(); HelpButtonDisable();
            StartPanelDownTimer.Enabled = true;
        }

        private void CustomizeButtonMethod()
        {
            StartButtonDisable(); CustomizeButtonDisable(); HelpButtonDisable();
            CustomizePanelDownTimer.Enabled = true;
        }

        private void HelpButtonMethod()
        {
            StartButtonDisable(); CustomizeButtonDisable(); HelpButtonDisable();
            HelpPanelReset();
            HelpPanel.Left = 277;
        }
        #endregion

        #region CustomizeButtons
        private async void UninstallButtonMethod()
        {
            CustomizePanelUpTimer.Enabled = true;
            GIF.Visible = true;
            try { await Uninstall(); } catch { }
            ProgressLabel.Text = "Progreso de las funciones";
            GIF.Visible = false;
            StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
        }


        private async  void RegistryButtonMethod()
        {
            CustomizePanelUpTimer.Enabled = true;
            GIF.Visible = true;
            try { await Registry(); } catch { }
            ProgressLabel.Text = "Progreso de las funciones";
            GIF.Visible = false;
            StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
        }


        private async void DownloadButtonMethod()
        {
            customizeDownload = true;
            CustomizePanelUpTimer.Enabled = true;
            await Task.Delay(400);
            StartPanelDownTimer.Enabled = true;
        }


        private async void LicenseButtonMethod()
        {
            customizeLicense = true;
            CustomizePanelUpTimer.Enabled = true;
            await Task.Delay(400);
            StartPanelDownTimer.Enabled = true;
        }
        #endregion

        #region HelpPanel
        private void HelpPanelReset()
        {
            //  Ocultar descripciones
            FAQ1Description.Visible = false;
            FAQ2Description.Visible = false;
            FAQ3Description.Visible = false; FAQ3Link.Visible = false;
            FAQ4Description.Visible = false;
            FAQ5Description.Visible = false;
            FAQ6Description.Visible = false; FAQ6Link.Visible = false;
            //  Ordenar títulos FAQ
            FAQ1Button.Top = 4;
            FAQ2Button.Top = 28;
            FAQ3Button.Top = 52;
            FAQ4Button.Top = 76;
            FAQ5Button.Top = 100;
            FAQ6Button.Top = 124;
            //  Mostrar títulos FAQ
            FAQ1Button.Visible = true;
            FAQ2Button.Visible = true;
            FAQ3Button.Visible = true;
            FAQ4Button.Visible = true;
            FAQ5Button.Visible = true;
            FAQ6Button.Visible = true;
            //  Ocultar FAQBackButton
            FAQBackButton.Visible = false;
            return;
        }

        private void FAQ1ButtonMethod()
        {
            if (FAQ1Description.Visible) { HelpPanelReset(); return; }
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ1Button.Top = 4;
            FAQ1Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ2ButtonMethod()
        {
            if (FAQ2Description.Visible) { HelpPanelReset(); return; }
            FAQ1Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ2Button.Top = 4;
            FAQ2Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ3ButtonMethod()
        {
            if (FAQ3Description.Visible) { HelpPanelReset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ3Button.Top = 4;
            FAQ3Description.Visible = true; FAQ3Link.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ4ButtonMethod()
        {
            if (FAQ4Description.Visible) { HelpPanelReset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ4Button.Top = 4;
            FAQ4Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ5ButtonMethod()
        {
            if (FAQ5Description.Visible) { HelpPanelReset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ5Button.Top = 4;
            FAQ5Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ6ButtonMethod()
        {
            if (FAQ6Description.Visible) { HelpPanelReset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Top = 4;
            FAQ6Description.Visible = true; FAQ6Link.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }
        #endregion

        #region KAV
        private void KAV()
        {
            kasperskyEdition = "Kaspersky Antivirus";
            kasperskySetupFile = "KAV(ES).exe";
            kasperskyLicenseFile = "KAV Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/antivirus";
            MainThread();
        }
        #endregion

        #region KIS
        private void KIS()
        {
            kasperskyEdition = "Kaspersky Internet Security";
            kasperskySetupFile = "KIS(ES).exe";
            kasperskyLicenseFile = "KIS Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/internet-security";
            MainThread();
        }
        #endregion

        #region KTS
        private void KTS()
        {
            kasperskyEdition = "Kaspersky Total Security";
            kasperskySetupFile = "KTS(ES).exe";
            kasperskyLicenseFile = "KTS Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/total-security";
            MainThread();
        }
        #endregion

        #region MainThread
        private async void MainThread()
        {
            StartPanelUpTimer.Enabled = true;
            GIF.Visible = true;
            await Task.Delay(1000);
            if (customizeDownload)
            {
                await DownloadKav();
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                ProgressLabel.Text = "Progreso de las funciones";
                GIF.Visible = false;
                customizeDownload = false;
                return;
            }
            if (customizeLicense)
            {
                await License();
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                ProgressLabel.Text = "Progreso de las funciones";
                GIF.Visible = false;
                customizeLicense = false;
                return;
            }
            try
            {
                await Uninstall();
                await Registry();
                await DownloadKav();
                await License();
            }
            catch (Exception)
            {
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                ProgressLabel.Text = "Progreso de las funciones";
                GIF.Visible = false;
                return;
            }
            await Task.Delay(1000);
            ProgressLabel.Text = "Instalación completada con éxito";
            GIF.Visible = false;
            MessageBoxEx.Show(this, "Kaspersky Custom Installer ha completado su trabajo con éxito. A partir de aquí, todo queda en manos del instalador de Kaspersky.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if(File.Exists(KCIDir + kasperskySetupFile)) Process.Start(KCIDir + kasperskySetupFile);
            Application.Exit();
        }
        #endregion

        #region Uninstall
        private async Task Uninstall()
        {
            if (RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
            {
                //RegistryKey uninstallkey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
                //foreach (var subkey in uninstallkey.GetSubKeyNames())
                //{
                //    RegistryKey uninstallsubkey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey($@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{subkey}");
                //    foreach (string value in uninstallsubkey.GetValueNames())
                //    {
                //        if (uninstallsubkey.GetValue(value).ToString().Contains("Kaspersky") == true)
                //        {
                //            if (value == "DisplayName")
                //            {
                //                kasperskyInstalledEdition = uninstallsubkey.GetValue(value).ToString();
                //                break;
                //            }
                //        }
                //    }
                //    foreach (string value in uninstallsubkey.GetValueNames())
                //    {
                //        if (uninstallsubkey.GetValue(value).ToString().Contains("Kaspersky") == true)
                //        {
                //            kasperskyGUID = "{" + subkey.Substring(subkey.IndexOf("{") + "{".Length, subkey.IndexOf("}") - Math.Abs(subkey.IndexOf("{") + "{".Length)) + "}";
                //            Clipboard.SetText(kasperskyGUID);
                //            break;
                //        }
                //    }
                //}


                foreach (string AVPkey in RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                {
                    if (AVPkey.Contains("AVP"))
                    {
                        kasperskyInstalledEdition = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductName").ToString();
                        kasperskyGUID = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductCode").ToString();
                        break;
                    }
                }
                
                ProgressLabel.Text = $"Iniciando desinstalación de {kasperskyInstalledEdition}...";
                await Task.Delay(1000);

                DialogResult answer = MessageBoxEx.Show(this, $"Antivirus {kasperskyInstalledEdition} detectado." + Environment.NewLine + Environment.NewLine + "Métodos de desinstalación:" + Environment.NewLine + "1. Desinstalación automática (debes cerrar Kaspersky). Pulsa SÍ." + Environment.NewLine + "2. Utilizando la herramienta KavRemover, creada oficialmente por la firma KasperskyLab, es la forma más eficaz y segura de desinstalar tu antivirus Kaspersky. Pulsa No." + Environment.NewLine + "3. Mediante el método habitual, utilizando el panel de control. Pulsa Cancelar.", "KCI", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                if (answer == DialogResult.Yes)
                {
                    ProgressLabel.Text = $"Esperando cierre de {kasperskyInstalledEdition}...";

                    while (Process.GetProcessesByName("avp").Length > 0) await Task.Delay(500);

                    ProgressLabel.Text = $"Desinstalando {kasperskyInstalledEdition}...";

                    if (kasperskyGUID == null) { MessageBox.Show("No ha sido posible realizar la desinstalación automáticamente. Prueba otro método de desinstalación."); throw new Exception(); }

                    Process.Start("msiexec", $"/x {kasperskyGUID} /quiet");
                }

                if (answer == DialogResult.No)
                {
                    try
                    {
                        await Download("http://media.kaspersky.com/utilities/ConsumerUtilities/kavremvr.exe", TempDir, "kavremvr.exe");
                        Directory.SetCurrentDirectory(TempDir);
                        Process.Start(TempDir + "kavremvr.exe");
                    }
                    catch (WebException) { MessageBoxEx.Show(this, "No ha sido posible realizar la descarga. Comprueba tu conexión a internet y vuelve a internarlo o utiliza otro método de desinstalación.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); throw new FileNotFoundException(); }
                    catch (Exception) { }

                    ProgressLabel.Text = $"Esperando desinstalación manual de {kasperskyInstalledEdition}...";
                }

                if (answer == DialogResult.Cancel)
                {
                    Process.Start(Path.Combine(System.Environment.SystemDirectory, "control.exe"));
                    ProgressLabel.Text = $"Esperando desinstalación manual de {kasperskyInstalledEdition}...";
                }
                
                while (RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\KasperskyLab") != null) await Task.Delay(500);
            }
        }
        #endregion

        #region Registry
        private async Task Registry()
        {
            ProgressLabel.Text = "Editando registro de Windows...";
            await Task.Delay(1000);
            try
            {
                if (RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
                    RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");

                if (RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Cryptography\RNG") != null)
                    RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).DeleteSubKeyTree(@"SOFTWARE\Microsoft\Cryptography\RNG");

                if (RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates") != null)
                    RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).DeleteSubKeyTree(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates");
            }
            catch (UnauthorizedAccessException exception)
            {
                GIF.Enabled = false;
                MessageBoxEx.Show(this, "No ha sido posible editar el registro de Windows." + Environment.NewLine + "Verifica haber desinstalado todo antivirus perteneciente a la firma KasperskyLab, y cierra cualquier anti-malware abierto.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                throw exception;
            }
        }
        #endregion

        #region DownloadKav
        private async Task DownloadKav()
        {
            ProgressLabel.Text = $"Descargando {kasperskyEdition}...";
            await Task.Delay(1000);
            try { await Download(kasperskyDownloadUrl, KCIDir, kasperskySetupFile); }
            catch (WebException)
            {
                GIF.Enabled = false;
                MessageBoxEx.Show(this, $"No ha sido posible realizar la descarga de {kasperskyEdition}." + Environment.NewLine + "Comprueba tu conexión a internet y realiza la descarga manualmente desde la web oficial, a la cual serás dirigido al cerrar este dialogo. Mientras tanto, KCI terminará el trabajo.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Process.Start(kasperskyWebsite);
                GIF.Enabled = true;
            }
            //if (!customizeDownload) { RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).SetValue("Installer", KCIDir + kasperskySetupFile, RegistryValueKind.String); }
        }
        #endregion

        #region License
        private async Task License()
        {
            ProgressLabel.Text = "Generando licencias...";
            await Task.Delay(1000);
            try { await Download(kasperskyLicenseUrl, KCIDir, kasperskyLicenseFile); }
            catch (WebException exception)
            {
                if (kasperskyEdition == "Kaspersky Internet Security")
                {
                    using (StreamWriter text = File.CreateText(KCIDir + "KIS Licencia.txt"))
                    {
                        text.WriteLine("Kaspersky Custom Installer (C)2020");
                        text.WriteLine(Environment.NewLine);
                        text.WriteLine("Kaspersky Internet Security licencia de activación (91 días):");
                        text.WriteLine("4CH4C-PPFDT-NFK4B-45R69");
                    }
                    return;
                }
                GIF.Enabled = false;
                MessageBoxEx.Show(this, $"No existen licencias disponibles. Inténtalo más adelante.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                GIF.Enabled = true;
            }
        }
        #endregion

        #region EnableMainButtons

        private void StartButtonEnable()
        {
            if (RegistryButton.Enabled == true)
            StartButton.Enabled = true;
        }

        private void CustomizeButtonEnable()
        {
            CustomizeButton.Enabled = true;
        }

        private void HelpButtonEnable()
        {
            HelpButton.Enabled = true; HelpButton.Image = Properties.Resources.HelpButtonEnabled; HelpButton.ImageActive = Properties.Resources.HelpButtonActive;
        }
        #endregion

        #region DisableMainButtons

        private void StartButtonDisable()
        {
            StartButton.Enabled = false;
        }

        private void CustomizeButtonDisable()
        {
            CustomizeButton.Enabled = false;
        }

        private void HelpButtonDisable()
        {
            HelpButton.Enabled = false; HelpButton.ImageActive = null; HelpButton.Image = Properties.Resources.HelpButtonDisabled;
        }
        #endregion

        #region HidePanels
        private void HidePanelsMethod()
        {
            if (StartPanel.Location.Y == 3) { StartPanelUpTimer.Enabled = true; customizeDownload = false; customizeLicense = false; StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable(); }
            if (CustomizePanel.Location.Y == 3) { CustomizePanelUpTimer.Enabled = true; StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable(); }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable(); }
        }
        #endregion

        #region Timer
        private void StartPanelDownTimer_Tick(object sender, EventArgs e)
        {
            StartPanel.Top += 9;
            if (StartPanel.Location.Y == 3) StartPanelDownTimer.Enabled = false;
            return;
        }

        private void StartPanelUpTimer_Tick(object sender, EventArgs e)
        {
            StartPanel.Top -= 18;
            if (StartPanel.Location.Y == -159) StartPanelUpTimer.Enabled = false;
            return;
        }

        private void CustomizePanelDownTimer_Tick(object sender, EventArgs e)
        {
            CustomizePanel.Top += 9;
            if (CustomizePanel.Location.Y == 3) CustomizePanelDownTimer.Enabled = false;
            return;
        }

        private void CustomizePanelUpTimer_Tick(object sender, EventArgs e)
        {
            CustomizePanel.Top -= 18;
            if (CustomizePanel.Location.Y == -285) CustomizePanelUpTimer.Enabled = false;
            return;
        }
        
        #endregion

        #region Download

        private async Task Download(string url, string dir, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                string progressLabelText = ProgressLabel.Text;
                client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs n)
                {
                    ProgressLabel.Text = $"{progressLabelText} Descargando {fileName}...{n.ProgressPercentage}%";
                };
                await client.DownloadFileTaskAsync(url, dir + fileName);
                ProgressLabel.Text = progressLabelText;
            }
        }


        #endregion

        #region Events
        private void StartButton_Click_1(object sender, EventArgs e) => StartButtonMethod();

        private void CustomizeButton_Click_1(object sender, EventArgs e) => CustomizeButtonMethod();

        private void HelpButton_Click(object sender, EventArgs e) => HelpButtonMethod();

        private void UninstallButton_Click(object sender, EventArgs e) => UninstallButtonMethod();

        private void RegistryButton_Click(object sender, EventArgs e) => RegistryButtonMethod();

        private void DownloadButton_Click(object sender, EventArgs e) => DownloadButtonMethod();

        private void LicenseButton_Click(object sender, EventArgs e) => LicenseButtonMethod();

        private void KAVButton_Click(object sender, EventArgs e) => KAV();

        private void KISButton_Click(object sender, EventArgs e) => KIS();

        private void KTSButton_Click(object sender, EventArgs e) => KTS();

        private void FAQ1Button_Click(object sender, EventArgs e) => FAQ1ButtonMethod();

        private void FAQ2Button_Click(object sender, EventArgs e) => FAQ2ButtonMethod();

        private void FAQ3Button_Click(object sender, EventArgs e) => FAQ3ButtonMethod();

        private void FAQ4Button_Click(object sender, EventArgs e) => FAQ4ButtonMethod();

        private void FAQ5Button_Click(object sender, EventArgs e) => FAQ5ButtonMethod();

        private void FAQ6Button_Click(object sender, EventArgs e) => FAQ6ButtonMethod();

        private void FAQ3Link_Click(object sender, EventArgs e) => Process.Start("http://tiny.cc/KCI-VirusTotal");

        private void FAQ6Link_Click(object sender, EventArgs e) => Process.Start("http://tiny.cc/KCI-YoutubeChannel");

        private void FAQBackButton_Click(object sender, EventArgs e) => HelpPanelReset();

        private void KCI_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void LeftPanel_MouseClick(object sender, MouseEventArgs e) => HidePanelsMethod();

        private void TitleLabel_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void DescriptionLabel_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void StartButtonDisabled_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void OutputTextbox_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void WaitEnterTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
                    MessageBoxEx.Show(this, "Desinstalación no completada." + Environment.NewLine + "Si el problema persiste, reinicia tu equipo.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else WaitEnterTextbox.Enabled = false;
        }
        #endregion

        #region Variables
        private static string KCIDir { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        private static string TempDir { get; set; } = Path.GetTempPath();
        private bool customizeDownload { get; set; }
        private bool customizeLicense { get; set; }
        private string kasperskyEdition { get; set; }
        private string kasperskyGUID { get; set; }
        private string kasperskyInstalledEdition { get; set; } = "KasperskyLab";
        private string kasperskySetupFile { get; set; }
        private string kasperskyLicenseFile { get; set; }
        private string kasperskyDownloadUrl { get; set; }
        private string kasperskyLicenseUrl { get; set; }
        private string kasperskyWebsite { get; set; }
    }
    #endregion

    #region CenterMessageBox
    public class MessageBoxEx
    {
        private static IWin32Window _owner;
        private static HookProc _hookProc;
        private static IntPtr _hHook;

        public static DialogResult Show(string text)
        {
            Initialize();
            return MessageBox.Show(text);
        }

        public static DialogResult Show(string text, string caption)
        {
            Initialize();
            return MessageBox.Show(text, caption);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton, options);
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon,
                                   defButton, options);
        }

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);

        public const int WH_CALLWNDPROCRET = 12;

        public enum CbtHookAction : int
        {
            HCBT_MOVESIZE = 0,
            HCBT_MINMAX = 1,
            HCBT_QS = 2,
            HCBT_CREATEWND = 3,
            HCBT_DESTROYWND = 4,
            HCBT_ACTIVATE = 5,
            HCBT_CLICKSKIPPED = 6,
            HCBT_KEYSKIPPED = 7,
            HCBT_SYSCOMMAND = 8,
            HCBT_SETFOCUS = 9
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("User32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

        [DllImport("user32.dll")]
        public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };

        static MessageBoxEx()
        {
            _hookProc = new HookProc(MessageBoxHookProc);
            _hHook = IntPtr.Zero;
        }

        private static void Initialize()
        {
            if (_hHook != IntPtr.Zero)
            {
                throw new NotSupportedException("multiple calls are not supported");
            }

            if (_owner != null)
            {
                _hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
            }
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(_hHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = _hHook;

            if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
            {
                try
                {
                    CenterWindow(msg.hwnd);
                }
                finally
                {
                    UnhookWindowsHookEx(_hHook);
                    _hHook = IntPtr.Zero;
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle recChild = new Rectangle(0, 0, 0, 0);
            bool success = GetWindowRect(hChildWnd, ref recChild);

            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;

            Rectangle recParent = new Rectangle(0, 0, 0, 0);
            success = GetWindowRect(_owner.Handle, ref recParent);

            Point ptCenter = new Point(0, 0);
            ptCenter.X = recParent.X + ((recParent.Width - recParent.X) / 2);
            ptCenter.Y = recParent.Y + ((recParent.Height - recParent.Y) / 2);


            Point ptStart = new Point(0, 0);
            ptStart.X = (ptCenter.X - (width / 2));
            ptStart.Y = (ptCenter.Y - (height / 2));

            ptStart.X = (ptStart.X < 0) ? 0 : ptStart.X;
            ptStart.Y = (ptStart.Y < 0) ? 0 : ptStart.Y;

            int result = MoveWindow(hChildWnd, ptStart.X, ptStart.Y, width,
                                    height, false);
        }

    }
    #endregion
}