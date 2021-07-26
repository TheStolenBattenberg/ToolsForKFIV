using System.Collections.Generic;
using System.Globalization;
using System.IO;

using KFIV.Utility.Math;

namespace KFIV.Format.OBJ
{
    public class OBJ
    {
        #region Data
        List<string> vertex;
        List<string> normal;
        List<string> texcoord;
        Dictionary<string, List<string>> groups;

        public uint NormalCount
        {
            get { return (uint)normal.Count; }
        }
        public uint TexcoordCount
        {
            get { return (uint)texcoord.Count; }
        }
        #endregion

        NumberFormatInfo nfi;

        public OBJ()
        {
            nfi = new CultureInfo("en-US", false).NumberFormat;

            vertex   = new List<string>();
            normal   = new List<string>();
            texcoord = new List<string>();
            groups   = new Dictionary<string, List<string>>();
        }

        public void AddVertex(float x, float y, float z)
        {
            string temp_str = "v";
            temp_str = temp_str + " " + x.ToString("F9", nfi);
            temp_str = temp_str + " " + y.ToString("F9", nfi);
            temp_str = temp_str + " " + z.ToString("F9", nfi);

            vertex.Add(temp_str);
        }
        public void AddNormal(float x, float y, float z)
        {
            string temp_str = "vn";
            temp_str = temp_str + " " + x.ToString("F9", nfi);
            temp_str = temp_str + " " + y.ToString("F9", nfi);
            temp_str = temp_str + " " + z.ToString("F9", nfi);

            normal.Add(temp_str);
        }
        public void AddTexcoord(float u, float v)
        {
            string temp_str = "vt";
            temp_str = temp_str + " " + u.ToString("F9", nfi);
            temp_str = temp_str + " " + v.ToString("F9", nfi);

            texcoord.Add(temp_str);
        }
        public void AddGroup(string name)
        {
            groups.Add(name, new List<string>());
        }
        public void AddFace(string group, int v1, int v2, int v3, int n1, int n2, int n3, int uv1, int uv2, int uv3)
        {
            string temp_str = "f";
            temp_str = temp_str + " " + v1.ToString() + "/" + uv1.ToString() + "/" + n1.ToString();
            temp_str = temp_str + " " + v2.ToString() + "/" + uv2.ToString() + "/" + n2.ToString();
            temp_str = temp_str + " " + v3.ToString() + "/" + uv3.ToString() + "/" + n3.ToString();

            groups[group].Add(temp_str);
        }

        public void Save(string path)
        {
            using(StreamWriter f = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                f.WriteLine("# ToolsForKFIV OBJ Writer");
                f.WriteLine("# https://github.com/TheStolenBattenberg/ToolsForKFIV");
                f.WriteLine("");

                //Write Vertices
                for(int i = 0; i < vertex.Count; ++i)
                    f.WriteLine(vertex[i]);

                f.WriteLine("# Vertex Count = " + vertex.Count.ToString());
                f.WriteLine();

                //Write Normals
                for (int i = 0; i < normal.Count; ++i)
                    f.WriteLine(normal[i]);

                f.WriteLine("# Normal Count = " + normal.Count.ToString());
                f.WriteLine();

                //Write Texture Coordinates
                for (int i = 0; i < texcoord.Count; ++i)
                    f.WriteLine(texcoord[i]);

                f.WriteLine("# Texture Coordinates = " + texcoord.Count.ToString());
                f.WriteLine();

                //Write Groups
                foreach(KeyValuePair<string, List<string>> kvp in groups)
                {
                    f.WriteLine("g " + kvp.Key);
                    for(int i = 0; i < kvp.Value.Count; ++i)
                    {
                        f.WriteLine(kvp.Value[i]);
                    }
                    f.WriteLine();
                }
            }
        }


        // Triangle Utilities
        public static Vector3 GenerateFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 U = Vector3.Subtract(v2, v1);
            Vector3 V = Vector3.Subtract(v3, v1);
            return Vector3.Cross(U, V);
        }
    }
}
