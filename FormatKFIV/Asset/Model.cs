using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using FormatKFIV.Utility;

namespace FormatKFIV.Asset
{
    /// <summary>
    /// The Model class provides storage for 3D data, such as triangle meshes.
    /// 
    /// It can also generate some simple shapes like boxes.
    /// </summary>
    public class Model
    {
        /// <summary>Components store restricted dimensional size data, such as an RGBA colour, XYZ position or UV texcoords</summary>
        public struct Components
        {
            private float[] components;

            /// <summary>Get or set the X component</summary>
            public float X
            {
                get { return components[0]; }
                set { components[0] = value; }
            }

            /// <summary>Get or set the Y component</summary>
            public float Y
            {
                get { return components[1]; }
                set { components[1] = value; }
            }

            /// <summary>Get or set the Z component</summary>
            public float Z
            {
                get { return components[2]; }
                set { components[2] = value; }
            }

            /// <summary>Get or set the W component</summary>
            public float W
            {
                get { return components[3]; }
                set { components[3] = value; }
            }

            /// <summary>Get or set the U component</summary>
            public float U
            {
                get { return components[0]; }
                set { components[0] = value; }
            }

            /// <summary>Get or set the V component</summary>
            public float V
            {
                get { return components[1]; }
                set { components[1] = value; }
            }

            /// <summary>Get or set the R component</summary>
            public float R
            {
                get { return components[0]; }
                set { components[0] = value; }
            }

            /// <summary>Get or set the G component</summary>
            public float G
            {
                get { return components[1]; }
                set { components[1] = value; }
            }

            /// <summary>Get or set the B component</summary>
            public float B
            {
                get { return components[2]; }
                set { components[2] = value; }
            }

            /// <summary>Get or set the A component</summary>
            public float A
            {
                get { return components[3]; }
                set { components[3] = value; }
            }

            /// <summary>Construct a 2 dimensional components</summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public Components(float x, float y)
            {
                components = new float[2];
                components[0] = x;
                components[1] = y;
            }

            /// <summary>Construct a 3 dimensional components</summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            public Components(float x, float y, float z)
            {
                components = new float[3];
                components[0] = x;
                components[1] = y;
                components[2] = z;
            }

            /// <summary>Construct a 4 dimensional components</summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <param name="w"></param>
            public Components(float x, float y, float z, float w)
            {
                components = new float[4];
                components[0] = x;
                components[1] = y;
                components[2] = z;
                components[3] = w;
            }

            /// <summary>Compare if two vertices are equal to one another</summary>
            /// <param name="obj">The vertex to compare with</param>
            /// <returns>True if they are equal</returns>
            public override bool Equals(object obj)
            {
                if(!(obj is Components v)) 
                { 
                    return false; 
                }

                if(v.components.Length != components.Length)
                {
                    return false;
                }

                bool isEqual = true;
                for(byte i = 0; i < components.Length; ++i)
                {
                    isEqual = isEqual & (components[i] == v.components[i]);
                }

                return isEqual;
            }

            /// <summary>Some C# shit, not sure.</summary>
            /// <returns>Apparently a hash code.</returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            /// <summary>Convert the data of the vertex to a string for easy display</summary>
            /// <returns>The vertex as a string</returns>
            public override string ToString()
            {
                return string.Join(", ", components);
            }

            public static bool operator ==(Components lhs, Components rhs)
            {
                return lhs.Equals(rhs);
            }
            public static bool operator !=(Components lhs, Components rhs)
            {
                return !lhs.Equals(rhs);
            }
        }

        /// <summary>Interface for different primitive types</summary>
        public interface IPrimitiveType
        {
            int[] Indices { get; }
            int[] Vertices { get; }
            int[] Normals { get; }
            int[] Texcoords { get; }
            int[] Colours { get; }
        }

        /// <summary>Triangle Primitive Type</summary>
        public struct TrianglePrimitive : IPrimitiveType
        {
            private int[] _indices;

            public int[] Indices
            {
                get { return _indices; }
                set { _indices = value; }
            }
            public int[] Vertices
            {
                get
                {
                    return new int[] { _indices[0], _indices[1], _indices[2] };
                }
            }
            public int[] Normals
            {
                get
                {
                    return new int[] { _indices[3], _indices[4], _indices[5] };
                }
            }
            public int[] Texcoords
            {
                get
                {
                    return new int[] { _indices[6], _indices[7], _indices[8] };
                }
            }
            public int[] Colours
            {
                get
                {
                    return new int[] { _indices[9], _indices[10], _indices[11] };
                }
            }

            public static TrianglePrimitive New()
            {
                TrianglePrimitive primitive = new TrianglePrimitive();
                primitive._indices = new int[12];

                return primitive;
            }

            public void FlipIndices()
            {
                int[] tempIndices = new int[_indices.Length];
                _indices.CopyTo(tempIndices, 0);

                _indices[0] = tempIndices[2];   //Vertex Indices
                _indices[2] = tempIndices[0];
                _indices[3] = tempIndices[5];   //Normal Indices
                _indices[5] = tempIndices[3];
                _indices[6] = tempIndices[8];   //Texcoord Indices
                _indices[8] = tempIndices[6];
                _indices[9] = tempIndices[11];  //Colour Indices
                _indices[11] = tempIndices[9];
            }
        }

        /// <summary>Line Primitive Type</summary>
        public struct LinePrimitive : IPrimitiveType
        {
            private int[] _indices;

            public int[] Indices
            {
                get { return _indices; }
                set { _indices = value; }
            }
            public int[] Vertices
            {
                get
                {
                    return new int[] { _indices[0], _indices[1] };
                }
            }
            public int[] Normals
            {
                get
                {
                    return null;
                }
            }
            public int[] Texcoords
            {
                get
                {
                    return null;
                }
            }
            public int[] Colours
            {
                get
                {
                    return new int[] { _indices[2], _indices[3] };
                }
            }

            public static LinePrimitive New()
            {
                LinePrimitive primitive = new LinePrimitive();
                primitive._indices = new int[4];

                return primitive;
            }
        }

        /// <summary>Mesh stores primitive data and a transform</summary>
        public struct Mesh
        {
            //Mesh Transform
            public Vector3f position;
            public Vector3f rotation;
            public Vector3f scale;

            //Mesh Data
            public IPrimitiveType[] primitives;
            public int textureSlot;
            
            public int PrimitiveCount
            {
                get
                {
                    return primitives.Length;
                }
            }
        }

        /// <summary>Texture slot contains information about a meshes texture</summary>
        public struct TextureSlot
        {
            public uint slotKey;    //Some unique identifier for the texture
        }

        //Data
        private List<Components> _vertices;
        private List<Components> _normals;
        private List<Components> _texcoords;
        private List<Components> _colours;
        private List<TextureSlot> _textureSlots;
        private List<Mesh> _meshes;

        /// <summary> Returns the list of vertices </summary>
        public List<Components> Vertices
        {
            get { return _vertices; }
        }

        /// <summary> Returns the list of normals </summary>
        public List<Components> Normals
        {
            get { return _normals; }
        }

        /// <summary> Returns the list of texcoords </summary>
        public List<Components> Texcoords
        {
            get { return _texcoords; }
        }

        /// <summary> Returns the list of colours </summary>
        public List<Components> Colours
        {
            get { return _colours; }
        }

        /// <summary> Returns the list of texture slots </summary>
        public List<TextureSlot> TextureSlots
        {
            get { return _textureSlots; }
        }

        /// <summary> Returns the list of meshes </summary>
        public List<Mesh> Meshes
        {
            get { return _meshes; }
        }

        /// <summary> Returns the number of vertices </summary>
        public int VertexCount
        {
            get { return _vertices.Count; }
        }

        /// <summary> Returns the number of normals </summary>
        public int NormalCount
        {
            get { return _normals.Count; }
        }

        /// <summary> Returns the number of texcoords </summary>
        public int TexcoordCount
        {
            get { return _texcoords.Count; }
        }

        /// <summary> Returns the number of colours </summary>
        public int ColourCount
        {
            get { return _colours.Count; }
        }

        /// <summary> Returns the number of texture slots </summary>
        public int TextureSlotCount
        {
            get { return _textureSlots.Count; }
        }

        /// <summary> Returns the number of meshes </summary>
        public int MeshCount
        {
            get { return _meshes.Count; }
        }

        /// <summary> Returns the total number of primitives</summary>
        public int PrimitiveCount
        {
            get
            {
                int count = 0;
                foreach(Mesh mesh in _meshes)
                {
                    count += mesh.PrimitiveCount;
                }
                return count;
            }
        }


        public Model()
        {
            _vertices = new List<Components>();
            _normals = new List<Components>();
            _texcoords = new List<Components>();
            _colours = new List<Components>();
            _meshes = new List<Mesh>();
            _textureSlots = new List<TextureSlot>();
        }

        /// <summary>Adds a unique vertex to the vertex list</summary>
        /// <param name="x">X component of the vertex</param>
        /// <param name="y">Y component of the vertex</param>
        /// <param name="z">Z component of the vertex</param>
        /// <returns>The index of the vertex in the vertex list</returns>
        public int AddUniqueVertex(float x, float y, float z)
        {
            return AddUnique(_vertices, new Components(x, y, z));
        }

        /// <summary>Adds a unique vertex to the vertex list</summary>
        /// <param name="vertex">The vertex</param>
        /// <returns>The index of the vertex in the vertex list</returns>
        public int AddUniqueVertex(Components vertex)
        {
            return AddUnique(_vertices, vertex);
        }

        /// <summary>Adds a vertex to the vertex list </summary>
        /// <param name="x">X component of the vertex</param>
        /// <param name="y">Y component of the vertex</param>
        /// <param name="z">Z component of the vertex</param>
        /// <returns>The index of the vertex in the vertex list</returns>
        public int AddVertex(float x, float y, float z)
        {
            return AddNonUnique(_vertices, new Components(x, y, z));
        }

        /// <summary>Adds a vertex to the vertex list </summary>
        /// <param name="vertex">The vertex</param>
        /// <returns>The index of the vertex in the vertex list</returns>
        public int AddVertex(Components vertex)
        {
            return AddNonUnique(_vertices, vertex);
        }

        /// <summary>Adds a unique normal to the normal list</summary>
        /// <param name="x">X component of the normal</param>
        /// <param name="y">Y component of the normal</param>
        /// <param name="z">Z component of the normal</param>
        /// <returns>The index of the normal in the normal list</returns>
        public int AddUniqueNormal(float x, float y, float z)
        {
            return AddUnique(_normals, new Components(x, y, z));
        }

        /// <summary>Adds a unique normal to the normal list</summary>
        /// <param name="normal">The normal</param>
        /// <returns>The index of the vertex in the normal list</returns>
        public int AddUniqueNormal(Components normal)
        {
            return AddUnique(_normals, normal);
        }

        /// <summary>Adds a normal to the normal list </summary>
        /// <param name="x">X component of the normal</param>
        /// <param name="y">Y component of the normal</param>
        /// <param name="z">Z component of the normal</param>
        /// <returns>The index of the normal in the normal list</returns>
        public int AddNormal(float x, float y, float z)
        {
            return AddNonUnique(_normals, new Components(x, y, z));
        }

        /// <summary>Adds a normal to the normal list </summary>
        /// <param name="normal">The normal</param>
        /// <returns>The index of the vertex in the normal list</returns>
        public int AddNormal(Components normal)
        {
            return AddNonUnique(_normals, normal);
        }

        /// <summary>Adds a unique texcoord to the texcoord list</summary>
        /// <param name="u">U component of the texcoord</param>
        /// <param name="v">V component of the texcoord</param>
        /// <returns>The index of the texcoord in the texcoord list</returns>
        public int AddUniqueTexcoord(float u, float v)
        {
            return AddUnique(_texcoords, new Components(u, v));
        }

        /// <summary>Adds a unique texcoord to the texcoord list</summary>
        /// <param name="texcoord">The texcoord</param>
        /// <returns>The index of the texcoord in the texcoord list</returns>
        public int AddUniqueTexcoord(Components texcoord)
        {
            return AddUnique(_texcoords, texcoord);
        }

        /// <summary>Adds a texcoord to the texcoord list</summary>
        /// <param name="u">U component of the texcoord</param>
        /// <param name="v">V component of the texcoord</param>
        /// <returns>The index of the texcoord in the texcoord list</returns>
        public int AddTexcoord(float u, float v)
        {
            return AddNonUnique(_texcoords, new Components(u, v));
        }

        /// <summary>Adds a texcoord to the texcoord list</summary>
        /// <param name="texcoord">The texcoord</param>
        /// <returns>The index of the texcoord in the texcoord list</returns>
        public int AddTexcoord(Components texcoord)
        {
            return AddNonUnique(_texcoords, texcoord);
        }

        /// <summary>Adds a unique colour to the colour list</summary>
        /// <param name="r">R component of the colour</param>
        /// <param name="g">G component of the colour</param>
        /// <param name="b">B component of the colour</param>
        /// <param name="a">A component of the colour</param>
        /// <returns>The index of the colour in the colour list</returns>
        public int AddUniqueColour(float r, float g, float b, float a)
        {
            return AddUnique(_colours, new Components(r, g, b, a));
        }

        /// <summary>Adds a unique colour to the colour list</summary>
        /// <param name="colour">the colour</param>
        /// <returns>The index of the colour in the colour list</returns>
        public int AddUniqueColour(Components colour)
        {
            return AddUnique(_colours, colour);
        }

        /// <summary>Adds a colour to the colour list</summary>
        /// <param name="r">R component of the colour</param>
        /// <param name="g">G component of the colour</param>
        /// <param name="b">B component of the colour</param>
        /// <param name="a">A component of the colour</param>
        /// <returns>The index of the colour in the colour list</returns>
        public int AddColour(float r, float g, float b, float a)
        {
            return AddNonUnique(_colours, new Components(r, g, b, a));
        }

        /// <summary>Adds a colour to the colour list</summary>
        /// <param name="colour">the colour</param>
        /// <returns>The index of the colour in the colour list</returns>
        public int AddColour(Components colour)
        {
            return AddNonUnique(_colours, colour);
        }

        /// <summary>Adds a mesh to the mesh list</summary>
        /// <param name="mesh">the mesh</param>
        /// <returns>The index of the mesh in the mesh list</returns>
        public int AddMesh(Mesh mesh)
        {
            _meshes.Add(mesh);
            return _meshes.Count - 1;
        }

        /// <summary>Adds a texture slot to the texture slot list</summary>
        /// <param name="textureSlot">the texture slot</param>
        /// <returns>The index of the texture slot in the texture slot list</returns>
        public int AddTextureSlot(TextureSlot textureSlot)
        {
            _textureSlots.Add(textureSlot);
            return _textureSlots.Count - 1;
        }

        /// <summary>Generates a cube made from line primitives, using two corners.</summary>
        /// <param name="cornerA">The first corner</param>
        /// <param name="cornerB">The second corner</param>
        /// <returns>A line cube</returns>
        public static Model GenerateLineCube(Vector3f cornerA, Vector3f cornerB, Colour colour)
        {
            Model result = new Model();

            Mesh mesh = new Mesh();
            mesh.position = Vector3f.Zero;
            mesh.rotation = Vector3f.Zero;
            mesh.scale = Vector3f.One;
            mesh.primitives = new IPrimitiveType[12];
            mesh.textureSlot = -1;

            LinePrimitive B1 = LinePrimitive.New();
            B1.Indices[0] = result.AddUniqueVertex(cornerA.X, cornerA.Y, cornerA.Z);
            B1.Indices[1] = result.AddUniqueVertex(cornerB.X, cornerA.Y, cornerA.Z);
            B1.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            B1.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[0] = B1;

            LinePrimitive B2 = LinePrimitive.New();
            B2.Indices[0] = result.AddUniqueVertex(cornerB.X, cornerA.Y, cornerA.Z);
            B2.Indices[1] = result.AddUniqueVertex(cornerB.X, cornerB.Y, cornerA.Z);
            B2.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            B2.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[1] = B2;

            LinePrimitive B3 = LinePrimitive.New();
            B3.Indices[0] = result.AddUniqueVertex(cornerB.X, cornerB.Y, cornerA.Z);
            B3.Indices[1] = result.AddUniqueVertex(cornerA.X, cornerB.Y, cornerA.Z);
            B3.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            B3.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[2] = B3;

            LinePrimitive B4 = LinePrimitive.New();
            B4.Indices[0] = result.AddUniqueVertex(cornerA.X, cornerB.Y, cornerA.Z);
            B4.Indices[1] = result.AddUniqueVertex(cornerA.X, cornerA.Y, cornerA.Z);
            B4.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            B4.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[3] = B4;

            LinePrimitive T1 = LinePrimitive.New();
            T1.Indices[0] = result.AddUniqueVertex(cornerA.X, cornerA.Y, cornerB.Z);
            T1.Indices[1] = result.AddUniqueVertex(cornerB.X, cornerA.Y, cornerB.Z);
            T1.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            T1.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[4] = T1;

            LinePrimitive T2 = LinePrimitive.New();
            T2.Indices[0] = result.AddUniqueVertex(cornerB.X, cornerA.Y, cornerB.Z);
            T2.Indices[1] = result.AddUniqueVertex(cornerB.X, cornerB.Y, cornerB.Z);
            T2.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            T2.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[5] = T2;

            LinePrimitive T3 = LinePrimitive.New();
            T3.Indices[0] = result.AddUniqueVertex(cornerB.X, cornerB.Y, cornerB.Z);
            T3.Indices[1] = result.AddUniqueVertex(cornerA.X, cornerB.Y, cornerB.Z);
            T3.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            T3.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[6] = T3;

            LinePrimitive T4 = LinePrimitive.New();
            T4.Indices[0] = result.AddUniqueVertex(cornerA.X, cornerB.Y, cornerB.Z);
            T4.Indices[1] = result.AddUniqueVertex(cornerA.X, cornerA.Y, cornerB.Z);
            T4.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            T4.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[7] = T4;

            LinePrimitive C1 = LinePrimitive.New();
            C1.Indices[0] = result.AddUniqueVertex(cornerA.X, cornerA.Y, cornerA.Z);
            C1.Indices[1] = result.AddUniqueVertex(cornerA.X, cornerA.Y, cornerB.Z);
            C1.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            C1.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[8] = C1;

            LinePrimitive C2 = LinePrimitive.New();
            C2.Indices[0] = result.AddUniqueVertex(cornerB.X, cornerA.Y, cornerA.Z);
            C2.Indices[1] = result.AddUniqueVertex(cornerB.X, cornerA.Y, cornerB.Z);
            C2.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            C2.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[9] = C2;

            LinePrimitive C3 = LinePrimitive.New();
            C3.Indices[0] = result.AddUniqueVertex(cornerB.X, cornerB.Y, cornerA.Z);
            C3.Indices[1] = result.AddUniqueVertex(cornerB.X, cornerB.Y, cornerB.Z);
            C3.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            C3.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[10] = C3;

            LinePrimitive C4 = LinePrimitive.New();
            C4.Indices[0] = result.AddUniqueVertex(cornerA.X, cornerB.Y, cornerA.Z);
            C4.Indices[1] = result.AddUniqueVertex(cornerA.X, cornerB.Y, cornerB.Z);
            C4.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            C4.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
            mesh.primitives[11] = C4;

            result.AddMesh(mesh);

            return result;
        }

        /// <summary>Generates a sphere made from line primitives, using an origin and radius</summary>
        /// <param name="origin">The center point of the sphere</param>
        /// <param name="radius">The radius of the sphere</param>
        /// <param name="resolution">The number of line segments used per band. Minimum of 4.</param>
        /// <param name="colour">The colour of the sphere</param>
        /// <returns>A line sphere</returns>
        public static Model GenerateLineSphere(Vector3f origin, float radius, int resolution, Colour colour)
        {
            Model result = new Model();

            resolution = Math.Max(4, resolution);

            Mesh mesh = new Mesh();
            mesh.position = Vector3f.Zero;
            mesh.rotation = Vector3f.Zero;
            mesh.scale = Vector3f.One;
            mesh.primitives = new IPrimitiveType[3 * resolution];
            mesh.textureSlot = -1;

            float segmentSize = 6.28318530718f / resolution;

            for (int i = 0; i < resolution; ++i)
            {
                float p1A = MathF.Cos(segmentSize * i) * radius;
                float p1B = MathF.Sin(segmentSize * i) * radius;
                float p2A = MathF.Cos(segmentSize * (i + 1)) * radius;
                float p2B = MathF.Sin(segmentSize * (i + 1)) * radius;

                LinePrimitive lp;

                //XZ Axis
                lp = LinePrimitive.New();
                lp.Indices[0] = result.AddUniqueVertex(origin.X + p1A, origin.Y + 0f, origin.Z + p1B);
                lp.Indices[1] = result.AddUniqueVertex(origin.X + p2A, origin.Y + 0f, origin.Z + p2B);
                lp.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
                lp.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
                mesh.primitives[(3 * i) + 0] = lp;

                //XY Axis
                lp = LinePrimitive.New();
                lp.Indices[0] = result.AddUniqueVertex(origin.X + p1A, origin.Y + p1B, origin.Z + 0f);
                lp.Indices[1] = result.AddUniqueVertex(origin.X + p2A, origin.Y + p2B, origin.Z + 0f);
                lp.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
                lp.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
                mesh.primitives[(3 * i) + 1] = lp;

                //ZY Axis
                lp = LinePrimitive.New();
                lp.Indices[0] = result.AddUniqueVertex(origin.X + 0f, origin.Y + p1B, origin.Z + p1A);
                lp.Indices[1] = result.AddUniqueVertex(origin.X + 0f, origin.Y + p2B, origin.Z + p2A);
                lp.Indices[2] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
                lp.Indices[3] = result.AddUniqueColour(colour.R / 255f, colour.G / 255f, colour.B / 255f, colour.A / 255f);
                mesh.primitives[(3 * i) + 2] = lp;
            }

            result.AddMesh(mesh);

            return result;
        }

        /// <summary> Generates a flat / face normal</summary>
        /// <param name="v1">Vertex 1</param>
        /// <param name="v2">Vertex 2</param>
        /// <param name="v3">Vertex 3</param>
        /// <returns>The normal, duh?</returns>
        public static Vector3f GenerateFlatNormal(Vector3f v1, Vector3f v2, Vector3f v3)
        {
            Vector3f U = Vector3f.Subtract(v2, v1);
            Vector3f V = Vector3f.Subtract(v3, v1);

            return Vector3f.Normalize(Vector3f.Cross(U, V));
        }

        /// <summary>Adds'Components' to a list of 'Components' only if it is unique </summary>
        /// <param name="list">The list to add components too</param>
        /// <param name="components">The Components</param>
        /// <returns>The index of components (or the found duplicate components) in the list</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int AddUnique(List<Components> list, Components components)
        {
            for(int i = 0; i < list.Count; ++i)
            {
                if(list[i] == components)
                {
                    return i;
                }
            }

            list.Add(components);
            return list.Count - 1;
        }

        /// <summary>Adds 'Components' to a list of 'Components' </summary>
        /// <param name="list">The list to add components too</param>
        /// <param name="components">The Components</param>
        /// <returns>The index of components in the list</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int AddNonUnique(List<Components> list, Components components)
        {
            list.Add(components);
            return list.Count - 1;
        }
    }
}
