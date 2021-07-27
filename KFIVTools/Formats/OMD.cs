using System;
using System.Collections.Generic;

using KFIV.Utility.IO;
using KFIV.Utility.Math;

namespace KFIV.Format.OMD
{
    public class OMD
    {
        /**
         * This region contains all structures and types of the OM2 Format.
        **/
        #region Format Types
        public struct OMDHeader
        {
            public uint EoF;
            public ushort numMesh;
            public ushort numTriangle;
            public uint numVertex;
            public uint offStructA;

            public OMDHeader(InputStream ins)
            {
                EoF = ins.ReadUInt32();
                numMesh = ins.ReadUInt16();
                numTriangle = ins.ReadUInt16();
                numVertex = ins.ReadUInt32();
                offStructA = ins.ReadUInt32();
            }
        };
        public struct OMDStructA
        {
            public uint unknown0x00;
            public uint unknown0x04;
            public uint unknown0x08;
            public uint unknown0x0c;
        };
        public struct OMDMesh
        {
            public Vector3 translationXYZ;
            public Vector3 rotationXYZ;
            public Vector3 scaleXYZ;
            public uint offTristrip;
            public uint numTristrip;
            public uint unknown0x2c;

            public List<OMDTristrip> tristrip;

            public OMDMesh(InputStream ins)
            {
                translationXYZ = ins.ReadVector3s();
                rotationXYZ = ins.ReadVector3s();
                scaleXYZ = ins.ReadVector3s();
                offTristrip = ins.ReadUInt32();
                numTristrip = ins.ReadUInt32();
                unknown0x2c = ins.ReadUInt32();

                tristrip = new List<OMDTristrip>();
            }
        }
        public struct OMDTristrip
        {
            public uint unknown0x00;
            public uint unknown0x04;
            public uint unknown0x08;
            public uint unknown0x0c;
            public uint unknown0x10;
            public uint unknown0x14;
            public uint unknown0x18;
            public uint unknown0x1c;
            public uint unknown0x20;
            public uint unknown0x24;
            public uint unknown0x28;
            public uint unknown0x2c;
            public uint unknown0x30;
            public uint unknown0x34;
            public uint unknown0x38;
            public uint unknown0x3c;
            public uint unknown0x40;
            public uint unknown0x44;
            public uint unknown0x48;
            public uint unknown0x4c;
            public uint unknown0x50;
            public uint unknown0x54;
            public uint unknown0x58;
            public uint unknown0x5c;
            public uint unknown0x60;
            public uint unknown0x64;
            public uint unknown0x68;
            public uint unknown0x6c;
            public byte numVertex;
            public byte unknown0x71;
            public ushort unknown0x72;
            public uint unknown0x74;
            public uint unknown0x78;
            public uint unknown0x7c;
            public uint unknown0x80;
            public uint unknown0x84;
            public uint unknown0x88;
            public uint unknown0x8c;

            public List<OMDVertex> vertices;

            public OMDTristrip(InputStream ins)
            {
                unknown0x00 = ins.ReadUInt32();
                unknown0x04 = ins.ReadUInt32();
                unknown0x08 = ins.ReadUInt32();
                unknown0x0c = ins.ReadUInt32();
                unknown0x10 = ins.ReadUInt32();
                unknown0x14 = ins.ReadUInt32();
                unknown0x18 = ins.ReadUInt32();
                unknown0x1c = ins.ReadUInt32();
                unknown0x20 = ins.ReadUInt32();
                unknown0x24 = ins.ReadUInt32();
                unknown0x28 = ins.ReadUInt32();
                unknown0x2c = ins.ReadUInt32();
                unknown0x30 = ins.ReadUInt32();
                unknown0x34 = ins.ReadUInt32();
                unknown0x38 = ins.ReadUInt32();
                unknown0x3c = ins.ReadUInt32();
                unknown0x40 = ins.ReadUInt32();
                unknown0x44 = ins.ReadUInt32();
                unknown0x48 = ins.ReadUInt32();
                unknown0x4c = ins.ReadUInt32();
                unknown0x50 = ins.ReadUInt32();
                unknown0x54 = ins.ReadUInt32();
                unknown0x58 = ins.ReadUInt32();
                unknown0x5c = ins.ReadUInt32();
                unknown0x60 = ins.ReadUInt32();
                unknown0x64 = ins.ReadUInt32();
                unknown0x68 = ins.ReadUInt32();
                unknown0x6c = ins.ReadUInt32();
                numVertex = ins.ReadByte();
                unknown0x71 = ins.ReadByte();
                unknown0x72 = ins.ReadUInt16();
                unknown0x74 = ins.ReadUInt32();
                unknown0x78 = ins.ReadUInt32();
                unknown0x7c = ins.ReadUInt32();
                unknown0x80 = ins.ReadUInt32();
                unknown0x84 = ins.ReadUInt32();
                unknown0x88 = ins.ReadUInt32();
                unknown0x8c = ins.ReadUInt32();

                vertices = new List<OMDVertex>();
            }
        }
        public struct OMDVertex
        {
            public Vector4 PositionXYZp;
            public Vector4 NormalXYZp;
            public Vector4 TexcoordUVpp;
            public Vector4 ColourRGBA;

            public OMDVertex(InputStream ins)
            {
                PositionXYZp = ins.ReadVector4h(1024f);
                NormalXYZp = ins.ReadVector4h(4096f);
                TexcoordUVpp = ins.ReadVector4h(4096f);
                ColourRGBA = ins.ReadVector4h(1f);
            }
        }
        #endregion

        #region Data
        OMDHeader header;
        List<OMDMesh> meshes;

        #endregion

        /*
        public struct Tristrip
        {
            public byte[] ukn00 { private set; get; }
            public DMAHeader ukn10 { private set; get; }
            public DMAPacket ukn20 { private set; get; }
            public DMAPacket ukn30 { private set; get; }
            public DMAPacket ukn40 { private set; get; }
            public DMAPacket ukn50 { private set; get; }
            public DMAHeader ukn60 { private set; get; }
            public uint numVertex { private set; get; }
            public byte[] ukn71 { private set; get; }
            public DMAHeader ukn80 { private set; get; }

            public List<Vertex>   vertices;
            public List<Triangle> triangles;

            public Tristrip(InputStream bin)
            {
                ukn00 = bin.ReadBytes(16);
                ukn10 = bin.ReadDMAHeader();
                ukn20 = bin.ReadDMAPacket();
                ukn30 = bin.ReadDMAPacket();
                ukn40 = bin.ReadDMAPacket();
                ukn50 = bin.ReadDMAPacket();
                ukn60 = bin.ReadDMAHeader();
                numVertex = bin.ReadByte();
                ukn71 = bin.ReadBytes(15);
                ukn80 = bin.ReadDMAHeader();

                uint i = 0;

                //Read vertices for this tristrip
                vertices = new List<Vertex>();

                List<uint> breaks = new List<uint>();

                for(i = 0; i < numVertex; ++i)
                {
                    Vertex v = new Vertex(bin);

                    //Need to make sure we haven't already added this break by checking
                    //the last position too
                    if (v.Normal.w <= -7.9999f && !breaks.Contains(i-1))
                        breaks.Add(i);

                    vertices.Add(v);
                }

                //Skip 16 unknown bytes
                bin.ReadBytes(16);


                //Deconstruct Tristrip to triangles
                triangles = new List<Triangle>();

                i = 0;
                while (i < (numVertex - 2))
                {
                    if ((i % 2) == 1)
                        triangles.Add(new Triangle(i + 1, i, i + 2));
                    else
                        triangles.Add(new Triangle(i, i + 1, i + 2));

                    //Special condition to split tristrip on break
                    i++;
                    if (breaks.Contains(i+2))
                        i += 2;
                }
            }
        }
        */

        public static OMD FromFile(string path)
        {
            OMD omd;
            using(InputStream ins = new InputStream(path))
            {
                omd = ReadOMDData(ins);
            }

            return omd;
        }
        public static OMD FromMemory(byte[] memory)
        {
            OMD omd;
            using (InputStream ins = new InputStream(memory))
            {
                omd = ReadOMDData(ins);
            }

            return omd;
        }

        private static OMD ReadOMDData(InputStream ins)
        {
            OMD omd = new OMD();
            omd.meshes = new List<OMDMesh>();

            //
            // Read OMD Header
            //
            omd.header = new OMDHeader(ins);

            //
            // Read OMD StructA
            //
            if(omd.header.offStructA == 0)
                Console.WriteLine("StructA not present");
            else
                Console.WriteLine("StructA is present");

            //
            // Read OMD Meshes
            //
            for(uint i = 0; i < omd.header.numMesh; ++i)
            {
                OMDMesh mesh = new OMDMesh(ins);

                ins.Jump(mesh.offTristrip);
                for(uint j = 0; j < mesh.numTristrip; ++j)
                {
                    OMDTristrip tristrip = new OMDTristrip(ins);

                    for(uint k = 0; k < tristrip.numVertex; ++k)
                        tristrip.vertices.Add(new OMDVertex(ins));

                    ins.ReadBytes(0x10);

                    mesh.tristrip.Add(tristrip);
                }
                ins.Return();

                omd.meshes.Add(mesh);

                if (mesh.unknown0x2c != 0)
                    Console.WriteLine("OMDMesh::unknown0x2c isn't always 0");
            }
            return omd;
        }

        public void Save(string path)
        {
            OBJ.OBJ obj = new OBJ.OBJ();

            //
            // Copy Vertices, Normals and Texcoords
            // Build faces
            //
            uint groupID = 1;
            string groupName = "";
            int tInd = 1;
            int oInd = 0; 

            foreach(OMDMesh mesh in meshes)
            {
                // Make group name
                groupName = "Mesh_" + groupID.ToString("D4");

                obj.AddGroup(groupName);

                foreach (OMDTristrip tristrip in mesh.tristrip)
                {
                    // Copy Vertices, Normal, Texcoords
                    foreach(OMDVertex vertex in tristrip.vertices)
                    {
                        obj.AddVertex(-vertex.PositionXYZp.x, -vertex.PositionXYZp.y, vertex.PositionXYZp.z);
                        obj.AddNormal(-vertex.NormalXYZp.x, -vertex.NormalXYZp.y, vertex.NormalXYZp.z);
                        obj.AddTexcoord(vertex.TexcoordUVpp.x, 1.0f - vertex.TexcoordUVpp.y);
                    }

                    // Build Faces
                    for(int i = 0; i < tristrip.numVertex-2; ++i)
                    {
                        oInd = tInd + i;

                        Vector3 FN = OBJ.OBJ.GenerateFaceNormal(
                            tristrip.vertices[i].PositionXYZp.AsVec3, 
                            tristrip.vertices[i+1].PositionXYZp.AsVec3, 
                            tristrip.vertices[i+2].PositionXYZp.AsVec3);

                        float d = Vector3.Dot(tristrip.vertices[i].NormalXYZp.AsVec3, FN);

                        if (d < 0.0)
                            obj.AddFace(groupName, oInd + 1, oInd, oInd + 2, oInd + 1, oInd, oInd + 2, oInd + 1, oInd, oInd + 2);
                        else
                            obj.AddFace(groupName, oInd, oInd + 1, oInd + 2, oInd, oInd + 1, oInd + 2, oInd, oInd + 1, oInd + 2);

                    }

                    tInd += tristrip.numVertex;
                }

                groupID++;
            }

            obj.Save(path);
        }
    }
}
