using KFIV.Utility.Math;

namespace KFIV.Utility.Type
{
    public struct GIFPacket
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

    public struct Vector4
    { 
        public float x { set; get; }
        public float y { set; get; }
        public float z { set; get; }
        public float w { set; get; }

        public Vector3 AsVec3
        {
            get
            {
                return new Vector3(x, y, z);
            }
        }
    }
}
