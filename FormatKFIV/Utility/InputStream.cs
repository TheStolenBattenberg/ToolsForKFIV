using System;
using System.IO;
using System.Collections.Generic;

using FormatKFIV.TypePlayStation;

namespace FormatKFIV.Utility
{
    /// <summary>InputStream class provides a wrapper around BinaryReader, in order to provide extended binary IO functionality.</summary>
    /// 
    public class InputStream : BinaryReader
    {
        private Stack<long> SeekStack = new Stack<long>();

        #region Constructors
        /// <summary>An InputStream constructor</summary>
        /// <param name="buffer">A byte array you would like to read as a file.</param>
        /// 
        public InputStream(byte[] buffer) : base(new MemoryStream(buffer)) { }

        /// <summary>An InputStream constructor</summary>
        /// <param name="buffer">A byte array you would like to read as a file.</param>
        /// <param name="offset">Byte offset into the buffer.</param>
        /// <param name="length">Byte length of the desired stream.</param>
        public InputStream(byte[] buffer, int offset, int length) : base(new MemoryStream(buffer, offset, length)) { }

        /// <summary>An InputStream constructor</summary>
        /// <param name="filePath">A string containing the path to a file you would like to open for reading.</param>
        /// 
        public InputStream(string filePath) : base(File.OpenRead(filePath)) { }

        #endregion

        /// <summary>Jump to a location in the file by offset from start.</summary>
        /// <param name="offset">Offset to jump too.</param>
        public void Jump(long offset)
        {
            SeekStack.Push(BaseStream.Position);
            BaseStream.Position = offset;
        }
        public void Jump(long offset, SeekOrigin origin)
        {
            SeekStack.Push(BaseStream.Position);
            switch (origin)
            {
                case SeekOrigin.Begin:
                    BaseStream.Position = offset;
                    break;

                case SeekOrigin.Current:
                    BaseStream.Position = BaseStream.Position + offset;
                    break;

                case SeekOrigin.End:
                    BaseStream.Position = BaseStream.Length - offset;
                    break;
            }
        }

        /// <summary>Return to the last location at before a jump was performed.</summary>
        public void Return()
        {
            if (1 > SeekStack.Count)
            {
                throw new Exception("SeekStack does not contain a value to return to");
            }

            BaseStream.Position = SeekStack.Pop();
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            BaseStream.Seek(offset, origin);
        }

        /// <summary>Read fixed length string from stream.</summary>
        /// <param name="strLen">Length of the string to read.</param>
        /// <returns>The string with invalid characters trimmed.</returns>
        public string ReadFixedString(int strLen)
        {
            return new string(ReadChars(strLen)).TrimEnd('\0');
        }

        /// <summary>Read fixed length KFIVString from stream.</summary>
        /// <param name="length">Length of the string to read.</param>
        /// <returns>The string with invalid characters trimmed.</returns>
        public string ReadFixedKFIVString(int length)
        {
            //Read ushort buffer
            ushort[] buffer = new ushort[length];
            for(int i = 0; i < length; ++i)
                buffer[i] = ReadUInt16();

            //Convert String and return
            return KFIVString.GetKFIVString(buffer);
        }

        /// <summary>Read an 8 byte PS2 Structure Type</summary>
        /// <returns>Massive 'union' with all possible PS2 8 Byte Structure. Fuck C# amirite?</returns>
        public sceRegister8Byte ReadPS28ByteRegister()
        {
            sceRegister8Byte reg8Byte = new sceRegister8Byte();
            reg8Byte.data0_8 = ReadUInt64();

            return reg8Byte;
        }

        /// <summary>Read an 16 byte PS2 Structure Type</summary>
        /// <returns>Massive 'union' with all possible PS2 16 Byte Structure. Fuck C# amirite?</returns>
        public sceRegister16Byte ReadPS216ByteRegister()
        {
            sceRegister16Byte reg16Byte = new sceRegister16Byte();
            reg16Byte.data0_8 = ReadUInt64();
            reg16Byte.data8_8 = ReadUInt64();

            return reg16Byte;
        }

        /// <summary>Read a floating-point vector 3</summary>
        public Vector3f ReadVector3f()
        {
            float X = base.ReadSingle();
            float Y = base.ReadSingle();
            float Z = base.ReadSingle();

            return new Vector3f(X, Y, Z);
        }

        /// <summary>Read a floating-point vector 4</summary>
        public Vector4f ReadVector4f()
        {
            float X = base.ReadSingle();
            float Y = base.ReadSingle();
            float Z = base.ReadSingle();
            float W = base.ReadSingle();

            return new Vector4f(X, Y, Z, W);
        }

        /// <summary>Read a fixed-point vector 3, convert to a floating-point vector 3</summary>
        public Vector3f ReadVector3h(float dividor)
        {
            float X = base.ReadInt16() / dividor;
            float Y = base.ReadInt16() / dividor;
            float Z = base.ReadInt16() / dividor;

            return new Vector3f(X, Y, Z);
        }

        /// <summary>Read a fixed-point vector 4, convert to a floating-point vector 4</summary>
        public Vector4f ReadVector4h(float dividor)
        {
            float X = base.ReadInt16() / dividor;
            float Y = base.ReadInt16() / dividor;
            float Z = base.ReadInt16() / dividor;
            float W = base.ReadInt16() / dividor;

            return new Vector4f(X, Y, Z, W);
        }

        /// <summary>Read an amount of floats from the stream</summary>
        public float[] ReadSingles(int num)
        {
            float[] floatOut = new float[num];
            for(int i = 0; i < num; ++i)
            {
                floatOut[i] = ReadSingle();
            }

            return floatOut;
        }

        /// <summary>Length of the stream</summary>
        /// <returns>Length of stream (bytes)</returns>
        public long Length()
        {
            return BaseStream.Length;
        }

        /// <summary>Position of the stream</summary>
        /// <returns>Position of the stream</returns>
        public long Position()
        {
            return BaseStream.Position;
        }
    }
}
