using System;
using System.Collections.Generic;
using System.IO;

using KFIV.Utility.IO;
using KFIV.Utility.Type;
using KFIV.Utility.Math;

namespace KFIV.Format.OMD
{
    public class OMD
    {
        #region Format Types
        public struct Header
        {
            public uint lenFile { private set; get; }
            public ushort numMesh { private set; get; }
            public ushort numTriangle { private set; get; }
            public uint numVertex { private set; get; }
            public uint offUkn0C { private set; get; }

            public Header(InputStream bin)
            {
                lenFile     = bin.ReadUInt32();
                numMesh     = bin.ReadUInt16();
                numTriangle = bin.ReadUInt16();
                numVertex   = bin.ReadUInt32();
                offUkn0C    = bin.ReadUInt32();
            }
        }
        public struct Mesh
        {
            public Vector3 translation { private set; get; }
            public Vector3 rotation { private set; get; }
            public Vector3 scale { private set; get; }
            public uint offTristrips { private set; get; }
            public uint numTristrips { private set; get; }
            public uint ukn2C { private set; get; }

            public List<Tristrip> tristrips;

            public Mesh(InputStream bin)
            {
                translation = bin.ReadVector3s();
                rotation = bin.ReadVector3s();
                scale = bin.ReadVector3s();
                offTristrips = bin.ReadUInt32();
                numTristrips = bin.ReadUInt32();
                ukn2C = bin.ReadUInt32();

                //Read Tristrips
                tristrips = new List<Tristrip>((int) numTristrips);

                bin.Jump(offTristrips);
                {
                    for(uint i = 0; i < numTristrips; ++i)
                    {
                        tristrips.Add(new Tristrip(bin));
                    }
                }
                bin.Return();
            }
        }
        public struct Vertex
        {
            public Vector4 Position { private set; get; }
            public Vector4 Normal { private set; get; }
            public Vector4 Texcoord { private set; get; }
            public Vector4 Colour { private set; get; }

            public Vertex(InputStream bin)
            {
                Position = bin.ReadVector4h(256f);
                Normal = bin.ReadVector4h(4096f);
                Texcoord = bin.ReadVector4h(4096f);
                Colour = bin.ReadVector4h(1f);
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

        #endregion
        #region Data
        public Header header;
        public List<Mesh> meshes;

        #endregion

        public static OMD FromFile(string path)
        {
            //Open Stream
            InputStream bin = new InputStream(path);

            //Create OMD
            OMD omd = new OMD();

            //Read OMD Header
            omd.header = new Header(bin);

            //Read OMD Meshes
            omd.meshes = new List<Mesh>(omd.header.numMesh);

            for (uint i = 0; i < omd.header.numMesh; ++i)
                omd.meshes.Add(new Mesh(bin));

            return omd;
        }
        public void Save(string path)
        {
            using (StreamWriter obj = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                //Write title comment
                obj.WriteLine("# OMDtoOBJ Conversion");
                obj.WriteLine();

                uint num = 0;

                //Write vertices
                foreach(Mesh m in meshes)
                {
                    foreach(Tristrip ts in m.tristrips)
                    {
                        foreach(Vertex v in ts.vertices)
                        {
                            obj.Write("v");
                            obj.Write(" " + (-v.Position.x).ToString());
                            obj.Write(" " + (-v.Position.y).ToString());
                            obj.Write(" " + v.Position.z.ToString());
                            obj.WriteLine();

                            num++;
                        }
                    }
                }
                obj.WriteLine("# Vertex count " + num.ToString());
                obj.WriteLine();
                num = 0;

                //Write normals
                foreach(Mesh m in meshes)
                {
                    foreach(Tristrip ts in m.tristrips)
                    {
                        foreach(Vertex v in ts.vertices)
                        {
                            obj.Write("vn");
                            obj.Write(" " + (-v.Normal.x).ToString());
                            obj.Write(" " + (-v.Normal.y).ToString());
                            obj.Write(" " + v.Normal.z.ToString());
                            obj.WriteLine();

                            num++;
                        }
                    }
                }
                obj.WriteLine("# Normal count " + num.ToString());
                obj.WriteLine();
                num = 0;

                //Write texcoords
                foreach(Mesh m in meshes)
                {
                    foreach(Tristrip ts in m.tristrips)
                    {
                        foreach(Vertex v in ts.vertices)
                        {
                            obj.Write("vt");
                            obj.Write(" " + v.Texcoord.x.ToString());
                            obj.Write(" " + (-v.Texcoord.y).ToString());
                            obj.WriteLine();

                            num++;
                        }
                    }
                }
                obj.WriteLine("# Texcoord count " + num.ToString());
                obj.WriteLine();
                num = 0;

                //Write Meshes
                uint ind = 1;
                foreach(Mesh m in meshes)
                {        
                    obj.WriteLine("g Mesh" + num.ToString());
                    foreach (Tristrip ts in m.tristrips)
                    {
                        foreach (Triangle t in ts.triangles)
                        {
                            //Calculate Face Normal
                            Vector3 fA = ts.vertices[(int)t.A].Position.AsVec3;
                            Vector3 fB = ts.vertices[(int)t.B].Position.AsVec3;
                            Vector3 fC = ts.vertices[(int)t.C].Position.AsVec3;

                            Vector3 U = Vector3.Subtract(fB, fA);
                            Vector3 V = Vector3.Subtract(fC, fA);
                            Vector3 N = Vector3.Cross(U, V);

                            //Good enough to just use the first vertices normal
                            float d = Vector3.Dot(ts.vertices[(int)t.A].Normal.AsVec3, N);

                            //Flip triangles by seeing if they're backwards using the dotproduct
                            if (d > 0f)
                            {
                                obj.Write("f");
                                obj.Write(" " + (ind + t.A).ToString());
                                obj.Write("/" + (ind + t.A).ToString());
                                obj.Write("/" + (ind + t.A).ToString());
                                obj.Write(" " + (ind + t.B).ToString());
                                obj.Write("/" + (ind + t.B).ToString());
                                obj.Write("/" + (ind + t.B).ToString());
                                obj.Write(" " + (ind + t.C).ToString());
                                obj.Write("/" + (ind + t.C).ToString());
                                obj.Write("/" + (ind + t.C).ToString());
                                obj.WriteLine();
                            }
                            else
                            {
                                obj.Write("f");
                                obj.Write(" " + (ind + t.C).ToString());
                                obj.Write("/" + (ind + t.C).ToString());
                                obj.Write("/" + (ind + t.C).ToString());
                                obj.Write(" " + (ind + t.B).ToString());
                                obj.Write("/" + (ind + t.B).ToString());
                                obj.Write("/" + (ind + t.B).ToString());
                                obj.Write(" " + (ind + t.A).ToString());
                                obj.Write("/" + (ind + t.A).ToString());
                                obj.Write("/" + (ind + t.A).ToString());
                                obj.WriteLine();
                            }
                        }
                        ind += ts.numVertex;

                    }
                    obj.WriteLine();

                    num++;
                }
            }
        }
    }
}
