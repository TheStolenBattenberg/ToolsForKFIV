﻿using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Asset;
using FormatKFIV.Utility;
using FormatKFIV.FileFormat;
using FormatKFIV.TypePlayStation;

namespace FormatKFIV.FileFormat
{
    public class FFSceneMAP : FIFormat<Scene>
    {
        #region Format Structures
        public struct MAPHeader
        {
            public uint length;
            public uint offPieceOMD;
            public uint offPieceCSK;
            public uint offObjectStatic;
            public uint offObjectMoveable;
            public uint offObjectCSK;
            public uint offAnimation;
            public uint Unknown0x1C;
            public uint offItem;
            public uint offTexture;
            public uint offSoundFX;
            public ushort numPieceOMD;
            public ushort numPieceCSK;
            public ushort numObjectStatic;
            public ushort numObjectMoveable;
            public ushort numObjectCSK;
            public ushort numAnimation;
            public ushort Unknown0x38;
            public ushort numItem;
            public ushort numNodePiece;
            public ushort numDispGroup;
            public ushort numNodeObject;
            public ushort numNodeEntity;
            public ushort numNodeItem;

            public ushort Unknown0x46;
            public uint Unknown0x48;
            public float Unknown0x4C;
            public float Unknown0x50;
            public uint Unknown0x54;
            public uint Unknown0x58;
            public uint Unknown0x5C;
            public Vector4f WarpLocation;   //XYZ = XYZ, W = Camera Pan

            public byte musicTrackID;

            public byte Unknown0x71;
            public ushort Unknown0x72;
            public uint Unknown0x74;
            public uint Unknown0x78;
            public uint Unknown0x7C;
        }
        public struct MAPDrawParam
        {
            public byte[] todo; //32 bytes
        }
        public struct MAPLightParam
        {
            public float[] transform;       //64 bytes
            public float[] litColour;       //48 bytes
            public float[] ambientColour;   //16 bytes  
        }
        public struct MAPScrollParam
        {
            public byte[] todo; //0x340 bytes
        }   //TO-DO
        public struct MAPNodePiece
        {
            public Vector4f Position;
            public Vector4f Rotation;
            public Vector4f Scale;
            public short OMDIndex;
            public short CSKIndex;
            public byte  DrawParamId;
            public byte  LightParamId;
            public short Pad0x36;       //Always 0x0000
            public byte  DispGroup;
            public byte  Pad0x39;       //Always 0x00.
            public byte  Pad0x3A;       //Always 0xFF? Maybe pad.
            public byte Unknown0x3B;    //Writing to adress in PCSX2 always does number you put + 4?.. Odd value...
            public uint Flags;          //0x10000000 = Red, 0x01000000 = Translucent, 0x00010000 = Flashing/BreakTexture, 
            public Vector4f omdAABBMin;
            public Vector4f omdAABBMax;
            public Vector4f cskAABBMin;
            public Vector4f cskAABBMax;
        }  //Stores information about how map pieces are layed out in the world.
        public struct MAPDispGroup
        {
            public byte[] todo; //0x40 bytes
        }   //TODO 
        public struct MAPStructA
        {
            public Vector3f SomeVec;
            public uint Unknown0x0C;
        } 
        public struct MAPNodeObject
        {
            public Vector4f Position;
            public Vector4f Rotation;
            public Vector4f Scale;
            public short ClassId;
            public short RenderMeshId;
            public short CollisionMeshId;
            public short Unknown0x36;
            public short Unknown0x38;
            public short Unknown0x3A;
            public short Unknown0x3C;
            public short Unknown0x3E;

            public byte[] ClassParams;
        }
        public struct MAPNodeEntity
        {

        }
        public struct MAPNodeItem
        {

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
                ".map",
            },
            Type = FEType.Scene,
            AllowExport = false,
            Validator = FFSceneMAP.FileIsValid
        };

        /// <summary>Validates a file to see if it is KFIV Map Format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    // :)
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

        public Scene LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            //We don't use these, so we null them right away.
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(filepath))
            {
                return ImportMAP(ins);
            }
        }
        public Scene LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            //We don't use these, so we null them right away.
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(buffer))
            {
                return ImportMAP(ins);
            }
        }

        public Scene ImportMAP(InputStream ins)
        {
            Scene OUT = new Scene();

            //Try Importing MAP data
            try
            {
                //Read MAP header
                MAPHeader mapHeader = new MAPHeader
                {
                    length = ins.ReadUInt32(),
                    offPieceOMD = ins.ReadUInt32(),
                    offPieceCSK = ins.ReadUInt32(),
                    offObjectStatic = ins.ReadUInt32(),
                    offObjectMoveable = ins.ReadUInt32(),
                    offObjectCSK = ins.ReadUInt32(),
                    offAnimation = ins.ReadUInt32(),
                    Unknown0x1C = ins.ReadUInt32(),
                    offItem = ins.ReadUInt32(),
                    offTexture = ins.ReadUInt32(),
                    offSoundFX = ins.ReadUInt32(),
                    numPieceOMD = ins.ReadUInt16(),
                    numPieceCSK = ins.ReadUInt16(),
                    numObjectStatic = ins.ReadUInt16(),
                    numObjectMoveable = ins.ReadUInt16(),
                    numObjectCSK = ins.ReadUInt16(),
                    numAnimation = ins.ReadUInt16(),
                    Unknown0x38 = ins.ReadUInt16(),
                    numItem = ins.ReadUInt16(),
                    numNodePiece = ins.ReadUInt16(),
                    numDispGroup = ins.ReadUInt16(),
                    numNodeObject = ins.ReadUInt16(),
                    numNodeEntity = ins.ReadUInt16(),
                    numNodeItem = ins.ReadUInt16(),

                    Unknown0x46 = ins.ReadUInt16(),
                    Unknown0x48 = ins.ReadUInt32(),
                    Unknown0x4C = ins.ReadSingle(),
                    Unknown0x50 = ins.ReadSingle(),
                    Unknown0x54 = ins.ReadUInt32(),
                    Unknown0x58 = ins.ReadUInt32(),
                    Unknown0x5C = ins.ReadUInt32(),

                    WarpLocation = ins.ReadVector4f(),

                    musicTrackID = ins.ReadByte(),
                    Unknown0x71 = ins.ReadByte(),
                    Unknown0x72 = ins.ReadUInt16(),
                    Unknown0x74 = ins.ReadUInt32(),
                    Unknown0x78 = ins.ReadUInt32(),
                    Unknown0x7C = ins.ReadUInt32()
                };

                //Read Map Draw Param
                MAPDrawParam[] mapDrawParam = new MAPDrawParam[32];
                for (int i = 0; i < 32; ++i)
                {
                    mapDrawParam[i] = new MAPDrawParam
                    {
                        todo = ins.ReadBytes(32)
                    };
                }

                //Read Map Light Param
                MAPLightParam[] mapLightParam = new MAPLightParam[32];
                for(int i = 0; i < 32; ++i)
                {
                    mapLightParam[i] = new MAPLightParam
                    {
                        transform = ins.ReadSingles(16),
                        litColour = ins.ReadSingles(12),
                        ambientColour = ins.ReadSingles(4)
                    };
                }

                //Read Map Scroll Param
                MAPScrollParam mapScrollParam = new MAPScrollParam
                {
                    todo = ins.ReadBytes(0x340)
                };

                //Read Map Nodes (Piece)
                MAPNodePiece[] pieceNodes = new MAPNodePiece[mapHeader.numNodePiece];
                for(int i = 0; i < mapHeader.numNodePiece; ++i)
                {
                    pieceNodes[i] = new MAPNodePiece
                    {
                        Position = ins.ReadVector4f(),
                        Rotation = ins.ReadVector4f(),
                        Scale    = ins.ReadVector4f(),
                        OMDIndex = ins.ReadInt16(),
                        CSKIndex = ins.ReadInt16(),
                        DrawParamId  = ins.ReadByte(),
                        LightParamId = ins.ReadByte(),
                        Pad0x36      = ins.ReadInt16(),
                        DispGroup    = ins.ReadByte(),
                        Pad0x39      = ins.ReadByte(),
                        Pad0x3A      = ins.ReadByte(),
                        Unknown0x3B  = ins.ReadByte(),
                        Flags        = ins.ReadUInt32(),
                        omdAABBMin = ins.ReadVector4f(),
                        omdAABBMax = ins.ReadVector4f(),
                        cskAABBMin = ins.ReadVector4f(),
                        cskAABBMax = ins.ReadVector4f()
                    };
                }

                //Read Map Display Groups
                MAPDispGroup[] mapDispGroup = new MAPDispGroup[mapHeader.numDispGroup];
                for(int i = 0; i < mapHeader.numDispGroup; ++i)
                {
                    mapDispGroup[i] = new MAPDispGroup
                    {
                        todo = ins.ReadBytes(64)
                    };
                }

                //Read Map StructA
                MAPStructA[] mapStructA = new MAPStructA[32];
                for (int i = 0; i < 32; ++i)
                {
                    mapStructA[i] = new MAPStructA
                    {
                        SomeVec = ins.ReadVector3f(),
                        Unknown0x0C = ins.ReadUInt32()
                    };
                }

                //Read Map Object Nodes
                MAPNodeObject[] mapObjectNode = new MAPNodeObject[mapHeader.numNodeObject];
                for(int i = 0; i < mapHeader.numNodeObject; ++i)
                {
                    mapObjectNode[i] = new MAPNodeObject
                    {
                        Position = ins.ReadVector4f(),
                        Rotation = ins.ReadVector4f(),
                        Scale = ins.ReadVector4f(),
                        ClassId = ins.ReadInt16(),
                        RenderMeshId = ins.ReadInt16(),
                        CollisionMeshId = ins.ReadInt16(),
                        Unknown0x36 = ins.ReadInt16(),
                        Unknown0x38 = ins.ReadInt16(),
                        Unknown0x3A = ins.ReadInt16(),
                        Unknown0x3C = ins.ReadInt16(),
                        Unknown0x3E = ins.ReadInt16(),
                        ClassParams = ins.ReadBytes(64),
                    };
                }

                //
                // MAP DATA
                //
                FFModelOMD   OMDLoader = new FFModelOMD();
                FFTextureTX2 TX2Loader = new FFTextureTX2();

                //Read OMD Models

                //We do this to retrieve proper Texture W/H and colour mode.
                FFModelOMD.OMDModel[] pieceOMDs = new FFModelOMD.OMDModel[mapHeader.numPieceOMD];
                ins.Jump(mapHeader.offPieceOMD);
                for (int i = 0; i < mapHeader.numPieceOMD; ++i)
                {
                    pieceOMDs[i] = FFModelOMD.ImportOMDFromStream(ins);
                }
                ins.Return();

                //Unpack textures

                //Texture Writer
                FFTexturePNG pngHandler = new FFTexturePNG();

                //Save each texture as PNG.
                Texture texs = UnpackMAPTX2(ins, mapHeader, pieceOMDs);
                pngHandler.SaveToFile("D:\\REEEE\\test\\tx2Out", texs);


                List<int> PieceDrawMdl = new List<int>();

                //Load Objects
                Model[] mapObjectOMD = new Model[mapHeader.numObjectStatic];
                Texture[] mapObjectTX2 = new Texture[mapHeader.numObjectStatic];

                ins.Jump(mapHeader.offObjectStatic);
                for(int i = 0; i < mapHeader.numObjectStatic; ++i)
                {
                    mapObjectOMD[i] = OMDLoader.ImportOMD(ins);
                    mapObjectTX2[i] = TX2Loader.ImportTX2(ins);

                    if(mapObjectTX2[i] == null)
                    {
                        Console.WriteLine("wtf?");
                    }
                }
                ins.Return();


                //
                // Scene Building
                //
                //Read MAP Peice OMDs
                ins.Jump(mapHeader.offPieceOMD);
                for (int i = 0; i < mapHeader.numPieceOMD; ++i)
                {
                    PieceDrawMdl.Add(OUT.AddModel(OMDLoader.ImportOMD(ins)));
                }
                ins.Return();

                foreach (MAPNodePiece piece in pieceNodes)
                {
                    Scene.Chunk chunk = new Scene.Chunk();

                    chunk.Flags = piece.Flags;
                    chunk.drawModelID = piece.OMDIndex;
                    chunk.hitcModelID = piece.CSKIndex;
                    chunk.Position = piece.Position;
                    chunk.Rotation = piece.Rotation;
                    chunk.Scale    = piece.Scale;

                    OUT.AddChunk(chunk);
                }

                foreach(MAPNodeObject obj in mapObjectNode)
                {
                    Scene.Object sObj = new Scene.Object
                    {
                        Position = obj.Position,
                        Rotation = obj.Rotation,
                        Scale = obj.Scale,

                        ClassId = obj.ClassId,
                        MeshId = -1,
                        TextureId = -1
                    };

                    if(obj.RenderMeshId != -1)
                    {
                        sObj.MeshId = OUT.AddModel(mapObjectOMD[obj.RenderMeshId]);
                        sObj.TextureId = OUT.AddTexture(mapObjectTX2[obj.RenderMeshId]);
                    }

                    OUT.sceneObject.Add(sObj);
                }
            } catch(Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                return null;
            }

            return OUT;
        }

        private Texture UnpackMAPTX2(InputStream ins, MAPHeader header, FFModelOMD.OMDModel[] omdModels)
        {
            Texture textures = new Texture();

            //Byte array mimics PS2 VRAM
            byte[] vram = new byte[4194304];
            //Array of OMD models for texture restoration
            FFModelOMD.OMDModel[] models = omdModels;

            //Read MAP Textures

            //Calculate Texture Chunk Size
            long tx2ChunkSize = header.offSoundFX - header.offTexture;
            ins.Jump(header.offTexture);


            int texID = 5;
            while ((ins.Position() - header.offTexture) < tx2ChunkSize)
            {
                int tx2Length = (int)((ins.ReadUInt32() - 7) * 16);

                //Sometimes TX2 data isn't alligned properly. no idea why.
                if(tx2Length < 0)
                {
                    ins.Seek(12, System.IO.SeekOrigin.Current);
                    continue;
                }

                ins.Seek(44, System.IO.SeekOrigin.Current); //Skip 44 bytes we really don't care about.
                sceGsBitbltbuf bitbltbuf = ins.ReadPS28ByteRegister()._sceGsBitbltbuf;
                ins.Seek(24, System.IO.SeekOrigin.Current);
                sceGsTrxreg texreg = ins.ReadPS28ByteRegister()._sceGsTrxreg;
                ins.Seek(40, System.IO.SeekOrigin.Current); //Skip 72 bytes we also really don't care about.

                //Read data into buffer
                byte[] tx2Buffer = ins.ReadBytes(tx2Length);
                int vramDestination = (int)(bitbltbuf.DBP * 0x100);

                //Copy buffer into vram
                Array.Copy(tx2Buffer, 0, vram, vramDestination, tx2Length);

                texID--;

                if (-1 < texID)
                    break;
                Console.WriteLine($"Read TX2 Texture {5 - texID}");
            }
            ins.Return();

            //Extract Textures using OMDs

            //Allocate a dictionary to store texture locations we've already extracted.
            Dictionary<uint, bool> keys = new Dictionary<uint, bool>();

            //Loop through each OMD model.
            foreach(FFModelOMD.OMDModel omdModel in models)
            {
                //For each omdMesh in the model...
                foreach(FFModelOMD.OMDMesh omdMesh in omdModel.meshes)
                {
                    //And each tristrip packet inside that mesh...
                    foreach(FFModelOMD.OMDTristripPacket omdTriS in omdMesh.tristrips)
                    {
                        //Do magic with PS2 registers
                        int clutAddr = (int)(omdTriS.tex0Data.CBP * 0x100);
                        int clutFrmt = (int)(omdTriS.tex0Data.CPSM);
                        int imgbAddr = (int)(omdTriS.tex0Data.TBP * 0x100);
                        int imgbFrmt = (int)(omdTriS.tex0Data.PSM);
                        int imgbW = (int)Math.Pow(2, omdTriS.tex0Data.TW);
                        int imgbH = (int)Math.Pow(2, omdTriS.tex0Data.TH);

                        uint texKey = (omdTriS.tex0Data.CBP << 16) | (omdTriS.tex0Data.TBP);

                        if (keys.ContainsKey(texKey))
                        {
                            continue;
                        }
                        keys.Add(texKey, true);

                        //Console.WriteLine($"Clut Address: {(clutAddr / 0x100).ToString("X4")}");
                        Console.WriteLine($"Image Address: {(imgbAddr / 0x100).ToString("X4")}");

                        //Extract a texture from our PS2 Vram
                        Texture.ImageBuffer imgdBuffer = new Texture.ImageBuffer
                        {
                            Name = null,
                            Width = (uint)imgbW,
                            Height = (uint)imgbH,
                            Format = Texture.PSMtoColourMode((uint)imgbFrmt),
                        };

                        int lineByteSize = 0;

                        switch(imgdBuffer.Format)
                        {
                            case Texture.ColourMode.M4:
                                imgdBuffer.Length = (imgdBuffer.Width >> 1) * imgdBuffer.Height;
                                imgdBuffer.ClutCount = 1;
                                imgdBuffer.ClutIDs = new int[1];
                                imgdBuffer.data = new byte[imgdBuffer.Length];

                                lineByteSize = (int)(imgdBuffer.Width >> 1);
                                break;

                            case Texture.ColourMode.M8:
                                imgdBuffer.Length = imgdBuffer.Width * imgdBuffer.Height;
                                imgdBuffer.ClutCount = 1;
                                imgdBuffer.ClutIDs = new int[1];
                                imgdBuffer.data = new byte[imgdBuffer.Length];

                                lineByteSize = (int)imgdBuffer.Width;
                                break;

                            case Texture.ColourMode.D16:
                                imgdBuffer.Length = (imgdBuffer.Width * 2) * imgdBuffer.Height;
                                imgdBuffer.data = new byte[imgdBuffer.Length];

                                lineByteSize = (int)imgdBuffer.Width * 2;
                                break;

                            case Texture.ColourMode.D24:
                                imgdBuffer.Length = (imgdBuffer.Width * 3) * imgdBuffer.Height;
                                imgdBuffer.data = new byte[imgdBuffer.Length];

                                lineByteSize = (int)imgdBuffer.Width * 3;
                                break;

                            case Texture.ColourMode.D32:
                                imgdBuffer.Length = (imgdBuffer.Width * 4) * imgdBuffer.Height;
                                imgdBuffer.data = new byte[imgdBuffer.Length];

                                lineByteSize = (int)imgdBuffer.Width * 4;
                                break;
                        }

                        //Copy Texture by rows... This needs to actually work please.
                        int imgReadOffset = imgbAddr;
                        int imgWriteOffset = 0;
                        for(int i = 0; i < imgdBuffer.Height; ++i)
                        {
                            Array.Copy(vram, imgReadOffset, imgdBuffer.data, imgWriteOffset, lineByteSize);
                            imgWriteOffset += lineByteSize;
                            imgReadOffset += lineByteSize;
                        }

                        if (imgdBuffer.Format == Texture.ColourMode.M4 || imgdBuffer.Format == Texture.ColourMode.M8)
                        {
                            Texture.PS2UnswizzleImageData(ref imgdBuffer.data, imgdBuffer.Format, (int)imgdBuffer.Width, (int)imgdBuffer.Height);
                        }
                        //Does texture have clut?
                        if(imgdBuffer.Format == Texture.ColourMode.M4 || imgdBuffer.Format == Texture.ColourMode.M8)
                        {
                            Texture.ClutBuffer clutBuffer = new Texture.ClutBuffer
                            {
                                Format = Texture.PSMtoColourMode((uint)clutFrmt),
                            };

                            switch(imgdBuffer.Format)
                            {
                                case Texture.ColourMode.M4:
                                    clutBuffer.Width = 8;
                                    clutBuffer.Height = 2;
                                    clutBuffer.Length = (clutBuffer.Width * 4) * clutBuffer.Height;
                                    clutBuffer.data = new byte[clutBuffer.Length];
                                    Array.Copy(vram, clutAddr, clutBuffer.data, 0, clutBuffer.Length);

                                    Texture.ConvColourSpaceBGRAtoRGBA(ref clutBuffer.data, Texture.ColourMode.D32, true);

                                    break;

                                case Texture.ColourMode.M8:
                                    clutBuffer.Width = 16;
                                    clutBuffer.Height = 16;
                                    clutBuffer.Length = (clutBuffer.Width * 4) * clutBuffer.Height;
                                    clutBuffer.data = new byte[clutBuffer.Length];

                                    //Read palette
                                    int lineReadOffset = clutAddr + 2048;
                                    int lineWriteOffset = 0;
                                    int lineStride = 256;

                                    for(int i = 0; i < 16; ++i)
                                    {
                                        Array.Copy(vram, lineReadOffset, clutBuffer.data, lineWriteOffset, 64);
                                        lineReadOffset  += lineStride;
                                        lineWriteOffset += 64;
                                    }

                                    Texture.ConvColourSpaceBGRAtoRGBA(ref clutBuffer.data, Texture.ColourMode.D32, true);
                                    break;
                            }

                            imgdBuffer.ClutIDs[0] = textures.PutClut(clutBuffer);
                        }

                        textures.PutSubimage(imgdBuffer);
                    }
                }
            }

            return textures;
        }


        public void SaveToFile(string filepath, Scene data)
        {
            throw new NotImplementedException();
        }
    }
}