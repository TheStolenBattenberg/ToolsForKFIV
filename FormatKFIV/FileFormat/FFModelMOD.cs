using System;
using System.Collections.Generic;

using FormatKFIV.Utility;
using FormatKFIV.Asset;


namespace FormatKFIV.FileFormat
{
    public class FFModelMOD : FIFormat<Model>
    {
        #region Format Structures
        public struct MODHeader
        {
            public uint modelLength;
            public uint modelOffset;
            public uint textureLength;
            public uint textureOffset;
        }
        public struct MODModelHeader
        {
            public uint modelLength;
            public uint Unknown0x04;
            public uint Unknown0x08;
            public uint Unknown0x0C;

            public Vector4f Translation;
            public Vector4f Rotation;
            public Vector4f Scale;

            public uint numMesh;
            public uint offMesh;
            public uint numVertex;
            public uint offVertex;
            public uint numNormal;
            public uint offNormal;
            public uint Unknown0x58;
            public uint Unknown0x5C;
        }
        public struct MODModelMesh
        {
            public ushort numVertex;
            public ushort Unknown0x02;
            public uint Unknown0x04;
            public ulong Unknown0x08;
            public ulong Unknown0x10;
            public ulong Unknown0x18;

            public MODModelVertex[] vertices;
        }
        public struct MODModelVertex
        {
            public ushort PId;
            public ushort NId;
            public byte CR;
            public byte CG;
            public byte CB;
            public byte CA;
            public float U;
            public float V;
            public uint Unknown0x10;
            public uint Unknown0x14;
            public uint Unknown0x18;
            public uint Unknown0x1C;
        }
        public struct MODModelVec4
        {
            public short X, Y, Z, W;
        }

        #endregion
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".mod",
            },
            Type = FEType.Model,
            AllowExport = false,
            Validator = FFModelMOD.FileIsValid
        };

        /// <summary>Validates a file to see if it is PS2 ICO Format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {

                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    Console.WriteLine(Ex.StackTrace);
                    return false;
                }
            }

            return validFile;
        }

        #endregion

        public Model LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            Model M = null;
            Texture T = null;

            using (InputStream ins = new InputStream(filepath))
            {
                M = ImportMOD(ins, out T);
            }

            ret2 = T;
            ret3 = null;
            ret4 = null;
            return M;
        }

        public Model LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            Model M = null;
            Texture T = null;

            using (InputStream ins = new InputStream(buffer))
            {
                M = ImportMOD(ins, out T);
            }

            ret2 = T;
            ret3 = null;
            ret4 = null;
            return M;
        }

        public Model ImportMOD(InputStream ins, out Texture tex)
        {
            Model   OutM = new Model();
            Texture OutT = new Texture();

            try
            {
                //Read main MOD Header
                MODHeader header = new MODHeader
                {
                    modelLength = ins.ReadUInt32(),
                    modelOffset = ins.ReadUInt32(),
                    textureLength = ins.ReadUInt32(),
                    textureOffset = ins.ReadUInt32()
                };

                //Read the contained model
                ins.Jump(header.modelOffset);
                MODModelHeader modelHeader = new MODModelHeader
                {
                    modelLength = ins.ReadUInt32(),
                    Unknown0x04 = ins.ReadUInt32(),
                    Unknown0x08 = ins.ReadUInt32(),
                    Unknown0x0C = ins.ReadUInt32(),
                    Translation = ins.ReadVector4f(),
                    Rotation = ins.ReadVector4f(),
                    Scale = ins.ReadVector4f(),
                    numMesh = ins.ReadUInt32(),
                    offMesh = ins.ReadUInt32(),
                    numVertex = ins.ReadUInt32(),
                    offVertex = ins.ReadUInt32(),
                    numNormal = ins.ReadUInt32(),
                    offNormal = ins.ReadUInt32(),
                    Unknown0x58 = ins.ReadUInt32(),
                    Unknown0x5C = ins.ReadUInt32()
                };

                //Read de_vertices
                List<Vector4f> vertices = new List<Vector4f>();
                ins.Jump(header.modelOffset + modelHeader.offVertex);
                for(int i = 0; i < modelHeader.numVertex; ++i)
                {
                    vertices.Add(ins.ReadVector4f());
                }
                ins.Return();

                //Read de_normals
                List<Vector4f> normals = new List<Vector4f>();
                ins.Jump(header.modelOffset + modelHeader.offNormal);
                for(int i = 0; i < modelHeader.numNormal; ++i)
                {
                    normals.Add(ins.ReadVector4f());
                }
                ins.Return();

                //Read meshes
                List<MODModelMesh> meshes = new List<MODModelMesh>();
                ins.Jump(header.modelOffset + modelHeader.offMesh);
                for(int i = 0; i < modelHeader.numMesh; ++i)
                {
                    MODModelMesh modMesh = new MODModelMesh
                    {
                        numVertex = ins.ReadUInt16(),
                        Unknown0x02 = ins.ReadUInt16(),
                        Unknown0x04 = ins.ReadUInt32(),
                        Unknown0x08 = ins.ReadUInt64(),
                        Unknown0x10 = ins.ReadUInt64(),
                        Unknown0x18 = ins.ReadUInt64()
                    };
                    modMesh.vertices = new MODModelVertex[modMesh.numVertex];

                    for(int j = 0; j < modMesh.numVertex; ++j)
                    {
                        modMesh.vertices[j] = new MODModelVertex
                        {
                            PId = ins.ReadUInt16(),
                            NId = ins.ReadUInt16(),
                            CR = ins.ReadByte(),
                            CG = ins.ReadByte(),
                            CB = ins.ReadByte(),
                            CA = ins.ReadByte(),
                            U = ins.ReadSingle(),
                            V = ins.ReadSingle(),
                            Unknown0x10 = ins.ReadUInt32(),
                            Unknown0x14 = ins.ReadUInt32(),
                            Unknown0x18 = ins.ReadUInt32(),
                            Unknown0x1C = ins.ReadUInt32()
                        };

                        Console.WriteLine(modMesh.vertices[j].Unknown0x14);
                    }

                    meshes.Add(modMesh);
                }

                ins.Return();

                //Read the contained texture
                ins.Jump(header.textureOffset);
                FFTextureTM2 tm2Handler = new FFTextureTM2();
                OutT = tm2Handler.LoadFromMemory(ins.ReadBytes((int)header.textureLength), out _, out _, out _);
                ins.Return();

                //Do MOD -> Model
                List<Model.Triangle> triangles = new List<Model.Triangle>();
                Model.Mesh mdlMesh = new Model.Mesh();

                for (int i = 0; i < meshes.Count; ++i)
                {
                    MODModelMesh modMesh = meshes[i];

                    //Add Vertices, Normals Etc...
                    for(int j = 0; j < modMesh.numVertex-2; ++j)
                    {
                        MODModelVertex V1, V2, V3;
                        Vector4f P1, P2, P3;
                        Vector4f N1, N2, N3;

                        if ((j & 1) == 1)
                        {
                            V1 = modMesh.vertices[j + 1];
                            V2 = modMesh.vertices[j + 0];
                            V3 = modMesh.vertices[j + 2];
                            P1 = vertices[V1.PId];
                            P2 = vertices[V2.PId];
                            P3 = vertices[V3.PId];
                            N1 = normals[V1.NId];
                            N2 = normals[V2.NId];
                            N3 = normals[V3.NId];
                        }
                        else
                        {
                            V1 = modMesh.vertices[j + 1];
                            V2 = modMesh.vertices[j + 0];
                            V3 = modMesh.vertices[j + 2];
                            P1 = vertices[V1.PId];
                            P2 = vertices[V2.PId];
                            P3 = vertices[V3.PId];
                            N1 = normals[V1.NId];
                            N2 = normals[V2.NId];
                            N3 = normals[V3.NId];
                        }

                        Model.Triangle tri = new Model.Triangle
                        {
                            vIndices = new ushort[3],
                            nIndices = new ushort[3],
                            tIndices = new ushort[3],
                            cIndices = new ushort[3]
                        };

                        tri.vIndices[0] = (ushort)OutM.AddVertex(P1.X / 64f, -(P1.Y / 64f), P1.Z / 64f);
                        tri.vIndices[1] = (ushort)OutM.AddVertex(P2.X / 64f, -(P2.Y / 64f), P2.Z / 64f);
                        tri.vIndices[2] = (ushort)OutM.AddVertex(P3.X / 64f, -(P3.Y / 64f), P3.Z / 64f);
                        tri.nIndices[0] = (ushort)OutM.AddNormal(N1.X, N1.Y, N1.Z);
                        tri.nIndices[1] = (ushort)OutM.AddNormal(N2.X, N2.Y, N2.Z);
                        tri.nIndices[2] = (ushort)OutM.AddNormal(N3.X, N3.Y, N3.Z);
                        tri.tIndices[0] = (ushort)OutM.AddTexcoord(V1.U, V1.V);
                        tri.tIndices[1] = (ushort)OutM.AddTexcoord(V2.U, V2.V);
                        tri.tIndices[2] = (ushort)OutM.AddTexcoord(V3.U, V3.V);
                        tri.cIndices[0] = (ushort)OutM.AddColour(1f, 1f, 1f, 1f);
                        tri.cIndices[1] = (ushort)OutM.AddColour(1f, 1f, 1f, 1f);
                        tri.cIndices[2] = (ushort)OutM.AddColour(1f, 1f, 1f, 1f);

                        Vector3f FaceNormal = Model.GenerateFaceNormal(
                            new Vector3f(P1.X, P1.Y, P1.Z),
                            new Vector3f(P2.X, P2.Y, P2.Z),
                            new Vector3f(P3.X, P3.Y, P3.Z));

                        Vector3f AverageNorm = Vector3f.Normalize(Vector3f.Average(
                            new Vector3f(N1.X, N1.Y, N1.Z), 
                            new Vector3f(N2.X, N2.Y, N2.Z),
                            new Vector3f(N3.X, N3.Y, N3.Z)));

                        if (Vector3f.Dot(AverageNorm, FaceNormal) > 0)
                            tri.FlipIndices();

                        triangles.Add(tri);
                    }

                }

                mdlMesh.triangles = triangles.ToArray();
                mdlMesh.numTriangle = (uint)triangles.Count;
                OutM.AddMesh(mdlMesh);

            } catch(Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                tex  = null;
                return null;
            }

            tex = OutT;
            return OutM;
        }

        public void SaveToFile(string filepath, Model data)
        {
            throw new NotImplementedException();
        }
    }
}
