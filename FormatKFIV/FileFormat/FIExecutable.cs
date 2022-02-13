using System;
using System.Collections.Generic;
using System.Text;

namespace FormatKFIV.FileFormat
{
    /// <summary>Interface enabling the loading and presentation of data stored in KFIV exes. SIKE! To do :)</summary>
    /// 
    public interface FIExecutable
    {
        public struct EXEData
        {
            public uint dataType;
            public uint dataForm;   //Array or 
            public uint dataCount;  //When dataForm == array

            /// <summary>Byte Buffer of data</summary>
            public byte[] buffer;
        }
    }
}
