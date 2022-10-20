using System;
using System.IO;
using System.Globalization;

using FormatKFIV.Asset;

namespace FormatKFIV.FileFormat
{
    public class FFModelOBJ : FIFormat<Model>
    {
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".obj",
            },
            Type = FEType.Texture,
            Validator = null,

            AllowExport = true,
            FormatDescription = "Waveform Object (OBJ)",
            FormatFilter = "Waveform Object (*.obj)|*.obj"
        };

        #endregion

        public void SaveToFile(string filepath, Model data)
        {
            SaveOBJ(filepath, data);
        }

        private void SaveOBJ(string filepath, Model data)
        {
            using (StreamWriter sr = new StreamWriter(File.OpenWrite(filepath)))
            {
                sr.WriteLine("# Tools For KFIV Waveform Object Export");
                sr.WriteLine();

                //Write Model Vertices.
                for (int i = 0; i < data.VertexCount; ++i)
                {
                    Model.Components v = data.Vertices[i];

                    //Write Vertex Line
                    sr.Write("v ");
                    sr.Write(v.X.ToString("0.00000000", CultureInfo.InvariantCulture) + " ");
                    sr.Write(v.Y.ToString("0.00000000", CultureInfo.InvariantCulture) + " ");
                    sr.Write(v.Z.ToString("0.00000000", CultureInfo.InvariantCulture));
                    sr.WriteLine();
                }
                sr.WriteLine("# Vertex Count = " + data.VertexCount.ToString());
                sr.WriteLine();

                //Write Model Normals
                for (int i = 0; i < data.NormalCount; ++i)
                {
                    Model.Components n = data.Normals[i];

                    //Write Vertex Line
                    sr.Write("vn ");
                    sr.Write(n.X.ToString("0.00000000", CultureInfo.InvariantCulture) + " ");
                    sr.Write(n.Y.ToString("0.00000000", CultureInfo.InvariantCulture) + " ");
                    sr.Write(n.Z.ToString("0.00000000", CultureInfo.InvariantCulture));
                    sr.WriteLine();
                }
                sr.WriteLine("# Normal Count = " + data.NormalCount.ToString());
                sr.WriteLine();

                //Write Model Texcoords
                for (int i = 0; i < data.NormalCount; ++i)
                {
                    Model.Components t = data.Texcoords[i];

                    //Write Vertex Line
                    sr.Write("vt ");
                    sr.Write(t.U.ToString("0.00000000", CultureInfo.InvariantCulture) + " ");
                    sr.Write(t.V.ToString("0.00000000", CultureInfo.InvariantCulture));
                    sr.WriteLine();
                }
                sr.WriteLine("# Texcoord Count = " + data.NormalCount.ToString());
                sr.WriteLine();

                //Write Model Groups
                for(int i = 0; i < data.MeshCount; ++i)
                {
                    Model.Mesh m = data.Meshes[i];

                    //Group Header
                    sr.WriteLine("g Mesh" + i.ToString("D4"));
                    
                    //Faces
                    for(int j = 0; j < m.PrimitiveCount; ++j)
                    {
                        switch(m.primitives[j])
                        {
                            case Model.TrianglePrimitive tri:
                                sr.Write("f ");
                                sr.Write((1 + tri.Indices[0]).ToString() + "/");
                                sr.Write((1 + tri.Indices[1]).ToString() + "/");
                                sr.Write((1 + tri.Indices[2]).ToString() + " ");
                                sr.Write((1 + tri.Indices[6]).ToString() + "/");
                                sr.Write((1 + tri.Indices[7]).ToString() + "/");
                                sr.Write((1 + tri.Indices[8]).ToString() + " ");
                                sr.Write((1 + tri.Indices[3]).ToString() + "/");
                                sr.Write((1 + tri.Indices[4]).ToString() + "/");
                                sr.Write((1 + tri.Indices[5]).ToString() + " ");
                                sr.WriteLine();
                                break;

                            default:
                                Console.WriteLine("Cannot export non triangle primitive types to Wavefront Object!");
                                break;
                        }
                    }

                    sr.WriteLine("# Primitive Count = " + m.PrimitiveCount.ToString());
                    sr.WriteLine();
                }
            }
        }

        public Model LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }
        public Model LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }

    }
}
