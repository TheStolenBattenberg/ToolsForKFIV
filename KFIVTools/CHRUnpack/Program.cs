using System;
using System.IO;

using KFIV.Format.CHR;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("CHRUnpack for King's Field IV (The Ancient City)\n");

        if (args.Length < 1)
        {
            Console.WriteLine("Usage:\n\tCHRUnpack.exe f.chr\n");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Get path for extraction
        string unpackPath = Path.GetDirectoryName(args[0]) + "\\" + Path.GetFileNameWithoutExtension(args[0]) + "\\";
        string unpackName = Path.GetFileNameWithoutExtension(args[0]);
        Directory.CreateDirectory(unpackPath);

        //Read CHR
        Console.WriteLine("Unpacking...");
        CHR chr = CHR.FromFile(args[0]);

        //Save CHR data
        Console.WriteLine("Writing Files...");
        chr.Save(unpackPath + unpackName);

        //Finished
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
