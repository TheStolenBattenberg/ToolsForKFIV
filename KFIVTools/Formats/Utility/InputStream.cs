using System.IO;
using System.Collections.Generic;

using KFIV.Utility.Type;

namespace KFIV.Utility.IO
{
    public class InputStream : BinaryReader
    {
        public InputStream(byte[] memoryLocation) : base(new MemoryStream(memoryLocation))
        {
            offsetStack = new Stack<long>();
        }
        public InputStream(string path) : base(File.Open(path, FileMode.Open))
        {
            offsetStack = new Stack<long>();
        }

        //Extension: Hierarchical reading
        private Stack<long> offsetStack;
        public void Jump(long offset)
        {
            //Add BaseStream position to stack
            offsetStack.Push(base.BaseStream.Position);

            //Seek to offset
            base.BaseStream.Seek(offset, SeekOrigin.Begin);
        }
        public void Return()
        {
            if (offsetStack.Count < 1)
                return;

            base.BaseStream.Seek(offsetStack.Pop(), SeekOrigin.Begin);
        }

        //Extension: Direct seek & position
        public void Seek(long offset, SeekOrigin origin)
        {
            base.BaseStream.Seek(offset, origin);
        }
        public long Tell()
        {
            return base.BaseStream.Position;
        }

        //Extension: PS2 Types
        public float ReadFixed16()
        {
            return base.ReadInt16() / 4096f;
        }
        public GIFPacket ReadGIFPacket()
        {
            GIFPacket gif = new GIFPacket();
            gif.data = base.ReadBytes(8);
            gif.tag = base.ReadInt64();

            return gif;
        }
        public DMAHeader ReadDMAHeader()
        {
            DMAHeader dma = new DMAHeader();

            base.ReadBytes(12);

            dma.ukn0C = base.ReadByte();
            dma.ukn0D = base.ReadByte();
            dma.size = base.ReadByte();
            dma.ukn0F = base.ReadByte();

            return dma;
        }

        //Extension: Vector
        public Vector4 ReadVector4s()
        {
            Vector4 vec = new Vector4();
            vec.x = base.ReadSingle();
            vec.y = base.ReadSingle();
            vec.z = base.ReadSingle();
            vec.w = base.ReadSingle();

            return vec;
        }
        public Vector4 ReadVector4f()
        {
            Vector4 vec = new Vector4();
            vec.x = ReadFixed16();
            vec.y = ReadFixed16();
            vec.z = ReadFixed16();
            vec.w = ReadFixed16();

            return vec;
        }
    }
}
