using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using KFIV.Utility.IO;
using KFIV.Utility.Math;
using KFIV.Format.TM2;

namespace KFIV.Format.MOD
{
    public class MOD
    {

        /**
         * This region contains all structures and types of the MOD Format.
        **/ 
        #region Format Types
        public struct MODHeader
        {
            public uint modelLength;
            public uint modelOffset;
            public uint textureLength;
            public uint textureOffset;

            public MODHeader(InputStream ins)
            {
                modelLength = ins.ReadUInt32();
                modelOffset = ins.ReadUInt32();
                textureLength = ins.ReadUInt32();
                textureOffset = ins.ReadUInt32();
            }
        };
        public struct MODModelHeader
        {
            public uint modelLength;
            public byte unknown0x04;
            public byte unknown0x05;
            public ushort unknown0x06;
            public uint unknown0x08;
            public uint unknown0x0C;
            public Vector4 transformXYZp;
            public Vector4 rotationXYZp;
            public Vector4 scaleXYZp;
            public uint numTriPacket;
            public uint offTriPacket;
            public uint numVertex;
            public uint offVertex;
            public uint numStructA;
            public uint offStructA;
            public uint reserved1;
            public uint reserved2;

            public MODModelHeader(InputStream ins)
            {
                modelLength = ins.ReadUInt32();
                unknown0x04 = ins.ReadByte();
                unknown0x05 = ins.ReadByte();
                unknown0x06 = ins.ReadUInt16();
                unknown0x08 = ins.ReadUInt32();
                unknown0x0C = ins.ReadUInt32();
                transformXYZp = ins.ReadVector4s();
                rotationXYZp = ins.ReadVector4s();
                scaleXYZp = ins.ReadVector4s();
                numTriPacket = ins.ReadUInt32();
                offTriPacket = ins.ReadUInt32();
                numVertex = ins.ReadUInt32();
                offVertex = ins.ReadUInt32();
                numStructA = ins.ReadUInt32();
                offStructA = ins.ReadUInt32();
                reserved1 = ins.ReadUInt32();
                reserved2 = ins.ReadUInt32();
            }
        };
        public struct MODModelTriPacket
        {
            public ushort numTri;
            public ushort unknown0x02;
            public ushort unknown0x04;
            public ushort unknown0x06;

            public ushort unknown0x08;
            public ushort unknown0x0a;
            public ushort unknown0x0c;
            public ushort unknown0x0e;
            public ulong unknown0x10;   //Always '00 00 00 00 00 00 00 00'
            public ulong unknown0x18;   //Always '60 00 00 00 00 00 00 00'

            public MODModelTriPacket(InputStream ins)
            {
                numTri = ins.ReadUInt16();
                unknown0x02 = ins.ReadUInt16();
                unknown0x04 = ins.ReadUInt16();
                unknown0x06 = ins.ReadUInt16();
                unknown0x08 = ins.ReadUInt16();
                unknown0x0a = ins.ReadUInt16();
                unknown0x0c = ins.ReadUInt16();
                unknown0x0e = ins.ReadUInt16();
                unknown0x10 = ins.ReadUInt64();
                unknown0x18 = ins.ReadUInt64();
            }
        };
        public struct MODModelTristrip
        {
            public ushort vertexInd;
            public ushort structInd;
            public ushort unknown0x04;
            public ushort unknown0x06;
            public float texU;
            public float texV;

            public Vector4 normalXYZp;

            public MODModelTristrip(InputStream ins)
            {
                vertexInd = ins.ReadUInt16();
                structInd = ins.ReadUInt16();
                unknown0x04 = ins.ReadUInt16();
                unknown0x06 = ins.ReadUInt16();
                texU = ins.ReadSingle();
                texV = ins.ReadSingle();
                normalXYZp = ins.ReadVector4s();
            }
        };

        #endregion

        List<Vector4> MODVertex;
        List<Vector4> MODStructA;
        List<MODModelTristrip> MODTristrip;
        TM2.TM2 tm2;

        public static MOD FromFile(string path)
        {
            MOD mod = new MOD();
            mod.MODVertex = new List<Vector4>();
            mod.MODStructA = new List<Vector4>();
            mod.MODTristrip = new List<MODModelTristrip>();

            using (InputStream ins = new InputStream(path))
            {
                //Read Header of MOD
                MODHeader header = new MODHeader(ins);

                //
                // Model Read
                //

                //Read Model Header
                ins.Seek(header.modelOffset, SeekOrigin.Begin);
                MODModelHeader modelHeader = new MODModelHeader(ins);

                //Read Vertices
                ins.Jump(header.modelOffset + modelHeader.offVertex);

                for(int i = 0; i < modelHeader.numVertex; ++i)
                    mod.MODVertex.Add(ins.ReadVector4s());

                ins.Return();

                //Read StructA
                ins.Jump(header.modelOffset + modelHeader.offStructA);

                for (int i = 0; i < modelHeader.numStructA; ++i)
                    mod.MODStructA.Add(ins.ReadVector4s());

                ins.Return();

                //Read Tristrips
                ins.Jump(header.modelOffset + modelHeader.offTriPacket);

                for (int i = 0; i < modelHeader.numTriPacket; ++i)
                {
                    MODModelTriPacket mtp = new MODModelTriPacket(ins);

                    for(int j = 0; j < mtp.numTri; ++j)
                    {
                        mod.MODTristrip.Add(new MODModelTristrip(ins));
                    }
                }
                    

                ins.Return();

                //Read Texture
                ins.Jump(header.textureOffset);
                mod.tm2 = TM2.TM2.FromMemory(ins.ReadBytes((int)header.textureLength));
                ins.Return();
            }

            return mod;
        }

        public void Save(string path)
        {
            OBJ.OBJ obj = new OBJ.OBJ();

            //Write all vertices
            foreach(Vector4 vert in MODVertex)
                obj.AddVertex(vert.x, vert.y, vert.z);

            //Write all normals
            foreach (MODModelTristrip mts in MODTristrip)
                obj.AddNormal(mts.normalXYZp.x, mts.normalXYZp.y, mts.normalXYZp.z);

            //Write all texcoords
            foreach (MODModelTristrip mts in MODTristrip)
                obj.AddTexcoord(mts.texU, mts.texV);

            //Write all groups
            int nuvInd = 1;

            string groupName = "Mesh_0001";
            obj.AddGroup(groupName);

            for(int i = 0; i < MODTristrip.Count-2; ++i)
            {
                obj.AddFace(groupName,
                    1 + MODTristrip[i].vertexInd, 1 + MODTristrip[i + 1].vertexInd, 1 + MODTristrip[i + 2].vertexInd,
                    nuvInd, nuvInd + 1, nuvInd + 2, nuvInd, nuvInd + 1, nuvInd + 2);

                nuvInd++;
            }

            //Save obj to disc
            obj.Save(path + ".obj");

            //
            // Save png to disc
            //

            //Calculate pixel format, apply fixes
            PixelFormat pFmt = PixelFormat.Format32bppArgb;
            switch (tm2.header.texPSM)
            {
                case 0: 
                    pFmt = PixelFormat.Format32bppArgb;
                    TM2.TM2.BGRtoRGB32(ref tm2.pixelData, (uint)tm2.header.texPixelSize);
                    break;
                case 1: 
                    pFmt = PixelFormat.Format24bppRgb;
                    break;
                case 2: 
                    pFmt = PixelFormat.Format16bppArgb1555;
                    break;
                case 19: 
                    pFmt = PixelFormat.Format8bppIndexed;
                    TM2.TM2.CLUT8BPPFix(ref tm2.clutData, (uint)tm2.header.texClutSize);
                    break;
                case 20: 
                    pFmt = PixelFormat.Format4bppIndexed;
                    break;
            }

            using (Bitmap bm = new Bitmap((int)tm2.header.texWidth, (int)tm2.header.texHeight, pFmt))
            {
                //When image is indexed, copy the clut first.
                if (tm2.header.clutOffset != 0)
                {
                    ColorPalette pal = bm.Palette;

                    int palOffset = 0;
                    for (uint i = 0; i < (tm2.header.texClutSize / 4); ++i)
                    {
                        pal.Entries[i] = Color.FromArgb(tm2.clutData[palOffset + 3], tm2.clutData[palOffset + 2], tm2.clutData[palOffset + 1], tm2.clutData[palOffset + 0]);
                        palOffset += 4;
                    }

                    bm.Palette = pal;
                }

                //Copy Image
                BitmapData bmd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, pFmt);
                Marshal.Copy(tm2.pixelData, 0, bmd.Scan0, tm2.header.texPixelSize);
                bm.UnlockBits(bmd);

                bm.Save(path + ".png", ImageFormat.Png);
            }
        }
    }
}