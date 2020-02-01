using System.IO;

using KFIV.Utility.IO;

namespace KFIV.Format.CHR
{
    public class ARM
    {
        #region Format Types
        public struct Header
        {
            public uint lenTX2 { private set; get; }
            public uint lenOM2 { private set; get; }
            public uint lenMIX { private set; get; }
            public uint ukn0C { private set; get; }
            public uint ukn10 { private set; get; }
            public uint offTX2 { private set; get; }
            public uint offOM2 { private set; get; }
            public uint offMIX { private set; get; }
            public uint ukn20 { private set; get; }
            public uint ukn24 { private set; get; }
            public uint ukn28 { private set; get; }
            public uint ukn2C { private set; get; }
            public uint ukn30 { private set; get; }
            public uint ukn34 { private set; get; }
            public uint ukn38 { private set; get; }
            public uint ukn3C { private set; get; }

            public void Read(InputStream ins)
            {
                lenTX2 = ins.ReadUInt32();
                lenOM2 = ins.ReadUInt32();
                lenMIX = ins.ReadUInt32();
                ukn0C = ins.ReadUInt32();
                ukn10 = ins.ReadUInt32();
                offTX2 = ins.ReadUInt32();
                offOM2 = ins.ReadUInt32();
                offMIX = ins.ReadUInt32();
                ukn20 = ins.ReadUInt32();
                ukn24 = ins.ReadUInt32();
                ukn28 = ins.ReadUInt32();
                ukn2C = ins.ReadUInt32();
                ukn30 = ins.ReadUInt32();
                ukn34 = ins.ReadUInt32();
                ukn38 = ins.ReadUInt32();
                ukn3C = ins.ReadUInt32();
            }
        }

        #endregion
    }
}
