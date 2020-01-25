namespace KFIV.Utility.Type
{
    public struct GIFPacket
    {
        public byte[] data { set; get; }
        public long   tag  { set; get; }
    }
    public struct Vector4
    { 
        public float x { set; get; }
        public float y { set; get; }
        public float z { set; get; }
        public float w { set; get; }
    }
}
