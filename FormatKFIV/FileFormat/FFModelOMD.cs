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
            public ulong unknown0x80;
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
            Model ResultingModel = new Model();

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

                            if (V.NW != 0 && V.NW != -32768)
                            {
                                Console.WriteLine($"N Flag: {V.NW}");
                            }

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

                //Convert OMD Meshes to our native mesh format.
                for (int i = 0; i < meshes.Length; ++i)
                {
                    OMDMesh omdMesh = meshes[i];
                    Model.Mesh mdlMesh = new Model.Mesh();
                    List<Model.Triangle> mdlTris = new List<Model.Triangle>();
                   

                    foreach(OMDTristripPacket tristripP in omdMesh.tristrips)
                    {
                        for(int j = 0; j < tristripP.vertices.Length - 2; ++j)
                        {
                            //Get OMD Vertices
                            OMDVertex V1, V2, V3;
                            if ((j & 1) == 1)
                            {
                                V1 = tristripP.vertices[j + 1];
                                V2 = tristripP.vertices[j + 0];
                                V3 = tristripP.vertices[j + 2];
                            }
                            else
                            {
                                V1 = tristripP.vertices[j + 0];
                                V2 = tristripP.vertices[j + 1];
                                V3 = tristripP.vertices[j + 2];
                            }


                            if (V3.NW == -32768)
                            {
                                j += 1;
                                continue;
                            }

                            //Generate Indices
                            Model.Triangle tri = new Model.Triangle
                            {
                                vIndices = new ushort[3],
                                nIndices = new ushort[3],
                                tIndices = new ushort[3],
                                cIndices = new ushort[3]
                            };

                            //Console.WriteLine($"OMD [V1W = {V1.PW}, V2W = {V2.PW}, V3W = {V3.PW}]");
                            //Console.WriteLine($"OMD [N1W = {V1.NW}, N2W = {V2.NW}, N3W = {V3.NW}]");

                            tri.vIndices[0] = (ushort)ResultingModel.AddVertex(V1.PX / 256f, -V1.PY / 256f, -V1.PZ / 256f);
                            tri.vIndices[1] = (ushort)ResultingModel.AddVertex(V2.PX / 256f, -V2.PY / 256f, -V2.PZ / 256f);
                            tri.vIndices[2] = (ushort)ResultingModel.AddVertex(V3.PX / 256f, -V3.PY / 256f, -V3.PZ / 256f);
                            tri.nIndices[0] = (ushort)ResultingModel.AddNormal(V1.NX / 4096f, -V1.NY / 4096f, -V1.NZ / 4096f);
                            tri.nIndices[1] = (ushort)ResultingModel.AddNormal(V2.NX / 4096f, -V2.NY / 4096f, -V2.NZ / 4096f);
                            tri.nIndices[2] = (ushort)ResultingModel.AddNormal(V3.NX / 4096f, -V3.NY / 4096f, -V3.NZ / 4096f);
                            tri.tIndices[0] = (ushort)ResultingModel.AddTexcoord(V1.TU / 4096f, V1.TV / 4096f);
                            tri.tIndices[1] = (ushort)ResultingModel.AddTexcoord(V2.TU / 4096f, V2.TV / 4096f);
                            tri.tIndices[2] = (ushort)ResultingModel.AddTexcoord(V3.TU / 4096f, V3.TV / 4096f);
                            tri.cIndices[0] = (ushort)ResultingModel.AddColour(V1.CR / 255f, V1.CG / 255f, V1.CB / 255f, 1f);
                            tri.cIndices[1] = (ushort)ResultingModel.AddColour(V2.CR / 255f, V2.CG / 255f, V2.CB / 255f, 1f);
                            tri.cIndices[2] = (ushort)ResultingModel.AddColour(V3.CR / 255f, V3.CG / 255f, V3.CB / 255f, 1f);

                            //
                            // An annoying aspect to these models that still hasn't got a proper solution
                            // is the fliping of seemingly non predictable triangles in the vertex data.
                            // In order to fix that, we generate a flat normal and an averaged vertex normal
                            // and perform a dot product between them. If the result is less than 0,
                            // we flip the face.
                            //

                            //Generate a flat normal
                            Vector3f FaceNormal = Model.GenerateFaceNormal(
                                new Vector3f(V1.PX, V1.PY, V1.PZ),
                                new Vector3f(V2.PX, V2.PY, V2.PZ),
                                new Vector3f(V3.PX, V3.PY, V3.PZ));

                            //Average our vertex normals approximate what the flat normal would have been
                            Vector3f AverageNorm = Vector3f.Normalize(Vector3f.Average(
                                new Vector3f(V1.NX, V1.NY, V1.NZ),
                                new Vector3f(V2.NX, V2.NY, V2.NZ),
                                new Vector3f(V3.NX, V3.NY, V3.NZ)));

                            //Get a dot product between the two flat normals
                            if(Vector3f.Dot(AverageNorm, FaceNormal) < 0)
                                tri.FlipIndices();

                            mdlTris.Add(tri);
                        }
                    }

                    mdlMesh.numTriangle = (uint)mdlTris.Count;
                    mdlMesh.triangles = mdlTris.ToArray();

                    ResultingModel.AddMesh(mdlMesh);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                return null;
            }

            return ResultingModel;
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
