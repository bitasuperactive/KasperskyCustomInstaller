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
            //  Comprobar si KCI se encuentra en ejecución
            if (Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1) { MessageBox.Show("Kaspersky Custom Installer ya se encuentra en ejecución.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); this.Close(); }

            //  Iniciar Forma y establecer directorio de trabajo
            InitializeComponent();

            Directory.SetCurrentDirectory(TempDir);

            //  Comprobar privilegios de administrador
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) { MainError += 2; }

            //  Comprobar conexión a internet
            try { using (var client = new WebClient()) client.OpenRead("https://www.google.com/"); } catch(Exception ex) { MainError += 1; }

            //  Bloquear botones condicionalmente y definir Output
            switch (MainError)
            {
                case 3: StartButtonDisable(); CustomizeButtonDisable(); MainErrorOutput(); return;
                case 2: StartButtonDisable(); RegistryButtonDisable(); MainErrorOutput(); return;
                case 1: StartButtonDisable(); DownloadButtonDisable(); LicenseButtonDisable(); MainErrorOutput(); return;
            }

            SilverColor(); OutputTextbox.AppendText("Aquí se refleja el progreso de la instalación.");
        }
        #endregion

        #region OutputTextBox
        private void MainErrorOutput()
        {
            DeleteOutput(); RedColor();
            switch (MainError)
            {
                case 3: OutputTextbox.AppendText("*** Permisos de administrador requeridos." + Environment.NewLine + "*** Conexión a internet requerida."); break;
                case 2: OutputTextbox.AppendText("*** Permisos de administrador requeridos."); break;
                case 1: OutputTextbox.AppendText("*** Conexión a internet requerida."); break;
            } return;
        }

        //Métodos fuente
        private void DeleteOutput() => OutputTextbox.Text = null;
        private void SilverColor() => OutputTextbox.SelectionColor = Color.Silver;
        private void GreenColor() => OutputTextbox.SelectionColor = Color.Green;
        private void RedColor() => OutputTextbox.SelectionColor = Color.Red;
        #endregion

        #region MainButtons
        private void StartButtonMethod()
        {
            //  Bloquear StartButton error
            if (!StartButton.Enabled) return;
            //  Bajar StartPanel
            DisableMainButtons();
            StartPanelMove = 1;
            Timer1.Enabled = true;
            return;
        }

        private void CustomizeButtonMethod()
        {
            //  Deshabilitar MainButtons
            DisableMainButtons();
            //  Bajar StartPanel
            CustomizePanelMove = 1;
            Timer1.Enabled = true;
            return;
        }

        private void HelpButtonMethod()
        {
            //  Deshabilitar MainButtons
            DisableMainButtons();
            //  Desplazar y organizar HelpPanel
            HelpPanel_Reset();
            HelpPanel.Left = 277;
            return;
        }
        #endregion

        #region CustomizeButtons
        private async void UninstallButtonMethod()
        {
            //  Iniciar Timer para subir CustomizePanel y esperar a que termine
            await Task.Delay(100);
            Step = 1;
            Timer1.Enabled = true; while (Timer1.Enabled) await Task.Delay(25);
            //  Iniciar proceso de desinstalación
            DeleteOutput();
            await Uninstall();
            MainErrorOutput();
            EnableMainButtons();
            return;
        }


        private async void RegistryButtonMethod()
        {
            //  Iniciar Timer para subir CustomizePanel
            await Task.Delay(100);
            Step = 2;
            Timer1.Enabled = true; while (Timer1.Enabled) await Task.Delay(25);
            //  Iniciar eliminación de las claves regedit
            DeleteOutput();
            await Registry();
            MainErrorOutput();
            EnableMainButtons();
            return;
        }


        private async void DownloadButtonMethod()
        {
            //  Iniciar Timer para subir CustomizePanel
            await Task.Delay(100);
            Step = 3;
            Timer1.Enabled = true; while (Timer1.Enabled) await Task.Delay(25);
            //  Iniciar Timer para subir StartPanel
            StartPanelMove = 1; Timer1.Enabled = true;
            return;
        }


        private async void LicenseButtonMethod()
        {
            //  Iniciar Timer para subir CustomizePanel
            await Task.Delay(100);
            Step = 4;
            Timer1.Enabled = true; while (Timer1.Enabled) await Task.Delay(25);
            //  Iniciar Timer para subir StartPanel
            StartPanelMove = 1; Timer1.Enabled = true;
            return;
        }
        #endregion

        #region HelpPanel
        private void HelpPanel_Reset()
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
            if (FAQ1Description.Visible) { HelpPanel_Reset(); return; }
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
            if (FAQ2Description.Visible) { HelpPanel_Reset(); return; }
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
            if (FAQ3Description.Visible) { HelpPanel_Reset(); return; }
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
            if (FAQ4Description.Visible) { HelpPanel_Reset(); return; }
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
            if (FAQ5Description.Visible) { HelpPanel_Reset(); return; }
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
            if (FAQ6Description.Visible) { HelpPanel_Reset(); return; }
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

        private void FAQ3LinkMethod()
        {
            try { Process.Start("http://tiny.cc/KCI-VirusTotal"); } catch { }
            return;
        }

        private void FAQ6LinkMethod()
        {
            try { Process.Start("http://tiny.cc/KCI-YoutubeChannel"); } catch { }
            return;
        }

        private void FAQBackButtonMethod()
        {
            HelpPanel_Reset();
            FAQBackButton.Visible = false;
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
            Timer1.Enabled = true; while (Timer1.Enabled) await Task.Delay(25);
            if (Step == 3) { DeleteOutput(); Download(); return; }
            if (Step == 4) { DeleteOutput(); License(); return; }

            await Presentation();
            await Uninstall();
            await Registry(); if (Error == 1) { Error = 0; return; }
            await Download();
            await License();
            Closure();
            return;
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
            Timer1.Enabled = true; while (Timer1.Enabled) await Task.Delay(25);
            if (Step == 3) { DeleteOutput(); Download(); return; }
            if (Step == 4) { DeleteOutput(); License(); return; }

            await Presentation();
            await Uninstall();
            await Registry(); if (Error == 1) { Error = 0; return; }
            await Download();
            await License();
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
            Timer1.Enabled = true; while (Timer1.Enabled) await Task.Delay(25);
            if (Step == 3) { DeleteOutput(); Download(); return; }
            if (Step == 4) { DeleteOutput(); License(); return; }

            await Presentation();
            await Uninstall();
            await Registry(); if (Error == 1) { Error = 0; return; }
            await Download();
            await License();
            Closure();
            return;
        }
        #endregion

        #region Presentation
        private async Task Presentation()
        {
            DeleteOutput();
            OutputTextbox.AppendText("Kaspersky Custom Installer (C)2019-2020");
            await Task.Delay(4000);
            DeleteOutput();
            await Task.Delay(500);
            return;
        }
        #endregion

        #region Uninstall
        private async Task Uninstall()
        {
            OutputTextbox.AppendText("[Paso 1] Desinstalar Antivirus Manualmente ");
            await Task.Delay(3000);
            //Blur label
            BlurSettings("Pulsa ENTER cuando finalices la desinstalación de tu antivirus.", Properties.Resources.BlurPicture1);
            await Task.Delay(100);
            //Kaspersky Remover
            if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\KasperskyLab") != null)
            {
                if (MessageBoxEx.Show(this, "Antivirus KasperskyLab detectado." + Environment.NewLine + "¿Deseas utilizar KavRemover para realizar la desinstalación?" + Environment.NewLine + Environment.NewLine + "Más Información:" + Environment.NewLine + "Kavremover es una potente herramienta opcional, desarrollada oficialmente por la firma KasperskyLab, siendo la forma más rápida y limpia de desinstalar tu antivirus Kaspersky.", "Información", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (!File.Exists(TempDir + "kavremover.exe")) {
                        try { using (WebClient client = new WebClient()) {
                                client.DownloadProgressChanged += DownloadProgress;
                                await client.DownloadFileTaskAsync("http://media.kaspersky.com/utilities/ConsumerUtilities/kavremvr.exe", TempDir + "kavremover.exe");
                            } } catch(Exception ex) { } }
                    BlurLabel.Text = "Pulsa ENTER cuando finalices la desinstalación de tu antivirus.";
                    try { Process.Start(TempDir + "kavremover.exe"); } catch(Exception ex) { MessageBoxEx.Show(this, "No ha sido posible iniciar Kavremover.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            WaitEnterTextbox.Enabled = true; WaitEnterTextbox.Focus();
            while (BlurPicture.Visible) await Task.Delay(25);
            WaitEnterTextbox.Enabled = false;
            await Task.Delay(500);
            GreenColor(); OutputTextbox.AppendText("√" + Console.Out.NewLine);
            await Task.Delay(2000);
            Step = 0;
            return;
        }
        #endregion

        #region Registry
        private async Task Registry()
        {
            OutputTextbox.AppendText("[Paso 2] Editar Registro de Windows ");
            if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\KasperskyLab") == null && Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\SystemCertificates\\SPC\\Certificates") == null) goto DONE;
            await Task.Delay(3000);
            try {
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\KasperskyLab");
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Cryptography\\RNG");
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\Microsoft\\SystemCertificates\\SPC\\Certificates");
            } catch(UnauthorizedAccessException ex) { goto ERROR; }
        DONE:
            await Task.Delay(500);
            GreenColor(); OutputTextbox.AppendText("√" + Environment.NewLine);
            await Task.Delay(2000);
            return;
        ERROR:
            await Task.Delay(500);
            RedColor(); OutputTextbox.AppendText("X");
            if (Step == 2) { Step = 0; OutputTextbox.AppendText(Console.Out.NewLine); }
            RedColor(); OutputTextbox.AppendText(Console.Out.NewLine + "*** Acceso Denegado.");
            await Task.Delay(500);
            MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. El acceso al registro de windows ha sido denegado." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Desinstala manualmente tu antivirus y reinicia el sistema.", "Instalación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await Task.Delay(100);
            DeleteOutput();
            EnableMainButtons();
            Error = 1;
            return;
        }
        #endregion

        #region Download
        private async Task Download()
        {
            OutputTextbox.AppendText($"[Paso 3] Descargar {kasperskyEdition} ");
            if (File.Exists(KCIDir + kasperskySetupFile)) goto DONE;
            await Task.Delay(1000);
            try { using (WebClient client = new WebClient()) {
                    OutputText = OutputTextbox.Text;
                    client.DownloadProgressChanged += DownloadProgress;
                    await client.DownloadFileTaskAsync(kasperskyDownloadUrl, KCIDir + kasperskySetupFile);
                } } catch (Exception ex) { goto ERROR; }
            if (Step == 0) { try { Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + kasperskySetupFile, RegistryValueKind.String); } catch(Exception ex) { } }
        DONE:
            if (Step == 0 && Error == 0) { try { Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", KCIDir + kasperskySetupFile, RegistryValueKind.String); } catch { } }
            await Task.Delay(500);
            GreenColor(); OutputTextbox.AppendText("√" + Console.Out.NewLine);
            await Task.Delay(2000);
            if (Step == 3)
            {
                MainErrorOutput();
                EnableMainButtons();
                Step = 0;
            }
            return;
        ERROR:
            await Task.Delay(500);
            RedColor(); OutputTextbox.AppendText("X");
            if (Step == 3) { OutputTextbox.AppendText(Console.Out.NewLine); }
            RedColor(); OutputTextbox.AppendText(Console.Out.NewLine + "*** Descarga Fallida." + Console.Out.NewLine);
            await Task.Delay(500);
            MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Descarga fallida." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Descarga Kaspersky Antivirus manualmente desde su web oficial." + Environment.NewLine + "Serás dirigido a ella al cerrar este dialogo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            try { Process.Start(kasperskyWebsite); } catch(Exception ex) { MessageBoxEx.Show(this, "No ha sido posible acceder a la web.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            await Task.Delay(100);
            if (Step == 3)
            {
                EnableMainButtons();
                MainErrorOutput();
                Step = 0;
                return;
            }
            BlurSettings($"Pulsa ENTER cuando finalices la descarga manual de {kasperskyEdition}.", Properties.Resources.BlurPicture2);
            WaitEnterTextbox.Enabled = true; WaitEnterTextbox.Focus();
            while (BlurLabel.Visible) await Task.Delay(25);
            WaitEnterTextbox.Enabled = false;
            await Task.Delay(500);
            Error += 1;
            License();
            return;
        }
        #endregion

        #region License
        private async Task License()
        {
            OutputTextbox.AppendText("[Paso 4] Generar Licencia de Registro ");
            if (File.Exists(KCIDir + kasperskyLicenseFile)) goto DONE;
            await Task.Delay(3000);
            try { using (WebClient client = new WebClient()) { client.DownloadFile(kasperskyLicenseUrl, KCIDir + kasperskyLicenseFile); } } catch (Exception ex) { goto ERROR; }
        DONE:
            await Task.Delay(500);
            GreenColor(); OutputTextbox.AppendText("√");
            await Task.Delay(2000);
            if (Step == 4)
            {
                MainErrorOutput();
                EnableMainButtons();
                Step = 0;
            }
            return;
        ERROR:
            await Task.Delay(500);
            RedColor(); OutputTextbox.AppendText("X");
            if (Step == 4) { OutputTextbox.AppendText(Console.Out.NewLine); }
            RedColor(); OutputTextbox.AppendText(Console.Out.NewLine + "*** No existen licencias disponibles.");
            if (Step == 4)
            {
                await Task.Delay(500);
                MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Licencia no generada." + Environment.NewLine + Environment.NewLine + "Más información: No existen licencias disponibles por el momento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Delay(100);
                EnableMainButtons();
                MainErrorOutput();
                Step = 0;
                return;
            }
            await Task.Delay(3000);
            Closure();
            return;
        }
        #endregion

        #region Closure
        private async Task Closure()
        {
            MessageBoxEx.Show(this, "Kaspersky reset finalizado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await Task.Delay(100);
            RestartButton.Location = new Point(229, 153);
            BlurSettings("Debes reiniciar el sistema para continuar con la instalación.", Properties.Resources.BlurPicture3);
            return;
        }

        private void RestartButtonMethod()
        {
            try { Process.Start("shutdown.exe", "-r -t 00"); } catch(Exception ex) { MessageBoxEx.Show(this, "No ha sido posible reiniciar el sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            this.Close();
        }
        #endregion

        #region EnableMainButtons
        private void EnableMainButtons()
        {
            if (MainError == 0) { StartButtonEnable(); }
            if (MainError != 3) { CustomizeButtonEnable(); }
            HelpButtonEnable();
            return;
        }

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
        private void DisableMainButtons()
        {
            StartButtonDisable();
            CustomizeButtonDisable();
            HelpButtonDisable();
            return;
        }

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
            if (StartPanel.Location.Y == 3 || CustomizePanel.Location.Y == 3) { Timer1.Enabled = true; Step = 0; EnableMainButtons(); }
            if (HelpPanel.Location.X == 277) { HelpPanel.Left = -653; Step = 0; EnableMainButtons(); }
            return;
        }
        #endregion

        #region Timer1
        private void Timer1Method()
        {
            //  Bajar/subir StartPanel/CustomizePanel
            if (StartPanelMove == 1) { StartPanel.Top += 9; if (StartPanel.Location.Y == 3) { Timer1.Enabled = false; StartPanelMove = 2; return; } }
            if (StartPanelMove == 2) { StartPanel.Top -= 18; if (StartPanel.Location.Y == -159) { Timer1.Enabled = false; StartPanelMove = 0; return; } }
            if (CustomizePanelMove == 1) { CustomizePanel.Top += 9; if (CustomizePanel.Location.Y == 3) { Timer1.Enabled = false; CustomizePanelMove = 2; return; } }
            if (CustomizePanelMove == 2) { CustomizePanel.Top -= 18; if (CustomizePanel.Location.Y == -285) { Timer1.Enabled = false; CustomizePanelMove = 0; return; } }
        }
        #endregion

        #region DownloadProgress
        void DownloadProgress(object sender, DownloadProgressChangedEventArgs p)
        {
            DownloadProgressValue = p.ProgressPercentage;
            switch (BlurPicture.Visible)
            {
                case true: BlurLabel.Text = "Descargando Kavremover... " + DownloadProgressValue + "%"; break;
                case false: DeleteOutput(); OutputTextbox.AppendText(OutputText + DownloadProgressValue + "% "); break;
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

        private void FAQ3Link_Click(object sender, EventArgs e) => FAQ3LinkMethod();

        private void FAQ6Link_Click(object sender, EventArgs e) => FAQ6LinkMethod();

        private void FAQBackButton_Click(object sender, EventArgs e) => FAQBackButtonMethod();

        private void RestartButton_Click(object sender, EventArgs e) => RestartButtonMethod();

        private void Timer1_Tick(object sender, EventArgs e) => Timer1Method();

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
        string TestDir { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string KCIDir { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        string TempDir { get; set; } = Path.GetTempPath();
        int StartPanelMove { get; set; }
        int CustomizePanelMove { get; set; }
        int Step { get; set; }
        int Error { get; set; } = 99;
        int MainError { get; set; }
        int DownloadProgressValue { get; set; }
        string kasperskyEdition { get; set; }
        string kasperskySetupFile { get; set; }
        string kasperskyLicenseFile { get; set; }
        string kasperskyDownloadUrl { get; set; }
        string kasperskyLicenseUrl { get; set; }
        string kasperskyWebsite { get; set; }
        string OutputText { get; set; }

        private void BlurPicture_Click(object sender, EventArgs e)
        {

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