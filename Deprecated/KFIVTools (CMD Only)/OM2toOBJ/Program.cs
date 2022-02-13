using System;
using System.IO;

using KFIV.Format.OM2;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("OM2toOBJ for King's Field IV (The Ancient City)\n");

        if (args.Length < 1)
        {
            Console.WriteLine("Usage:\nOM2toOBJ.exe f.om2\n");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Get Write Path
        string writePath = Path.GetDirectoryName(args[0]) + "\\";
        string writeName = Path.GetFileNameWithoutExtension(args[0]);

        //Open OM2
        Console.WriteLine("Reading OM2...");
        OM2 om2 = OM2.FromFile(args[0]);

        if (om2 == null)
        {
            Console.WriteLine("Failed to read OM2");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Save Models
        Console.WriteLine("Writing Model...");
        om2.Save(writePath + writeName + ".obj");

        //Finished
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}