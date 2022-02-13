using System;
using System.IO;
using KFIV.Format.DAT1;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("DATUnpack for King's Field IV (The Ancient City)\n");

        if(args.Length < 1)
        {
            Console.WriteLine("Usage:\nDATUnpack.exe f.dat\n");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Get path for extraction
        string unpackPath = Path.GetDirectoryName(args[0]) + "\\" + Path.GetFileNameWithoutExtension(args[0]) + "\\";

        //Read DAT
        Console.WriteLine("Reading DAT...");
        DAT dat = DAT.FromFile(args[0]);

        if(dat == null)
        {
            Console.WriteLine("Failed to read DAT.");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Unpack DAT
        Console.WriteLine("Unpacking DAT...");
        for(uint i = 0; i < dat.Count; ++i)
            dat[i].Save(unpackPath);

        //Finished
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

}
