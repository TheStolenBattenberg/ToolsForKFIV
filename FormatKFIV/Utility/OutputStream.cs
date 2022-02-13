using System;
using System.IO;
using System.Collections.Generic;

using FormatKFIV.TypePlayStation;

namespace FormatKFIV.Utility
{
    public class OutputStream : BinaryWriter
    {
        public OutputStream(string filepath) : base(File.OpenWrite(filepath)) { }

        //I hate having to cast to the type every fucking time.
        public void WriteByte(byte value)
        {
            Write(value);
        }
        public void WriteUInt16(ushort value)
        {
            Write(value);
        }
    }
}
