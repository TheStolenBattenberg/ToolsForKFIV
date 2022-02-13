using System.Collections.Generic;
using System.IO;
using System;

using KFIV.Utility.IO;
using KFIV.Utility.Type;
using KFIV.Utility.Maths;

namespace KFIV.Format.OM2
{
    public class OM2
    {
        #region Format Types
        public struct Header
        {
            public uint length { private set; get; }        //Total length of OM2
            public ushort numMesh { private set; get; }     //Total number of meshes
            public ushort numTristrip { private set; get; } //Total number of tristrips
            public ushort numTriangle { private set; get; } //Total number of triangles
            public ushort numVertex { private set; get; }   //Total number of vertices

            public void Read(InputStream ins)
            {
                length = ins.ReadUInt32();
                numMesh = ins.ReadUInt16();
                numTristrip = ins.ReadUInt16();
                numTriangle = ins.ReadUInt16();
                numVertex = ins.ReadUInt16();
                ins.ReadBytes(4);
            }
        }
        public struct Struct1
        { 
            public float ukn00 { private set; get; }
            public float ukn04 { private set; get; }
            public float ukn08 { private set; get; }
            public byte  ukn0C { private set; get; }

            public void Read(InputStream ins)
            {
                ukn00 = ins.ReadSingle();
                ukn04 = ins.ReadSingle();
                ukn08 = ins.ReadSingle();
                ukn0C = ins.ReadByte();
                ins.ReadBytes(3);
            }
        }
        public struct Vertex
        {
            public Vector4 Position { private set; get; }
            public Vector4 Normal { private set; get; }
            public Vector4 Texcoord { private set; get; }
            public Vector4 Colour { private set; get; }

            public void Read(InputStream ins)
            {
                Position = ins.ReadVector4s();
                Normal = ins.ReadVector4s();
                Texcoord = ins.ReadVector4s();
                Colour = ins.ReadVector4s();
            }
        }
        public struct Triangle
        {
            public uint A { private set; get; }
            public uint B { private set; get; }
            public uint C { private set; get; }

            public Triangle(uint a, uint b, uint c)
            {
                A = a;
                B = b;
                C = c;
            }
        }
        public struct Tristrip
        { 
            public byte[]    ukn00 { private set; get; }

            public DMAHeader ukn10 { private set; get; }
            public DMAPacket ukn20 { private set; get; }
            public DMAPacket ukn30 { private set; get; }
            public DMAPacket ukn40 { private set; get; }
            public byte[]    ukn50 { private set; get; }

            public DMAHeader dmaVertex { private set; get; }
            public byte numVertex { private set; get; }
            public byte[] ukn91 { private set; get; }

            //Data
            public List<Vertex>   vertices { private set; get; }
            public List<Triangle> triangles { private set; get; }

            public void Read(InputStream ins)
            {
                ukn00 = ins.ReadBytes(16);
                ukn10 = ins.ReadDMAHeader();
                ukn20 = ins.ReadDMAPacket();
                ukn30 = ins.ReadDMAPacket();
                ukn40 = ins.ReadDMAPacket();
                ukn50 = ins.ReadBytes(48);

                dmaVertex = ins.ReadDMAHeader();
                numVertex = ins.ReadByte();
                ukn91 = ins.ReadBytes(31);

                //Read Vertices
                vertices = new List<Vertex>(numVertex);
                for (int i = 0; i < numVertex; ++i)
                {
                    Vertex v = new Vertex();
                    v.Read(ins);

                    vertices.Add(v);
                }

                triangles = new List<Triangle>();
                for (uint i = 0; i < numVertex - 2; ++i)
                {
                    if ((i % 2) == 1)
                        triangles.Add(new Triangle(i + 1, i, i + 2));
                    else
                        triangles.Add(new Triangle(i, i + 1, i + 2));
                }

                ins.ReadBytes(16);
            }
        }
        public struct Mesh
        {
            public uint offTristrips { private set; get; }
            public ushort numTristrips { private set; get; }
            public byte ukn06 { private set; get; }
            public byte ukn07 { private set; get; }

            //Data
            public List<Tristrip> tristrips { private set; get; }

            public void Read(InputStream ins)
            {
                offTristrips = ins.ReadUInt32();
                numTristrips = ins.ReadUInt16();
                ukn06 = ins.ReadByte();
                ukn07 = ins.ReadByte();
                ins.ReadBytes(8);

                ins.Jump(offTristrips);
                {
                    tristrips = new List<Tristrip>();
                    for(int i = 0; i < numTristrips; ++i)
                    {
                        Tristrip ts = new Tristrip();
                        ts.Read(ins);

                        tristrips.Add(ts);
                    }
                }
                ins.Return();
            }
        }

        #endregion

        public Header header;
        public List<Struct1> struct1s;
        public List<Mesh> meshes;

        public static OM2 FromFile(string path)
        {
            //Open Stream
            InputStream ins = new InputStream(path);

            //Read OM2
            OM2 om2 = new OM2();
            om2.header = new Header();
            om2.header.Read(ins);

            om2.struct1s = new List<Struct1>(32);
            for(int i = 0; i < 32; ++i)
            {
                Struct1 s1 = new Struct1();
                s1.Read(ins);

                om2.struct1s.Add(s1);
            }

            om2.meshes = new List<Mesh>();
            for(int i = 0; i < om2.header.numMesh; ++i)
            {
                Mesh m = new Mesh();
                m.Read(ins);

                om2.meshes.Add(m);
            }

            return om2;
        }
     
        public void Save(string path)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                //Title Comment
                sw.WriteLine("# Converted by OM2toOBJ");
                sw.WriteLine("# https://github.com/TheStolenBattenberg/ToolsForKFIV");
                sw.WriteLine();

                //Data
                uint offIndex = 1;
                uint tspIndex = 0;
                uint grpIndex = 0;

                foreach(Mesh m in meshes)
                {
                    //Write Vertices
                    foreach(Tristrip ts in m.tristrips)
                    {
                        sw.WriteLine("# Mesh " + grpIndex.ToString() + "/"+"Tristrip "+ tspIndex.ToString()+". vertices (num = "+ts.numVertex.ToString()+")");

                        foreach(Vertex v in ts.vertices)
                        {
                            //Write Position
                            sw.Write("v ");
                            sw.Write(" " + (-v.Position.x).ToString());
                            sw.Write(" " + (-v.Position.y).ToString());
                            sw.Write(" " + v.Position.z.ToString());
                            sw.WriteLine();

                            //Write Normal
                            sw.Write("vn ");
                            sw.Write(" " + (-v.Normal.x).ToString());
                            sw.Write(" " + (-v.Normal.y).ToString());
                            sw.Write(" " + v.Normal.z.ToString());
                            sw.WriteLine();

                            //Write Texcoord
                            sw.Write("vt ");
                            sw.Write(" " + v.Texcoord.x.ToString());
                            sw.Write(" " + (1f - v.Texcoord.y).ToString());
                            sw.WriteLine();
                        }
                        sw.WriteLine();

                        tspIndex++;
                    }
                    tspIndex = 0;

                    //Write Triangles
                    sw.WriteLine("g Mesh" + grpIndex.ToString());
                    foreach(Tristrip ts in m.tristrips)
                    {
                        foreach(Triangle t in ts.triangles)
                        {
                            //Calculate Face Normal
                            Vector3 fA = ts.vertices[(int) t.A].Position.AsVec3;
                            Vector3 fB = ts.vertices[(int) t.B].Position.AsVec3;
                            Vector3 fC = ts.vertices[(int) t.C].Position.AsVec3;

                            Vector3 U = Vector3.Subtract(fB, fA);
                            Vector3 V = Vector3.Subtract(fC, fA);
                            Vector3 N = Vector3.Cross(U, V);

                            //Good enough to just use the first vertices normal
                            float d = Vector3.Dot(ts.vertices[(int)t.A].Normal.AsVec3, N);

                            //Flip triangles by seeing if they're backwards using the dotproduct
                            if (d > 0)
                            {
                                sw.Write("f");
                                sw.Write(" " + (offIndex + t.A).ToString());
                                sw.Write("/" + (offIndex + t.A).ToString());
                                sw.Write("/" + (offIndex + t.A).ToString());
                                sw.Write(" " + (offIndex + t.B).ToString());
                                sw.Write("/" + (offIndex + t.B).ToString());
                                sw.Write("/" + (offIndex + t.B).ToString());
                                sw.Write(" " + (offIndex + t.C).ToString());
                                sw.Write("/" + (offIndex + t.C).ToString());
                                sw.Write("/" + (offIndex + t.C).ToString());
                            }
                            else
                            {
                                sw.Write("f");
                                sw.Write(" " + (offIndex + t.C).ToString());
                                sw.Write("/" + (offIndex + t.C).ToString());
                                sw.Write("/" + (offIndex + t.C).ToString());
                                sw.Write(" " + (offIndex + t.B).ToString());
                                sw.Write("/" + (offIndex + t.B).ToString());
                                sw.Write("/" + (offIndex + t.B).ToString());
                                sw.Write(" " + (offIndex + t.A).ToString());
                                sw.Write("/" + (offIndex + t.A).ToString());
                                sw.Write("/" + (offIndex + t.A).ToString());
                            }
                            sw.WriteLine();
                        }
                        offIndex += ts.numVertex;
                    }
                    sw.WriteLine();

                    grpIndex++;
                }
            }
        }
    }
}
