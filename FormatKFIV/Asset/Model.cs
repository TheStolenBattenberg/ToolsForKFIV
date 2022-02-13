using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Utility;

namespace FormatKFIV.Asset
{
    /// <summary>Provides storage for model type data.</summary>
    public class Model
    {
        /// <summary>Storage for triangle data</summary>
        public struct Triangle
        {
            /// <summary>Vertex Indices</summary>
            public ushort[] vIndices;
            /// <summary>Normal Indices</summary>
            public ushort[] nIndices;
            /// <summary>Texcoord Indices</summary>
            public ushort[] tIndices;
            /// <summary>Colour Indices</summary>
            public ushort[] cIndices;

            public void FlipIndices()
            {
                ushort tempInd;
                tempInd = vIndices[0];
                vIndices[0] = vIndices[2];
                vIndices[2] = tempInd;
                tempInd = nIndices[0];
                nIndices[0] = nIndices[2];
                nIndices[2] = tempInd;
                tempInd = tIndices[0];
                tIndices[0] = tIndices[2];
                tIndices[2] = tempInd;
                tempInd = cIndices[0];
                cIndices[0] = cIndices[2];
                cIndices[2] = tempInd;
            }
        }

        /// <summary>Storage for vertex data</summary>
        public struct Vertex
        {
            /// <summary>X Component</summary>
            public float X;
            /// <summary>Y Component</summary>
            public float Y;
            /// <summary>Z Component</summary>
            public float Z;

            public override bool Equals(object obj)
            {
                if (!(obj is Vertex v)) { return false; }

                return v.X == this.X && v.Y == this.Y && v.Z == this.Z;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            public override string ToString()
            {
                return "{X=" + X.ToString() + ",Y=" + Y.ToString() + ",Z=" + Z.ToString() + "}";
            }

            //Operators
            public static bool operator ==(Vertex lhs, Vertex rhs)
            {
                return lhs.Equals(rhs);
            }
            public static bool operator !=(Vertex lhs, Vertex rhs)
            {
                return !lhs.Equals(rhs);
            }
        }

        /// <summary>Storage for normal data</summary>
        public struct Normal
        {
            /// <summary>X Component</summary>
            public float X;
            /// <summary>Y Component</summary>
            public float Y;
            /// <summary>Z Component</summary>
            public float Z;
        }

        /// <summary>Storage for texcoord data</summary>
        public struct Texcoord
        {
            /// <summary>U Component</summary>
            public float U;
            /// <summary>V Component</summary>
            public float V;
        }

        /// <summary>Storage for colour data</summary>
        public struct Colour
        {
            /// <summary>R Component</summary>
            public float R;
            /// <summary>G Component</summary>
            public float G;
            /// <summary>B Component</summary>
            public float B;
            /// <summary>A Component</summary>
            public float A;
        }

        /// <summary>Storage for mesh data</summary>
        public struct Mesh
        {
            public uint numTriangle;
            public Triangle[] triangles;
        }

        /// <summary>Transformation Data</summary>
        public struct Transform
        {
            public Vector3f position;
            public Vector3f rotation;
            public Vector3f scale;
        }

        //Properties
        public Transform transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        public int MeshCount 
        {
            get { return _meshes.Count; }
        }
        public int TriangleCount
        {
            get { return _tottricount; }
        }
        public int VertexCount
        {
            get { return _vertices.Count; }
        }
        public int NormalCount
        {
            get { return _normals.Count; }
        }
        public int TexcoordCount
        {
            get { return _texcoords.Count; }
        }

        //Members
        private Transform _transform;
        private List<Mesh> _meshes = new List<Mesh>();

        private List<Vertex> _vertices = new List<Vertex>();
        private List<Normal> _normals = new List<Normal>();
        private List<Texcoord> _texcoords = new List<Texcoord>();
        private List<Colour> _colours = new List<Colour>();
        private int _tottricount = 0;

        /// <summary>Adds a new vertex if it doesn't exist, or gets the ID if it does</summary>
        /// <returns>Vertex ID</returns>
        public int AddVertexExclusive(float x, float y, float z)
        {
            return AddVertexExclusive(new Vertex
            {
                X = x,
                Y = y,
                Z = z
            });
        }
        /// <summary>Adds a new vertex if it doesn't exist, or gets the ID if it does</summary>
        /// <returns>Vertex ID</returns>
        public int AddVertexExclusive(Vertex v)
        {
            //Search list to see if vertex already exists
            for(int i = 0; i < _vertices.Count; ++i)
            {
                if(_vertices[i] == v)
                {
                    return i;
                }
            }

            //If we got here without returning, we need to add the vertex
            return AddVertex(v);
        }
        /// <summary>Adds a vertex</summary>
        /// <returns>Vertex ID</returns>
        public int AddVertex(float x, float y, float z)
        {
            return AddVertex(new Vertex
            {
                X = x,
                Y = y,
                Z = z
            });
        }
        /// <summary>Adds a vertex</summary>
        /// <returns>Vertex ID</returns>
        public int AddVertex(Vertex v)
        {
            _vertices.Add(v);
            return _vertices.Count - 1;
        }
        public Vertex GetVertex(int index)
        {
            return _vertices[index];
        }

        public int AddNormal(float x, float y, float z)
        {
            return AddNormal(new Normal
            {
                X = x,
                Y = y,
                Z = z
            });
        }
        public int AddNormal(Normal n)
        {
            _normals.Add(n);
            return _normals.Count - 1;
        }
        public Normal GetNormal(int index)
        {
            return _normals[index];
        }

        public int AddTexcoord(float u, float v)
        {
            return AddTexcoord(new Texcoord
            {
                U = u,
                V = v
            });
        }
        public int AddTexcoord(Texcoord tc)
        {
            _texcoords.Add(tc);
            return _texcoords.Count - 1;
        }
        public Texcoord GetTexcoord(int index)
        {
            return _texcoords[index];
        }

        public int AddColour(float r, float g, float b, float a)
        {
            return AddColour(new Colour
            {
                R = r,
                G = g,
                B = b,
                A = a
            });
        }
        public int AddColour(Colour c)
        {
            _colours.Add(c);
            return _colours.Count - 1;
        }
        public Colour GetColour(int index)
        {
            return _colours[index];
        }

        public int AddMesh(Mesh mesh)
        {
            _tottricount += (int)mesh.numTriangle;

            _meshes.Add(mesh);
            return _meshes.Count - 1;
        }
        public Mesh GetMesh(int index)
        {
            return _meshes[index];
        }

        /// <summary>Generates a flat normal from three vertices.</summary>
        /// <param name="v1">First vertex</param>
        /// <param name="v2">Second vertex</param>
        /// <param name="v3">Third vertex</param>
        /// <returns>The generated flat normal</returns>
        public static Vector3f GenerateFaceNormal(Vector3f v1, Vector3f v2, Vector3f v3)
        {
            Vector3f U = Vector3f.Subtract(v2, v1);
            Vector3f V = Vector3f.Subtract(v3, v1);

            return Vector3f.Normalize(Vector3f.Cross(U, V));
        }
    }
}
