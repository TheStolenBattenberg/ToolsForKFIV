using System;
using System.IO;

using KFIV.Format.MAP;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("MAPUnpack for King's Field IV (The Ancient City)\n");

        if (args.Length < 1)
        {
            Console.WriteLine("Usage:\n\tMAPUnpack.exe f.map\n");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        string unpackPath = Path.GetDirectoryName(args[0]) + "\\" + Path.GetFileNameWithoutExtension(args[0]) + "\\";
        Directory.CreateDirectory(unpackPath);
        
        //Read MAP
        Console.WriteLine("Unpacking...");
        MAP map = MAP.FromFile(args[0]);

        //Save MAP data
        Console.WriteLine("Writing Files...");
        map.Save(unpackPath);

        //Finished
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
