using System;
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

            public OM2Primitive[] primitives;
        }

        public struct OM2Vertex
        {
            public float PX, PY, PZ, PW;
            public float NX, NY, NZ, NW;
            public float TU, TV, TS, TT;
            public float CR, CG, CB, CA;
        }

        public struct OM2Primitive
        {
            public sceDmaTag DMATag;
            public ulong Unknown0x10;   //These could be some weird fromsoft thing. They seem to contain a counter, for the number ...
            public ulong Unknown0x18;   // ... of following 16 byte rows. DMA?..
            public sceGifTag GIFTag;
            public sceGsTex0 tex0Data;
            public ulong tex0Addr;
            public sceGsTex1 tex1Data;
            public ulong tex1Addr;
            public byte[] Unknown0x50;  //48 Unknown Bytes. Most likely PS2 Register stuff, but it's mostly unused.
            public ulong Unknown0x80;   //See comment on Unknown0x10 and Unknown0x18
            public ulong Unknown0x88;   // ^^^
            public byte numVertex;
            public byte[] unknown0x91;  //15 Bytes... Absolutely no clue.
            public ulong unknown0xA0;   //Near duplicate of the last 16 bytes.
            public ulong unknown0xA8;   // ^^^

            public OM2Vertex[] vertices;

            //These come after all the vertices, hence N for number.
            public ulong unknown0xN0;
            public ulong unknown0xN8;
        }

        #endregion
        #region Format Parameters
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".om2",
            },
            Type = FEType.Model,
            AllowExport = false,
            Validator = FFModelOM2.FileIsValid
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
                return ImportOM2(ins);
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
                return ImportOM2(ins, null);
            }
        }

        public Model ImportOM2(InputStream ins, FFModelOMD.OMDModel? omd = null)
        {
            long insPosition = ins.Position();

            Model om2Out = new Model();

            try
            {
                //Read OM2
                OM2Header header = new OM2Header
                {
                    length = ins.ReadUInt32(),
                    numMesh = ins.ReadUInt16(),
                    numPrimitive = ins.ReadUInt16(),
                    numTriangle = ins.ReadUInt16(),
                    numVertex = ins.ReadUInt16(),
                    Unknown0x0C = ins.ReadUInt32()
                };

                OM2StructA[] structAs = new OM2StructA[32];
                for(int i = 0; i < 32; ++i)
                {
                    structAs[i] = new OM2StructA
                    {
                        Unknown0x00 = ins.ReadSingle(),
                        Unknown0x04 = ins.ReadSingle(),
                        Unknown0x08 = ins.ReadSingle(),
                        Unknown0x0C = ins.ReadInt32()
                    };
                }

                OM2Mesh[] meshes = new OM2Mesh[header.numMesh];
                for(int i = 0; i < header.numMesh; ++i)
                {
                    meshes[i] = new OM2Mesh
                    {
                        offPrimitive = ins.ReadUInt32(),
                        numPrimitive = ins.ReadUInt16(),
                        Unknown0x06 = ins.ReadByte(),
                        Unknown0x07 = ins.ReadByte(),
                        Unknown0x08 = ins.ReadUInt32(),
                        Unknown0x0C = ins.ReadUInt32()
                    };

                    meshes[i].primitives = new OM2Primitive[meshes[i].numPrimitive];
                }

                foreach(OM2Mesh mesh in meshes)
                {
                    ins.Jump(insPosition + mesh.offPrimitive);

                    for(int i = 0; i < mesh.numPrimitive; ++i)
                    {
                        OM2Primitive primitive = new OM2Primitive
                        {
                            DMATag = ins.ReadPS216ByteRegister()._sceDmaTag,
                            Unknown0x10 = ins.ReadUInt64(),
                            Unknown0x18 = ins.ReadUInt64(),
                            GIFTag = ins.ReadPS216ByteRegister()._sceGifTag,
                            tex0Data = ins.ReadPS28ByteRegister()._sceGsTex0,
                            tex0Addr = ins.ReadUInt64(),
                            tex1Data = ins.ReadPS28ByteRegister()._sceGsTex1,
                            tex1Addr = ins.ReadUInt64(),
                            Unknown0x50 = ins.ReadBytes(48),
                            Unknown0x80 = ins.ReadUInt64(),
                            Unknown0x88 = ins.ReadUInt64(),
                            numVertex = ins.ReadByte(),
                            unknown0x91 = ins.ReadBytes(15),
                            unknown0xA0 = ins.ReadUInt64(),
                            unknown0xA8 = ins.ReadUInt64()
                        };

                        OM2Vertex[] vertices = new OM2Vertex[primitive.numVertex];

                        for(int j = 0; j < primitive.numVertex; ++j)
                        {
                            vertices[j] = new OM2Vertex
                            {
                                PX = ins.ReadSingle(),
                                PY = ins.ReadSingle(),
                                PZ = ins.ReadSingle(),
                                PW = ins.ReadSingle(),
                                NX = ins.ReadSingle(),
                                NY = ins.ReadSingle(),
                                NZ = ins.ReadSingle(),
                                NW = ins.ReadSingle(),
                                TU = ins.ReadSingle(),
                                TV = ins.ReadSingle(),
                                TS = ins.ReadSingle(),
                                TT = ins.ReadSingle(),
                                CR = ins.ReadSingle(),
                                CG = ins.ReadSingle(),
                                CB = ins.ReadSingle(),
                                CA = ins.ReadSingle()
                            };
                        }

                        primitive.unknown0xN0 = ins.ReadUInt64();
                        primitive.unknown0xN8 = ins.ReadUInt64();

                        primitive.vertices = vertices;
                        mesh.primitives[i] = primitive;
                    }

                    ins.Return();
                }

                //Clean up
                ins.Seek(insPosition + header.length, System.IO.SeekOrigin.Begin);

                //Convert OM2
                List<Model.IPrimitiveType> primitives = new List<Model.IPrimitiveType>();

                int om2MeshId = 0;
                foreach(OM2Mesh om2Mesh in meshes)
                {
                    Model.Mesh mesh = new Model.Mesh();
                    primitives.Clear();

                    //Copy Transform of OMD
                    if (omd.HasValue)
                    {
                        FFModelOMD.OMDModel omdV = omd.Value;

                        FFModelOMD.OMDMesh omdMesh = omdV.meshes[0];
                        if (om2MeshId < omdV.header.numMesh)
                        {
                            omdMesh = omdV.meshes[om2MeshId];
                        }

                        mesh.position = omdMesh.translation;
                        mesh.rotation = omdMesh.rotation;
                        mesh.scale = omdMesh.scale;
                    }
                    else
                    {
                        mesh.position = Vector3f.Zero;
                        mesh.rotation = Vector3f.Zero;
                        mesh.scale = Vector3f.One;
                    }

                    foreach (OM2Primitive om2Prim in om2Mesh.primitives)
                    {
                        OM2Vertex[] om2Vertices = new OM2Vertex[3];
                        for(int i = 0; i < om2Prim.numVertex - 2; ++i)
                        {
                            if((i & 1) == 1)
                            {
                                om2Vertices[0] = om2Prim.vertices[i + 1];
                                om2Vertices[1] = om2Prim.vertices[i + 0];
                                om2Vertices[2] = om2Prim.vertices[i + 2];
                            }
                            else
                            {
                                om2Vertices[0] = om2Prim.vertices[i + 0];
                                om2Vertices[1] = om2Prim.vertices[i + 1];
                                om2Vertices[2] = om2Prim.vertices[i + 2];
                            }

                            //Tristrip break
                            if(om2Vertices[2].NW != 0)
                            {
                                i += 1;
                                continue;
                            }

                            Model.TrianglePrimitive tri = Model.TrianglePrimitive.New();
                            for(int j = 0; j < 3; ++j)
                            {
                                tri.Indices[0 + j] = om2Out.AddVertex(om2Vertices[j].PX, -om2Vertices[j].PY, -om2Vertices[j].PZ);
                                tri.Indices[3 + j] = om2Out.AddNormal(om2Vertices[j].NX, -om2Vertices[j].NY, -om2Vertices[j].NZ);
                                tri.Indices[6 + j] = om2Out.AddTexcoord(om2Vertices[j].TU, om2Vertices[j].TV);
                                tri.Indices[9 + j] = om2Out.AddColour(om2Vertices[j].CR / 128f, om2Vertices[j].CG / 128f, om2Vertices[j].CB / 128f, om2Vertices[j].CA / 128f);
                            }

                            //See the comment at this same point in FFModelOMD to understand why this is fresh bullshit on a paper plate...
                            Vector3f faceNormal = Model.GenerateFlatNormal(
                                new Vector3f(om2Vertices[0].PX, -om2Vertices[0].PY, -om2Vertices[0].PZ),
                                new Vector3f(om2Vertices[1].PX, -om2Vertices[1].PY, -om2Vertices[1].PZ),
                                new Vector3f(om2Vertices[2].PX, -om2Vertices[2].PY, -om2Vertices[2].PZ));

                            Vector3f averageNormal = Vector3f.Average(
                                new Vector3f(om2Vertices[0].NX, -om2Vertices[0].NY, -om2Vertices[0].NZ),
                                new Vector3f(om2Vertices[1].NX, -om2Vertices[1].NY, -om2Vertices[1].NZ),
                                new Vector3f(om2Vertices[2].NX, -om2Vertices[2].NY, -om2Vertices[2].NZ));

                            if(Vector3f.Dot(averageNormal, faceNormal) < 0)
                            {
                                tri.FlipIndices();
                            }

                            primitives.Add(tri);
                        }
                    }

                    mesh.primitives = primitives.ToArray();
                    om2Out.AddMesh(mesh);

                    om2MeshId++;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                return null;
            }

            return om2Out;
        }

        public void SaveToFile(string filepath, Model data)
        {
            throw new NotImplementedException();
        }
    }
}
