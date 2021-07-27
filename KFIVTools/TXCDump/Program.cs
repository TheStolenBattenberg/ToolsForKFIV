using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using KFIV.Utility.IO;
using KFIV.Utility.Charset;

namespace TXCDump
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TXCDump for King's Field IV (The Ancient City)\n");

            if (args.Length < 1)
            {
                Console.WriteLine("Usage:\nTXCDump.exe f.txc\n");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            //Calculate output path/name
            string writePath = Path.GetDirectoryName(args[0]) + "\\";
            string writeName = Path.GetFileNameWithoutExtension(args[0]);

            //Write all text in the TXC file to a .TXT
            using (StreamWriter sr = new StreamWriter(File.Open(writePath+writeName+".txt", FileMode.Create)))
            {
                //Read all text from the TXC
                using (InputStream ins = new InputStream(args[0]))
                {
                    uint numStrPtr = ins.ReadUInt32();

                    //Go through each StrPtr and dump the contents to the txt file
                    for(uint i = 0; i < numStrPtr; ++i)
                    {
                        uint StrPtr = ins.ReadUInt32();

                        //They throw 0'd pointers in 'cause FromSoft like to be cunts.
                        if (StrPtr == 0)
                            continue;

                        //Beautiful Jump/Return IO logic making this ez
                        ins.Jump(StrPtr);
                        sr.Write("<[" + ins.Tell().ToString("X8") + "]> ");
                        sr.WriteLine(ins.ReadKFStringTerminated());
                        ins.Return();
                    }
                }
            }


            // Wait for program termination
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
