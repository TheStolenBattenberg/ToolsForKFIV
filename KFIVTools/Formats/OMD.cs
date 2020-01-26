using System;
using System.Collections.Generic;
using System.IO;

using KFIV.Utility.IO;
using KFIV.Utility.Type;

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
            public byte[] ukn00 { private set; get; }
            public uint   ukn0C { private set; get; }
            public float  ukn10 { private set; get; }
            public float  ukn14 { private set; get; }
            public float  ukn18 { private set; get; }
            public float  ukn1C { private set; get; }
            public float  ukn20 { private set; get; }
            public uint offTristrips { private set; get; }
            public uint numTristrips { private set; get; }
            public uint ukn2C { private set; get; }

            public List<Tristrip> tristrips;

            public Mesh(InputStream bin)
            {
                ukn00 = bin.ReadBytes(12);
                ukn0C = bin.ReadUInt32();
                ukn10 = bin.ReadSingle();
                ukn14 = bin.ReadSingle();
                ukn18 = bin.ReadSingle();
                ukn1C = bin.ReadSingle();
                ukn20 = bin.ReadSingle();
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
            public Vector4 position { private set; get; }
            public Vector4 normal { private set; get; }
            public Vector4 texcoord { private set; get; }
            public Vector4 colour { private set; get; }

            public Vertex(InputStream bin)
            {
                position = bin.ReadVector4f();
                normal = bin.ReadVector4f();
                texcoord = bin.ReadVector4f();
                colour = bin.ReadVector4f();
            }
        }
        public struct Triangle
        {
            public uint ind1 { private set; get; }
            public uint ind2 { private set; get; }
            public uint ind3 { private set; get; }

            public Triangle(uint x, uint y, uint z)
            {
                ind1 = x;
                ind2 = y;
                ind3 = z;
            }
        }
        public struct Tristrip
        {
            public byte[] ukn00 { private set; get; }
            public GIFPacket ukn20 { private set; get; }
            public GIFPacket ukn30 { private set; get; }
            public GIFPacket ukn40 { private set; get; }
            public GIFPacket ukn50 { private set; get; }
            public byte[] ukn60 { private set; get; }
            public uint numVertex { private set; get; }
            public byte[] ukn71 { private set; get; }

            public List<Vertex>   vertices;
            public List<Triangle> triangles;

            public Tristrip(InputStream bin)
            {
                ukn00 = bin.ReadBytes(32);
                ukn20 = bin.ReadGIFPacket();
                ukn30 = bin.ReadGIFPacket();
                ukn40 = bin.ReadGIFPacket();
                ukn50 = bin.ReadGIFPacket();
                ukn60 = bin.ReadBytes(16);
                numVertex = bin.ReadByte();
                ukn71 = bin.ReadBytes(31);

                //Read vertices for this tristrip
                vertices = new List<Vertex>();

                for(uint i = 0; i < numVertex; ++i)
                    vertices.Add(new Vertex(bin));

                //Skip 16 unknown bytes
                bin.ReadBytes(16);

                //Deconstruct Tristrip to triangles
                triangles = new List<Triangle>();

                for (int i = 0; i < numVertex - 2; i++)
                {
                    if ((i % 2) == 1)
                        triangles.Add(new Triangle((uint)i + 1, (uint)i, (uint)i + 2));
                    else
                        triangles.Add(new Triangle((uint)i, (uint)i + 1, (uint)i + 2));
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
                            obj.Write(" " + v.position.z.ToString());
                            obj.Write(" " + (-v.position.y).ToString());
                            obj.Write(" " + v.position.x.ToString());
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
                            obj.Write(" " + v.normal.x.ToString());
                            obj.Write(" " + (-v.normal.y).ToString());
                            obj.Write(" " + v.normal.z.ToString());
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
                            obj.Write(" " + v.texcoord.x.ToString());
                            obj.Write(" " + (-v.texcoord.y).ToString());
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
                            obj.Write("f");
                            obj.Write(" " + (ind + t.ind1).ToString());
                            obj.Write("/" + (ind + t.ind1).ToString());
                            obj.Write("/" + (ind + t.ind1).ToString());
                            obj.Write(" " + (ind + t.ind2).ToString());
                            obj.Write("/" + (ind + t.ind2).ToString());
                            obj.Write("/" + (ind + t.ind2).ToString());
                            obj.Write(" " + (ind + t.ind3).ToString());
                            obj.Write("/" + (ind + t.ind3).ToString());
                            obj.Write("/" + (ind + t.ind3).ToString());
                            obj.WriteLine();
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
