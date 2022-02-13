using System;
using System.IO;

using KFIV.Format.TM2;
using KFIV.Utility.IO;

namespace TMXConv
{
    public struct TMXConvFlags
    {
        public bool writeAsTM2 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TMXConv for King's Field IV (The Ancient City)\n");

            if (args.Length < 1)
            {
                Console.WriteLine("Usage:\nTMXConv.exe f.tmx\n");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            //Get Write Path
            string writePath = Path.GetDirectoryName(args[0]) + "\\";
            string writeName = Path.GetFileNameWithoutExtension(args[0]);

            //Reading TMX Header
            using (InputStream ins = new InputStream(args[0]))
            {
                //Read Rough Header
                uint totalSize = ins.ReadUInt32();
                uint totalTM2s = ins.ReadUInt32();
                ins.ReadUInt32();
                ins.ReadUInt32();

                //Read Image Reference
                for (uint i = 0; i < totalTM2s; ++i)
                {

                    uint tm2Offset = ins.ReadUInt32();
                    ins.ReadUInt32();
                    ins.ReadUInt32();
                    ins.ReadUInt32();

                    ins.Jump(tm2Offset);
                    uint tm2Size = ins.ReadUInt32();
                    ins.Seek(-4, SeekOrigin.Current);

                    TM2 tm2 = TM2.FromMemory(ins.ReadBytes((int)tm2Size));
                    tm2.Save(writePath + writeName + "_" + i.ToString("D4") + ".png");

                    ins.Return();
                }
            }
        }
    }
}