using System;
using System.Collections.Generic;

using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    /// <summary>Provides storage and utility for reading/writing a KFIV.DAT Resource file</summary>
    public class FFResourceDAT
    {
        #region Format Structures
        /// <summary>Contains information about the DAT archive file.</summary>
        public struct DATHeader
        {
            /// <summary>Total length of the archive file.</summary>
            public uint length;
            /// <summary>Total count of contained files.</summary>
            public uint numFile;
            /// <summary>Ignore. Address used only when loaded on the PS2</summary>
            public uint bufDest_ps2_internal;
            public uint pad0x0C;

            public long pad0x10;
            public long pad0x18;
            public long pad0x20;
            public long pad0x28;
            public long pad0x30;
            public long pad0x38;
        };  //Length = 64 Bytes

        /// <summary>Contains information about an individual file inside the archive.</summary>
        public struct DATFile
        {
            /// <summary>File name/path.</summary>
            public string name;
            /// <summary>File true data length.</summary>
            public uint length;
            /// <summary>File padded data length.</summary>
            public uint paddedLength;
            /// <summary>File offset inside archive.</summary>
            public uint offset;
            /// <summary>File data buffer.</summary>
            public byte[] buffer;

        };  //Length = 64 Bytes + N Bytes


        #endregion

        //Properties
        /// <summary>Returns the total count of files present in the DAT.</summary>
        public int FileCount
        {
            get { return FileList.Count; }
        }
        
        //Members
        private List<DATFile> FileList = new List<DATFile>();

        /// <summary>Load a Resource DAT from local storage.</summary>
        /// <param name="filePath">The path to the file you would like to load.</param>
        /// <returns>The resulting DAT file, or null if something bad happens.</returns>
        public static FFResourceDAT LoadFromFile(string filePath)
        {
            //Create FFResourceDAT to store out DAT File Data
            FFResourceDAT ResultingDATFile = new FFResourceDAT();

            using (InputStream ins = new InputStream(filePath))
            {
                //Try reading from the DAT file.
                try
                {
                    //Read Header
                    DATHeader ffDatHeader = new DATHeader
                    {
                        length = ins.ReadUInt32(),
                        numFile = ins.ReadUInt32(),
                        bufDest_ps2_internal = ins.ReadUInt32(),
                        pad0x0C = ins.ReadUInt32(),
                        pad0x10 = ins.ReadInt64(),
                        pad0x18 = ins.ReadInt64(),
                        pad0x20 = ins.ReadInt64(),
                        pad0x28 = ins.ReadInt64(),
                        pad0x30 = ins.ReadInt64(),
                        pad0x38 = ins.ReadInt64()
                    };

                    //Read Each File and store it in the DAT file.
                    for (var itr = 0; itr < ffDatHeader.numFile; ++itr)
                    {
                        //Load basic parameters of the DAT file
                        DATFile ffDatFile = new DATFile
                        {
                            name = ins.ReadFixedString(52),
                            length = ins.ReadUInt32(),
                            paddedLength = ins.ReadUInt32(),
                            offset = ins.ReadUInt32(),
                            buffer = null,
                        };

                        //Load data from the DAT file and 'buffer' it.
                        ins.Jump(ffDatFile.offset);
                        ffDatFile.buffer = ins.ReadBytes((int)ffDatFile.length);    //Why the fuck do C# WANT THIS AS AN INT?
                        ins.Return();                                               //NOT EXACTLY GONNA READ -1 BYTES AM I C# LIBARY DEV LAD

                        ResultingDATFile.FileList.Add(ffDatFile);
                    }

                }
                catch (Exception Expt)
                {
                    //Whoops, we fucked something up.
                    Console.WriteLine(Expt.Message);
                    Console.WriteLine(Expt.StackTrace);
                    return null;
                }
            }

             return ResultingDATFile;
        }

        /// <summary>Clear all files from the DAT internal file list</summary>
        public void Clear()
        {
            FileList.Clear();
        }

        public DATFile this[int i]
        {
            get 
            {
                if (i < 0 || i > FileList.Count)
                    return FileList[0]; // :)

                return FileList[i]; 
            }
        }
    }
}
