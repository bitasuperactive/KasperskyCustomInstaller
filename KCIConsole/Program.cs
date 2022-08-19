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
using System.Reflection;
using System.Threading;

namespace KCIConsola
{
    class Program
    {
        #region Main
        static void Main(string[] args)
        {
            Console.Title = $"KCI {Assembly.GetEntryAssembly().GetName().Version.ToString()}";
            Console.SetWindowSize(71, 28);
            Console.SetBufferSize(71, 28);
            Directory.SetCurrentDirectory(TempDir);
            Update();
            CheckRequirements();
            MainMenu();
        }


        #region Update
        private static async void Update()
        {
            Console.Write("Buscando actualizaciones...", Color.White);
            using (WebClient client = new WebClient()) { try { client.DownloadFile("https://www.tiny.cc/KCI-Version", TempDir + "kciversion"); } catch (Exception) { return; } }
            ClearCurrentLine();
            /*
            if (DownloadException)
            {
                Newline(); Newline();
                ExceptionColor("No ha sido posible acceder al servidor.");
                MessageBox.Show("[Informe]" + Environment.NewLine + "No ha sido posible acceder al servidor." + Environment.NewLine + Environment.NewLine + "Descarga la última versión disponible manualmente desde el tutorial oficial de Kaspersky Custom Installer.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.ReadKey();
                return;
            }
            */
            string appCurrentVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            string appUpdateVersion = File.ReadAllText(TempDir + "kciversion");
            if (double.Parse(appUpdateVersion) > double.Parse(appCurrentVersion))
            {
                Console.Write("Actualización disponible.", Color.White);
                if (MessageBox.Show($"Actualización disponible." + Environment.NewLine + $"Versión actual: {appCurrentVersion}" + Environment.NewLine + $"Versión disponible: {appUpdateVersion}" + Environment.NewLine + Environment.NewLine + $"¿Deseas descargar la versión {appUpdateVersion}?", "Información", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    ClearCurrentLine();
                    Download("https://www.tiny.cc/KCI-Console", KCIDir, $"KCI Console {appUpdateVersion}.exe").Wait();
                    if (DownloadException)
                    {
                        Newline(); Newline();
                        ExceptionColor("Actualización fallida.");
                        MessageBox.Show("[Informe]" + Environment.NewLine + "No ha sido posible descargar la actualización." + Environment.NewLine + Environment.NewLine + "Descarga la última versión disponible manualmente desde el tutorial oficial de Kaspersky Custom Installer.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    Console.WriteLine("OK!               ", Color.SpringGreen);
                    Console.WriteLine("KCI Console se reiniciará abriendo la versión actualizada...");
                    Thread.Sleep(3000);
                    try { Process.Start(KCIDir + $"KCI Console {appUpdateVersion}.exe"); } catch (Exception) { MessageBox.Show($"No ha sido posible abrir \"KCI Console {appUpdateVersion}\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
                    Environment.Exit(0);
                }
                else { return; }
            }
            /*
            else
            {
                MessageBox.Show($"Kaspersky Custom Installer {currentAppVersion} es la última versión disponible.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            */
            //File.Delete(TempDir + "kciversion");
        }
        #endregion

        private static void CheckRequirements()
        {
            //if (Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1) { MessageBox.Show("Kaspersky Custom Installer ya se encuentra en ejecución.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); Environment.Exit(0); }
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) executionLevelException = true;
            try { using (var client = new WebClient()) client.OpenRead("https://www.google.com/"); } catch (WebException) { connectionException = true; }
        }

        private static void ConsoleReset()
        {
            Console.Clear();
            if (executionLevelException)
            {
                Console.BackgroundColor = Color.Crimson;
                Console.Write("Permisos de administrador requeridos.                                  ", Color.Black);
            }
            if (connectionException)
            {
                Console.BackgroundColor = Color.Crimson;
                Console.Write("Conexión a internet requerida.                                         ", Color.Black);
            }
            Console.ResetColor();
        }

        #region Menus
        private static void MainMenu(int input = 0)
        {
            ConsoleReset();
            Newline();
            ColorMenu("         ------------------------------------------------------");
            ColorMenu("         - Kaspersky Custom Installer                         -");
            ColorMenu("         -                                                    -");
            ColorMenu("         - (1) Iniciar Instalación                            -");
            ColorMenu("         - (2) Personalizar Instalación                       -");
            ColorMenu("         - (0) Opciones Adicionales                           -");
            ColorMenu("         ------------------------------------------------------");
            Newline();
            Console.Write("Selecciona la opción deasada: ", Color.White);

            try { input = int.Parse(Console.ReadLine()); } catch (Exception) { MainMenu(); }
            switch (input)
            {
                case 1: MainProcess(); break;
                case 2: CustomizeMenu(); break;
                case 0: OptionsMenu(); break;
            }
            MainMenu();
        }

        private static void CustomizeMenu(int input = 0)
        {
            ConsoleReset();
            Newline();
            ColorMenu("         ------------------------------------------------------");
            ColorMenu("         - Personalizar Instalación                           -");
            ColorMenu("         -                                                    -");
            ColorMenu("         - (1) Desinstalar Antivirus                          -");
            ColorMenu("         - (2) Corregir Registros de Windows                  -");
            ColorMenu("         - (3) Descargar KasperskyLab Antimalware             -");
            ColorMenu("         - (4) Descargar Licencia de Registro                 -");
            ColorMenu("         ------------------------------------------------------");
            Newline();
            Console.Write("Selecciona la opción deasada: ", Color.White);

            try { input = int.Parse(Console.ReadLine()); } catch (Exception) { MainMenu(); }
            switch (input)
            {
                case 1: Uninstall(1); break;
                case 2: Registry(2); if (!genericException) Console.ReadKey(); break;
                case 3: Setup(3); if (!DownloadException) Console.ReadKey(); break;
                case 4: License(4); if (!DownloadException) Console.ReadKey(); break;
            }
            MainMenu();
        }

        private static void OptionsMenu(int input = 0)
        {
            ConsoleReset();
            Newline();
            ColorMenu("         ------------------------------------------------------");
            ColorMenu("         - Opciones Adicionales                               -");
            ColorMenu("         -                                                    -");
            //ColorMenu("         - (1) Buscar actualizaciones                         -");
            ColorMenu("         - (1) Descargar Kaspersky Custom Installer GUI       -");
            ColorMenu("         - (2) Acceder al tutorial oficial de KCI             -");
            ColorMenu("         ------------------------------------------------------");
            Newline();
            Console.Write("Selecciona la opción deasada: ", Color.White);

            try { input = int.Parse(Console.ReadLine()); } catch (Exception) { MainMenu(); }
            switch (input)
            {
                //case 1: Update(); break;
                case 1:
                    Download("http://tiny.cc/KCI-GUI", KCIDir, "KCI GUI.exe").Wait();
                    if (DownloadException)
                    {
                        Newline(); Newline();
                        ExceptionColor("Descarga fallida.");
                        MessageBox.Show("[Informe]" + Environment.NewLine + "No ha sido posible realizar la descarga." + Environment.NewLine + Environment.NewLine + "Realiza la descarga manualmente desde el tutorial oficial de Kaspersky Custom Installer.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Console.WriteLine("OK!               ", Color.SpringGreen);
                    Console.ReadKey();
                    break;
                case 2: try { Process.Start("http://tiny.cc/KCI-YoutubeChannel"); } catch(WebException) { MessageBox.Show("No ha sido posible acceder al URL." + Environment.NewLine + "Busca manualmente en youtube \"bitasuperactive KCI tutorial\" para encontrar el vídeo tutorial oficial.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } break;
            }
            MainMenu();
        }
        #endregion
        #endregion

        #region MainProcess
        private static void MainProcess()
        {
            Console.Clear();
            Console.Write("Kaspersky Custom Installer (C)2020", Color.White);
            Newline(); Newline();

            Uninstall(0); if (genericException) return;
            Registry(0); if (genericException) return;
            Newline(); Newline();
            KasperskyMenu();
            Setup(0);
            License(0);
            Closure();
        }
        #endregion

        #region Uninstall
        private static void Uninstall(int ProcessID)
        {
            Newline(); Newline();
            Console.Write("[Paso 1] ", Color.Gold); Console.WriteLine("Desinstalar antivirus manualmente.", Color.PaleGoldenrod);
            if (RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\KasperskyLab") != null)
            {
                var msgbox = MessageBox.Show("KasperskyLab Antimalware detectado." + Environment.NewLine + "¿Deseas utilizar KavRemover para realizar la desinstalación?" + Environment.NewLine + Environment.NewLine + "Más Información:" + Environment.NewLine + "Kavremover es una herramienta opcional desarrollada oficialmente por la firma KasperskyLab, siendo la forma más eficaz de desinstalar tu antivirus Kaspersky.", "Información", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (msgbox == DialogResult.OK)
                {
                    Download("http://media.kaspersky.com/utilities/ConsumerUtilities/kavremvr.exe", TempDir, "kavremover.exe").Wait();
                    try { Process.Start(TempDir + "kavremover.exe"); } catch (Exception) { MessageBox.Show("No ha sido posible iniciar Kavremover.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            Console.WriteLine("Pulsa la tecla 'P' para abrir Panel de Control si es necesario.", Color.LightGray);
            Console.Write("Pulsa 'Enter' cuando finalices la desinstalación manual.", Color.LightGray);
            ConsoleKeyInfo cki;
            while (true)
            {
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.P) Process.Start(Environment.SystemDirectory + @"\appwiz.cpl");
                if (cki.Key == ConsoleKey.Enter) break;
            }
            if (RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\KasperskyLab") != null)
            {
                genericException = true;
                Newline(); Newline();
                ExceptionColor("Desinstalación no completada.");
                MessageBox.Show("[Informe]" + Environment.NewLine + "Desinstalación de KasperskyLab no completada." + Environment.NewLine + Environment.NewLine + "Verifica haber desinstalado correctamente tu antivirus utilizando Kavremover o Panel de Control, y reinicia tu sistema operativo.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else { genericException = false; }
        }
        #endregion

        #region Registry
        private static void Registry(int ProcessID)
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
                ExceptionColor("Acceso denegado.");
                MessageBox.Show("[Informe]" + Environment.NewLine + "Acceso al registro de windows denegado." + Environment.NewLine + Environment.NewLine + "Verifica tus privilegios como administrador y comprueba que no existe ningún software anti-malware, instalado en tu sistema operativo, que pueda bloquear el proceso." + Environment.NewLine + "Por último, reinicia tu sistema operativo para completar los cámbios realizados.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception) { }
            Console.Write("OK!", Color.SpringGreen);
        }
        #endregion

        #region Setup
        private static void Setup(int ProcessID)
        {
            Newline(); Newline();
            Console.Write("[Paso 3] ", Color.Gold); Console.WriteLine("Descargar Kaspersky.", Color.PaleGoldenrod);
            if (ProcessID == 3) KasperskyMenu();
            Download(KasperskySetupUrl, KCIDir, KasperskySetupName).Wait();
            if (DownloadException)
            {
                //try { File.Delete(KCIDir + KasperskySetupName); } catch { }
                ExceptionColor("Acceso web denegado.");
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
        private static void License(int ProcessID)
        {
            Newline(); Newline();
            Console.Write("[Paso 4] ", Color.Gold); Console.WriteLine("Descargar Licencia de Registro.", Color.PaleGoldenrod);
            if (ProcessID == 4) KasperskyMenu();
            Download(KasperskyLicenseUrl, KCIDir, KasperskyLicenseName).Wait();
            if (DownloadException)
            {
                Download("https://www.tiny.cc/KCI-AllLicenses", KCIDir, "KasperskyLab Licencias.txt").Wait();
                if (DownloadException)
                {
                    //try { File.Delete(KCIDir + KasperskySetupName); } catch { }
                    ExceptionColor("No existen licencias disponibles en este momento.");
                    Console.ReadKey();
                    return;
                }
            }
            Console.WriteLine("OK!               ", Color.SpringGreen);
        }
        #endregion

        #region Closure
        private static void Closure()
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

        #region KasperskyMenu
        private static void KasperskyMenu(int Option = 0)
        {
            Newline();
            ColorMenu("KasperskyLab Ediciones Disponibles");
            ColorMenu("(1) Kaspersky Antivirus          [Protección Básica]");
            ColorMenu("(2) Kaspersky Internet Security  [Protección Premium]");
            ColorMenu("(3) Kaspersky Total Security     [Protección Platinum]");
            ColorMenu("(0) Cancelar");
            Newline();
        loop:
            Console.Write("Selecciona la edición deasada: ", Color.White);
            try { Option = int.Parse(Console.ReadLine()); } catch (Exception) { goto loop; }
            switch (Option)
            {
                case 0: MainMenu(); break;
                case 1:
                    KasperskyEdition = "Kaspersky Antivirus";
                    KasperskySetupName = "KAV(ES).exe";
                    KasperskyLicenseName = "KAV Licencia.txt";
                    KasperskySetupUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kav2018/for_reg_es/startup.exe";
                    KasperskyLicenseUrl = "https://www.tiny.cc/KCI-KAV-License";
                    KasperskyWebsite = "https://www.kaspersky.es/downloads/thank-you/antivirus-free-trial";
                    return;
                case 2:
                    KasperskyEdition = "Kaspersky Internet Security";
                    KasperskySetupName = "KIS(ES).exe";
                    KasperskyLicenseName = "KIS Licencia.txt";
                    KasperskySetupUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kis2018/for_reg_es/startup.exe";
                    KasperskyLicenseUrl = "https://www.tiny.cc/KCI-KIS-License";
                    KasperskyWebsite = "https://www.kaspersky.es/downloads/thank-you/internet-security-free-trial";
                    return;
                case 3:
                    KasperskyEdition = "Kaspersky Total Security";
                    KasperskySetupName = "KTS(ES).exe";
                    KasperskyLicenseName = "KTS Licencia.txt";
                    KasperskySetupUrl = "https://products.s.kaspersky-labs.com/spanish/homeuser/kts2018/for_reg_es/startup.exe";
                    KasperskyLicenseUrl = "https://www.tiny.cc/KCI-KTS-License";
                    KasperskyWebsite = "https://www.kaspersky.es/downloads/thank-you/total-security-free-trial";
                    return;
            }
            goto loop;
        }
        #endregion

        #region Methods
        private static void ColorMenu(string Text)
        {
            StyleSheet styleSheet = new StyleSheet(Color.White);
            styleSheet.AddStyle("- Kaspersky Custom Installer                         -[a-z]*", Color.Cyan);
            styleSheet.AddStyle("- Personalizar Instalación                           -[a-z]*", Color.Cyan);
            styleSheet.AddStyle("- Opciones Adicionales                               -[a-z]*", Color.Cyan);
            styleSheet.AddStyle("0[a-z]*", Color.Cyan);
            styleSheet.AddStyle("1[a-z]*", Color.Cyan);
            styleSheet.AddStyle("2[a-z]*", Color.Cyan);
            styleSheet.AddStyle("3[a-z]*", Color.Cyan);
            styleSheet.AddStyle("4[a-z]*", Color.Cyan);
            styleSheet.AddStyle("5[a-z]*", Color.Cyan);
            styleSheet.AddStyle("Protección Básica[a-z]*", Color.LightGray);
            styleSheet.AddStyle("Protección Premium[a-z]*", Color.LightGray);
            styleSheet.AddStyle("Protección Platinum[a-z]*", Color.LightGray);
            styleSheet.AddStyle("KasperskyLab Ediciones Disponibles[a-z]*", Color.Cyan);
            Console.WriteLineStyled(Text, styleSheet);
        }
        private static void ExceptionColor(string Text)
        {
            Console.BackgroundColor = Color.Crimson;
            Console.Write(Text, Color.Black);
            Console.ResetColor();
        }
        private static async Task Download(string URL, string Dir, string FileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs p) { int DownloadValue = p.ProgressPercentage; Console.Write($"\rDescargando...{DownloadValue}%", Color.LightGray); };
                    await client.DownloadFileTaskAsync(URL, Dir + FileName);
                    //Finish WebClient once File Downloaded.
                }
                DownloadException = false;
            }
            catch (WebException) { DownloadException = true; }
            finally { ClearCurrentLine(); }
        }
        private static void Newline() => Console.Write(Environment.NewLine);
        private static void ClearCurrentLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
        #endregion

        #region Variables
        private static string KCIDir { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        private static string TempDir { get; set; } = Path.GetTempPath();
        private static bool executionLevelException { get; set; }
        private static bool connectionException { get; set; }
        private static bool genericException { get; set; }
        private static bool DownloadException { get; set; }
        private static string KasperskyEdition { get; set; }
        private static string KasperskySetupName { get; set; }
        private static string KasperskyLicenseName { get; set; }
        private static string KasperskySetupUrl { get; set; }
        private static string KasperskyLicenseUrl { get; set; }
        private static string KasperskyWebsite { get; set; }
        #endregion
    }
}