using System;
using System.Drawing;
using System.Windows.Forms;

namespace ToolsForKFIV
{
    /// <summary>A STATIC class used to store program settings</summary>
    public static class Settings
    {
        /// <summary>Defines the logging level.</summary>
        public static int LoggingLevel = 3;  //0 = Exception Only, 1 = Ex and Error, 2 = Ex, Error and Warning, 3 = Everything

        public static Color mtBgCC = Color.FromArgb(255, 40, 22, 46);
        public static Color mtXAxC = Color.FromArgb(255, 244, 67, 54); //X-Axis Colour
        public static Color mtYAxC = Color.FromArgb(255, 3, 169, 244); //Y-Axis Colour
        public static Color mtZAxC = Color.FromArgb(255, 76, 175, 80); //Y-Axis Colour

    }

    /// <summary>A STATIC class used to assist in logging</summary>
    public class Logger
    {
        public static void LogInfo(string msg)
        {
            if (Settings.LoggingLevel >= 3)
            {
                var dateTime = DateTime.Now;

                Console.Write("<[INFO][");
                Console.Write(dateTime.ToShortTimeString());
                Console.Write("]> ");
                Console.WriteLine(msg);
            }
        }
        public static void LogWarn(string msg)
        {
            if (Settings.LoggingLevel >= 2)
            {
                var dateTime = DateTime.Now;

                Console.Write("<[WARN][");
                Console.Write(dateTime.ToShortTimeString());
                Console.Write("]> ");
                Console.WriteLine(msg);
            }
        }

        public static void LogError(string msg)
        {
            if (Settings.LoggingLevel >= 1)
            {
                var dateTime = DateTime.Now;

                Console.Write("<[SHIT][");
                Console.Write(dateTime.ToShortTimeString());
                Console.Write("]> ");
                Console.WriteLine(msg);
            }
        }
    }

    /// <summary>Program Entry Point</summary>
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ResourceManager.Initialize(AppDomain.CurrentDomain.BaseDirectory);
            InputManager.Initialize();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
