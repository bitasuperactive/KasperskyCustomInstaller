using System;
using System.IO;
using System.Net;
using System.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Security.Principal;
using Console = Colorful.Console;
using Colorful;

namespace KCIConsola
{
    class Program
    {
        #region Main
        static void Main(string[] args)
        {
            Console.Title = "KCI";
            Console.SetWindowSize(80, 28);
            Console.SetBufferSize(80, 28);

            Program This = new Program();
            Directory.SetCurrentDirectory(This.TempDir);
            This.CheckRequirements();
            This.Menu();
        }

        private void CheckRequirements()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) executionLevelException = true;
            try { using (var client = new WebClient()) client.OpenRead("https://www.google.com/"); } catch (WebException) { connectionException = true; }
        }

        private void ConsoleReset()
        {
            Console.Clear();
            if (executionLevelException)
            {
                Console.BackgroundColor = Color.Crimson;
                Console.Write("Permisos de administrador requeridos.                                           ", Color.Black);
            }
            if (connectionException)
            {
                Console.BackgroundColor = Color.Crimson;
                Console.Write("Conexión a internet requerida.                                                  ", Color.Black);
            }
            Console.ResetColor();
        }

        private void Menu(int input = 0)
        {
            ConsoleReset();
            Newline();
            ColorMenu("             ------------------------------------------------------");
            ColorMenu("             - MENU :: KCI                                        -");
            ColorMenu("             -                                                    -");
            ColorMenu("             - {0} Iniciar Instalación                            -");
            ColorMenu("             - {1} [Paso 1] Desinstalar Antivirus                 -");
            ColorMenu("             - {2} [Paso 2] Corregir registros de Windows         -");
            ColorMenu("             - {3} [Paso 3] Descargar Kaspersky                   -");
            ColorMenu("             - {4} [Paso 4] Generar Licencia de Registro          -");
            ColorMenu("             - {9} Actualizar Kaspersky Custom Installer          -");
            ColorMenu("             ------------------------------------------------------");
            Newline();
            Console.Write("Selecciona la opción deasada: ", Color.White);

            try { input = int.Parse(Console.ReadLine()); } catch (Exception) { Menu(); }
            switch (input)
            {
                case 0: MainProcess(); break;
                case 1: Uninstall(1); break;
                case 2: Registry(2); if (!genericException) Console.ReadKey(); break;
                case 3: Setup(3); if (!DownloadException) Console.ReadKey(); break;
                case 4: License(4); if (!DownloadException) Console.ReadKey(); break;
                case 9: Update(); break;
            }
            Menu();
        }
        #endregion

        #region MainProcess
        private void MainProcess()
        {
            Console.Clear();
            Console.Write("Kaspersky Custom Installer (C)2020", Color.White); Newline(); Newline();

            Uninstall(0); if (genericException) return;
            Registry(0); if (genericException) return;
            Newline(); Newline();
            SecondMenu();
            Setup(0);
            License(0);
            Closure();
        }
        #endregion

        #region Uninstall
        private void Uninstall(int ProcessID)
        {
            Newline(); Newline();
            Console.Write("[Paso 1] ", Color.Gold); Console.WriteLine("Desinstalar antivirus manualmente.", Color.PaleGoldenrod);
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\KasperskyLab");
            if (key != null)
            {
                var msgbox = MessageBox.Show("Antivirus KasperskyLab detectado." + Environment.NewLine + "¿Deseas utilizar KavRemover para realizar la desinstalación?" + Environment.NewLine + Environment.NewLine + "Más Información:" + Environment.NewLine + "Kavremover es una herramienta opcional desarrollada oficialmente por la firma KasperskyLab, siendo la forma más eficaz de desinstalar tu antivirus Kaspersky.", "Información", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (msgbox == DialogResult.OK)
                {
                    Download("http://media.kaspersky.com/utilities/ConsumerUtilities/kavremvr.exe", TempDir, "kavremover.exe").Wait();
                    try { Process.Start(TempDir + "kavremover.exe"); } catch (Exception) { MessageBox.Show("No ha sido posible iniciar Kavremover.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            else { Process.Start(Environment.SystemDirectory + @"\appwiz.cpl"); }
            Console.Write("Pulsa cualquier tecla cuando finalices la desinstalación manual.", Color.LightGray);
            Console.ReadKey();
            if (key != null)
            {
                Newline(); Newline();
                ExceptionOutput("Desinstalación no completada.");
                MessageBox.Show("[Informe]" + Environment.NewLine + "Desinstalación de KasperskyLab no completada." + Environment.NewLine + Environment.NewLine + "Verifica haber desinstalado correctamente tu antivirus utilizando Kavremover o Panel de Control, y reinicia tu sistema operativo.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                genericException = true;
                return;
            }
            else { genericException = false; }
        }
        #endregion

        #region Registry
        private void Registry(int ProcessID)
        {
            Newline(); Newline();
            Console.Write("[Paso 2] ", Color.Gold); Console.WriteLine("Corregir registros de Windows.", Color.PaleGoldenrod);
            try
            {
                if (executionLevelException) { throw new UnauthorizedAccessException(); }
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).DeleteSubKeyTree("SOFTWARE\\KasperskyLab");
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).DeleteSubKeyTree("SOFTWARE\\Microsoft\\Cryptography\\RNG");
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).DeleteSubKeyTree("SOFTWARE\\Microsoft\\SystemCertificates\\SPC\\Certificates");
                genericException = false;
            }
            catch (UnauthorizedAccessException)
            {
                genericException = true;
                Newline();
                ExceptionOutput("Acceso denegado.");
                MessageBox.Show("[Informe]" + Environment.NewLine + "Acceso al registro de windows denegado." + Environment.NewLine + Environment.NewLine + "Verifica tus privilegios como administrador, y reinicia tu sistema operativo para completar la desinstalación de tu antivirus.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Console.Write("OK!", Color.SpringGreen);
        }
        #endregion

        #region Setup
        private void Setup(int ProcessID)
        {
            Newline(); Newline();
            Console.Write("[Paso 3] ", Color.Gold); Console.WriteLine("Descargar Kaspersky.", Color.PaleGoldenrod);
            if (ProcessID == 3) SecondMenu();
            Download(KasperskySetupUrl, KCIDir, KasperskySetupName).Wait();
            if (DownloadException)
            {
                //try { File.Delete(KCIDir + KasperskySetupName); } catch { }
                ExceptionOutput("Acceso web denegado.");
                MessageBox.Show("[Informe]" + Environment.NewLine + "Descarga fallida." + Environment.NewLine + Environment.NewLine + "Descarga Kaspersky Antivirus manualmente desde su web oficial." + Environment.NewLine + "Serás dirigido a ella al cerrar este dialogo.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { Process.Start(KasperskyWebsite); } catch (Exception) { MessageBox.Show("No ha sido posible acceder a la web.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                Newline(); Console.Write("Pulsa cualquier tecla cuando finalices la descarga manual.", Color.LightGray);
                Console.ReadKey();
                return;
            }
            Console.WriteLine("OK!               ", Color.SpringGreen);
        }
        #endregion

        #region License
        private void License(int ProcessID)
        {
            Newline(); Newline();
            Console.Write("[Paso 4] ", Color.Gold); Console.WriteLine("Generar Licencia de Registro.", Color.PaleGoldenrod);
            if (ProcessID == 4) SecondMenu();
            Download(KasperskyLicenseUrl, KCIDir, KasperskyLicenseName).Wait();
            if (DownloadException)
            {
                Download("https://www.tiny.cc/KCI-AllLicenses", KCIDir, "KasperskyLab Licencias.txt").Wait();
                if (DownloadException)
                {
                    //try { File.Delete(KCIDir + KasperskySetupName); } catch { }
                    ExceptionOutput("No existen licencias disponibles por el momento.");
                    Console.ReadKey();
                    return;
                }
            }
            Console.WriteLine("OK!               ", Color.SpringGreen);
        }
        #endregion

        #region Closure
        private void Closure()
        {
            try { RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce").SetValue("KasperskyLabInstaller", KCIDir + KasperskySetupName, RegistryValueKind.String); } catch (Exception) { }
            var msgbox = MessageBox.Show("Kaspersky reset finalizado con éxito." + Environment.NewLine + Environment.NewLine + "Pulsa OK para reiniciar tu sistema operativo y continuar con la instalación.", "Información", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (msgbox == DialogResult.OK)
            {
                try { Process.Start("shutdown.exe", "-r -t 00"); } catch (Exception) { MessageBox.Show("No ha sido posible reiniciar tu sistema operativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            }
            Environment.Exit(0);
        }
        #endregion

        #region Update
        private void Update()
        {

        }
        #endregion

        #region SecondMenu
        private void SecondMenu(int Option = 0)
        {
            Newline();
            ColorMenu("KasperskyLab Ediciones Disponibles");
            ColorMenu("{1} Kaspersky Antivirus              -   {Protección Básica}");
            ColorMenu("{2} Kaspersky Internet Security      -   {Protección Premium}");
            ColorMenu("{3} Kaspersky Total Security         -   {Protección Platinum}");
            Newline();
            Console.Write("Selecciona la edición deasada: ", Color.White);
        loop:
            try { Option = int.Parse(Console.ReadLine()); } catch (Exception) { goto loop; }
            switch (Option)
            {
                case 1:
                    KasperskyEdition = "Kaspersky Antivirus";
                    KasperskySetupName = "KAV(ES).exe";
                    KasperskyLicenseName = "KAV Licencia.txt";
                    KasperskySetupUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";
                    KasperskyLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
                    KasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/antivirus";
                    return;
                case 2:
                    KasperskyEdition = "Kaspersky Internet Security";
                    KasperskySetupName = "KIS(ES).exe";
                    KasperskyLicenseName = "KIS Licencia.txt";
                    KasperskySetupUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";
                    KasperskyLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
                    KasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/internet-security";
                    return;
                case 3:
                    KasperskyEdition = "Kaspersky Total Security";
                    KasperskySetupName = "KTS(ES).exe";
                    KasperskyLicenseName = "KTS Licencia.txt";
                    KasperskySetupUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe";
                    KasperskyLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
                    KasperskyWebsite = "https://www.kaspersky.com/downloads/thank-you/total-security";
                    return;
            }
            goto loop;
        }
        #endregion

        #region Methods
        private void ColorMenu(string Text)
        {
            StyleSheet styleSheet = new StyleSheet(Color.White);
            styleSheet.AddStyle("MENU :: KCI[a-z]*", Color.Cyan);
            styleSheet.AddStyle("0[a-z]*", Color.Cyan);
            styleSheet.AddStyle("1[a-z]*", Color.Cyan);
            styleSheet.AddStyle("2[a-z]*", Color.Cyan);
            styleSheet.AddStyle("3[a-z]*", Color.Cyan);
            styleSheet.AddStyle("4[a-z]*", Color.Cyan);
            styleSheet.AddStyle("9[a-z]*", Color.Cyan);
            styleSheet.AddStyle("-   {Protección Básica}[a-z]*", Color.LightGray);
            styleSheet.AddStyle("-   {Protección Premium}[a-z]*", Color.LightGray);
            styleSheet.AddStyle("-   {Protección Platinum}[a-z]*", Color.LightGray);
            styleSheet.AddStyle("KasperskyLab Ediciones Disponibles[a-z]*", Color.Cyan);
            Console.WriteLineStyled(Text, styleSheet);
        }
        private void ExceptionOutput(string Text)
        {
            Console.BackgroundColor = Color.Crimson;
            Console.Write(Text, Color.Black);
            Console.ResetColor();
        }
        private async Task Download(string URL, string Dir, string FileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs p) { int DownloadValue = p.ProgressPercentage; Console.Write($"\rDescargando...{DownloadValue}%", Color.LightGray); };
                    await client.DownloadFileTaskAsync(URL, Dir + FileName);
                }
                DownloadException = false;
            }
            catch (WebException) { DownloadException = true; }
            finally
            {
                Console.Write("\r", Console.WindowWidth);
            }
        }
        #endregion

        #region Variables
        public void ClearCurrentLine() => Console.Write("\r", Console.WindowWidth);
        private void Newline() => Console.Write(Environment.NewLine);
        private string KCIDir { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        private string TempDir { get; set; } = Path.GetTempPath();
        private bool executionLevelException { get; set; }
        private bool connectionException { get; set; }
        private bool genericException { get; set; }
        private bool DownloadException { get; set; }
        private string KasperskyEdition { get; set; }
        private string KasperskySetupName { get; set; }
        private string KasperskyLicenseName { get; set; }
        private string KasperskySetupUrl { get; set; }
        private string KasperskyLicenseUrl { get; set; }
        private string KasperskyWebsite { get; set; }
        #endregion
    }
}