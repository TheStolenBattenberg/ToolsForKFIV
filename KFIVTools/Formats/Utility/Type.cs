using KFIV.Utility.Maths;

namespace KFIV.Utility.Type
{
    public struct DMAPacket
    {
        public byte[] data { set; get; }
        public long   tag  { set; get; }
    }
    public struct DMAHeader
    {
        public byte ukn0C { set; get; }
        public byte ukn0D { set; get; }
        public byte size  { set; get; }
        public byte ukn0F { set; get; }
    }
}
