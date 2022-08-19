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
        {
            //if (Process.GetProcesses().Count(process => process.ProcessName == Process.GetCurrentProcess().ProcessName) > 1) { MessageBox.Show("Kaspersky Custom Installer no responde.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); Environment.Exit(0); }
            InitializeComponent();
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                accountTypeException = true;
                StartButtonDisable(); RegistryButtonDisable();
                Output1.ForeColor = Color.Red; Output1.Text = "*** Permisos de Administrador requeridos.";
            }
            else Output1.ForeColor = Color.Silver; Output1.Text = "Aquí se refleja el progreso de la instalación.";
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
            try { await Uninstall(); } catch { }
            StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
            return;
        }


        private async void RegistryButtonMethod()
        {
            CustomizePanelUpTimer.Enabled = true;
            try { await Registry(); } catch { }
            StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
            return;
        }


        private async void DownloadButtonMethod()
        {
            Step = 3;
            CustomizePanelUpTimer.Enabled = true;
            await Task.Delay(400);
            StartPanelDownTimer.Enabled = true;
            return;
        }


        private async void LicenseButtonMethod()
        {
            Step = 4;
            CustomizePanelUpTimer.Enabled = true;
            await Task.Delay(400);
            StartPanelDownTimer.Enabled = true;
            return;
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
        private async void KAVButtonMethod()
        {
            kasperskyEdition = "Kaspersky Antivirus";
            kasperskySetupFile = "KAV(ES).exe";
            kasperskyLicenseFile = "KAV Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/antivirus";

            await Task.Delay(100);
            StartPanelUpTimer.Enabled = true;
            if (Step == 3) { DownloadKaspersky(); return; }
            if (Step == 4) { License(); return; }
            try
            {
                await Uninstall();
                await Registry();
                await DownloadKaspersky();
                await License();
            } catch (Exception) { return; }
            Closure();
        }
        #endregion

        #region KIS
        private async void KISButtonMethod()
        {
            kasperskyEdition = "Kaspersky Internet Security";
            kasperskySetupFile = "KIS(ES).exe";
            kasperskyLicenseFile = "KIS Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/internet-security";

            await Task.Delay(100);
            StartPanelUpTimer.Enabled = true; //while (StartPanelDown.Enabled) await Task.Delay(25);
            if (Step == 3) { DownloadKaspersky(); return; }
            if (Step == 4) { License(); return; }

            try
            {
                await Uninstall();
                await Registry();
                await DownloadKaspersky();
                await License();
            } catch (Exception) { return; }
            Closure();
            return;
        }
        #endregion

        #region KTS
        private async void KTSButtonMethod()
        {
            kasperskyEdition = "Kaspersky Total Security";
            kasperskySetupFile = "KTS(ES).exe";
            kasperskyLicenseFile = "KTS Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/total-security";

            await Task.Delay(100);
            StartPanelUpTimer.Enabled = true;
            if (Step == 3) { DownloadKaspersky(); return; }
            if (Step == 4) { License(); return; }
            try
            {
                await Uninstall();
                await Registry();
                await DownloadKaspersky();
                await License();
            } catch (Exception) { return; }
            Closure();
            return;
        }
        #endregion

        #region Uninstall
        private async Task Uninstall()
        {
            Output1.Text = "1. Desinstalar KasperskyLab";
            await Task.Delay(3000);
            BlurSettings("Pulsa ENTER cuando finalices la desinstalación de tu antivirus.", Properties.Resources.BlurPicture1);
            await Task.Delay(100);
            if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\KasperskyLab") != null)
            {
                if (MessageBoxEx.Show(this, "Antivirus KasperskyLab detectado." + Environment.NewLine + "¿Deseas utilizar KavRemover para realizar la desinstalación?" + Environment.NewLine + Environment.NewLine + "Más Información:" + Environment.NewLine + "Kavremover es una potente herramienta opcional, desarrollada oficialmente por la firma KasperskyLab, siendo la forma más rápida y limpia de desinstalar tu antivirus Kaspersky.", "KCI", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    try { await Download("http://media.kaspersky.com/utilities/ConsumerUtilities/kavremvr.exe", TempDir, "kavremover.exe"); }
                    catch (Exception) { MessageBox.Show("No ha sido posible iniciar KavRemover."); }
                    BlurLabel.Text = "Pulsa ENTER cuando finalices la desinstalación de tu antivirus.";
                    try { Directory.SetCurrentDirectory(TempDir); Process.Start(TempDir + "kavremover.exe"); } catch(Exception) { }
                }
            }
            WaitEnterTextbox.Enabled = true; WaitEnterTextbox.Focus();
            while (BlurPicture.Visible) await Task.Delay(25);
            WaitEnterTextbox.Enabled = false;
            if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\KasperskyLab") != null) { MessageBoxEx.Show(this, "Desinstalación no realizada."); throw new Exception(); }
            await Task.Delay(500);
            //Done output
            await Task.Delay(2000);
            return;
        }
        #endregion

        #region Registry
        private async Task Registry()
        {
            Output2.Visible = true;
            await Task.Delay(1000);
            try
            {
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Cryptography\\RNG");
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates");
            }
            catch (UnauthorizedAccessException exception)
            {
                await Task.Delay(500);
                //Error output
                await Task.Delay(500);
                MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. El acceso al registro de windows ha sido denegado." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Desinstala manualmente tu antivirus y reinicia el sistema.", "Instalación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Delay(100);
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                throw exception;
            }
            await Task.Delay(500);
            //Done output
            await Task.Delay(2000);
        }
        #endregion

        #region Download
        private async Task DownloadKaspersky()
        {
            Output3.Visible = true;
            if (!File.Exists(KCIDir + kasperskySetupFile))
                try { Download(kasperskyDownloadUrl, KCIDir, kasperskySetupFile); }
                catch(WebException)
                {
                    await Task.Delay(500);
                    //Error output
                    await Task.Delay(500);
                    MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Descarga fallida." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Descarga Kaspersky Antivirus manualmente desde su web oficial." + Environment.NewLine + "Serás dirigido a ella al cerrar este dialogo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    try { Process.Start(kasperskyWebsite); } catch (Exception ex) { MessageBoxEx.Show(this, "No ha sido posible acceder a la web.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    await Task.Delay(100);
                    if (Step == 3)
                    {
                        StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                        //reset output
                        Step = 0;
                        return;
                    }
                    BlurSettings($"Pulsa ENTER cuando finalices la descarga manual de {kasperskyEdition}.", Properties.Resources.BlurPicture2);
                    WaitEnterTextbox.Enabled = true; WaitEnterTextbox.Focus();
                    while (BlurLabel.Visible) await Task.Delay(25);
                    WaitEnterTextbox.Enabled = false;
                    await Task.Delay(500);
                    License();
                    return;
                }
            await Task.Delay(1000);
            if (Step == 0) { try { Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + kasperskySetupFile, RegistryValueKind.String); } catch(Exception) { } }
            await Task.Delay(500);
            //Done output
            await Task.Delay(2000);
            if (Step == 3)
            {
                //Reset output
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                Step = 0;
            }
            return;
        }
        #endregion

        #region License
        private async Task License()
        {
            Output4.Visible = true;
            if (!File.Exists(KCIDir + kasperskyLicenseFile))
            try { await Task.Delay(3000); using (WebClient client = new WebClient()) { client.DownloadFile(kasperskyLicenseUrl, KCIDir + kasperskyLicenseFile); } }
                catch (WebException)
                {
                    await Task.Delay(500);
                    //Error output
                    if (Step == 4)
                    {
                        await Task.Delay(500);
                        MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Licencia no generada." + Environment.NewLine + Environment.NewLine + "Más información: No existen licencias disponibles por el momento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        await Task.Delay(100);
                        StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                        //Reset output
                        Step = 0;
                        return;
                    }
                    await Task.Delay(3000);
                    Closure();
                    return;
                }
            await Task.Delay(500);
            //Done output
            await Task.Delay(2000);
            if (Step == 4)
            {
                //Reset Output
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                Step = 0;
            }
            return;
        }
        #endregion

        #region Closure
        private async Task Closure()
        {
            RestartButton.Location = new Point(229, 153);
            RestartButton.Visible = true;
            BlurSettings("Debes reiniciar el sistema para continuar con la instalación.", Properties.Resources.BlurPicture3);
            MessageBoxEx.Show(this, "Kaspersky reset finalizado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await Task.Delay(100);
            return;
        }

        private void RestartButtonMethod()
        {
            try { Process.Start("shutdown.exe", "-r -t 00"); } catch(Exception ex) { MessageBoxEx.Show(this, "No ha sido posible reiniciar el sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            this.Close();
        }
        #endregion

        #region EnableMainButtons

        private void StartButtonEnable()
        {
            StartButton.Enabled = true; StartButton.BackColor = Color.Gold; StartButton.Cursor = Cursors.Hand;
        }

        private void CustomizeButtonEnable()
        {
            CustomizeButton.Enabled = true; CustomizeButton.ForeColor = Color.FromArgb(0, 168, 142); CustomizeButton.IdleForecolor = Color.FromArgb(0, 168, 142);
        }

        private void HelpButtonEnable()
        {
            HelpButton.Enabled = true; HelpButton.Image = Properties.Resources.HelpButtonEnabled; HelpButton.ImageActive = Properties.Resources.HelpButtonActive;
        }
        #endregion

        #region DisableMainButtons

        private void StartButtonDisable()
        {
            StartButton.Enabled = false; StartButton.BackColor = Color.Gray; StartButton.Cursor = Cursors.Default;
        }

        private void CustomizeButtonDisable()
        {
            CustomizeButton.Enabled = false; CustomizeButton.ForeColor = Color.DimGray; CustomizeButton.IdleForecolor = Color.DimGray;
        }

        private void HelpButtonDisable()
        {
            HelpButton.Enabled = false; HelpButton.ImageActive = null; HelpButton.Image = Properties.Resources.HelpButtonDisabled;
        }
        #endregion

        #region DisableCustomizePanelButtons
        private void UninstallButtonDisable()
        {
            UninstallButton.Enabled = false; UninstallButton.Image = Properties.Resources.UninstallButtonDisabled;
        }

        private void RegistryButtonDisable()
        {
            RegistryButton.Enabled = false; RegistryButton.Image = Properties.Resources.RegistryButtonDisabled;
        }

        private void DownloadButtonDisable()
        {
            DownloadButton.Enabled = false; DownloadButton.Image = Properties.Resources.DownloadButtonDisabled;
        }

        private void LicenseButtonDisable()
        {
            LicenseButton.Enabled = false; LicenseButton.Image = Properties.Resources.LicenseButtonDisabled;
        }
        #endregion

        #region HidePanels
        private void HidePanelsMethod()
        {
            if (StartPanel.Location.Y == 3) { StartPanelUpTimer.Enabled = true; Step = 0; StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable(); }
            if (CustomizePanel.Location.Y == 3) { CustomizePanelUpTimer.Enabled = true; Step = 0; StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable(); }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; Step = 0; StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable(); }
            return;
        }
        #endregion

        #region Timer1
        private void StartPanelDownTimer_Tick(object sender, EventArgs e)
        {
            StartPanel.Top += 9;
            if (StartPanel.Location.Y == 3) StartPanelDownTimer.Enabled = false;
        }

        private void StartPanelUpTimer_Tick(object sender, EventArgs e)
        {
            StartPanel.Top -= 18;
            if (StartPanel.Location.Y == -159) StartPanelUpTimer.Enabled = false;
        }

        private void CustomizePanelDownTimer_Tick(object sender, EventArgs e)
        {
            CustomizePanel.Top += 9;
            if (CustomizePanel.Location.Y == 3) CustomizePanelDownTimer.Enabled = false;
        }

        private void CustomizePanelUpTimer_Tick(object sender, EventArgs e)
        {
            CustomizePanel.Top -= 18;
            if (CustomizePanel.Location.Y == -285) { CustomizePanelUpTimer.Enabled = false; }
            //Wait 400 miliseconds once finished.
        }
        
        #endregion

        #region Download1

        private async Task Download(string url, string dir, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs progress)
                {
                    switch (BlurPicture.Visible)
                    {
                        case true: BlurLabel.Text = "Descargando Kavremover... " + progress.ProgressPercentage + "%"; break;
                        case false: /*Show output progress*/ break;
                    }
                };
                await client.DownloadFileTaskAsync(url, dir + fileName);
            }
        }
        #endregion

        #region BlurSettings
        private void BlurSettings(string Text, System.Drawing.Image Picture)
        {
            BlurPicture.Image = Picture;
            BlurLabel.BackColor = Color.White;
            BlurLabel.Text = Text;
            BlurPicture.BringToFront();
            BlurLabel.BringToFront();
            BlurLabel.Visible = true;
            BlurPicture.Visible = true;
        }
        #endregion

        #region Events
        private void StartButton_Click(object sender, EventArgs e) => StartButtonMethod();

        private void CustomizeButton_Click(object sender, EventArgs e) => CustomizeButtonMethod();

        private void HelpButton_Click(object sender, EventArgs e) => HelpButtonMethod();

        private void UninstallButton_Click(object sender, EventArgs e) => UninstallButtonMethod();

        private void RegistryButton_Click(object sender, EventArgs e) => RegistryButtonMethod();

        private void DownloadButton_Click(object sender, EventArgs e) => DownloadButtonMethod();

        private void LicenseButton_Click(object sender, EventArgs e) => LicenseButtonMethod();

        private void KAVButton_Click(object sender, EventArgs e) => KAVButtonMethod();

        private void KISButton_Click(object sender, EventArgs e) => KISButtonMethod();

        private void KTSButton_Click(object sender, EventArgs e) => KTSButtonMethod();

        private void FAQ1Button_Click(object sender, EventArgs e) => FAQ1ButtonMethod();

        private void FAQ2Button_Click(object sender, EventArgs e) => FAQ2ButtonMethod();

        private void FAQ3Button_Click(object sender, EventArgs e) => FAQ3ButtonMethod();

        private void FAQ4Button_Click(object sender, EventArgs e) => FAQ4ButtonMethod();

        private void FAQ5Button_Click(object sender, EventArgs e) => FAQ5ButtonMethod();

        private void FAQ6Button_Click(object sender, EventArgs e) => FAQ6ButtonMethod();

        private void FAQ3Link_Click(object sender, EventArgs e) => Process.Start("http://tiny.cc/KCI-VirusTotal");

        private void FAQ6Link_Click(object sender, EventArgs e) => Process.Start("http://tiny.cc/KCI-YoutubeChannel");

        private void FAQBackButton_Click(object sender, EventArgs e) => HelpPanelReset();

        private void RestartButton_Click(object sender, EventArgs e) => RestartButtonMethod();

        private void KCI_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void LeftPanel_MouseClick(object sender, MouseEventArgs e) => HidePanelsMethod();

        private void TitleLabel_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void DescriptionLabel_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void StartButtonDisabled_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void OutputTextbox_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void OutputTextbox_Enter(object sender, EventArgs e) => ActiveControl = BlurPicture;

        private void WaitEnterTextbox_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) BlurPicture.Visible = false; BlurLabel.Visible = false; return; }
        #endregion

        #region Variables
        private static string KCIDir { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        private static string TempDir { get; set; } = Path.GetTempPath();
        private int Step { get; set; }
        private string kasperskyEdition { get; set; }
        private string kasperskySetupFile { get; set; }
        private string kasperskyLicenseFile { get; set; }
        private string kasperskyDownloadUrl { get; set; }
        private string kasperskyLicenseUrl { get; set; }
        private string kasperskyWebsite { get; set; }
        private bool accountTypeException { get; set; }
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