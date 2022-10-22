using System;
using System.Collections.Generic;

using FormatKFIV.Asset;
using FormatKFIV.Utility;
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
            public Vector4f position;
            public Vector4f rotation;
            public Vector4f scale;
            public byte[] unknown0x30;  //Unknown bytes (48 of 'em)
        }
        public struct MAPNodeItem
        {
            public Vector4f position;
            public Vector4f rotation;
            public Vector4f scale;
            public ushort classID;
            public ushort omdID;
            public uint unknown0x34;
            public uint unknown0x38;
            public uint unknown0x3c;
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

                    Console.WriteLine($"Object {i} (Class ID: " +
                        $"{mapObjectNode[i].ClassId}, RMesh ID: " +
                        $"{mapObjectNode[i].RenderMeshId}, CMesh ID: " +
                        $"{mapObjectNode[i].CollisionMeshId}, 0x36: " +
                        $"{mapObjectNode[i].Unknown0x36.ToString("X4")}, 0x38: " +
                        $"{mapObjectNode[i].Unknown0x38.ToString("X4")}, 0x3A: " +
                        $"{mapObjectNode[i].Unknown0x3A.ToString("X4")}, 0x3C: " +
                        $"{mapObjectNode[i].Unknown0x3C.ToString("X4")}, 0x3E: " +
                        $"{mapObjectNode[i].Unknown0x3E.ToString("X4")}");
                }

                //Read Map Entity Nodes
                MAPNodeEntity[] mapEntityNodes = new MAPNodeEntity[mapHeader.numNodeEntity];
                for(int i = 0; i < mapHeader.numNodeEntity; ++i)
                {
                    mapEntityNodes[i] = new MAPNodeEntity
                    {
                        position = ins.ReadVector4f(),
                        rotation = ins.ReadVector4f(),
                        scale = ins.ReadVector4f(),
                        unknown0x30 = ins.ReadBytes(48)
                    };
                }

                MAPNodeItem[] mapItemNodes = new MAPNodeItem[mapHeader.numNodeItem];
                for(int i = 0; i < mapHeader.numNodeItem; ++i)
                {
                    mapItemNodes[i] = new MAPNodeItem
                    {
                        position = ins.ReadVector4f(),
                        rotation = ins.ReadVector4f(),
                        scale = ins.ReadVector4f(),
                        classID = ins.ReadUInt16(),
                        omdID = ins.ReadUInt16(),
                        unknown0x34 = ins.ReadUInt32(),
                        unknown0x38 = ins.ReadUInt32(),
                        unknown0x3c = ins.ReadUInt32()
                    };
                }

                //
                // MAP DATA
                //
                FFModelOMD OMDLoader = new FFModelOMD();
                FFModelOM2 OM2Loader = new FFModelOM2();

                FFTextureTX2 TX2Loader = new FFTextureTX2();
                FFTexturePNG PNGFormat = new FFTexturePNG();

                /*
                // Attempt texture extraction
                FFModelOMD.OMDModel[] mapOMDs = new FFModelOMD.OMDModel[mapHeader.numPieceOMD];
                Texture mapTextures;

                ins.Jump(mapHeader.offPieceOMD);
                for (int i = 0; i < mapHeader.numPieceOMD; ++i)
                {
                    mapOMDs[i] = FFModelOMD.ImportOMDFromStream(ins);
                }
                ins.Return();

                mapTextures = UnpackMAPTX2(ins, mapHeader, ref mapOMDs);
                PNGFormat.SaveToFile("D:\\REEEE\\KF4TexOut\\tex", mapTextures);
                */

                // Map Geometry
                Console.WriteLine("FFSceneMAP -> Importing Geometry...");
                int[] mapChunkOMD = new int[mapHeader.numPieceOMD];
                int[] mapChunkCSK = new int[mapHeader.numPieceCSK];

                ins.Jump(mapHeader.offPieceOMD);
                for (int i = 0; i < mapHeader.numPieceOMD; ++i)
                {
                    OUT.omdData.Add(OMDLoader.ImportOMD(ins));
                    mapChunkOMD[i] = OUT.omdData.Count - 1;
                }
                ins.Return();

                ins.Jump(mapHeader.offPieceCSK);
                for(int i = 0; i < mapHeader.numPieceCSK; ++i)
                {
                    OUT.cskData.Add(FFModelCSK.CSKToModel(FFModelCSK.ImportCSK(ins)));
                    mapChunkCSK[i] = OUT.cskData.Count - 1;
                }
                ins.Return();

                foreach (MAPNodePiece piece in pieceNodes)
                {
                    Scene.Chunk chunk = new Scene.Chunk()
                    {
                        position = new Vector3f(piece.Position.X, -piece.Position.Y, -piece.Position.Z),
                        rotation = new Vector3f(piece.Rotation.X, 6.28318530718f - piece.Rotation.Y, 6.28318530718f - piece.Rotation.Z),
                        scale = piece.Scale.ToVector3(),
                        drawModelID = -1,
                        collisionModelID = -1,
                        drawAABB = -1,
                        collisionAABB = -1
                    };

                    Vector3f aabbMin;
                    Vector3f aabbMax;

                    if(piece.OMDIndex >= 0 && piece.OMDIndex < mapChunkOMD.Length)
                    {
                        aabbMin = new Vector3f(piece.omdAABBMin.X, -piece.omdAABBMin.Y, -piece.omdAABBMin.Z);
                        aabbMax = new Vector3f(piece.omdAABBMax.X, -piece.omdAABBMax.Y, -piece.omdAABBMax.Z);
                        OUT.aabbData.Add(Model.GenerateLineCube(aabbMin, aabbMax, Colour.FromARGB(255, 255, 127, 127)));
                        
                        chunk.drawAABB = OUT.aabbData.Count - 1;
                        chunk.drawModelID = mapChunkOMD[piece.OMDIndex];
                    }

                    if(piece.CSKIndex >= 0 && piece.CSKIndex < mapChunkCSK.Length)
                    {
                        aabbMin = new Vector3f(piece.cskAABBMin.X, -piece.cskAABBMin.Y, -piece.cskAABBMin.Z);
                        aabbMax = new Vector3f(piece.cskAABBMax.X, -piece.cskAABBMax.Y, -piece.cskAABBMax.Z);
                        OUT.aabbData.Add(Model.GenerateLineCube(aabbMin, aabbMax, Colour.FromARGB(255, 127, 127, 255)));

                        chunk.collisionAABB = OUT.aabbData.Count - 1;
                        chunk.collisionModelID = mapChunkCSK[piece.CSKIndex];
                    }
                    
                    OUT.chunks.Add(chunk);
                }


                //
                // Map Objects
                //
                Console.WriteLine("FFSceneMAP -> Importing Objects...");
                int[] mapObjectOMD = new int[mapHeader.numObjectStatic];      //OMD
                int[] mapObjectOM2 = new int[mapHeader.numObjectMoveable];    //OM2
                int[] mapObjectOM2TX2 = new int[mapHeader.numObjectMoveable]; //OM2 TX2
                int[] mapObjectOMDTX2 = new int[mapHeader.numObjectStatic];   //OMD TX2
                int[] mapObjectCSK = new int[mapHeader.numObjectCSK];         //CSK

                ins.Jump(mapHeader.offObjectStatic);
                for(int i = 0; i < mapHeader.numObjectStatic; ++i)
                {
                    OUT.omdData.Add(OMDLoader.ImportOMD(ins));
                    OUT.texData.Add(TX2Loader.ImportTX2(ins));

                    mapObjectOMD[i] = OUT.omdData.Count - 1;
                    mapObjectOMDTX2[i] = OUT.texData.Count - 1;
                }
                ins.Return();

                ins.Jump(mapHeader.offObjectMoveable);
                for(int i = 0; i < mapHeader.numObjectMoveable; ++i)
                {
                    OUT.om2Data.Add(OM2Loader.ImportOM2(ins));
                    OUT.texData.Add(TX2Loader.ImportTX2(ins));

                    mapObjectOM2[i] = OUT.om2Data.Count - 1;
                    mapObjectOM2TX2[i] = OUT.texData.Count - 1;
                }
                ins.Return();

                ins.Jump(mapHeader.offObjectCSK);
                for(int i = 0; i < mapHeader.numObjectCSK; ++i)
                {
                    OUT.cskData.Add(FFModelCSK.CSKToModel(FFModelCSK.ImportCSK(ins)));
                    mapObjectCSK[i] = OUT.cskData.Count - 1;
                }
                ins.Return();
                 
                foreach(MAPNodeObject mapObject in mapObjectNode)
                {

                    Scene.Object sceneObject = new Scene.Object
                    {
                        position = new Vector3f(mapObject.Position.X, -mapObject.Position.Y, -mapObject.Position.Z),
                        rotation = new Vector3f(mapObject.Rotation.X, 6.28318530718f - mapObject.Rotation.Y, 6.28318530718f - mapObject.Rotation.Z),
                        scale = mapObject.Scale.ToVector3(),
                        classID = mapObject.ClassId,
                        classParams = mapObject.ClassParams,
                        drawModelID = -1,
                        textureID = -1,
                        collisionModelID = -1,
                        
                    };

                    switch(sceneObject.classID)
                    {
                        case 0x001A:
                        case 0x0020:
                        case 0x0041:
                        case 0x0044:
                        case 0x0045:
                        case 0x0046:
                            if(mapObject.RenderMeshId >= 0 && mapObject.RenderMeshId < mapObjectOM2.Length)
                            {
                                sceneObject.drawModelID = mapObjectOM2[mapObject.RenderMeshId];
                                sceneObject.textureID = mapObjectOM2TX2[mapObject.RenderMeshId];
                            }
                            break;

                        default:
                            if (mapObject.RenderMeshId >= 0 && mapObject.RenderMeshId < mapObjectOMD.Length)
                            {
                                sceneObject.drawModelID = mapObjectOMD[mapObject.RenderMeshId];
                                sceneObject.textureID = mapObjectOMDTX2[mapObject.RenderMeshId];
                            }
                            break;
                    }



                    if(mapObject.CollisionMeshId >= 0 && mapObject.CollisionMeshId < mapObjectCSK.Length)
                    {
                        sceneObject.collisionModelID = mapObjectCSK[mapObject.CollisionMeshId];
                    }

                    OUT.objects.Add(sceneObject);
                }

                //
                // Map Items
                //
                Console.WriteLine("FFSceneMAP -> Importing Items...");
                int[] mapItemOMD = new int[mapHeader.numItem];
                int[] mapItemTX2 = new int[mapHeader.numItem];

                ins.Jump(mapHeader.offItem);
                for (int i = 0; i < mapHeader.numItem; ++i)
                {
                    OUT.omdData.Add(OMDLoader.ImportOMD(ins));
                    OUT.texData.Add(TX2Loader.ImportTX2(ins));

                    mapItemOMD[i] = OUT.omdData.Count - 1;
                    mapItemTX2[i] = OUT.texData.Count - 1;
                }
                ins.Return();

                foreach(MAPNodeItem mapItem in mapItemNodes)
                {
                    Scene.Item sceneItem = new Scene.Item()
                    {
                        position = new Vector3f(mapItem.position.X, -mapItem.position.Y, -mapItem.position.Z),
                        rotation = new Vector3f(mapItem.rotation.X, 6.28318530718f - mapItem.rotation.Y, 6.28318530718f - mapItem.rotation.Z),
                        scale = mapItem.scale.ToVector3(),
                        classID = mapItem.classID,
                        omdID = mapItemOMD[mapItem.omdID],
                        texID = mapItemTX2[mapItem.omdID]
                    };

                    OUT.items.Add(sceneItem);
                }

            } catch(Exception Ex) {

                //SOMETHINGS FUCKED UP BARRY! BETTER GET THE CHAINSAW.
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                return null;
            }

            return OUT;
        }

        private Texture UnpackMAPTX2(InputStream ins, MAPHeader header, ref FFModelOMD.OMDModel[] omdModels)
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

            int texID = 0;
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
                ins.Seek(40, System.IO.SeekOrigin.Current); //Skip 40 bytes we also really don't care about.

                //Read data into buffer
                byte[] tx2Buffer = ins.ReadBytes(tx2Length);

                int vramDestination = (int)(bitbltbuf.DBP * 0x100);

                //Copy buffer into vram
                Array.Copy(tx2Buffer, 0, vram, vramDestination, tx2Length);

                texID++;
                Console.WriteLine($"Read TX2 Texture {texID}");
                Console.WriteLine($"DBP: {bitbltbuf.DBP.ToString("X8")}");
                Console.WriteLine($"SBP: {bitbltbuf.SBP.ToString("X8")}");
                Console.WriteLine($"Width: {texreg.RRW.ToString("D8")}");
                Console.WriteLine($"Height: {texreg.RRH.ToString("D8")}");
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

                        uint texKey = ((omdTriS.tex0Data.CBP & 0xFFFF) << 16) | (omdTriS.tex0Data.TBP & 0xFFFF);

                        if (keys.ContainsKey(texKey))
                        {
                            continue;
                        }
                        keys.Add(texKey, true);

                        //Console.WriteLine($"Clut Address: {(clutAddr / 0x100).ToString("X4")}");
                        Console.WriteLine($"Image Address: {(imgbAddr / 0x100).ToString("X4")}");
                        Console.WriteLine($"Image BW: {omdTriS.tex0Data.TBW * 0x40}");
                        Console.WriteLine($"Image W: {imgbW}");

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
                                imgdBuffer.Name = "[TBP 0x"+omdTriS.tex0Data.TBP.ToString("X8")+"][CBP 0x"+omdTriS.tex0Data.CBP.ToString("X8") +"][BPP 4]";

                                lineByteSize = (int)(imgdBuffer.Width >> 1);
                                break;

                            case Texture.ColourMode.M8:
                                imgdBuffer.Length = imgdBuffer.Width * imgdBuffer.Height;
                                imgdBuffer.ClutCount = 1;
                                imgdBuffer.ClutIDs = new int[1];
                                imgdBuffer.data = new byte[imgdBuffer.Length];
                                imgdBuffer.Name = "[TBP 0x" + omdTriS.tex0Data.TBP.ToString("X8") + "][CBP 0x" + omdTriS.tex0Data.CBP.ToString("X8") + "][BPP 8]";

                                lineByteSize = (int)imgdBuffer.Width;
                                break;

                            case Texture.ColourMode.D16:
                                imgdBuffer.Length = (imgdBuffer.Width * 2) * imgdBuffer.Height;
                                imgdBuffer.data = new byte[imgdBuffer.Length];
                                imgdBuffer.Name = "[TBP 0x" + omdTriS.tex0Data.TBP.ToString("X8") + "][BPP 16]";

                                lineByteSize = (int)imgdBuffer.Width * 2;
                                break;

                            case Texture.ColourMode.D24:
                                imgdBuffer.Length = (imgdBuffer.Width * 3) * imgdBuffer.Height;
                                imgdBuffer.data = new byte[imgdBuffer.Length];
                                imgdBuffer.Name = "[TBP 0x" + omdTriS.tex0Data.TBP.ToString("X8") + "][BPP 24]";

                                lineByteSize = (int)imgdBuffer.Width * 3;
                                break;

                            case Texture.ColourMode.D32:
                                imgdBuffer.Length = (imgdBuffer.Width * 4) * imgdBuffer.Height;
                                imgdBuffer.data = new byte[imgdBuffer.Length];
                                imgdBuffer.Name = "[TBP 0x" + omdTriS.tex0Data.TBP.ToString("X8") + "][BPP 32]";

                                lineByteSize = (int)imgdBuffer.Width * 4;
                                break;
                        }

                        //Copy Texture by rows... This needs to actually work please.
                        int imgReadOffset = imgbAddr;
                        int imgWriteOffset = 0;
                        for(int i = 0; i < imgdBuffer.Height; ++i)
                        {
                            Array.Copy(vram, imgReadOffset, imgdBuffer.data, imgWriteOffset, lineByteSize);
                            imgReadOffset += 256;
                            imgWriteOffset += lineByteSize;
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

                            int lineReadOffset = 0, lineWriteOffset = 0;

                            switch (imgdBuffer.Format)
                            {
                                case Texture.ColourMode.M4:
                                    clutBuffer.Width = 8;
                                    clutBuffer.Height = 2;
                                    clutBuffer.Length = (clutBuffer.Width * 4) * clutBuffer.Height;
                                    clutBuffer.data = new byte[clutBuffer.Length];

                                    //Read palette
                                    lineReadOffset = clutAddr;
                                    lineWriteOffset = 0;

                                    for (int i = 0; i < clutBuffer.Height; ++i)
                                    {
                                        Array.Copy(vram, lineReadOffset, clutBuffer.data, lineWriteOffset, (clutBuffer.Width * 4));
                                        lineReadOffset += 256;
                                        lineWriteOffset += (int) (clutBuffer.Width * 4);
                                    }

                                    Texture.ConvColourSpaceBGRAtoRGBA(ref clutBuffer.data, Texture.ColourMode.D32, true);

                                    break;

                                case Texture.ColourMode.M8:
                                    clutBuffer.Width = 16;
                                    clutBuffer.Height = 16;
                                    clutBuffer.Length = (clutBuffer.Width * 4) * clutBuffer.Height;
                                    clutBuffer.data = new byte[clutBuffer.Length];

                                    //Read palette
                                    lineReadOffset = clutAddr + 2048;
                                    lineWriteOffset = 0;

                                    for(int i = 0; i < clutBuffer.Height; ++i)
                                    {
                                        Array.Copy(vram, lineReadOffset, clutBuffer.data, lineWriteOffset, (clutBuffer.Width * 4));
                                        lineReadOffset  += 256;
                                        lineWriteOffset += (int)( clutBuffer.Width * 4);
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