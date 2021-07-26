using System;
using System.IO;

using KFIV.Format.TM2;

namespace TM2toPNG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TM2toPNG for King's Field IV (The Ancient City)\n");

            if (args.Length < 1)
            {
                Console.WriteLine("Usage:\nTM2toPNG.exe f.tm2\n");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            //Get Write Path
            string writePath = Path.GetDirectoryName(args[0]) + "\\";
            string writeName = Path.GetFileNameWithoutExtension(args[0]);

            //Open TM2
            Console.WriteLine("Reading TM2...");
            TM2 tm2 = TM2.FromFile(args[0]);

            if (tm2 == null)
            {
                Console.WriteLine("Failed to read TM2");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            //Save PNG
            Console.WriteLine("Writing PNG...");
            tm2.Save(writePath + writeName);


            //Finished
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
