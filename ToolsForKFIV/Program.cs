using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using FormatKFIV.Utility;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ToolsForKFIV
{
    /// <summary>A STATIC class used to store program settings</summary>
    public class Settings
    {
        /// <summary>Defines the logging level.</summary>
        public int LoggingLevel = 3;  //0 = Exception Only, 1 = Ex and Error, 2 = Ex, Error and Warning, 3 = Everything

        public Colour mtBgCC = Colour.FromARGB(255, 40, 22, 46);
        public Colour mtXAxC = Colour.FromARGB(255, 244, 67, 54);
        public Colour mtYAxC = Colour.FromARGB(255, 3, 169, 244);
        public Colour mtZAxC = Colour.FromARGB(255, 76, 175, 80);
        public bool mtShowGridAxis = true;

        public static Settings LoadConfiguration()
        {
            Settings settings;

            string confYaml = "";

            if(!File.Exists("configuration.yaml"))
            {
                settings = new Settings();

                settings.SaveConfiguration();
                return settings;
            }

            using(StreamReader sr = new StreamReader(File.OpenRead("configuration.yaml")))
            {
                confYaml = sr.ReadToEnd();
            }

            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            settings = deserializer.Deserialize<Settings>(confYaml);

            return settings;
        }
        public void SaveConfiguration()
        {
            var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var confYaml = serializer.Serialize(this);
            
            using (StreamWriter sw = new StreamWriter(File.OpenWrite("configuration.yaml")))
            {
                sw.Write(confYaml);
            }
        }
    }

    /// <summary>A STATIC class used to assist in logging</summary>
    public class Logger
    {
        public static void LogInfo(string msg)
        {
            if (ResourceManager.settings.LoggingLevel >= 3)
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
            if (ResourceManager.settings.LoggingLevel >= 2)
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
            if (ResourceManager.settings.LoggingLevel >= 1)
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
