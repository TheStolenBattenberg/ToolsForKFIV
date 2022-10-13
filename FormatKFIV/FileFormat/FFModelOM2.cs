﻿using System;
using System.Collections.Generic;

using FormatKFIV.Asset;
using FormatKFIV.TypePlayStation;
using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    public class FFModelOM2 : FIFormat<Model>
    {
        #region Format Structures
        /// <summary>Container for a whole OM2 Model</summary>
        public struct OM2Model
        {
            public OM2Header header;
        }

        /// <summary>Contains basic information about an OM2.</summary>
        public struct OM2Header
        {
            public uint length;
            public ushort numMesh;
            public ushort numPrimitive;
            public ushort numTriangle;
            public ushort numVertex;
            public uint Unknown0x0C;

        }

        /// <summary>Some unidentified structure. Transformation?..</summary>
        public struct OM2StructA
        {
            public float Unknown0x00;
            public float Unknown0x04;
            public float Unknown0x08;
            public int   Unknown0x0C;
        }

        /// <summary>Declaration structure for a OM2 Mesh</summary>
        public struct OM2Mesh
        {
            public uint offPrimitive;
            public ushort numPrimitive;
            public byte Unknown0x06;
            public byte Unknown0x07;
            public uint Unknown0x08;
            public uint Unknown0x0C;
        }

        public struct OM2Primitive
        {
            public sceDmaTag DMATag;
            public ulong Unknown0x10;
            public ulong Unknown0x18;

        }

        #endregion

        public FIParameters Parameters => throw new NotImplementedException();


        public Model LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }

        public Model LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(string filepath, Model data)
        {
            throw new NotImplementedException();
        }
    }
}