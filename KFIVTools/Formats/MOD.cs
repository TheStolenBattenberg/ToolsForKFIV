﻿using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System;

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
            public uint numNormal;
            public uint offNormal;
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
                numNormal = ins.ReadUInt32();
                offNormal = ins.ReadUInt32();
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

            public List<MODModelTristrip> tristrips;

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

                tristrips = new List<MODModelTristrip>();
            }
        };
        public struct MODModelTristrip
        {
            public ushort vertexInd;
            public ushort normalInd;
            public ushort tcoordInd;
            public ushort unknown0x04;
            public ushort unknown0x06;
            public float texU;
            public float texV;

            public Vector4 unknownXYZp;

            public MODModelTristrip(InputStream ins)
            {
                vertexInd = ins.ReadUInt16();
                normalInd = ins.ReadUInt16();

                tcoordInd = 1;

                unknown0x04 = ins.ReadUInt16();
                unknown0x06 = ins.ReadUInt16();
                texU = ins.ReadSingle();
                texV = ins.ReadSingle();
                unknownXYZp = ins.ReadVector4s();
            }
        };

        #endregion

        List<Vector4> MODVertex;
        List<Vector4> MODNormal;
        List<MODModelTriPacket> MODTriPacket;
        TM2.TM2 tm2;

        public static MOD FromFile(string path)
        {
            MOD mod = new MOD();
            mod.MODVertex = new List<Vector4>();
            mod.MODNormal = new List<Vector4>();
            mod.MODTriPacket = new List<MODModelTriPacket>();

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
                ins.Jump(header.modelOffset + modelHeader.offNormal);

                for (int i = 0; i < modelHeader.numNormal; ++i)
                    mod.MODNormal.Add(ins.ReadVector4s());

                ins.Return();

                //Read Tristrips
                ins.Jump(header.modelOffset + modelHeader.offTriPacket);

                for (int i = 0; i < modelHeader.numTriPacket; ++i)
                {
                    MODModelTriPacket mtp = new MODModelTriPacket(ins);

                    for(int j = 0; j < mtp.numTri; ++j)
                    {
                        mtp.tristrips.Add(new MODModelTristrip(ins));
                    }

                    mod.MODTriPacket.Add(mtp);
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
                obj.AddVertex(-vert.x, -vert.y, vert.z);

            foreach(Vector4 norm in MODNormal)
                obj.AddNormal(-norm.x, -norm.y, norm.z);


            //Write all texcoords
            foreach (MODModelTriPacket mtp in MODTriPacket)
            {
                // Fuck you C#, this could be foreach but you playin'
                for (int i = 0; i < mtp.numTri; ++i)
                {
                    MODModelTristrip mts = mtp.tristrips[i];

                    obj.AddTexcoord(mts.texU, 1.0f - mts.texV);
                    mts.tcoordInd = (ushort)obj.TexcoordCount;
                    mtp.tristrips[i] = mts;
                }
            }

            //Write all groups
            int groupNumber = 1;
            string groupName = "";

            foreach(MODModelTriPacket mtp in MODTriPacket)
            {
                groupName = "Mesh_" + groupNumber.ToString("D4");
                obj.AddGroup(groupName);

                for (int i = 0; i < mtp.numTri - 2; ++i)
                {
                    MODModelTristrip v1 = mtp.tristrips[i + 1];
                    MODModelTristrip v2 = mtp.tristrips[i + 2];
                    MODModelTristrip v3 = mtp.tristrips[i + 0];

                    // Because tristrips fucking suck, we need to use a dotproduct between
                    // each normal and the calculated face normal.
                    // It is good enough to just use the first normal, but an average might be better.
                    Vector3 FN = OBJ.OBJ.GenerateFaceNormal(MODVertex[v1.vertexInd].AsVec3, MODVertex[v2.vertexInd].AsVec3, MODVertex[v3.vertexInd].AsVec3);
                    float d = Vector3.Dot(MODNormal[v1.normalInd].AsVec3, FN);

                    if(d < 0)
                    {
                        obj.AddFace(groupName,
                            1 + v2.vertexInd, 1 + v1.vertexInd, 1 + v3.vertexInd,
                            v2.normalInd, v1.normalInd, v3.normalInd,
                            v2.tcoordInd, v1.tcoordInd, v3.tcoordInd);
                    }
                    else
                    {
                        obj.AddFace(groupName,
                            1 + v3.vertexInd, 1 + v1.vertexInd, 1 + v2.vertexInd,
                            v3.normalInd, v1.normalInd, v2.normalInd,
                            v3.tcoordInd, v1.tcoordInd, v2.tcoordInd);
                    }
                }

                groupNumber++;
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
                    TM2.TM2.CLUT8BPPFix(ref tm2.clutData, (uint)tm2.header.texClutSize, tm2.header.texClutAlpha == 1);
                    break;
                case 20: 
                    pFmt = PixelFormat.Format4bppIndexed;
                    break;
            }

            Console.WriteLine("Clut Alpha: " + tm2.header.texClutAlpha);

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