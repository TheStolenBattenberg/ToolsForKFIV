using System;
using System.IO;

using KFIV.Format.MOD;

namespace MODUnpack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MODUnpack for King's Field IV (The Ancient City)\n");

            if (args.Length < 1)
            {
                Console.WriteLine("Usage:\nMODUnpack.exe f.mod\n");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            //Get Write Path
            string writePath = Path.GetDirectoryName(args[0]) + "\\";
            string writeName = Path.GetFileNameWithoutExtension(args[0]);

            //Open TX2
            Console.WriteLine("Reading MOD...");
            MOD mod = MOD.FromFile(args[0]);

            if (mod == null)
            {
                Console.WriteLine("Failed to read MOD");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            //Save OBJ
            Console.WriteLine("Writing OBJ...");
            mod.Save(writePath + writeName);


            //Finished
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
