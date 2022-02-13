using System;
using System.Collections.Generic;

using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    /// <summary>Provides storage and utility for reading/writing a rev_para.dat file</summary>
    public class FFParamReverb
    {
        #region Format Structures
        /// <summary>A single entry in reverb parameter data.</summary>
        public struct RevParaEntry {
            /// <summary>Base Reverb Mode. Studio1, Studio2, Studio3, Pipe ETC...</summary>
            public uint ReverbMode;
            /// <summary>Depth of Reverb (left channel)</summary>
            public ushort DepthL;
            /// <summary>Depth of Reverb (right channel)</summary>
            public ushort DepthR;
            /// <summary>Delay of Reverb</summary>
            public ushort Delay;
            /// <summary>Feedback of Reverb</summary>
            public ushort Feedback;
            /// <summary>Volume of Reverb (left channel)</summary>
            public ushort VolumeL;
            /// <summary>Volume of Reverb (right channel)</summary>
            public ushort VolumeR;
        }

        #endregion

        //Properties
        public int ParamCount
        {
            get { return ParamSet.Count; }
        }

        //Members
        private List<RevParaEntry> ParamSet = new List<RevParaEntry>();

        public static FFParamReverb LoadFromBuffer(byte[] buffer)
        {
            FFParamReverb resultingParamFile = new FFParamReverb();

            //Read from file
            using (InputStream ins = new InputStream(buffer))
            {
                for(var i = 0; i < (ins.Length() / 16); ++i)
                {
                    resultingParamFile.ParamSet.Add(new RevParaEntry
                    {
                        ReverbMode = ins.ReadUInt32(),
                        DepthL     = ins.ReadUInt16(),
                        DepthR     = ins.ReadUInt16(),
                        Delay      = ins.ReadUInt16(),
                        Feedback   = ins.ReadUInt16(),
                        VolumeL    = ins.ReadUInt16(),
                        VolumeR    = ins.ReadUInt16()
                    });
                }
            }

            return resultingParamFile;
        }

        public RevParaEntry this[int i]
        {
            get { return ParamSet[i]; }
        }
    }
}
