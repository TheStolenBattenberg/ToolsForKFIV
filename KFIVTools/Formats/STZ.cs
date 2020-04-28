using System;
using System.Collections.Generic;
using System.IO;

using KFIV.Utility.IO;

namespace STA.Format.STZ
{
    public class STZ
    {
        public struct Header
        {
            public uint tag     { private set; get; }
            public uint length  { private set; get; }
            public uint numFile { private set; get; }
            public uint ukn0C   { private set; get; }

            public Header(InputStream ins)
            {
                tag     = ins.ReadUInt32();
                length  = ins.ReadUInt32();
                numFile = ins.ReadUInt32();
                ukn0C   = ins.ReadUInt32();
            }
        }
        public struct File
        {
            public uint tag   { private set; get; }
            public uint size  { private set; get; }
            public uint size2 { private set; get; }
            public uint size3 { private set; get; }
            public uint ukn10 { private set; get; }
            public uint ukn14 { private set; get; }
            public uint offset { private set; get; }
            public ushort ukn1C { private set; get; }
            public ushort ukn1E { private set; get; }

            public byte[] data { private set; get; }

            public File(InputStream ins)
            {
                tag = ins.ReadUInt32();
                size = ins.ReadUInt32();
                size2 = ins.ReadUInt32();
                size3 = ins.ReadUInt32();
                ukn10 = ins.ReadUInt32();
                ukn14 = ins.ReadUInt32();
                offset = ins.ReadUInt32();

                ukn1C = ins.ReadUInt16();
                ukn1E = ins.ReadUInt16();

                //Read file data
                ins.Jump(2048 * offset);
                {
                    data = ins.ReadBytes((int)size);
                }
                ins.Return();
            }


        }

        public STZ(string path)
        {
            //Open File
            InputStream ins = new InputStream(path);

            Header header = new Header(ins);

            //Read files
            List<File> files = new List<File>((int) header.numFile);
            for (uint i = 0; i < header.numFile; ++i)
                files.Add(new File(ins));

            //Save Files
            string outPath = Path.GetDirectoryName(path);

            uint fInd = 0;
            foreach(File f in files)
            {
                OutputStream.WriteFile(outPath + "\\File_" + fInd.ToString("D8"), f.data);

                fInd++;
            }
        }
    }
}
