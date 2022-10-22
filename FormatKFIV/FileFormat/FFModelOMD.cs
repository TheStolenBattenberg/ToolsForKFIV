using System;
using System.Collections.Generic;

using FormatKFIV.Asset;
using FormatKFIV.TypePlayStation;
using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    /// <summary>Provides utility for reading a OMD Model file</summary>
    public class FFModelOMD : FIFormat<Model>
    {
        #region Format Structures
        /// <summary>Container for a whole OMD Model</summary>
        public struct OMDModel
        {
            public OMDHeader header;
            public OMDMesh[] meshes;
        }

        /// <summary>Contains basic information about an OMD.</summary>
        public struct OMDHeader
        {
            /// <summary>Total length of file</summary>
            public uint length;
            /// <summary>Number of meshes contained</summary>
            public ushort numMesh;
            /// <summary>Total Triangle Count</summary>
            public ushort numTriangle;
            /// <summary>Total Vertex Count</summary>
            public uint numVertex;
            /// <summary>Unknown. Zero most of the time.</summary>
            public uint Unknown0x0C;
        }

        /// <summary>Contains information about an OMD Mesh</summary>
        public struct OMDMesh
        {
            /// <summary>Translation of the mesh</summary>
            public Vector3f translation;
            /// <summary>Rotation of the mesh</summary>
            public Vector3f rotation;
            /// <summary>Scale of the mesh</summary>
            public Vector3f scale;
            /// <summary>Offset to mesh triangle strips. Relative to file start.</summary>
            public uint offTristrips;
            /// <summary>Number of triangle strips in mesh.</summary>
            public uint numTristrips;

            public uint pad;

            /// <summary>Array with all tristrips present in the mesh</summary>
            public OMDTristripPacket[] tristrips;
        }

        /// <summary>Vertex Format</summary>
        public struct OMDVertex
        {
            public short PX, PY, PZ, PW;
            public short NX, NY, NZ, NW;
            public short TU, TV, Tp1, Tp2;
            public ushort CR, CG, CB, CA;
        }

        /// <summary>Tristrip Packet</summary>
        public struct OMDTristripPacket
        {
            public uint qwLength;
            public uint pad0x04;
            public ulong pad0x08;
            public ulong unknown0x10;
            public ulong unknown0x18;
            public sceGifTag gifTag0;
            public sceGsTex1 tex1Data;
            public ulong tex1Addr;
            public sceGsTex0 tex0Data;
            public ulong tex0Addr;
            public sceGsAlpha alpha1Data;
            public ulong alpha1Addr;
            public ulong unknown0x60;
            public ulong unknown0x68;
            public byte numVertex;
            public byte[] unknown0x71;  //15 Bytes...
            public ulong unknown0x80;   //Near duplicate of the last 16 bytes.
            public ulong unknown0x88;

            public OMDVertex[] vertices;

            //These come after all the vertices, hence N for number.
            public ulong unknwon0xN0;
            public ulong unknwon0xN8;
        }

        #endregion
        #region Format Parameters
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".omd",
            },
            Type = FEType.Model,
            AllowExport = false,
            Validator = FFModelOMD.FileIsValid
        };

        /// <summary>Validates a file to see if it is OMD Format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    //Can't think of a good check for this right now. This is really bad tho.
                    return true;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    Console.WriteLine(Ex.StackTrace);
                    return false;
                }
            }
        }

        #endregion

        public Model LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            //We don't use these, so we null them right away.
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(filepath))
            {
                return ImportOMD(ins);
            }
        }
        public Model LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            //We don't use these, so we null them right away.
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(buffer))
            {
                return ImportOMD(ins);
            }
        }

        public Model ImportOMD(InputStream ins)
        {
            Model result = new Model();

            long omdStartOffset = ins.Position();

            try
            {
                //Read OMD Header
                OMDHeader header = new OMDHeader
                {
                    length = ins.ReadUInt32(),
                    numMesh = ins.ReadUInt16(),
                    numTriangle = ins.ReadUInt16(),
                    numVertex = ins.ReadUInt32(),
                    Unknown0x0C = ins.ReadUInt32()
                };

                //Read OMD Meshes
                OMDMesh[] meshes = new OMDMesh[header.numMesh];
                for (int i = 0; i < header.numMesh; ++i)
                {
                    //Read Main OMD Mesh structure
                    meshes[i] = new OMDMesh
                    {
                        translation = ins.ReadVector3f(),
                        rotation = ins.ReadVector3f(),
                        scale = ins.ReadVector3f(),
                        offTristrips = ins.ReadUInt32(),
                        numTristrips = ins.ReadUInt32(),
                        pad = ins.ReadUInt32()
                    };
                    meshes[i].tristrips = new OMDTristripPacket[meshes[i].numTristrips];

                    //Read Mesh Tristrips
                    ins.Jump(omdStartOffset + meshes[i].offTristrips);

                    for (int j = 0; j < meshes[i].numTristrips; ++j)
                    {
                        //Read Main OMD Tristrip structure
                        OMDTristripPacket tristrip = new OMDTristripPacket
                        {
                            qwLength = ins.ReadUInt32(),
                            pad0x04 = ins.ReadUInt32(),
                            pad0x08 = ins.ReadUInt64(),
                            unknown0x10 = ins.ReadUInt64(),
                            unknown0x18 = ins.ReadUInt64(),
                            gifTag0 = ins.ReadPS216ByteRegister()._sceGifTag,
                            tex1Data = ins.ReadPS28ByteRegister()._sceGsTex1,
                            tex1Addr = ins.ReadUInt64(),
                            tex0Data = ins.ReadPS28ByteRegister()._sceGsTex0,
                            tex0Addr = ins.ReadUInt64(),
                            alpha1Data = ins.ReadPS28ByteRegister()._sceGsAlpha,
                            alpha1Addr = ins.ReadUInt64(),
                            unknown0x60 = ins.ReadUInt64(),
                            unknown0x68 = ins.ReadUInt64(),
                            numVertex = ins.ReadByte(),
                            unknown0x71 = ins.ReadBytes(15),
                            unknown0x80 = ins.ReadUInt64(),
                            unknown0x88 = ins.ReadUInt64(),
                        };
                        tristrip.vertices = new OMDVertex[tristrip.numVertex];

                        //Read Tristrip vertices
                        for (int k = 0; k < tristrip.vertices.Length; ++k)
                        {
                            OMDVertex V = new OMDVertex
                            {
                                PX = ins.ReadInt16(),
                                PY = ins.ReadInt16(),
                                PZ = ins.ReadInt16(),
                                PW = ins.ReadInt16(),
                                NX = ins.ReadInt16(),
                                NY = ins.ReadInt16(),
                                NZ = ins.ReadInt16(),
                                NW = ins.ReadInt16(),
                                TU = ins.ReadInt16(),
                                TV = ins.ReadInt16(),
                                Tp1 = ins.ReadInt16(),
                                Tp2 = ins.ReadInt16(),
                                CR = ins.ReadUInt16(),
                                CG = ins.ReadUInt16(),
                                CB = ins.ReadUInt16(),
                                CA = ins.ReadUInt16(),
                            };

                            tristrip.vertices[k] = V;
                        }

                        meshes[i].tristrips[j] = tristrip;

                        //Some random bytes we skip for now.
                        ins.Seek(16, System.IO.SeekOrigin.Current);
                    }
                    ins.Return();
                }

                //Go to the end of the OMD buffer for sanities sake.
                ins.Seek(omdStartOffset + header.length, System.IO.SeekOrigin.Begin);

                //
                // Convert OMD to internal model asset
                //

                //How many texture slots does the OMD use?
                List<uint> textureSlots = new List<uint>();

                foreach(OMDMesh omdMesh in meshes)
                {
                    //Skip empty meshes.
                    if(omdMesh.tristrips.Length <= 0)
                    {
                        continue;
                    }

                    foreach(OMDTristripPacket tristrip in omdMesh.tristrips)
                    {
                        //Skip empty tristrips.
                        if((tristrip.numVertex - 2) <= 0)
                        {
                            continue;
                        }

                        uint textureKey = (tristrip.tex0Data.TBP << 16) | tristrip.tex0Data.CBP;
                        if(!textureSlots.Contains(textureKey))
                        {
                            textureSlots.Add(textureKey);
                        }
                    }
                }

                //Split OMD meshes according to texture slots
                List<Model.IPrimitiveType> primitives = new List<Model.IPrimitiveType>();

                foreach(uint textureKey in textureSlots)
                {
                    foreach(OMDMesh omdMesh in meshes)
                    {
                        if(omdMesh.tristrips.Length <= 0)
                        {
                            continue;
                        }

                        Model.Mesh mesh = new Model.Mesh();

                        foreach (OMDTristripPacket tristrip in omdMesh.tristrips)
                        {
                            if ((tristrip.numVertex - 2) <= 0)
                            {
                                continue;
                            }

                            uint tristripKey = (tristrip.tex0Data.TBP << 16) | tristrip.tex0Data.CBP;
                            if(tristripKey != textureKey)
                            {
                                continue;
                            }

                            for (int i = 0; i < tristrip.numVertex - 2; ++i)
                            {
                                OMDVertex[] V = new OMDVertex[3];
                                if ((i & 1) == 1)
                                {
                                    V[0] = tristrip.vertices[i + 1];
                                    V[1] = tristrip.vertices[i + 0];
                                    V[2] = tristrip.vertices[i + 2];
                                }
                                else
                                {
                                    V[0] = tristrip.vertices[i + 0];
                                    V[1] = tristrip.vertices[i + 1];
                                    V[2] = tristrip.vertices[i + 2];
                                }

                                if (V[2].NW == -32768)
                                {
                                    i += 1;
                                    continue;
                                }

                                Model.TrianglePrimitive tri = Model.TrianglePrimitive.New();
                                for (int j = 0; j < 3; ++j)
                                {
                                    tri.Indices[0 + j] = result.AddVertex(V[j].PX, -V[j].PY, -V[j].PZ);
                                    tri.Indices[3 + j] = result.AddNormal(V[j].NX / 4096f, -V[j].NY / 4096f, -V[j].NZ / 4096f);
                                    tri.Indices[6 + j] = result.AddTexcoord(V[j].TU / 4096f, V[j].TV / 4096f);
                                    tri.Indices[9 + j] = result.AddColour(V[j].CR / 128f, V[j].CG / 128f, V[j].CB / 128f, V[j].CA / 128f);
                                }

                                //
                                // An annoying aspect to these models that still hasn't got a proper solution
                                // is the fliping of seemingly non predictable triangles in the vertex data.
                                // In order to fix that, we generate a flat normal and an averaged vertex normal
                                // and perform a dot product between them. If the result is less than 0,
                                // we flip the face.
                                //

                                //See the comment at this same point in FFModelOMD to understand why this is fresh bullshit on a paper plate...
                                Vector3f faceNormal = Model.GenerateFlatNormal(
                                    new Vector3f(V[0].PX, -V[0].PY, -V[0].PZ),
                                    new Vector3f(V[1].PX, -V[1].PY, -V[1].PZ),
                                    new Vector3f(V[2].PX, -V[2].PY, -V[2].PZ));

                                Vector3f averageNormal = Vector3f.Average(
                                    new Vector3f(V[0].NX / 4096f, -V[0].NY / 4096f, -V[0].NZ / 4096f),
                                    new Vector3f(V[1].NX / 4096f, -V[1].NY / 4096f, -V[1].NZ / 4096f),
                                    new Vector3f(V[2].NX / 4096f, -V[2].NY / 4096f, -V[2].NZ / 4096f));

                                if (Vector3f.Dot(averageNormal, faceNormal) < 0)
                                {
                                    tri.FlipIndices();
                                }

                                primitives.Add(tri);
                            }
                        }

                        mesh.position = new Vector3f(omdMesh.translation.X, -omdMesh.translation.Y, -omdMesh.translation.Z);
                        mesh.rotation = new Vector3f(omdMesh.rotation.X, 6.28318530718f - omdMesh.rotation.Y, 6.28318530718f - omdMesh.rotation.Z);
                        mesh.scale = omdMesh.scale;
                        mesh.primitives = primitives.ToArray();

                        result.AddMesh(mesh);

                        primitives.Clear();
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                return null;
            }

            return result;
        }

        public static OMDModel ImportOMDFromStream(InputStream ins)
        {
            OMDModel omdOut = new OMDModel();

            try
            {
                //Store relative location of the OMD in ins.
                long relativeLocation = ins.Position();


                //Read OMD Header
                OMDHeader header = new OMDHeader
                {
                    length      = ins.ReadUInt32(),
                    numMesh     = ins.ReadUInt16(),
                    numTriangle = ins.ReadUInt16(),
                    numVertex   = ins.ReadUInt32(),
                    Unknown0x0C = ins.ReadUInt32()
                };
                omdOut.header = header;

                //Read OMD Meshes
                omdOut.meshes = new OMDMesh[header.numMesh];

                for(int i = 0; i < header.numMesh; ++i)
                {
                    //Read a single mesh
                    OMDMesh omdMesh = new OMDMesh
                    {
                        translation = ins.ReadVector3f(),
                        rotation = ins.ReadVector3f(),
                        scale = ins.ReadVector3f(),
                        offTristrips = ins.ReadUInt32(),
                        numTristrips = ins.ReadUInt32(),
                        pad = ins.ReadUInt32()
                    };

                    Vector3f newT = omdMesh.translation;
                    omdMesh.translation = new Vector3f(newT.X, -newT.Y, -newT.Z);
                    newT = omdMesh.rotation;
                    omdMesh.rotation = new Vector3f(newT.X, 6.28318530718f - newT.Y, 6.28318530718f - newT.Z);

                    //Read mesh tristrip packets
                    omdMesh.tristrips = new OMDTristripPacket[omdMesh.numTristrips];

                    ins.Jump(relativeLocation + omdMesh.offTristrips);

                    for(int j = 0; j < omdMesh.numTristrips; ++j)
                    {
                        //Read a single tristrip packet
                        OMDTristripPacket tristripPacket = new OMDTristripPacket
                        {
                            qwLength = ins.ReadUInt32(),
                            pad0x04 = ins.ReadUInt32(),
                            pad0x08 = ins.ReadUInt64(),
                            unknown0x10 = ins.ReadUInt64(),
                            unknown0x18 = ins.ReadUInt64(),
                            gifTag0 = ins.ReadPS216ByteRegister()._sceGifTag,
                            tex1Data = ins.ReadPS28ByteRegister()._sceGsTex1,
                            tex1Addr = ins.ReadUInt64(),
                            tex0Data = ins.ReadPS28ByteRegister()._sceGsTex0,
                            tex0Addr = ins.ReadUInt64(),
                            alpha1Data = ins.ReadPS28ByteRegister()._sceGsAlpha,
                            alpha1Addr = ins.ReadUInt64(),
                            unknown0x60 = ins.ReadUInt64(),
                            unknown0x68 = ins.ReadUInt64(),
                            numVertex = ins.ReadByte(),
                            unknown0x71 = ins.ReadBytes(15),
                            unknown0x80 = ins.ReadUInt64(),
                            unknown0x88 = ins.ReadUInt64(),
                        };

                        tristripPacket.vertices = new OMDVertex[tristripPacket.numVertex];

                        //Read packet vertices.
                        for (int k = 0; k < tristripPacket.numVertex; ++k)
                        {
                            OMDVertex vertex = new OMDVertex
                            {
                                PX = ins.ReadInt16(),
                                PY = ins.ReadInt16(),
                                PZ = ins.ReadInt16(),
                                PW = ins.ReadInt16(),
                                NX = ins.ReadInt16(),
                                NY = ins.ReadInt16(),
                                NZ = ins.ReadInt16(),
                                NW = ins.ReadInt16(),
                                TU = ins.ReadInt16(),
                                TV = ins.ReadInt16(),
                                Tp1 = ins.ReadInt16(),
                                Tp2 = ins.ReadInt16(),
                                CR = ins.ReadUInt16(),
                                CG = ins.ReadUInt16(),
                                CB = ins.ReadUInt16(),
                                CA = ins.ReadUInt16()
                            };

                            tristripPacket.vertices[k] = vertex;
                        }

                        //Read extra tristrip packet data that is stored after the vertices.
                        tristripPacket.unknwon0xN0 = ins.ReadUInt64();
                        tristripPacket.unknwon0xN8 = ins.ReadUInt64();

                        omdMesh.tristrips[j] = tristripPacket;
                    }

                    ins.Return();

                    //Place mesh into our mesh array
                    omdOut.meshes[i] = omdMesh;
                }

                //Seek to the end of the OMD mesh for b2b reading purposes
                ins.Seek(relativeLocation + header.length, System.IO.SeekOrigin.Begin);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
            }

            return omdOut;
        }

        public void SaveToFile(string filepath, Model data)
        {
            throw new NotImplementedException();
        }
    }
}
