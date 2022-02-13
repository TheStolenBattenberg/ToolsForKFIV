using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using KFIV.Utility.IO;
using KFIV.Utility.Maths;

namespace KFIV.Format.GLTF
{
    public class GLTF
    {
        #region GLTF2 Types
        /**
         * Container for whole GLTF model
        **/
        public class GLTFModel
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public GLTFAsset asset { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public GLTFBuffer[] buffers { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public List<GLTFBufferView> bufferViews { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public List<GLTFAccessor<float>> accessors { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public List<GLTFMesh> meshes { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public List<GLTFNode> nodes { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public List<GLTFScene> scenes { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint scene { get; set; }
        }

        /**
         * GLTFScene
        **/
        public class GLTFScene
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint[] nodes { get; set; }

        }
        /**
         * Contains what could be described as a GLTF Header
        **/
        public class GLTFAsset
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public string version { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public string generator { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public string copyright { get; set; }
        }

        /**
         * Defines a binary data file
        **/
        public class GLTFBuffer
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint byteLength { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public string uri { get; set; }
        }

        /**
         * Defines sections of a buffer
        **/
        public class GLTFBufferView
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint buffer { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint byteLength { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint byteOffset { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint byteStride { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint target { get; set; }
        }

        /**
         * Defines a point in the scene
        **/
        public class GLTFNode
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public string name { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint[] children { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint mesh { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] matrix { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] translation { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] rotation { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] scale { get; set; }
        }

        /**
         * Defines an accessor to a buffer view
         * 
         * 'type' is to be one of the following:
         * - SCALAR
         * - VEC2
         * - VEC3
         * - VEC4
         * - MAT2
         * - MAT3
         * - MAT4
        **/
        public class GLTFAccessor<T>
        {
            public enum AccessorElementType
            {
                SBYTE = 5120,
                UBYTE = 5121,
                SSHORT = 5122,
                USHORT = 5123,
                SINT = 5124,
                UINT = 5125,
                FLOAT = 5126
            };

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint bufferView { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint byteOffset { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public AccessorElementType componentType { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint count { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public T[] max { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public T[] min { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public string type { get; set; }
        }

        /**
         * 
        **/
        public class GLTFAttributes
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint POSITION { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint NORMAL { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint TANGENT { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint TEXCOORD_0 { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint TEXCOORD_1 { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint COLOR_0 { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint JOINTS_0 { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint WEIGHTS_0 { get; set; }
        }

        /**
         * 
        **/
        public class GLTFPrimitive
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public GLTFAttributes attributes { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint indices { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint material { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint mode { get; set; }
        }

        /**
         * 
        **/
        public class GLTFMesh
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public GLTFPrimitive[] primitives { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public float[] weights { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public string name { get; set; }
        }

        /**
         * 
        **/
        public class GLTFTexture
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint sampler { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public uint source { get; set; }
        }

        /**
         * 
        **/
        public class GLTFImage
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public string uri { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public uint bufferView { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public string mimeType { get; set; }
        }

        #endregion

        public class Group
        {
            public string name;
            public List<uint> indices;
            public Vector3 translation;
            public Vector3 rotation;
            public Vector3 scale;
        }

        private List<Vector3> vertices;
        private List<Vector3> normals;
        private List<Vector2> texcoords;
        private Dictionary<string, Group> groups;

        public GLTF()
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            texcoords = new List<Vector2>();
            groups = new Dictionary<string, Group>();
        }

        //
        //
        //
        public int VertexCount
        {
            get { return vertices.Count; }
        }
        public int NormalCount
        {
            get { return normals.Count; }
        }
        public int TexcoordCount
        {
            get { return texcoords.Count; }
        }

        public Vector3 GenerateFaceNormal(int a, int b, int c)
        {
            Vector3 U = Vector3.Subtract(vertices[b], vertices[a]);
            Vector3 V = Vector3.Subtract(vertices[c], vertices[a]);
            return Vector3.Cross(U, V);
        }

        public Vector3 FetchNormal(int index)
        {
            return normals[index];
        }

        public void GroupCreate(string groupName)
        {
            Group group = new Group
            {
                name = groupName,
                indices = new List<uint>()
            };

            groups.Add(groupName, group);
        }
        public void GroupSetTranslation(string groupName, Vector3 translation)
        {
            groups[groupName].translation = translation;
        }
        public void GroupSetRotation(string groupName, Vector3 rotation)
        {
            groups[groupName].rotation = rotation;
        }
        public void GroupSetScale(string groupName, Vector3 scale)
        {
            groups[groupName].scale = scale;
        }
        public void AddFace(string groupName, uint a, uint b, uint c)
        {
            groups[groupName].indices.Add(a);
            groups[groupName].indices.Add(b);
            groups[groupName].indices.Add(c);
        }
        public void AddVertex(float x, float y, float z)
        {
            vertices.Add(new Vector3(x, y, z));
        }
        public void AddNormal(float x, float y, float z)
        {
            normals.Add(new Vector3(x, y, z));
        }
        public void AddTexcoord(float u, float v)
        {
            texcoords.Add(new Vector2(u, v));
        }

        //
        //
        //
        public void Save(string filepath, string filename)
        {
            //Setup Serializer Options
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            //Some data from our lump-to-write we want to save.
            uint offToVertices;
            uint offToNormals;
            uint offToTexcoords;
            uint offToIndices;
            uint numOfIndices = 0;
            long sizeOfBuffer;

            //Export our binary lump
            Console.WriteLine("glTF -> Exporting binary data...");

            using(OutputStream ous = new OutputStream(filepath + filename + ".bin"))
            {
                //Write Vertices to lump
                Console.WriteLine("glTF -> Writing Vertices:");
                offToVertices = (uint)ous.Tell();
                foreach (Vector3 vertex in vertices)
                    ous.WriteVector3(vertex);

                Console.WriteLine($"\tVertex Offset: 0x{offToVertices.ToString("x8")}");
                Console.WriteLine($"\tVertex Count: {vertices.Count.ToString("D8")}");

                //Write Normals to lump
                Console.WriteLine("glTF -> Writing Normals:");
                offToNormals = (uint)ous.Tell();
                foreach(Vector3 normal in normals)
                    ous.WriteVector3(normal);

                Console.WriteLine($"\tNormal Offset: 0x{offToNormals.ToString("X8")}");
                Console.WriteLine($"\tNormal Count: {normals.Count.ToString("D8")}");

                //Write Texcoords to lump
                Console.WriteLine("glTF -> Writing Texcoords:");
                offToTexcoords = (uint)ous.Tell();
                foreach (Vector2 texcoord in texcoords)
                    ous.WriteVector2(texcoord);

                Console.WriteLine($"\tTexcoord Offset: 0x{offToTexcoords.ToString("X8")}");
                Console.WriteLine($"\tTexcoord Count: {texcoords.Count.ToString("D8")}");

                //Write Indices to lump
                Console.WriteLine("glTF -> Writing Indices:");
                offToIndices = (uint)ous.Tell();
                foreach(Group group in groups.Values)
                {
                    foreach(uint index in group.indices)
                    {
                        ous.Write(index);
                        numOfIndices++;
                    }
                }

                Console.WriteLine($"\tIndices Offset: 0x{offToTexcoords.ToString("X8")}");
                Console.WriteLine($"\tIndices Count: {numOfIndices.ToString("D8")}");


                sizeOfBuffer = ous.Tell();
            }

            //Start building glTF data ready to be serialized and wrote to disk
            Console.WriteLine("glTF -> Building JSON Data");

            //Create the glTF Model, add header information
            GLTFModel model = new GLTFModel
            {
                asset = new GLTFAsset
                {
                    version = "2.0",
                    generator = "ToolsForKFIV",
                    copyright = default
                }
            };

            //
            // First, write buffer information starting with the actual buffer
            //
            model.buffers = new GLTFBuffer[]
            {
                new GLTFBuffer
                {
                    byteLength = (uint)sizeOfBuffer,
                    uri = filename + ".bin"
                }
            };

            //
            // Now 'bufferViews'...
            //
            model.bufferViews = new List<GLTFBufferView>
            {
                new GLTFBufferView  //All Vertices, Normals and Texcoords
                {
                    buffer = 0,
                    byteLength = offToIndices,
                    byteOffset = 0,
                    byteStride = default,
                    target = default
                }
            };

            // 
            // We need to add additional bufferViews for each meshes index buffer
            //
            uint tempIndicesOffset = offToIndices;
            foreach(Group group in groups.Values)
            {
                GLTFBufferView ibv = new GLTFBufferView
                {
                    buffer = 0,
                    byteLength = (uint)(4 * group.indices.Count),
                    byteOffset = tempIndicesOffset,
                    byteStride = default,
                    target = default
                };
                tempIndicesOffset += ibv.byteLength;
                model.bufferViews.Add(ibv);
            }

            //
            // After that hell, we need to add the accessors
            //
            model.accessors = new List<GLTFAccessor<float>>
            {
                new GLTFAccessor<float> //Vertices
                {
                    bufferView = 0,
                    byteOffset = 0,
                    componentType = GLTFAccessor<float>.AccessorElementType.FLOAT,
                    count = (uint)vertices.Count,
                    max = default,
                    min = default,
                    type = "VEC3"
                },
                new GLTFAccessor<float> //Normals
                {
                    bufferView = 0,
                    byteOffset = offToNormals,
                    componentType = GLTFAccessor<float>.AccessorElementType.FLOAT,
                    count = (uint)normals.Count,
                    max = default,
                    min = default,
                    type = "VEC3"
                },
                new GLTFAccessor<float> //Texcoords
                {
                    bufferView = 0,
                    byteOffset = offToTexcoords,
                    componentType = GLTFAccessor<float>.AccessorElementType.FLOAT,
                    count = (uint)texcoords.Count,
                    max = default,
                    min = default,
                    type = "VEC2"
                }
            };

            GLTFBufferView bv;
            for(int i = 1; i < model.bufferViews.Count; ++i)
            {
                bv = model.bufferViews[i];

                model.accessors.Add(new GLTFAccessor<float>
                {
                    bufferView = (uint)i,
                    byteOffset = 0,
                    componentType = GLTFAccessor<float>.AccessorElementType.UINT,
                    count = bv.byteLength / 4,
                    max = default,
                    min = default,
                    type = "SCALAR"
                });
            }

            //
            // New hell, we can now add meshes.
            //
            uint ibo = 3;
            model.meshes = new List<GLTFMesh>();
            foreach(Group group in groups.Values)
            {
                model.meshes.Add(new GLTFMesh
                {
                    primitives = new GLTFPrimitive[]
                    {
                        new GLTFPrimitive
                        {
                            attributes = new GLTFAttributes
                            {
                                POSITION   = 0,
                                NORMAL     = 1,
                                TANGENT    = default,
                                TEXCOORD_0 = 2,
                                TEXCOORD_1 = default,
                                COLOR_0    = default,
                                JOINTS_0   = default,
                                WEIGHTS_0  = default
                            },
                            indices = ibo,
                            material = default,
                            mode = 4
                        }
                    },
                    weights = default,
                    name = group.name
                });

                ibo++;
            }

            //
            // Oh my god, just scenes and nodes left now...
            //
            model.nodes = new List<GLTFNode>();
            uint j = 0;
            foreach (Group group in groups.Values)
            {
                model.nodes.Add(new GLTFNode
                {
                    name = group.name,
                    mesh = j,

                    children = default,
                    matrix = default,
                    translation = default,
                    rotation = default,
                    scale = default
                }) ;

                j++;
            }

            model.scenes = new List<GLTFScene>();

            GLTFScene scene = new GLTFScene
            {
                nodes = new uint[groups.Values.Count]
            };

            for (int i = 0; i < groups.Values.Count; ++i)
                scene.nodes[i] = (uint)i;

            model.scenes.Add(scene);

            model.scene = 0;

            //
            // Exporting the JSON data
            //
            string jsonString = JsonSerializer.Serialize(model, options);

            File.WriteAllText(filepath + filename + ".gltf", jsonString);
        }
    }
}