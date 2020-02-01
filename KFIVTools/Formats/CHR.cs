using System.IO;

using KFIV.Utility.IO;

namespace KFIV.Format.CHR
{
    public class CHR
    {
        #region Format Types
        public struct Header
        {
            public uint ukn00  { private set; get; }
            public uint offOM2 { private set; get; }
            public uint offMIX { private set; get; }
            public uint offTX2 { private set; get; }
            public uint offOMD { private set; get; }
            public uint offHD  { private set; get; }
            public uint offBD  { private set; get; }
            public uint lenBD  { private set; get; }

            public void Read(InputStream ins)
            {
                ukn00  = ins.ReadUInt32();
                offOM2 = ins.ReadUInt32();
                offMIX = ins.ReadUInt32();
                offTX2 = ins.ReadUInt32();
                offOMD = ins.ReadUInt32();
                offHD  = ins.ReadUInt32();
                offBD  = ins.ReadUInt32();
                lenBD  = ins.ReadUInt32();
            }
        }

        #endregion
        #region Data
        private byte[] OM2;
        private byte[] MIX;
        private byte[] TX2;
        private byte[] OMD;
        private byte[] HD;
        private byte[] BD;

        #endregion

        public static CHR FromFile(string path)
        {
            CHR chr = new CHR();

            using(InputStream ins = new InputStream(path))
            {
                //Read Header
                Header head = new Header();
                head.Read(ins);

                //Read Files
                uint length = 0;

                //Read OM2
                ins.Seek(head.offOM2, SeekOrigin.Begin);
                length = ins.ReadUInt32();
                ins.Seek(head.offOM2, SeekOrigin.Begin);
                chr.OM2 = ins.ReadBytes((int) length);

                //Read MIX
                ins.Seek(head.offMIX, SeekOrigin.Begin);
                length = ins.ReadUInt32();
                ins.Seek(head.offMIX, SeekOrigin.Begin);
                chr.MIX = ins.ReadBytes((int)length);

                //Read TX2
                ins.Seek(head.offTX2, SeekOrigin.Begin);
                length = ins.ReadUInt32();
                ins.Seek(head.offTX2, SeekOrigin.Begin);
                chr.TX2 = ins.ReadBytes((int)length);

                //Read OMD
                ins.Seek(head.offOMD, SeekOrigin.Begin);
                length = ins.ReadUInt32();
                ins.Seek(head.offOMD, SeekOrigin.Begin);
                chr.OMD = ins.ReadBytes((int)length);

                //Read HD
                ins.Seek(head.offHD+0x1C, SeekOrigin.Begin);
                length = ins.ReadUInt32();
                ins.Seek(head.offHD, SeekOrigin.Begin);
                chr.HD = ins.ReadBytes((int)length);

                //Read BD
                ins.Seek(head.offBD, SeekOrigin.Begin);
                chr.BD = ins.ReadBytes((int) head.lenBD);
            }

            return chr;
        }

        public void Save(string path)
        {
            OutputStream.WriteFile(path + ".om2", OM2);
            OutputStream.WriteFile(path + ".mix", MIX);
            OutputStream.WriteFile(path + ".tx2", TX2);
            OutputStream.WriteFile(path + ".omd", OMD);
            OutputStream.WriteFile(path + ".hd",  HD);
            OutputStream.WriteFile(path + ".bd",  BD);
        }
    }
}
