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
    #region Variables
    public partial class KCI : Form
    {
        string KCIDir { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        string TempDir { get; set; } = Path.GetTempPath();
        int StartPanelMove { get; set; }
        int CustomizePanelMove { get; set; }
        int Step { get; set; }
        int Error { get; set; } = 99;
        int InitializeError { get; set; }
        #endregion

        #region Initialize
        public KCI()
        {
            InitializeComponent(); Directory.SetCurrentDirectory(TempDir);

            DownloadButton.Visible = false;
            LicenseButton.Visible = false;

            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                DisableMainButtons();
                OutputTextbox.SelectionColor = Color.Red;
                OutputTextbox.AppendText("*** Permisos de administrador requeridos.");
                InitializeError = 2;
                return;
            }
            try { using (var client = new WebClient()) client.OpenRead("https://www.google.com/"); }
            catch
            {
                StartButton.Visible = false;
                DownloadButton.Visible = false;
                LicenseButton.Visible = false;
                OutputTextbox.SelectionColor = Color.Red;
                OutputTextbox.AppendText("*** Conexión a internet requerida.");
                OutputTextbox.AppendText(Environment.NewLine);
                InitializeError = 1;
                return;
            }
            OutputTextbox.SelectionColor = Color.Silver;
            OutputTextbox.AppendText("Aquí se refleja el progreso de la instalación.");
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            return;
        }
        #endregion

        #region Timer
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (StartPanelMove == 1) { StartPanel.Top += 9; if (StartPanel.Location.Y == 3) { Timer1.Enabled = false; StartPanelMove = 2; return; } }
            if (StartPanelMove == 2) { StartPanel.Top -= 18; if (StartPanel.Location.Y == -159) { Timer1.Enabled = false; StartPanelMove = 0; return; } }
            if (CustomizePanelMove == 1) { CustomizePanel.Top += 9; if (CustomizePanel.Location.Y == 3) { Timer1.Enabled = false; CustomizePanelMove = 2; return; } }
            if (CustomizePanelMove == 2) { CustomizePanel.Top -= 18; if (CustomizePanel.Location.Y == -285) { Timer1.Enabled = false; CustomizePanelMove = 0; return; } }
            return;
        }
        #endregion

        #region MainButtons
        private async void StartButton_Click(object sender, EventArgs e)
        {
            DisableMainButtons(); DisableSecondaryButtons();
            StartPanelMove = 1; Timer1.Enabled = true;
            return;
        }

        private async void CustomizeButton_Click(object sender, EventArgs e)
        {
            DisableMainButtons(); DisableSecondaryButtons();
            CustomizePanelMove = 1; Timer1.Enabled = true;
            return;
        }
        #endregion

        #region CustomizeButtons
        private async void UninstallButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Step = 1; Timer1.Enabled = true;
            while (Timer1.Enabled == true) { await Task.Delay(50); }
            OutputTextbox.Text = null;
            await UNINSTALL();
            OutputTextbox.Text = null;
            EnableMainButtons(); EnableSecondaryButtons();
            return;
        }


        private async void RegistryButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Step = 2; Timer1.Enabled = true;
            while (Timer1.Enabled == true) { await Task.Delay(50); }
            OutputTextbox.Text = null;
            await REGISTRY();
            OutputTextbox.Text = null;
            EnableMainButtons(); EnableSecondaryButtons();
            return;
        }


        private async void DownloadButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Step = 3; Timer1.Enabled = true;
            while (Timer1.Enabled == true) { await Task.Delay(50); }
            StartPanelMove = 1; Timer1.Enabled = true;
            return;
        }


        private async void LicenseButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Step = 4; Timer1.Enabled = true;
            while (Timer1.Enabled == true) { await Task.Delay(50); }
            StartPanelMove = 1; Timer1.Enabled = true;
            return;
        }
        #endregion

        #region SecondaryButtons

        private async void HelpButton_Click(object sender, EventArgs e)
        {
            DisableMainButtons(); DisableSecondaryButtons();
            HelpPanel_Reset(); HelpPanel.Left = 277;
            return;
        }
        #endregion

        #region HelpPanel
        private async void HelpPanel_Reset()
        {
            FAQ1Description.Visible = false;
            FAQ2Description.Visible = false;
            FAQ3Description.Visible = false; FAQ3Link.Visible = false;
            FAQ4Description.Visible = false;
            FAQ5Description.Visible = false;
            FAQ6Description.Visible = false; FAQ6Link.Visible = false;
            FAQ7Description.Visible = false;
            FAQ1Button.Top = 4;
            FAQ2Button.Top = 28;
            FAQ3Button.Top = 52;
            FAQ4Button.Top = 76;
            FAQ5Button.Top = 100;
            FAQ6Button.Top = 124;
            FAQ7Button.Top = 148;
            FAQ1Button.Visible = true;
            FAQ2Button.Visible = true;
            FAQ3Button.Visible = true;
            FAQ4Button.Visible = true;
            FAQ5Button.Visible = true;
            FAQ6Button.Visible = true;
            FAQ7Button.Visible = true;
            FAQBackButton.Visible = false;
            return;
        }

        private void FAQ1Button_Click(object sender, EventArgs e)
        {
            if (FAQ1Description.Visible == true) { HelpPanel_Reset(); return; }
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ7Button.Visible = false;
            FAQ1Button.Top = 4;
            FAQ1Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ2Button_Click(object sender, EventArgs e)
        {
            if (FAQ2Description.Visible == true) { HelpPanel_Reset(); return; }
            FAQ1Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ7Button.Visible = false;
            FAQ2Button.Top = 4;
            FAQ2Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ3Button_Click(object sender, EventArgs e)
        {
            if (FAQ3Description.Visible == true) { HelpPanel_Reset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ7Button.Visible = false;
            FAQ3Button.Top = 4;
            FAQ3Description.Visible = true; FAQ3Link.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ4Button_Click(object sender, EventArgs e)
        {
            if (FAQ4Description.Visible == true) { HelpPanel_Reset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ7Button.Visible = false;
            FAQ4Button.Top = 4;
            FAQ4Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ5Button_Click(object sender, EventArgs e)
        {
            if (FAQ5Description.Visible == true) { HelpPanel_Reset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ7Button.Visible = false;
            FAQ5Button.Top = 4;
            FAQ5Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ6Button_Click(object sender, EventArgs e)
        {
            if (FAQ6Description.Visible == true) { HelpPanel_Reset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ7Button.Visible = false;
            FAQ6Button.Top = 4;
            FAQ6Description.Visible = true; FAQ6Link.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private void FAQ7Button_Click(object sender, EventArgs e)
        {
            if (FAQ7Description.Visible == true) { HelpPanel_Reset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            FAQ7Button.Top = 4;
            FAQ7Description.Visible = true;
            FAQBackButton.Visible = true;
            return;
        }

        private async void FAQ3Link_Click(object sender, EventArgs e)
        {
            try { Process.Start("www.google.es"); } catch { } return;
        }

        private async void FAQ6Link_Click(object sender, EventArgs e)
        {
            try { Process.Start("www.google.es"); } catch { } return;
        }

        private void FAQBackButton_Click(object sender, EventArgs e)
        {
            HelpPanel_Reset();
            FAQBackButton.Visible = false;
            return;
        }
        #endregion

        #region INITIATION
        private async Task INITIATION()
        {
            OutputTextbox.Text = null;
            OutputTextbox.AppendText("Kaspersky Custom Installer (C)2019");
            await Task.Delay(4000);
            OutputTextbox.Text = null;
            await Task.Delay(500);
            return;
        }
        #endregion

        #region UNINSTALL
        private async Task UNINSTALL()
        {
            OutputTextbox.AppendText("{Paso 1} Desinstalar Antivirus Manualmente");
            await Task.Delay(3000);
            BlurLabel.Image = Properties.Resources.BlurLabel1;
            BlurLabel.Text = "Pulsa ENTER cuando finalices la desinstalación de tu antivirus";
            BlurLabel.BringToFront();
            BlurLabel.Visible = true;
            await Task.Delay(100);
            if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\KasperskyLab") != null)
            {
                if (MessageBoxEx.Show(this, "Antivirus KasperskyLab detectado." + Environment.NewLine +  "¿Deseas utilizar KavRemover para realizar la desinstalación?" + Environment.NewLine + Environment.NewLine + "Más Información:" + Environment.NewLine + "Kavremover, software desarrollado oficialmente por la firma KasperskyLab, es la forma más rápida, limpia y segura de eliminar todo registro de Kaspersky almacenado en el sistema.", "Información", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (!File.Exists(TempDir + "\\kavremover.exe")) { try { using (var client = new WebClient()) { client.DownloadFile("http://media.kaspersky.com/utilities/ConsumerUtilities/kavremvr.exe", TempDir + "\\kavremover.exe"); } } catch { } }
                    try { Process.Start(TempDir + "\\kavremover.exe"); } catch { MessageBoxEx.Show(this, "No ha sido posible iniciar Kavremover.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            WaitEnterTextbox.Focus();
            while (BlurLabel.Visible == true) { await Task.Delay(50); }
            this.Focus();
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Green;
            OutputTextbox.AppendText(" √" + Console.Out.NewLine);
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(2000);
            Step = 0;
            return;
        }
        #endregion

        #region REGISTRY
        private async Task REGISTRY()
        {
            OutputTextbox.AppendText("{Paso 2} Editar Registro de Windows");
            if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\KasperskyLab") != null) { goto START; }
            if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\SystemCertificates\SPC\Certificates") == null) { goto DONE; }

        START:
            await Task.Delay(3000);
            try { Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\KasperskyLab"); } catch { }
            try { Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Cryptography\\RNG"); } catch { }
            try { Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\Microsoft\\SystemCertificates\\SPC\\Certificates"); } catch { }
            if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\KasperskyLab") != null) { goto ERROR; }
            if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\SystemCertificates\\SPC\\Certificates") != null) { goto ERROR; }
            goto DONE;
        ERROR:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Red;
            OutputTextbox.AppendText(" X");
            if (Step == 2) { Step = 0;  OutputTextbox.AppendText(Console.Out.NewLine); OutputTextbox.SelectionColor = Color.Red; }
            OutputTextbox.AppendText(Console.Out.NewLine + "*** Acceso Denegado.");
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(500);
            MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. El acceso al registro de windows ha sido denegado." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Desinstala manualmente tu antivirus y reinicia el sistema.", "Instalación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await Task.Delay(100);
            OutputTextbox.Text = null;
            EnableMainButtons(); EnableSecondaryButtons();
            Error = 1;
            return;
        DONE:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Green;
            OutputTextbox.AppendText(" √" + Environment.NewLine);
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(2000);
            return;
        }
        #endregion

        #region KAV
        private async void KAVButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Timer1.Enabled = true;
            while (Timer1.Enabled == true) { await Task.Delay(50); }
            if (Step == 3) { OutputTextbox.Text = null; goto DOWNLOAD; }
            if (Step == 4) { OutputTextbox.Text = null; goto LICENSE; }

            await INITIATION();
            await UNINSTALL();
            await REGISTRY(); if (Error == 1) { Error = 0; return; }

        DOWNLOAD:
            OutputTextbox.AppendText("{Paso 3} Descargar Kaspersky Antivirus");
            if (File.Exists(KCIDir + "\\{KAV}{ES}.exe")) { goto DONE; }
            await Task.Delay(3000);
            try { using (var client = new WebClient()) { client.DownloadFile("https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe", KCIDir + "\\{KAV}{ES}.exe"); } } catch { goto ERROR; }
            if (Step == 0) { try { Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + "{KAV}{ES}.exe", RegistryValueKind.String); } catch { } }
            goto DONE;
        ERROR:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Red;
            OutputTextbox.AppendText(" X");
            if (Step == 3) { OutputTextbox.AppendText(Console.Out.NewLine); OutputTextbox.SelectionColor = Color.Red; }
            OutputTextbox.AppendText(Console.Out.NewLine + "*** Descarga Fallida." + Console.Out.NewLine);
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(500);
            MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Descarga fallida." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Descarga Kaspersky Antivirus manualmente desde su web oficial." + Environment.NewLine + "Serás dirigido a ella al cerrar este dialogo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            try { Process.Start("https://www.kaspersky.com/downloads/thank-you/antivirus"); } catch { MessageBoxEx.Show(this, "No ha sido posible acceder a la web.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            await Task.Delay(100);
            if (Step == 3)
            {
                EnableMainButtons(); EnableSecondaryButtons();
                OutputTextbox.Text = null;
                Step = 0;
                return;
            }
            BlurLabel.Image = Properties.Resources.BlurLabel2;
            BlurLabel.Text = "Pulsa ENTER cuando finalices la descarga manual de Kaspersky Antivirus.";
            BlurLabel.BringToFront();
            BlurLabel.Visible = true;
            WaitEnterTextbox.Focus();
            while (BlurLabel.Visible == true) { await Task.Delay(50); }
            this.Focus();
            await Task.Delay(500);
            Error += 1;
            goto LICENSE;
        DONE:
            if (Step == 0 && Error == 0) { try { Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + "{KAV}{ES}.exe", RegistryValueKind.String); } catch { } }
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Green;
            OutputTextbox.AppendText(" √" + Console.Out.NewLine);
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(2000);
            if (Step == 3)
            {
                OutputTextbox.Text = null;
                EnableMainButtons(); EnableSecondaryButtons();
                Step = 0;
                return;
            }

        LICENSE:
            OutputTextbox.AppendText("{Paso 4} Generar Licencia de Registro");
            if (File.Exists(KCIDir + "\\{KAV} {Licencia}.txt")) { goto D0NE; }
            await Task.Delay(3000);
            try { using (var client = new WebClient()) { client.DownloadFile("https://www.tiny.cc/kcidatabasekavlicense", KCIDir + "\\{KAV} {Licencia}.txt"); } } catch { goto ERR0R; }
            goto D0NE;
        ERR0R:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Red;
            OutputTextbox.AppendText(" X");
            if (Step == 4) { OutputTextbox.AppendText(Console.Out.NewLine); OutputTextbox.SelectionColor = Color.Red; }
            OutputTextbox.AppendText(Console.Out.NewLine + "*** No existen licencias disponibles.");
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            if (Step == 4)
            {
                await Task.Delay(500);
                MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Licencia no generada." + Environment.NewLine + Environment.NewLine + "Más información: No existen licencias disponibles por el momento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Delay(100);
                EnableMainButtons(); EnableSecondaryButtons();
                OutputTextbox.Text = null;
                Step = 0;
                return;
            }
            await Task.Delay(3000);
            END();
            return;
        D0NE:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Green;
            OutputTextbox.AppendText(" √");
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(2000);
            if (Step == 4)
            {
                OutputTextbox.Text = null;
                EnableMainButtons(); EnableSecondaryButtons();
                Step = 0;
                return;
            }
            END();
            return;
        }
        #endregion

        #region KIS
        private async void KISButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Timer1.Enabled = true;
            while (Timer1.Enabled == true) { await Task.Delay(50); }
            if (Step == 3) { OutputTextbox.Text = null; goto DOWNLOAD; }
            if (Step == 4) { OutputTextbox.Text = null; goto LICENSE; }

            await INITIATION();
            await UNINSTALL();
            await REGISTRY(); if (Error == 1) { Error = 0; return; }

        DOWNLOAD:
            OutputTextbox.AppendText("{Paso 3} Descargar Kaspersky Internet Security");
            if (File.Exists(KCIDir + "\\{KIS}{ES}.exe")) { goto DONE; }
            await Task.Delay(3000);
            try { using (var client = new WebClient()) { client.DownloadFile("https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe", KCIDir + "\\{KIS}{ES}.exe"); } } catch { goto ERROR; }
            if (Step == 0) { try { Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + "{KIS}{ES}.exe", RegistryValueKind.String); } catch { } }
            goto DONE;
        ERROR:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Red;
            OutputTextbox.AppendText(" X");
            if (Step == 3) { OutputTextbox.AppendText(Console.Out.NewLine); OutputTextbox.SelectionColor = Color.Red; }
            OutputTextbox.AppendText(Console.Out.NewLine + "*** Descarga Fallida." + Console.Out.NewLine);
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(500);
            MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Descarga fallida." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Descarga Kaspersky Internet Security manualmente desde su web oficial." + Environment.NewLine + "Serás dirigido a ella al cerrar este dialogo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            try { Process.Start("https://www.kaspersky.com/downloads/thank-you/internet-security"); } catch { MessageBoxEx.Show(this, "No ha sido posible acceder a la web.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            await Task.Delay(100);
            if (Step == 3)
            {
                EnableMainButtons(); EnableSecondaryButtons();
                OutputTextbox.Text = null;
                Step = 0;
                return;
            }
            BlurLabel.Image = Properties.Resources.BlurLabel2;
            BlurLabel.Text = "Pulsa ENTER cuando finalices la descarga manual de Kaspersky Internet Security.";
            BlurLabel.BringToFront();
            BlurLabel.Visible = true;
            WaitEnterTextbox.Focus();
            while (BlurLabel.Visible == true) { await Task.Delay(50); }
            this.Focus();
            await Task.Delay(500);
            Error += 1;
            goto LICENSE;
        DONE:
            if (Step == 0 && Error == 0) { try { Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + "{KIS}{ES}.exe", RegistryValueKind.String); } catch { } }
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Green;
            OutputTextbox.AppendText(" √" + Console.Out.NewLine);
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(2000);
            if (Step == 3)
            {
                OutputTextbox.Text = null;
                EnableMainButtons(); EnableSecondaryButtons();
                Step = 0;
                return;
            }

        LICENSE:
            OutputTextbox.AppendText("{Paso 4} Generar Licencia de Registro");
            if (File.Exists(KCIDir + "\\{KIS} {Licencia}.txt")) { goto D0NE; }
            await Task.Delay(3000);
            try { using (var client = new WebClient()) { client.DownloadFile("https://www.tiny.cc/kcidatabasekislicense", KCIDir + "\\{KIS} {Licencia}.txt"); } } catch { goto ERR0R; }
            goto D0NE;
        ERR0R:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Red;
            OutputTextbox.AppendText(" X");
            if (Step == 4) { OutputTextbox.AppendText(Console.Out.NewLine); OutputTextbox.SelectionColor = Color.Red; }
            OutputTextbox.AppendText(Console.Out.NewLine + "*** Licencia No Generada.");
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            if (Step == 4)
            {
                await Task.Delay(500);
                MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Licencia no generada." + Environment.NewLine + Environment.NewLine + "Más información: No existen licencias disponibles por el momento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Delay(100);
                EnableMainButtons(); EnableSecondaryButtons();
                OutputTextbox.Text = null;
                Step = 0;
                return;
            }
            await Task.Delay(3000);
            END();
            return;
        D0NE:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Green;
            OutputTextbox.AppendText(" √");
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(2000);
            if (Step == 4)
            {
                OutputTextbox.Text = null;
                EnableMainButtons(); EnableSecondaryButtons();
                Step = 0;
                return;
            }
            END();
            return;
        }
        #endregion

        #region KTS
        private async void KTSButton_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Timer1.Enabled = true;
            while (Timer1.Enabled == true) { await Task.Delay(50); }
            if (Step == 3) { OutputTextbox.Text = null; goto DOWNLOAD; }
            if (Step == 4) { OutputTextbox.Text = null; goto LICENSE; }

            await INITIATION();
            await UNINSTALL();
            await REGISTRY(); if (Error == 1) { Error = 0; return; }

        DOWNLOAD:
            OutputTextbox.AppendText("{Paso 3} Descargar Kaspersky Total Security");
            if (File.Exists(KCIDir + "\\{KTS}{ES}.exe")) { goto DONE; }
            await Task.Delay(3000);
            try { using (var client = new WebClient()) { client.DownloadFile("https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe", KCIDir + "\\{KTS}{ES}.exe"); } } catch { goto ERROR; }
            if (Step == 0) { try { Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + "{KTS}{ES}.exe", RegistryValueKind.String); } catch { } }
            goto DONE;
        ERROR:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Red;
            OutputTextbox.AppendText(" X");
            if (Step == 3) { OutputTextbox.AppendText(Console.Out.NewLine); OutputTextbox.SelectionColor = Color.Red; }
            OutputTextbox.AppendText(Console.Out.NewLine + "*** Descarga Fallida." + Console.Out.NewLine);
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(500);
            MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Descarga fallida." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Descarga Kaspersky Total Security manualmente desde su web oficial." + Environment.NewLine + "Serás dirigido a ella al cerrar este dialogo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            try { Process.Start("https://www.kaspersky.com/downloads/thank-you/total-security"); } catch { MessageBoxEx.Show(this, "No ha sido posible acceder a la web.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            await Task.Delay(100);
            if (Step == 3)
            {
                EnableMainButtons(); EnableSecondaryButtons();
                OutputTextbox.Text = null;
                Step = 0;
                return;
            }
            BlurLabel.Image = Properties.Resources.BlurLabel2;
            BlurLabel.Text = "Pulsa ENTER cuando finalices la descarga manual de Kaspersky Total Security.";
            BlurLabel.BringToFront();
            BlurLabel.Visible = true;
            WaitEnterTextbox.Focus();
            while (BlurLabel.Visible == true) { await Task.Delay(50); }
            this.Focus();
            await Task.Delay(500);
            Error += 1;
            goto LICENSE;
        DONE:
            if (Step == 0 && Error == 0) { try { Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + "{KTS}{ES}.exe", RegistryValueKind.String); } catch { } }
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Green;
            OutputTextbox.AppendText(" √" + Console.Out.NewLine);
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(2000);
            if (Step == 3)
            {
                OutputTextbox.Text = null;
                EnableMainButtons(); EnableSecondaryButtons();
                Step = 0;
                return;
            }

        LICENSE:
            OutputTextbox.AppendText("{Paso 4} Generar Licencia de Registro");
            if (File.Exists(KCIDir + "\\{KTS} {Licencia}.txt")) { goto D0NE; }
            await Task.Delay(3000);
            try { using (var client = new WebClient()) { client.DownloadFile("https://www.tiny.cc/kcidatabasektslicense", KCIDir + "\\{KTS} {Licencia}.txt"); } } catch { goto ERR0R; }
            goto D0NE;
        ERR0R:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Red;
            OutputTextbox.AppendText(" X");
            if (Step == 4) { OutputTextbox.AppendText(Console.Out.NewLine); OutputTextbox.SelectionColor = Color.Red; }
            OutputTextbox.AppendText(Console.Out.NewLine + "*** No existen licencias disponibles.");
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            if (Step == 4)
            {
                await Task.Delay(500);
                MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Licencia no generada." + Environment.NewLine + Environment.NewLine + "Más información: No existen licencias disponibles por el momento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Delay(100);
                EnableMainButtons(); EnableSecondaryButtons();
                OutputTextbox.Text = null;
                Step = 0;
                return;
            }
            await Task.Delay(3000);
            END();
            return;
        D0NE:
            await Task.Delay(500);
            OutputTextbox.SelectionColor = Color.Green;
            OutputTextbox.AppendText(" √");
            OutputTextbox.SelectionColor = Color.FromArgb(64, 64, 64);
            await Task.Delay(2000);
            if (Step == 4)
            {
                OutputTextbox.Text = null;
                EnableMainButtons(); EnableSecondaryButtons();
                Step = 0;
                return;
            }
            END();
            return;
        }
        #endregion

        #region END
        private async Task END()
        {
            MessageBoxEx.Show(this, "Kaspersky reset finalizado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await Task.Delay(100);
            BlurLabel.Image = Properties.Resources.BlurLabel3;
            RestartButton.Location = new Point(228, 151);
            BlurLabel.Text = Environment.NewLine + Environment.NewLine + Environment.NewLine + "Debes reiniciar el sistema para continuar con la instalación";
            BlurLabel.BringToFront(); RestartButton.BringToFront();
            RestartButton.Visible = true; BlurLabel.Visible = true;
            RestartButton.Focus();
            return;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown.exe", "-r -t 00");
            this.Close();
            return;
        }
        #endregion

        #region EnableButtons
        private void EnableMainButtons()
        {
            if (InitializeError == 2) { return; }
            if (InitializeError == 1) { CustomizeButton.Visible = true; return; }
            StartButton.Visible = true;
            CustomizeButton.Visible = true;
            return;
        }

        private void EnableSecondaryButtons()
        {
            HelpButton.Visible = true;
            return;
        }
        #endregion

        #region DisableButtons
        private void DisableMainButtons()
        {
            StartButton.Visible = false;
            CustomizeButton.Visible = false;
            return;
        }

        private void DisableSecondaryButtons()
        {
            HelpButton.Visible = false;
            return;
        }
        #endregion

        #region Events
        private void KCI_Click(object sender, EventArgs e)
        {
            if (StartPanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (CustomizePanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
        }

        private void LeftPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (StartPanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (CustomizePanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            return;
        }

        private void TitleLabel_Click(object sender, EventArgs e)
        {
            if (StartPanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (CustomizePanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            return;
        }

        private void DescriptionLabel_Click(object sender, EventArgs e)
        {
            if (StartPanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (CustomizePanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            return;
        }

        private void StartButtonDisabled_Click(object sender, EventArgs e)
        {
            if (StartPanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (CustomizePanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            return;
        }

        private void OutputTextbox_Click(object sender, EventArgs e)
        {
            if (StartPanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (CustomizePanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; Step = 0; EnableMainButtons(); EnableSecondaryButtons(); return; }
            return;
        }

        private void OutputTextbox_Enter(object sender, EventArgs e)
        {
            ActiveControl = BlurLabel;
            return;
        }

        private void WaitEnterTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { BlurLabel.Visible = false; return; }
            return;
        }
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