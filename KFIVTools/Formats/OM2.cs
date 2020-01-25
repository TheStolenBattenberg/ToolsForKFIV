using System;
using System.IO;
using System.Collections.Generic;

namespace KFIV.Format.OM2
{
    public class OM2
    {
        #region Format Types
        public struct GIFPacket //PS2 Gif Packet for DMA
        {
            public byte[] data { private set; get; }
            public long tag { private set; get; }

            public GIFPacket(BinaryReader bin)
            {
                data = bin.ReadBytes(8);
                tag = bin.ReadInt64();
            }
        }
        public struct Vector4s  //float (single) component vector
        {
            public float x { private set; get; }
            public float y { private set; get; }
            public float z { private set; get; }
            public float w { private set; get; }

            public Vector4s(BinaryReader bin)
            {
                x = bin.ReadSingle();
                y = bin.ReadSingle();
                z = bin.ReadSingle();
                w = bin.ReadSingle();
            }
        }

        public struct Header
        { 
            public uint fileLength    { private set; get; }
            public ushort numMesh     { private set; get; }
            public ushort numTriStrip { private set; get; }
            public ushort numUkn08    { private set; get; }
            public ushort numVertex   { private set; get; }

            public Header(BinaryReader bin)
            {
                fileLength = bin.ReadUInt32();
                numMesh = bin.ReadUInt16();
                numTriStrip = bin.ReadUInt16();
                numUkn08 = bin.ReadUInt16();
                numVertex = bin.ReadUInt16();

                bin.ReadBytes(4);   //Padding
            }
        }
        public struct Struct1
        {
            public float ukn00 { private set; get; }
            public float ukn04 { private set; get; }
            public float ukn08 { private set; get; }
            public uint set    { private set; get; }

            public Struct1(BinaryReader bin)
            {
                ukn00 = bin.ReadSingle();
                ukn04 = bin.ReadSingle();
                ukn08 = bin.ReadSingle();
                set = bin.ReadUInt32();
            }

        }
        public struct Mesh
        {
            public uint offTriStrips  { private set; get; }
            public ushort numTriStrip { private set; get; }
            public ushort ukn06       { private set; get; }

            public List<TriStrip> triStrips;

            public Mesh(BinaryReader bin)
            {
                offTriStrips = bin.ReadUInt32();
                numTriStrip = bin.ReadUInt16();
                ukn06 = bin.ReadUInt16();

                bin.ReadBytes(8);   //Padding

                //seek to meshes
                long oldPos = bin.BaseStream.Position;
                bin.BaseStream.Seek(offTriStrips, SeekOrigin.Begin);

                //read meshes
                triStrips = new List<TriStrip>(numTriStrip);

                for (uint i = 0; i < numTriStrip; ++i)
                    triStrips.Add(new TriStrip(bin));

                //seek to previous position
                bin.BaseStream.Seek(oldPos, SeekOrigin.Begin);
            }
            public void Save(string path)
            {
                using (StreamWriter obj = new StreamWriter(File.Open(path, FileMode.Create)))
                {
                    obj.WriteLine("# OM2toOBJ Conversion\n");

                    uint num;
                    uint ind;

                    //Write Vertices
                    num = 0;
                    foreach(TriStrip m in triStrips)
                    {
                        foreach (Vertex v in m.vertices)
                        {
                            obj.Write("v");
                            obj.Write(" " + v.position.x.ToString());
                            obj.Write(" " + v.position.y.ToString());
                            obj.Write(" " + v.position.z.ToString());
                            obj.Write("\n");

                            num++;
                        }
                    }
                    obj.WriteLine("# Number of Vertices " + num.ToString());
                    obj.WriteLine("");

                    //Write Normals
                    num = 0;
                    foreach (TriStrip m in triStrips)
                    {
                        foreach (Vertex v in m.vertices)
                        {
                            obj.Write("vn");
                            obj.Write(" " + v.normal.x.ToString());
                            obj.Write(" " + v.normal.y.ToString());
                            obj.Write(" " + v.normal.z.ToString());
                            obj.Write("\n");

                            num++;
                        }
                    }
                    obj.WriteLine("# Number of Normals " + num.ToString());
                    obj.WriteLine("");

                    //Write texcoords
                    num = 0;
                    foreach (TriStrip m in triStrips)
                    {
                        foreach (Vertex v in m.vertices)
                        {
                            obj.Write("vt");
                            obj.Write(" " + v.texcoord.x.ToString());
                            obj.Write(" " + v.texcoord.y.ToString());
                            obj.Write("\n");

                            num++;
                        }
                    }
                    obj.WriteLine("# Number of Texcoords " + num.ToString());
                    obj.WriteLine("");

                    //Write groups
                    num = 0;
                    ind = 0;
                    foreach (TriStrip m in triStrips)
                    {
                        for(uint i = 0; i < (m.numVertex / 3); ++i)
                        {
                            obj.Write("f");
                            obj.Write(" " + (1 + (3 * ind) + 0).ToString());
                            obj.Write("/" + (1 + (3 * ind) + 0).ToString());
                            obj.Write("/" + (1 + (3 * ind) + 0).ToString());

                            obj.Write(" " + (1 + (3 * ind) + 1).ToString());
                            obj.Write("/" + (1 + (3 * ind) + 1).ToString());
                            obj.Write("/" + (1 + (3 * ind) + 1).ToString());

                            obj.Write(" " + (1 + (3 * ind) + 2).ToString());
                            obj.Write("/" + (1 + (3 * ind) + 2).ToString());
                            obj.Write("/" + (1 + (3 * ind) + 2).ToString());
                            obj.Write("\n");

                            ind++;
                        }
                        num++;
                    }
                }
            }
        }
        public struct Vertex
        {
            public Vector4s position { private set; get; }
            public Vector4s normal { private set; get; }
            public Vector4s texcoord { private set; get; }
            public Vector4s colour { private set; get; }

            public Vertex(BinaryReader bin)
            {
                position = new Vector4s(bin);
                normal = new Vector4s(bin);
                texcoord = new Vector4s(bin);
                colour = new Vector4s(bin);
            }
        }
        public struct TriStrip
        { 
            public byte[] ukn00    { private set; get; }
            public GIFPacket ukn20 { private set; get; }
            public GIFPacket ukn30 { private set; get; }
            public GIFPacket ukn40 { private set; get; }
            public byte[] ukn50    { private set; get; }
            public byte[] ukn90    { private set; get; }

            public uint numVertex  { private set; get; }
            public byte[] uknA1    { private set; get; }

            public List<Vertex> vertices { private set; get; }

            public TriStrip(BinaryReader bin)
            {
                ukn00 = bin.ReadBytes(32);
                ukn20 = new GIFPacket(bin);
                ukn30 = new GIFPacket(bin);
                ukn40 = new GIFPacket(bin);
                ukn50 = bin.ReadBytes(64);
                ukn90 = bin.ReadBytes(16);
                numVertex = bin.ReadByte();
                uknA1 = bin.ReadBytes(15);

                //Read Vertices
                List<Vertex> tmpVertex = new List<Vertex>();
                for (uint i = 0; i < numVertex; ++i)
                    tmpVertex.Add(new Vertex(bin));

                //Unstrip vertices
                vertices = new List<Vertex>();

                for(int i = 0; i < tmpVertex.Count - 2; ++i)
                {
                    if ((i % 2) == 0)
                    {
                        vertices.Add(tmpVertex[i + 1]);
                        vertices.Add(tmpVertex[i]);
                        vertices.Add(tmpVertex[i + 2]);
                    }
                    else
                    {
                        vertices.Add(tmpVertex[i]);
                        vertices.Add(tmpVertex[i + 1]);
                        vertices.Add(tmpVertex[i + 2]);
                    }
                }
                numVertex = (uint) vertices.Count;

                //Clear temporary vertices
                tmpVertex.Clear();

                //Padding?
                bin.ReadBytes(16);  //no idea whatsoever
            }
        }

        #endregion
        #region Data
        private Header header;
        private List<Struct1> struct1s;
        private List<Mesh>   models;
        
        #endregion

        public static OM2 FromFile(string path)
        {
            //Open File
            BinaryReader bin;
            try
            {
                bin = new BinaryReader(File.Open(path, FileMode.Open));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            OM2 om2 = new OM2();

            //Read OM2 Header
            om2.header = new Header(bin);

            //Read OM2 struct1s
            om2.struct1s = new List<Struct1>(32);

            for (uint i = 0; i < 32; ++i)
                om2.struct1s.Add(new Struct1(bin));

            //Read OM2 Models
            om2.models = new List<Mesh>(om2.header.numMesh);

            for (uint i = 0; i < om2.header.numMesh; ++i)
                om2.models.Add(new Mesh(bin));

            //Finish
            bin.Close();

            return om2;
        }

        public uint Count
        {
            get { return header.numMesh; }
        }
        public Mesh this[uint i]
        {
            get { return models[(int)i]; }
        }
    }
}
