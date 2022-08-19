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
            if (Process.GetProcesses().Count(process => process.ProcessName == Process.GetCurrentProcess().ProcessName) > 1) { MessageBox.Show("Kaspersky Custom Installer no responde.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); Environment.Exit(0); }
            InitializeComponent(); Directory.SetCurrentDirectory(TempDir);
            //if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            //{
            //    StartButtonDisable();
            //    UninstallButton.Enabled = false; UninstallButton.Image = Properties.Resources.UninstallButtonDisabled;
            //    RegistryButton.Enabled = false; RegistryButton.Image = Properties.Resources.RegistryButtonDisabled;
            //    ErrorLabel.Visible = true;
            //}
        }
        #endregion

        #region MainButtons
        private void StartButtonMethod()
        {
            StartButtonDisable(); CustomizeButtonDisable(); HelpButtonDisable();
            StartPanelDownTimer.Enabled = true;
        }

        private void CustomizeButtonMethod()
        {
            customizeDownload = customizeLicense = false;
            StartButtonDisable(); CustomizeButtonDisable(); HelpButtonDisable();
            CustomizePanelDownTimer.Enabled = true;
        }
        private void HelpButtonMethod() => Process.Start("https://www.tiny.cc/KCI-YoutubeChannel");
        #endregion

        #region CustomizeButtons
        private async void UninstallButtonMethod()
        {
            CustomizePanelUpTimer.Enabled = true;
            GIF.Visible = true;
            try { await Uninstall(); } catch { }
            GIF.Visible = false;
            ProgressLabel.Text = "Progreso de las funciones";
            StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
        }


        private async  void RegistryButtonMethod()
        {
            CustomizePanelUpTimer.Enabled = true;
            GIF.Visible = true;
            try { await Registry(); } catch { }
            GIF.Visible = false;
            ProgressLabel.Text = "Progreso de las funciones";
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

        #region KAV
        private void KAV()
        {
            kasperskyEdition = "Kaspersky Antivirus";
            kasperskySetupFile = "KAV Setup.exe";
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
            kasperskySetupFile = "KIS Setup.exe";
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
            kasperskySetupFile = "KTS Setup.exe";
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
            if (customizeDownload)
            {
                await DownloadKav();
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                ProgressLabel.Text = "Progreso de las funciones";
                GIF.Visible = false;
                return;
            }
            if (customizeLicense)
            {
                await License();
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                ProgressLabel.Text = "Progreso de las funciones";
                GIF.Visible = false;
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
            if (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
            {
                foreach (string AVPkey in LocalMachine32View.OpenSubKey(@"SOFTWARE\KasperskyLab").GetSubKeyNames())
                {
                    if (AVPkey.Contains("AVP"))
                    {
                        kasperskyInstalledEdition = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductName").ToString();
                        kasperskyGUID = LocalMachine32View.OpenSubKey($@"SOFTWARE\KasperskyLab\{AVPkey}\environment").GetValue("ProductCode").ToString();
                        break;
                    }
                }
                
                ProgressLabel.Text = $"Iniciando desinstalación de {kasperskyInstalledEdition}...";
                await Task.Delay(1000);

                DialogResult answer = MessageBoxEx.Show(this, $"Antivirus {kasperskyInstalledEdition} detectado." + Environment.NewLine + Environment.NewLine + "Métodos de desinstalación:" + Environment.NewLine + "1. Desinstalación automática (debes cerrar Kaspersky). Pulsa SÍ." + Environment.NewLine + "2. Utilizando la herramienta KavRemover, creada oficialmente por la firma KasperskyLab, es la forma más eficaz y segura de desinstalar tu antivirus Kaspersky. Pulsa No." + Environment.NewLine + "3. Mediante el método habitual, utilizando el panel de control. Pulsa Cancelar.", "KCI", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                if (answer == DialogResult.Yes)
                {
                    ProgressLabel.Text = $"Esperando cierre de {kasperskyInstalledEdition}...";

                    if (Process.GetProcessesByName("avp").Length > 0)
                    {
                        MessageBoxEx.Show(this, $"Debes cerrar manualmente {kasperskyInstalledEdition} para continuar.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        while (Process.GetProcessesByName("avp").Length > 0) await Task.Delay(500);
                    }

                    ProgressLabel.Text = $"Desinstalando {kasperskyInstalledEdition}...";

                    if (kasperskyGUID == null)
                    {
                        MessageBox.Show("No ha sido posible realizar la desinstalación automáticamente. Prueba con otro método.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        throw new Exception();
                    }

                    Process.Start("msiexec", $"/x {kasperskyGUID} /quiet");
                }

                if (answer == DialogResult.No)
                {
                    try
                    {
                        await Download("http://media.kaspersky.com/utilities/ConsumerUtilities/kavremvr.exe", TempDir, "KavRemover.exe");
                        Process.Start(TempDir + "KavRemover.exe");
                    }
                    catch (WebException) { MessageBoxEx.Show(this, "No ha sido posible realizar la descarga. Comprueba tu conexión a internet e inténtalo de nuevo.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); throw new FileNotFoundException(); }
                    catch (Exception) { }

                    ProgressLabel.Text = $"Esperando desinstalación manual de {kasperskyInstalledEdition}...";
                }

                if (answer == DialogResult.Cancel)
                {
                    Process.Start(Path.Combine(System.Environment.SystemDirectory, "control"));
                    ProgressLabel.Text = $"Esperando desinstalación manual de {kasperskyInstalledEdition}...";
                }
                
                while (LocalMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null) await Task.Delay(500);
                try { File.Delete(TempDir + "KavRemover.exe"); } catch (Exception) { }
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
                //if (localMachine64View.OpenSubKey(@"SOFTWARE\KasperskyLab") != null)
                //    localMachine64View.DeleteSubKeyTree(@"SOFTWARE\KasperskyLab");
                //32Bits & 64Bits

                //if (localMachine64View.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography\RNG") != null)
                //    localMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Cryptography\RNG");

                LocalMachine64View.DeleteSubKeyTree(@"SOFTWARE\Microsoft\SystemCertificates\SPC\Certificates");
            }
            catch (UnauthorizedAccessException exception)
            {
                GIF.Enabled = false;
                MessageBoxEx.Show(this, "No ha sido posible editar el registro de Windows." + Environment.NewLine + "Reinicia la función de desinstalación. En caso de no detectar ningún producto de KasperskyLab, reinicia tu equipo y cierra cualquier anti-malware abierto.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StartButtonEnable(); CustomizeButtonEnable(); HelpButtonEnable();
                throw exception;
            }
            catch (ArgumentException) { }
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
                MessageBoxEx.Show(this, $"No ha sido posible realizar la descarga de {kasperskyEdition} Setup." + Environment.NewLine + "Comprueba tu conexión a internet y realiza la descarga manualmente desde la web oficial a la cual serás dirigido al cerrar este dialogo. Mientras tanto, KCI terminará el trabajo.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Process.Start(kasperskyWebsite);
                GIF.Enabled = true;
            }
            //if (!customizeDownload) { LocalMachine64View.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true).SetValue("Installer", KCIDir + kasperskySetupFile, RegistryValueKind.String); }
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
                        text.Write("Kaspersky Custom Installer (C)2020");
                        text.WriteLine(Environment.NewLine);
                        text.WriteLine("Kaspersky Internet Security licencia de activación (91 días):");
                        text.WriteLine("4CH4C-PPFDT-NFK4B-45R69");
                    }
                }
                else
                {
                    GIF.Enabled = false;
                    MessageBoxEx.Show(this, $"No existen licencias disponibles. Inténtalo más adelante.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    GIF.Enabled = true;
                }
            }
            try { File.Copy(KCIDir + kasperskyLicenseFile, TempDir + "kavlicense.txt", true); } catch (Exception) { }
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

        private void FAQ3Link_Click(object sender, EventArgs e) => Process.Start("http://tiny.cc/KCI-VirusTotal");

        private void FAQ6Link_Click(object sender, EventArgs e) => Process.Start("http://tiny.cc/KCI-YoutubeChannel");

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
        private RegistryKey LocalMachine32View { get; set; } = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
        private RegistryKey LocalMachine64View { get; set; } = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
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