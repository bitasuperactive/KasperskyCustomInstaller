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
using System.Reflection;

namespace KCI
{
    public partial class KCI : Form
    {
        #region Main
        public KCI()
        {
            if (Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1) { MessageBox.Show("Kaspersky Custom Installer no responde.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); this.Close(); }
            CheckRequirements();
            InitializeComponent();
            Directory.SetCurrentDirectory(tempDIR);
            //Update();
        }

        private void CheckRequirements()
        {
            DeleteOutput();
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                StartButtonDisable();
                RegistryButtonDisable();
                RedColor(); OutputTextbox.AppendText("*** Permisos de administrador requeridos." + Environment.NewLine);
            }
            try { using (var client = new WebClient()) client.OpenRead("https://www.google.com/"); }
            catch (WebException)
            {
                StartButtonDisable();
                UninstallButtonDisable();
                DownloadButtonDisable();
                LicenseButtonDisable();
                RedColor(); OutputTextbox.AppendText("*** Conexión a internet requerida.");
            }
            if (OutputTextbox.Text == string.Empty) { SilverColor(); OutputTextbox.AppendText("Aquí se refleja el progreso de la instalación."); }
        }

        private async void Update()
        {
            using (WebClient client = new WebClient()) { try { client.DownloadFile("https://www.tiny.cc/KCI-Version", tempDIR + "kciversion"); } catch (Exception) { return; } }
            string appCurrentVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            string appUpdateVersion = File.ReadAllText(tempDIR + "kciversion");
            if (double.Parse(appUpdateVersion) > double.Parse(appCurrentVersion))
            {
                if (MessageBox.Show($"Actualización disponible." + Environment.NewLine + $"Versión actual: {appCurrentVersion}" + Environment.NewLine + $"Versión disponible: {appUpdateVersion}" + Environment.NewLine + Environment.NewLine + $"¿Deseas descargar la versión {appUpdateVersion}?", "Información", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    Download("https://www.tiny.cc/KCI-Console", kciDIR, $"KCI Console {appUpdateVersion}.exe").Wait();
                    if (downloadEx)
                    {
                        RedColor(); OutputTextbox.AppendText("*** Actualización fallida.");
                        MessageBoxEx.Show("[Informe]" + Environment.NewLine + "No ha sido posible descargar la actualización." + Environment.NewLine + Environment.NewLine + "Descarga la última versión disponible manualmente desde el tutorial oficial de Kaspersky Custom Installer.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    MessageBoxEx.Show("Actualización descargada con éxito." + Environment.NewLine + Environment.NewLine + "Kaspersky Custom Installer se reiniciará abriendo la versión actualizada.", "KCI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    try { Process.Start(kciDIR + $"KCI {appUpdateVersion}.exe"); } catch (Exception) { MessageBox.Show($"No ha sido posible abrir \"KCI {appUpdateVersion}\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
                    this.Close();
                }
                else { return; }
            }
            //File.Delete(TempDir + "kciversion");
        }
        #endregion

        private async Task Download(string URL, string Dir, string FileName)
        {
            try
            {
                DownloadOutputLabel.Visible = true;
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs p)
                    {
                        switch (BlurPicture.Visible)
                        {
                            case true: BlurLabel.Text = "Descargando Kavremover... " + p.ProgressPercentage + "%"; break;
                            case false: DownloadOutputLabel.Text = "Descargando... " + p.ProgressPercentage + "%"; break;
                        }
                    };
                    await client.DownloadFileTaskAsync(URL, Dir + FileName);
                }
                downloadEx = false;
            }
            catch (WebException) { downloadEx = true; }
            finally { DownloadOutputLabel.Visible = false; DownloadOutputLabel.Text = string.Empty; }
        }

        #region OutputTextBox
        private void DeleteOutput() => OutputTextbox.Text = string.Empty;
        private void SilverColor() => OutputTextbox.SelectionColor = Color.Silver;
        private void GreenColor() => OutputTextbox.SelectionColor = Color.Green;
        private void RedColor() => OutputTextbox.SelectionColor = Color.Red;
        #endregion

        #region MainButtons
        private void StartButtonMethod()
        {
            if (!StartButton.Enabled) return;   //Bloquear StartButton error
            DisableMainButtons();
            Timer1Parameters(true, true, false);
            return;
        }

        private void CustomizeButtonMethod()
        {
            DisableMainButtons();
            Timer1Parameters(true, false, true);
            return;
        }

        private void HelpButtonMethod()
        {
            DisableMainButtons();
            FAQReset();
            FAQPanel.Left = 277;
            return;
        }

        private void RestartButtonMethod()
        {
            try { Process.Start("shutdown.exe", "-r -t 00"); } catch (Exception ex) { MessageBoxEx.Show(this, "No ha sido posible reiniciar tu sistema operativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            this.Close();
        }
        #endregion

        #region CustomizeButtons
        private async void UninstallButtonMethod(int ProcessID)
        {
            await Task.Delay(100);
            this.processID = ProcessID;
            await Timer1Parameters(true, false, false, true);     //Subir CustomizePanel y esperar a que termine
            MainProccess();
        }


        private async void RegistryButtonMethod(int ProcessID)
        {
            await Task.Delay(100);
            this.processID = ProcessID;
            await Timer1Parameters(true, false, false, true);     //Subir CustomizePanel y esperar a que termine
            MainProccess();
        }


        private async void DownloadButtonMethod(int ProcessID)
        {
            await Task.Delay(100);
            this.processID = ProcessID;
            await Timer1Parameters(true, false, false, true);     //Subir CustomizePanel y esperar a que termine
            Timer1Parameters(true, false, false);      //Bajar StartPanel
        }


        private async void LicenseButtonMethod(int ProcessID)
        {
            await Task.Delay(100);
            this.processID = ProcessID;
            await Timer1Parameters(true, false, false, true);     //Subir CustomizePanel y esperar a que termine
            Timer1Parameters(true, false, false);      //Subir StartPanel
        }
        #endregion

        #region FAQPanel
        private void FAQReset()
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
        }

        public void FAQShow(int FAQButton)
        {
            if (FAQ1Description.Visible || FAQ2Description.Visible || FAQ3Description.Visible || FAQ4Description.Visible || FAQ5Description.Visible || FAQ6Description.Visible) { FAQReset(); return; }
            FAQ1Button.Visible = false;
            FAQ2Button.Visible = false;
            FAQ3Button.Visible = false;
            FAQ4Button.Visible = false;
            FAQ5Button.Visible = false;
            FAQ6Button.Visible = false;
            switch (FAQButton)
            {
                case 1: FAQ1Button.Top = 4; FAQ1Button.Visible = true; FAQ1Description.Visible = true; break;
                case 2: FAQ2Button.Top = 4; FAQ2Button.Visible = true; FAQ2Description.Visible = true; break;
                case 3: FAQ3Button.Top = 4; FAQ3Button.Visible = true; FAQ3Description.Visible = true; FAQ3Link.Visible = true; break;
                case 4: FAQ4Button.Top = 4; FAQ4Button.Visible = true; FAQ4Description.Visible = true; break;
                case 5: FAQ5Button.Top = 4; FAQ5Button.Visible = true; FAQ5Description.Visible = true; break;
                case 6: FAQ6Button.Top = 4; FAQ6Button.Visible = true; FAQ6Description.Visible = true; FAQ6Link.Visible = true; break;
            }
            FAQBackButton.Visible = true;
        }
        #endregion

        #region KAV
        private async void KAVButtonMethod()
        {
            kasperskyEdition = "Kaspersky Antivirus";
            kasperskySetupName = "KAV(ES).exe";
            kasperskyLicenseName = "KAV Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/antivirus";

            await Timer1Parameters(true, false, false, true);

            MainProccess();
        }
        #endregion

        #region KIS
        private async void KISButtonMethod()
        {
            kasperskyEdition = "Kaspersky Internet Security";
            kasperskySetupName = "KIS(ES).exe";
            kasperskyLicenseName = "KIS Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/internet-security";

            await Timer1Parameters(true, false, false, true);

            MainProccess();
        }
        #endregion

        #region KTS
        private async void KTSButtonMethod()
        {
            kasperskyEdition = "Kaspersky Total Security";
            kasperskySetupName = "KTS(ES).exe";
            kasperskyLicenseName = "KTS Licencia.txt";
            kasperskyDownloadUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe";
            kasperskyLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
            kasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/total-security";

            await Timer1Parameters(true, false, false, true);

            MainProccess();
        }
        #endregion

        #region MainProccess
        private async void MainProccess()
        {
            switch (processID)
            {
                case 1: DeleteOutput(); Uninstall(); return;
                case 2: DeleteOutput(); Registry(); return;
                case 3: DeleteOutput(); Setup(); return;
                case 4: DeleteOutput(); License(); return;
            }
            
            await Presentation();
            await Uninstall(); if (exception == 1) { exception = 0; return; }
            await Registry(); if (exception == 1) { exception = 0; return; }
            await Setup();
            await License();
            Closure();
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
            BlurSettings("Pulsa ENTER cuando finalices la desinstalación de tu antivirus.", Properties.Resources.BlurPicture1);
            //Kaspersky Remover
            if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\KasperskyLab") != null)
            {
                if (MessageBoxEx.Show(this, "Antivirus KasperskyLab detectado." + Environment.NewLine + "¿Deseas utilizar KavRemover para realizar la desinstalación?" + Environment.NewLine + Environment.NewLine + "Más Información:" + Environment.NewLine + "Kavremover es una herramienta opcional desarrollada oficialmente por la firma KasperskyLab, siendo la forma más eficaz de desinstalar tu antivirus Kaspersky.", "Información", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                        try
                        {
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadProgressChanged += DownloadProgress;
                                await client.DownloadFileTaskAsync("http://media.kaspersky.com/utilities/ConsumerUtilities/kavremvr.exe", tempDIR + "kavremover.exe");
                                BlurLabel.Text = "Pulsa ENTER cuando finalices la desinstalación de tu antivirus.";
                            }
                        }
                        catch (Exception ex) {  }
                    try { Process.Start(tempDIR + "kavremover.exe"); } catch (Exception ex) { MessageBoxEx.Show(this, "No ha sido posible iniciar Kavremover.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            WaitEnterTextbox.Enabled = true; WaitEnterTextbox.Focus();
            while (WaitEnterTextbox.Enabled) await Task.Delay(25);
            if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\KasperskyLab") != null)
            {
                RedColor(); OutputTextbox.AppendText("X" + Console.Out.NewLine + Console.Out.NewLine + "*** Desinstalación no completada.");
                await Task.Delay(500);
                MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Desinstalación anti-malware no completada." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Verifica haber desinstalado correctamente tu antivirus utilizando Kavremover o Panel de Control, y reinicia tu sistema operativo.", "Instalación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Delay(100);
                CheckRequirements();
                EnableMainButtons();
                exception = 1;
                processID = 0;
                return;
            }
            await Task.Delay(500);
            GreenColor(); OutputTextbox.AppendText("√" + Console.Out.NewLine);
            await Task.Delay(2000);
            if (processID != 0) { EnableMainButtons(); CheckRequirements(); processID = 0; }
            return;
        }
        #endregion

        #region Registry
        private async Task Registry()
        {
            OutputTextbox.AppendText("[Paso 2] Editar Registro de Windows ");
            await Task.Delay(3000);
            try
            {
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\KasperskyLab");
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Cryptography\\RNG");
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SOFTWARE\\Microsoft\\SystemCertificates\\SPC\\Certificates");
            }
            catch (UnauthorizedAccessException ex)
            {
                await Task.Delay(500);
                RedColor(); OutputTextbox.AppendText("X");
                if (processID == 2) { processID = 0; OutputTextbox.AppendText(Console.Out.NewLine); }
                RedColor(); OutputTextbox.AppendText(Console.Out.NewLine + "*** Acceso Denegado.");
                await Task.Delay(500);
                MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Acceso al registro de windows denegado." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Verifica tus privilegios como administrador, y reinicia tu sistema operativo para completar la desinstalación de tu antivirus.", "Instalación interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Delay(100);
                DeleteOutput();
                EnableMainButtons();
                exception = 1;
                return;
            } catch (Exception ex) { }
            await Task.Delay(500);
            GreenColor(); OutputTextbox.AppendText("√" + Console.Out.NewLine);
            await Task.Delay(2000);
            if (processID != 0) { EnableMainButtons(); CheckRequirements(); processID = 0; }
            return;
        }
        #endregion

        #region Setup
        private async Task Setup()
        {
            OutputTextbox.AppendText($"[Paso 3] Descargar {kasperskyEdition} ");
            await Task.Delay(1000);
            try
            {
                using (WebClient client = new WebClient())
                {
                    DownloadOutputLabel.Visible = true;
                    client.DownloadProgressChanged += DownloadProgress;
                    await client.DownloadFileTaskAsync(kasperskyDownloadUrl, kciDIR + kasperskySetupName);
                    DownloadOutputLabel.Visible = false; DownloadOutputLabel.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                try { File.Delete(kciDIR + kasperskySetupName); } catch { }
                await Task.Delay(500);
                RedColor(); OutputTextbox.AppendText("X");
                if (processID == 3) OutputTextbox.AppendText(Console.Out.NewLine);
                RedColor(); OutputTextbox.AppendText(Console.Out.NewLine + "*** Descarga Fallida." + Console.Out.NewLine);
                await Task.Delay(500);
                MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Descarga fallida." + Environment.NewLine + Environment.NewLine + "Posibles soluciones: Descarga Kaspersky Antivirus manualmente desde su web oficial." + Environment.NewLine + "Serás dirigido a ella al cerrar este dialogo.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { Process.Start(kasperskyWebsite); } catch (Exception e) { MessageBoxEx.Show(this, "No ha sido posible acceder a la web.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                await Task.Delay(100);
                if (processID == 3)
                {
                    EnableMainButtons();
                    CheckRequirements();
                    processID = 0;
                    return;
                }
                BlurSettings($"Pulsa ENTER cuando finalices la descarga manual de {kasperskyEdition}.", Properties.Resources.BlurPicture2);
                WaitEnterTextbox.Enabled = true; WaitEnterTextbox.Focus();
                while (WaitEnterTextbox.Enabled) await Task.Delay(25);
                await Task.Delay(500);
                exception += 1;
                License();
                return;
            }
            if (processID == 0) { try { Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce", "Installer", kciDIR + kasperskySetupName, RegistryValueKind.String); } catch (Exception ex) { } }
            await Task.Delay(500);
            GreenColor(); OutputTextbox.AppendText("√" + Console.Out.NewLine);
            await Task.Delay(2000);
            if (processID != 0) { EnableMainButtons(); CheckRequirements(); processID = 0; }
            return;
        }
        #endregion

        #region License
        private async Task License()
        {
            OutputTextbox.AppendText("[Paso 4] Generar Licencia de Registro ");
            await Task.Delay(3000);
            try
            {
                using (WebClient client = new WebClient())
                {
                    DownloadOutputLabel.Visible = true;
                    client.DownloadProgressChanged += DownloadProgress;
                    await client.DownloadFileTaskAsync(kasperskyLicenseUrl, kciDIR + kasperskyLicenseName);
                    DownloadOutputLabel.Visible = false; DownloadOutputLabel.Text = null;
                }
            }
            catch (Exception ex)
            {
                try { File.Delete(kciDIR + kasperskyLicenseName); } catch { }
                DownloadOutputLabel.Visible = false;
                await Task.Delay(500);
                RedColor(); OutputTextbox.AppendText("X");
                if (processID == 4) OutputTextbox.AppendText(Console.Out.NewLine);
                RedColor(); OutputTextbox.AppendText(Console.Out.NewLine + "*** No existen licencias disponibles.");
                if (processID == 4)
                {
                    await Task.Delay(500);
                    MessageBoxEx.Show(this, "[Informe]" + Environment.NewLine + "1. Licencia no generada." + Environment.NewLine + Environment.NewLine + "Más información: No existen licencias disponibles por el momento.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    await Task.Delay(100);
                    EnableMainButtons();
                    CheckRequirements();
                    processID = 0;
                    return;
                }
                await Task.Delay(3000);
                return;
            }
            await Task.Delay(500);
            GreenColor(); OutputTextbox.AppendText("√" + Console.Out.NewLine);
            await Task.Delay(2000);
            if (processID != 0) { EnableMainButtons(); CheckRequirements(); processID = 0; }
            return;
        }
        #endregion

        #region Closure
        private async void Closure()
        {
            MessageBoxEx.Show(this, "Kaspersky reset finalizado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await Task.Delay(100);
            BlurSettings("Debes reiniciar el sistema para continuar con la instalación.", Properties.Resources.BlurPicture3);
            RestartButton.Location = new Point(181, 132); RestartButton.BringToFront(); RestartButton.Visible = true;
            return;
        }
        #endregion

        #region EnableMainButtons
        private void EnableMainButtons()
        {
            if (!executionLevelEx && !connectionEx) { StartButtonEnable(); }
            CustomizeButtonEnable();
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
            if (StartPanel.Location.Y == 3 || CustomizePanel.Location.Y == 3) { Timer1.Enabled = true; processID = 0; EnableMainButtons(); }
            if (FAQPanel.Location.X == 277) { FAQPanel.Left = -653; processID = 0; EnableMainButtons(); }
            return;
        }
        #endregion

        #region Timer1
        private async Task Timer1Parameters(bool Enabled, bool moveStartPanel, bool CustomizePanelMovement, bool Await = false)
        {
            Timer1.Enabled = Enabled;
            this.moveStartPanel = moveStartPanel;
            this.customizePanelMovement = CustomizePanelMovement;
            if (Await) while (Timer1.Enabled) await Task.Delay(25);
        }
        private void Timer1Method()
        {
            bool Show = false;

            if (moveStartPanel)
            {
                if (StartPanel.Location.Y == -159) Show = true;

                if (Show) StartPanel.Top += 9; else StartPanel.Top -= 18;

                if (StartPanel.Location.Y == 3 || StartPanel.Location.Y == -159) { Timer1Parameters(false, false, false); }
            }







            if (customizePanelMovement)
            {
                if (CustomizePanel.Location.Y == -285) Show = true;

                if (Show) CustomizePanel.Top += 9; else CustomizePanel.Top -= 18;

                if (CustomizePanel.Location.Y == 3 || CustomizePanel.Location.Y == -285) { Timer1Parameters(false, false, false); }
            }
        }
        #endregion

        #region DownloadProgress
        void DownloadProgress(object sender, DownloadProgressChangedEventArgs p)
        {
            switch (BlurPicture.Visible)
            {
                case true: BlurLabel.Text = "Descargando Kavremover... " + p.ProgressPercentage + "%"; break;
                case false: DownloadOutputLabel.Text = "Descargando... " + p.ProgressPercentage + "%" ; break;
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

        private void UninstallButton_Click(object sender, EventArgs e) => UninstallButtonMethod(1);

        private void RegistryButton_Click(object sender, EventArgs e) => RegistryButtonMethod(2);

        private void DownloadButton_Click(object sender, EventArgs e) => DownloadButtonMethod(3);

        private void LicenseButton_Click(object sender, EventArgs e) => LicenseButtonMethod(4);

        private void KAVButton_Click(object sender, EventArgs e) => KAVButtonMethod();

        private void KISButton_Click(object sender, EventArgs e) => KISButtonMethod();

        private void KTSButton_Click(object sender, EventArgs e) => KTSButtonMethod();

        private void FAQ1Button_Click(object sender, EventArgs e) => FAQShow(1);

        private void FAQ2Button_Click(object sender, EventArgs e) => FAQShow(2);

        private void FAQ3Button_Click(object sender, EventArgs e) => FAQShow(3);

        private void FAQ4Button_Click(object sender, EventArgs e) => FAQShow(4);

        private void FAQ5Button_Click(object sender, EventArgs e) => FAQShow(5);

        private void FAQ6Button_Click(object sender, EventArgs e) => FAQShow(6);

        private void FAQ3Link_Click(object sender, EventArgs e) { try { Process.Start("http://tiny.cc/KCI-VirusTotal"); } catch { } }

        private void FAQ6Link_Click(object sender, EventArgs e) { try { Process.Start("http://tiny.cc/KCI-YoutubeChannel"); } catch { } }

        private void FAQBackButton_Click(object sender, EventArgs e) => FAQReset();

        private void RestartButton_Click(object sender, EventArgs e) => RestartButtonMethod();

        private void Timer1_Tick(object sender, EventArgs e) => Timer1Method();

        private void KCI_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void LeftPanel_MouseClick(object sender, MouseEventArgs e) => HidePanelsMethod();

        private void TitleLabel_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void DescriptionLabel_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void StartButtonDisabled_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void OutputTextbox_Click(object sender, EventArgs e) => HidePanelsMethod();

        private void OutputTextbox_Enter(object sender, EventArgs e) => ActiveControl = BlurPicture;

        private void WaitEnterTextbox_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { BlurPicture.Visible = false; BlurLabel.Visible = false; WaitEnterTextbox.Enabled = false; } }
        #endregion

        #region Variables
        private string kciDIR { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        private string tempDIR { get; set; } = Path.GetTempPath();
        private bool moveStartPanel { get; set; }
        private bool customizePanelMovement { get; set; }
        private int processID { get; set; }
        private int exception { get; set; }
        private bool executionLevelEx { get; set; }
        private bool connectionEx { get; set; }
        private int downloadValue { get; set; }
        private bool downloadEx { get; set; }
        private string kasperskyEdition { get; set; }
        private string kasperskySetupName { get; set; }
        private string kasperskyLicenseName { get; set; }
        private string kasperskyDownloadUrl { get; set; }
        private string kasperskyLicenseUrl { get; set; }
        private string kasperskyWebsite { get; set; }
        private string outputText { get; set; }
        #endregion
    }

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