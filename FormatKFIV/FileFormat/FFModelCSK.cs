using System;
using System.Collections.Generic;

using FormatKFIV.Asset;
using FormatKFIV.TypePlayStation;
using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    public class FFModelCSK : FIFormat<Model>
    {
        #region Format Structures
        public struct CSKModel
        {
            public CSKHeader   header;
            public CSKGroup[]  groups;
            public CSKVertex[] vertices;
        }
        public struct CSKHeader
        {
            public uint numTotalGroup;
            public uint offGroups;
            public uint offVertices;
            public uint numVertex;
            public uint unknown0x10;
            public uint length;
            public uint offGroupTypeA;
            public uint offGroupTypeB;
            public uint offGroupTypeC;
            public uint offGroupTypeD;
            public uint offGroupTypeE;
            public uint offGroupTypeF;
            public uint offGroupTypeG;
            public uint offGroupTypeH;
            public ushort numGroupTypeA;
            public ushort numGroupTypeB;
            public ushort numGroupTypeC;
            public ushort numGroupTypeD;
            public ushort numGroupTypeE;
            public ushort numGroupTypeF;
            public ushort numGroupTypeG;
            public ushort numGroupTypeH;
            public uint pad0x48;
            public uint pad0x4C;
        }
        public struct CSKGroup
        {
            public ushort numIndices;
            public ushort unknown0x02;
            public ushort unknown0x04;
            public ushort unknown0x06;
            public uint unknown0x08;
            public uint unknown0x0C;
            public uint unknown0x10;
            public uint unknown0x14;
            public uint unknown0x18;
            public uint unknown0x1C;

            public uint[] indices;

            //NOT PART OF FILE FORMAT
            public uint groupType;
        }
        public struct CSKVertex
        {
            public float X;
            public float Y;
            public float Z;
            public float W;
        }
        #endregion
        #region Format Parameters
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".csk",
            },
            Type = FEType.Model,
            AllowExport = false,
            Validator = FFModelCSK.FileIsValid
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

        public Model LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }

        public Model LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(string filepath, Model data)
        {
            throw new NotImplementedException();
        }

        public static CSKModel ImportCSK(InputStream ins)
        {
            CSKModel csk = new CSKModel();

            try
            {
                long relativeLocation = ins.Position();

                //Read CSK Header
                csk.header = new CSKHeader
                {
                    numTotalGroup = ins.ReadUInt32(),
                    offGroups = ins.ReadUInt32(),
                    offVertices = ins.ReadUInt32(),
                    numVertex = ins.ReadUInt32(),
                    unknown0x10 = ins.ReadUInt32(),
                    length = ins.ReadUInt32(),
                    offGroupTypeA = ins.ReadUInt32(),
                    offGroupTypeB = ins.ReadUInt32(),
                    offGroupTypeC = ins.ReadUInt32(),
                    offGroupTypeD = ins.ReadUInt32(),
                    offGroupTypeE = ins.ReadUInt32(),
                    offGroupTypeF = ins.ReadUInt32(),
                    offGroupTypeG = ins.ReadUInt32(),
                    offGroupTypeH = ins.ReadUInt32(),
                    numGroupTypeA = ins.ReadUInt16(),
                    numGroupTypeB = ins.ReadUInt16(),
                    numGroupTypeC = ins.ReadUInt16(),
                    numGroupTypeD = ins.ReadUInt16(),
                    numGroupTypeE = ins.ReadUInt16(),
                    numGroupTypeF = ins.ReadUInt16(),
                    numGroupTypeG = ins.ReadUInt16(),
                    numGroupTypeH = ins.ReadUInt16(),
                    pad0x48 = ins.ReadUInt32(),
                    pad0x4C = ins.ReadUInt32()
                };

                //Read CSK Vertices
                csk.vertices = new CSKVertex[csk.header.numVertex];
                ins.Jump(relativeLocation + csk.header.offVertices);
                for(int i = 0; i < csk.header.numVertex; ++i)
                {
                    csk.vertices[i] = new CSKVertex
                    {
                        X = ins.ReadSingle(),
                        Y = ins.ReadSingle(),
                        Z = ins.ReadSingle(),
                        W = ins.ReadSingle()
                    };
                }
                ins.Return();

                //Read CSK A Groups
                csk.groups = new CSKGroup[csk.header.numTotalGroup];

                //Read A Groups
                ins.Jump(relativeLocation + csk.header.offGroupTypeA);
                for (int i = 0; i < csk.header.numGroupTypeA; ++i)
                {
                    csk.groups[i] = new CSKGroup
                    {
                        numIndices = ins.ReadUInt16(),
                        unknown0x02 = ins.ReadUInt16(),
                        unknown0x04 = ins.ReadUInt16(),
                        unknown0x06 = ins.ReadUInt16(),
                        unknown0x08 = ins.ReadUInt32(),
                        unknown0x0C = ins.ReadUInt32(),
                        unknown0x10 = ins.ReadUInt32(),
                        unknown0x14 = ins.ReadUInt32(),
                        unknown0x18 = ins.ReadUInt32(),
                        unknown0x1C = ins.ReadUInt32(),

                        groupType = 0   // 0 is group typeA
                    };

                    csk.groups[i].indices = new uint[csk.groups[i].numIndices];

                    //Read group indices
                    for(int j = 0; j < csk.groups[i].numIndices; ++j)
                    {
                        csk.groups[i].indices[j] = ins.ReadUInt16();
                        ins.ReadUInt16();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                    }
                }
                ins.Return();

                //Read B Groups
                ins.Jump(relativeLocation + csk.header.offGroupTypeB);
                for (int i = 0; i < csk.header.numGroupTypeB; ++i)
                {
                    csk.groups[i] = new CSKGroup
                    {
                        numIndices = ins.ReadUInt16(),
                        unknown0x02 = ins.ReadUInt16(),
                        unknown0x04 = ins.ReadUInt16(),
                        unknown0x06 = ins.ReadUInt16(),
                        unknown0x08 = ins.ReadUInt32(),
                        unknown0x0C = ins.ReadUInt32(),
                        unknown0x10 = ins.ReadUInt32(),
                        unknown0x14 = ins.ReadUInt32(),
                        unknown0x18 = ins.ReadUInt32(),
                        unknown0x1C = ins.ReadUInt32(),

                        groupType = 0   // 1 is group typeB
                    };

                    csk.groups[i].indices = new uint[csk.groups[i].numIndices];

                    //Read group indices
                    for (int j = 0; j < csk.groups[i].numIndices; ++j)
                    {
                        csk.groups[i].indices[j] = ins.ReadUInt16();
                        ins.ReadUInt16();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                    }
                }
                ins.Return();

                //Read C Groups
                ins.Jump(relativeLocation + csk.header.offGroupTypeC);
                for (int i = 0; i < csk.header.numGroupTypeC; ++i)
                {
                    csk.groups[i] = new CSKGroup
                    {
                        numIndices = ins.ReadUInt16(),
                        unknown0x02 = ins.ReadUInt16(),
                        unknown0x04 = ins.ReadUInt16(),
                        unknown0x06 = ins.ReadUInt16(),
                        unknown0x08 = ins.ReadUInt32(),
                        unknown0x0C = ins.ReadUInt32(),
                        unknown0x10 = ins.ReadUInt32(),
                        unknown0x14 = ins.ReadUInt32(),
                        unknown0x18 = ins.ReadUInt32(),
                        unknown0x1C = ins.ReadUInt32(),

                        groupType = 0   // 2 is group typeC
                    };

                    csk.groups[i].indices = new uint[csk.groups[i].numIndices];

                    //Read group indices
                    for (int j = 0; j < csk.groups[i].numIndices; ++j)
                    {
                        csk.groups[i].indices[j] = ins.ReadUInt16();
                        ins.ReadUInt16();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                    }
                }
                ins.Return();

                //Group Type D
                ins.Jump(relativeLocation + csk.header.offGroupTypeD);
                for (int i = 0; i < csk.header.numGroupTypeD; ++i)
                {
                    csk.groups[i] = new CSKGroup
                    {
                        numIndices = ins.ReadUInt16(),
                        unknown0x02 = ins.ReadUInt16(),
                        unknown0x04 = ins.ReadUInt16(),
                        unknown0x06 = ins.ReadUInt16(),
                        unknown0x08 = ins.ReadUInt32(),
                        unknown0x0C = ins.ReadUInt32(),
                        unknown0x10 = ins.ReadUInt32(),
                        unknown0x14 = ins.ReadUInt32(),
                        unknown0x18 = ins.ReadUInt32(),
                        unknown0x1C = ins.ReadUInt32(),

                        groupType = 2
                    };

                    csk.groups[i].indices = new uint[csk.groups[i].numIndices];

                    //Read group indices
                    for (int j = 0; j < csk.groups[i].numIndices; ++j)
                    {
                        csk.groups[i].indices[j] = ins.ReadUInt16();
                        ins.ReadUInt16();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                    }
                }
                ins.Return();

                //Group Type E
                ins.Jump(relativeLocation + csk.header.offGroupTypeE);
                for (int i = 0; i < csk.header.numGroupTypeE; ++i)
                {
                    csk.groups[i] = new CSKGroup
                    {
                        numIndices = ins.ReadUInt16(),
                        unknown0x02 = ins.ReadUInt16(),
                        unknown0x04 = ins.ReadUInt16(),
                        unknown0x06 = ins.ReadUInt16(),
                        unknown0x08 = ins.ReadUInt32(),
                        unknown0x0C = ins.ReadUInt32(),
                        unknown0x10 = ins.ReadUInt32(),
                        unknown0x14 = ins.ReadUInt32(),
                        unknown0x18 = ins.ReadUInt32(),
                        unknown0x1C = ins.ReadUInt32(),

                        groupType = 1
                    };

                    csk.groups[i].indices = new uint[csk.groups[i].numIndices];

                    //Read group indices
                    for (int j = 0; j < csk.groups[i].numIndices; ++j)
                    {
                        csk.groups[i].indices[j] = ins.ReadUInt16();
                        ins.ReadUInt16();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                        ins.ReadUInt32();
                    }
                }
                ins.Return();

                //seek to end of file
                ins.Seek(relativeLocation + csk.header.length, System.IO.SeekOrigin.Begin);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
            }

            return csk;
        }

        public static Model CSKToModel(CSKModel csk)
        {
            Model result = new Model();

            int[] groupColours = new int[]
            {
                result.AddUniqueColour(0.20392f, 0.59607f, 0.85882f, 1f),
                result.AddUniqueColour(0.94509f, 0.76862f, 0.05882f, 1f),
                result.AddUniqueColour(0.08627f, 0.62745f, 0.52156f, 1f),
            };

            foreach(CSKGroup group in csk.groups)
            {
                if(group.numIndices <= 0)
                {
                    continue;
                }

                Model.Mesh mesh = new Model.Mesh();
                mesh.position = Vector3f.Zero;
                mesh.rotation = Vector3f.Zero;
                mesh.scale = Vector3f.One;

                mesh.primitives = new Model.IPrimitiveType[group.numIndices - 2];
                mesh.textureSlot = -1;

                for(int i = 0; i < group.numIndices - 2; ++i)
                {
                    Model.TrianglePrimitive triangle = Model.TrianglePrimitive.New();

                    CSKVertex V1, V2, V3;
                    if ((i & 1) == 1)
                    {
                        V1 = csk.vertices[group.indices[i + 1]];
                        V2 = csk.vertices[group.indices[i + 0]];
                        V3 = csk.vertices[group.indices[i + 2]];
                    } else
                    {
                        V1 = csk.vertices[group.indices[i + 0]];
                        V2 = csk.vertices[group.indices[i + 1]];
                        V3 = csk.vertices[group.indices[i + 2]];
                    }

                    triangle.Indices[0] = result.AddUniqueVertex(V1.X, -V1.Y, -V1.Z);
                    triangle.Indices[1] = result.AddUniqueVertex(V2.X, -V2.Y, -V2.Z);
                    triangle.Indices[2] = result.AddUniqueVertex(V3.X, -V3.Y, -V3.Z);
                    triangle.Indices[3] = result.AddUniqueNormal(0f, 0f, 0f);
                    triangle.Indices[4] = triangle.Indices[3];
                    triangle.Indices[5] = triangle.Indices[3];
                    triangle.Indices[6] = result.AddUniqueTexcoord(0f, 0f);
                    triangle.Indices[7] = triangle.Indices[6];
                    triangle.Indices[8] = triangle.Indices[6];
                    triangle.Indices[9] = groupColours[group.groupType];
                    triangle.Indices[10] = groupColours[group.groupType];
                    triangle.Indices[11] = groupColours[group.groupType];

                    mesh.primitives[i] = triangle;
                }

                result.AddMesh(mesh);
            }

            return result;
        }
    }
}
