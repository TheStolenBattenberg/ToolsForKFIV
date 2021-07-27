using System.IO;
using System.Text;
using System.Collections.Generic;

using KFIV.Utility.Type;
using KFIV.Utility.Math;
using KFIV.Utility.Charset;

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
        public long Size()
        {
            return base.BaseStream.Length;
        }

        //Extension: King's Field String
        public string ReadKFStringLength(int length)
        {
            //Use a string builder purely 'cause the code looks cleaner
            StringBuilder kfString = new StringBuilder();

            //Loop until length
            for (int i = 0; i < length; ++i)
                kfString.Append(CharacterSet.ConvertCharacter(base.ReadUInt16()));

            return kfString.ToString();
        }
        public string ReadKFStringTerminated()
        {
            StringBuilder kfString = new StringBuilder();
            ushort currentChar = 0x0000;
            int i = 0;

            while (currentChar != 0xFFFF)
            {
                //Some logic for line termination
                currentChar = base.ReadUInt16();
                if (currentChar == 0xFFFF)
                    break;

                kfString.Append(CharacterSet.ConvertCharacter(currentChar));

                i++;
            }

            return kfString.ToString();
        }

        //Extension: PS2 Types
        public byte[] ReadBytesBetter(uint numByte)
        {
            byte[] buffer = new byte[numByte];

            for(uint i = 0; i < numByte; ++i)
                buffer[i] = base.ReadByte();

            return buffer;
        }
        public float ReadFixed16(float div)
        {
            return base.ReadInt16() / div;
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
        public DMAPacket ReadDMAPacket()
        {
            DMAPacket gif = new DMAPacket();
            gif.data = base.ReadBytes(8);
            gif.tag = base.ReadInt64();

            return gif;
        }

        //Extension: Vector
        public Vector3 ReadVector3s()
        {
            Vector3 vec = Vector3.Zero;

            vec.x = base.ReadSingle();
            vec.y = base.ReadSingle();
            vec.z = base.ReadSingle();

            return vec;
        }
        public Vector3 ReadVector3h(float div)
        {
            Vector3 vec = Vector3.Zero;

            vec.x = ReadFixed16(div);
            vec.y = ReadFixed16(div);
            vec.z = ReadFixed16(div);

            return vec;
        }
        public Vector4 ReadVector4s()
        {
            Vector4 vec = Vector4.Zero;

            vec.x = base.ReadSingle();
            vec.y = base.ReadSingle();
            vec.z = base.ReadSingle();
            vec.w = base.ReadSingle();

            return vec;
        }
        public Vector4 ReadVector4h(float div)
        {
            Vector4 vec = Vector4.Zero;

            vec.x = ReadFixed16(div);
            vec.y = ReadFixed16(div);
            vec.z = ReadFixed16(div);
            vec.w = ReadFixed16(div);

            return vec;
        }


    }
}
