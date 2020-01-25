using System;
using System.IO;

using KFIV.Format.OMD;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("OMDtoOBJ for King's Field IV (The Ancient City)\n");

        if (args.Length < 1)
        {
            Console.WriteLine("Usage:\nOMDtoOBJ.exe f.omd\n");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        //Read File
        Console.WriteLine("Reading OMD...");
        OMD omd = OMD.FromFile(args[0]);

        if(omd == null)
        {
            Console.WriteLine("Failed to read OMD");
            Console.WriteLine("Press any key to exit...");

            Console.ReadKey();
            return;
        }

        //Export OBJ
        string writePath = Path.GetDirectoryName(args[0]) + "\\";
        string writeName = Path.GetFileNameWithoutExtension(args[0]);

        Console.WriteLine("Writing OBJ...");
        omd.Save(writePath + writeName + ".obj");


        //Finished
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
