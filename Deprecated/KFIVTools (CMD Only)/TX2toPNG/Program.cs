using System;
using System.IO;

using KFIV.Format.TX2;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("TX2toPNG for King's Field IV (The Ancient City)\n");

        if (args.Length < 1)
        {
            Console.WriteLine("Usage:\nTX2toPNG.exe f.tx2\n");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Get Write Path
        string writePath = Path.GetDirectoryName(args[0]) + "\\";
        string writeName = Path.GetFileNameWithoutExtension(args[0]);

        //Open TX2
        Console.WriteLine("Reading TX2...");
        TX2 tx2 = TX2.FromFile(args[0]);

        if(tx2 == null)
        {
            Console.WriteLine("Failed to read TX2");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Save PNGs
        Console.WriteLine("Writing PNGs...");
        for (uint i = 0; i < tx2.Count; ++i)
            tx2[i].Save(writePath + writeName + "_" + i.ToString() + ".png");


        //Finished
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}