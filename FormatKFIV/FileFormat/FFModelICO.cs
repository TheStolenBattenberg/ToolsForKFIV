using System;

using FormatKFIV.TypePlayStation;
using FormatKFIV.Utility;
using FormatKFIV.Asset;

namespace FormatKFIV.FileFormat
{
    /// <summary>PlayStation 2 ICO file, for use in save data to provide a save file icon</summary>
    public class FFModelICO : FIFormat<Model>
    {
        #region Format Structures
        /// <summary>Contains basic information about an ICO</summary>
        public struct ICOHeader
        {
            /// <summary>ICO File Version</summary>
            public uint version;
            /// <summary>Number of contained shapes</summary>
            public uint numShape;
            /// <summary>Flags. 0x7 = Uncompressed Texture (16bit), 0x6 = Uncompressed Texture (32bit), 0xF = RLE Texture</summary>
            public uint flags;
            /// <summary>Anti aliasing coefficient (usually 1.0f)</summary>
            public float aa1Coeff;
        }

        /// <summary>ICO Fixed point vertex. Divide component by 1024f to get float value</summary>
        public struct ICOVertex
        {
            /// <summary>X Component</summary>
            public short X;
            /// <summary>Y Component</summary>
            public short Y;
            /// <summary>Z Component</summary>
            public short Z;
            /// <summary>W Component</summary>
            public short W;
        }

        /// <summary>ICO Fixed point normal. Divide component by 4096f to get float value</summary>
        public struct ICONormal
        {
            /// <summary>X Component</summary>
            public short X;
            /// <summary>Y Component</summary>
            public short Y;
            /// <summary>Z Component</summary>
            public short Z;
            /// <summary>W Component</summary>
            public short W;
        }

        /// <summary>ICO Fixed point texcoord. Divide component by 4096f to get float value</summary>
        public struct ICOTexcoord
        {
            /// <summary>U Component</summary>
            public short U;
            /// <summary>V Component</summary>
            public short V;
        }

        /// <summary>ICO RGBA Colour</summary>
        public struct ICOColour
        {
            /// <summary>R Component</summary>
            public byte R;
            /// <summary>G Component</summary>
            public byte G;
            /// <summary>B Component</summary>
            public byte B;
            /// <summary>A Component</summary>
            public byte A;
        }

        /// <summary>Contains a list of vertices that define a shape</summary>
        public struct ICOShape
        {
            /// <summary>List of vertices</summary>
            public ICOVertex[] vertices;
            /// <summary>List of normals</summary>
            public ICONormal[] normals;
            /// <summary>List of texcoords</summary>
            public ICOTexcoord[] texcoords;
            /// <summary>List of colours</summary>
            public ICOColour[] colours;
        }

        /// <summary>Contains basic information about an ICO</summary>
        public struct ICOModel
        {
            /// <summary>Number of vertices</summary>
            public uint numVertex;
            /// <summary>List of ICOShape</summary>
            public ICOShape[] shapes;
        }

        /// <summary>An animation key</summary>
        public struct ICOKey
        {
            /// <summary>Length of this key, in time units</summary>
            public float time;
            /// <summary>Value of this key. Unknown purpose</summary>
            public float value;
        }

        /// <summary>An animation frame</summary>
        public struct ICOFrame
        {
            /// <summary>The shape to use for this frame</summary>
            public uint shapeID;
            /// <summary>Total keys in the frame</summary>
            public uint numKey;
            /// <summary>List of keys</summary>
            public ICOKey[] keys;
        }

        /// <summary>An animation sequence</summary>
        public struct ICOSequence
        {
            /// <summary>Length of a frame</summary>
            public uint frameLength;
            /// <summary>Playback speed</summary>
            public float speed;
            /// <summary>Playback frame offset</summary>
            public uint playOffset;
            /// <summary>Count of keyframes inside sequence</summary>
            public uint numFrame;
            /// <summary>List of frames</summary>
            public ICOFrame[] frames;
        }

        /// <summary>Contains data for animations</summary>
        public struct ICOAnimation
        {
            /// <summary>The total count of sequences inside the animation</summary>
            public uint numSequence;
            /// <summary>List of sequences</summary>
            public ICOSequence[] sequences;
        }
        #endregion
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".ico",
            },
            Type = FEType.Model,
            AllowExport = false,
            Validator = FFModelICO.FileIsValid
        };

        /// <summary>Validates a file to see if it is PS2 ICO Format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    //Check ICO Version
                    validFile = (ins.ReadUInt32() == 0x00010000);
                    ins.ReadUInt32();

                    //Check ICO Flags
                    validFile = validFile & (ins.ReadUInt32() == 0x7);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    Console.WriteLine(Ex.StackTrace);
                    return false;
                }
            }

            return validFile;
        }

        #endregion

        /// <summary>Load an ICO from buffered storage</summary>
        /// <param name="buffer">A byte buffer containing ICO data.</param>
        /// <param name="ret2">(Optional, null) A Texture Class</param>
        /// <param name="ret3">(Optional, null) N/A</param>
        /// <param name="ret4">(Optional, null) N/A</param>
        /// <returns>A Model Class containing the ICO data</returns>
        public Model LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            Model M = null;
            using (InputStream ins = new InputStream(buffer))
            {
                M = ImportICO(ins, out ret2, out ret3, out ret4);
            }

            return M;
        }

        /// <summary>Load an ICO from the file system</summary>
        /// <param name="buffer">A byte buffer containing ICO data.</param>
        /// <param name="ret2">(Optional, null) A Texture Class</param>
        /// <param name="ret3">(Optional, null) N/A</param>
        /// <param name="ret4">(Optional, null) N/A</param>
        /// <returns>A Model Class containing the ICO data</returns>
        public Model LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            Model M = null;
            using (InputStream ins = new InputStream(filepath))
            {
                M = ImportICO(ins, out ret2, out ret3, out ret4);
            }

            return M;
        }

        /// <summary>Import ICO</summary>
        /// <param name="ins">An InputStream</param>
        /// <param name="ret2">(Optional, null) A Texture Class</param>
        /// <param name="ret3">(Optional, null) N/A</param>
        /// <param name="ret4">(Optional, null) N/A</param>
        /// <returns>A Model Class containing the ICO data</returns>
        private static Model ImportICO(InputStream ins, out object ret2, out object ret3, out object ret4)
        {
            Model result = new Model();
            Texture ResultingTexture = new Texture();

            try
            {
                //Read Header
                ICOHeader header = new ICOHeader
                {
                    version = ins.ReadUInt32(),
                    numShape = ins.ReadUInt32(),
                    flags = ins.ReadUInt32(),
                    aa1Coeff = ins.ReadSingle()
                };

                //Read ICO Model
                ICOModel model = new ICOModel
                {
                    numVertex = ins.ReadUInt32(),
                };
                model.shapes = new ICOShape[header.numShape];


                //Read ICO Model Shapes
                for(int i = 0; i < header.numShape; ++i)
                {
                    model.shapes[i] = new ICOShape
                    {
                        vertices = new ICOVertex[model.numVertex],
                        normals = new ICONormal[model.numVertex],
                        texcoords = new ICOTexcoord[model.numVertex],
                        colours = new ICOColour[model.numVertex]
                    };

                    //Read Vertices
                    for (int j = 0; j < model.numVertex; ++j)
                    {
                        //Vertex
                        model.shapes[i].vertices[j] = new ICOVertex
                        {
                            X = ins.ReadInt16(),
                            Y = ins.ReadInt16(),
                            Z = ins.ReadInt16(),
                            W = ins.ReadInt16()
                        };

                        //Normal
                        model.shapes[i].normals[j] = new ICONormal
                        {
                            X = ins.ReadInt16(),
                            Y = ins.ReadInt16(),
                            Z = ins.ReadInt16(),
                            W = ins.ReadInt16()
                        };

                        //Texcoord
                        model.shapes[i].texcoords[j] = new ICOTexcoord
                        {
                            U = ins.ReadInt16(),
                            V = ins.ReadInt16()
                        };

                        //Colour
                        model.shapes[i].colours[j] = new ICOColour
                        {
                            R = ins.ReadByte(),
                            G = ins.ReadByte(),
                            B = ins.ReadByte(),
                            A = ins.ReadByte()
                        };
                    }
                }

                //Read ICO Animation (todo - clean me)
                ICOAnimation anim = new ICOAnimation
                {
                    numSequence = ins.ReadUInt32(),
                };
                anim.sequences = new ICOSequence[anim.numSequence];

                for(int i = 0; i < anim.numSequence; ++i)
                {
                    anim.sequences[i] = new ICOSequence
                    {
                        frameLength = ins.ReadUInt32(),
                        speed = ins.ReadSingle(),
                        playOffset = ins.ReadUInt32(),
                        numFrame = ins.ReadUInt32()
                    };
                    anim.sequences[i].frames = new ICOFrame[anim.sequences[i].numFrame];

                    for(int j = 0; j < anim.sequences[i].numFrame; ++j)
                    {
                        anim.sequences[i].frames[j] = new ICOFrame
                        {
                            shapeID = ins.ReadUInt32(),
                            numKey = ins.ReadUInt32()
                        };
                        anim.sequences[i].frames[j].keys = new ICOKey[anim.sequences[i].frames[j].numKey];
                        
                        for(int k = 0; k < anim.sequences[i].frames[j].numKey; ++k)
                        {
                            anim.sequences[i].frames[j].keys[k] = new ICOKey
                            {
                                time = ins.ReadSingle(),
                                value = ins.ReadSingle()
                            };
                        }
                    }
                }

                //Read Texture (Assume 16bpp)
                long texLen = ins.Length() - ins.Position();

                Texture.ImageBuffer imgBuff = new Texture.ImageBuffer
                {
                    Width = 128,
                    Height = 128,
                    Length = (uint)texLen,
                    Format = Texture.ColourMode.D16,
                    ClutCount = 0,
                    ClutIDs = null,
                    data = ins.ReadBytes((int)texLen)
                };

                ResultingTexture.PutSubimage(imgBuff);

                //Convert each shape to a mesh
                for(int i = 0; i < model.shapes.Length; ++i)
                {
                    Model.Mesh mesh = new Model.Mesh();
                    mesh.textureSlot = -1;

                    mesh.primitives = new Model.IPrimitiveType[model.numVertex / 3];

                    for (int j = 0; j < model.numVertex; j += 3)
                    {
                        Model.TrianglePrimitive tri = Model.TrianglePrimitive.New();

                        for(int k = 0; k < 3; ++k)
                        {
                            ICOVertex V = model.shapes[i].vertices[j + k];
                            ICONormal N = model.shapes[i].normals[j + k];
                            ICOTexcoord T = model.shapes[i].texcoords[j + k];
                            ICOColour C = model.shapes[i].colours[j + k];

                            tri.Indices[0 + k] = result.AddVertex(V.X / 1024f, -V.Y / 1024f, -V.Z / 1024f);
                            tri.Indices[3 + k] = result.AddNormal(N.X / 4096f, -N.Y / 4096f, -N.Z / 4096f);
                            tri.Indices[6 + k] = result.AddTexcoord(T.U / 4096f, T.V / 4096f);
                            tri.Indices[9 + k] = result.AddColour(C.R / 128f, C.G / 128f, C.B / 128f, C.A / 128f);

                        }

                        mesh.primitives[j / 3] = tri;
                    }

                    result.AddMesh(mesh);
                }

            } catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);

                ret2 = null;
                ret3 = null;
                ret4 = null;
                return null;
            }

            ret2 = ResultingTexture;
            ret3 = null;
            ret4 = null;
            return result;
        }

        public void SaveToFile(string filepath, Model data)
        {
            throw new NotImplementedException();
        }
    }
}
