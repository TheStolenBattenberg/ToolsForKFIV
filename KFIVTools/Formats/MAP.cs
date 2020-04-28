using System.IO;
using System.Collections.Generic;

using KFIV.Utility.IO;
using KFIV.Utility.Type;
using KFIV.Utility.Math;

namespace KFIV.Format.MAP
{
    public class MAP
    {
        /**
         * This region contains definitons for the different sections
         * and structures of the MAP format
        **/
        #region Format Types
        public struct Header
        {
            public uint offEoF { private set; get; }                  //Pointer to end of file
            public uint offChunkPiece { private set; get; }           //Pointer to map piece chunk
            public uint offChunkPieceCollision { private set; get; }  //Pointer to map piece collsion chunk
            public uint offChunkObject { private set; get; }          //Pointer to object chunk
            public uint offChunkInteractable { private set; get; }    //Pointer to interactable chunk
            public uint offChunkObjectCollision { private set; get; } //Pointer to object collision chunk
            public uint offChunkAnimation { private set; get; }       //Pointer to animation chunk
            public uint offChunkUnused { private set; get; }          //Unused
            public uint offChunkItem { private set; get; }            //Pointer to item chunk
            public uint offChunkTexture { private set; get; }         //Pointer to texture chunk
            public uint offChunkSound { private set; get; }           //Pointer to sound chunk

            public ushort numPiece { private set; get; }           //Number of map pieces in map piece chunk
            public ushort numPieceCollision { private set; get; }  //Number of map piece collisions in piece collision chunk
            public ushort numObject { private set; get; }          //Number of objects in object chunk
            public ushort numInteractable { private set; get; }    //Number of interatables in interactable chunk
            public ushort numObjectCollision { private set; get; } //Number of object collision in object collision chunk
            public ushort numAnimation { private set; get; }       //Number of animations in animation chunk
            public ushort numUnused { private set; get; }          //Unused
            public ushort numItem { private set; get; }            //Number of items in item chunk

            public ushort numStructPiece { private set; get; } //Count of structs defining map layout
            public ushort numStructUknC { private set; get; }  //Count of structs defining ???
            public ushort numStructUknD { private set; get; }  //^^
            public ushort numStructUknE { private set; get; }  //^^
            public ushort numStructItem { private set; get; }  //Count of structs defining item locations

            public byte[] ukn46 { private set; get; } //0x3A unknown bytes

            public Header(InputStream ins)
            {
                offEoF = ins.ReadUInt32();
                offChunkPiece = ins.ReadUInt32();
                offChunkPieceCollision = ins.ReadUInt32();
                offChunkObject = ins.ReadUInt32();
                offChunkInteractable = ins.ReadUInt32();
                offChunkObjectCollision = ins.ReadUInt32();
                offChunkAnimation = ins.ReadUInt32();
                offChunkUnused = ins.ReadUInt32();
                offChunkItem = ins.ReadUInt32();
                offChunkTexture = ins.ReadUInt32();
                offChunkSound = ins.ReadUInt32();
                numPiece = ins.ReadUInt16();
                numPieceCollision = ins.ReadUInt16();
                numObject = ins.ReadUInt16();
                numInteractable = ins.ReadUInt16();
                numObjectCollision = ins.ReadUInt16();
                numAnimation = ins.ReadUInt16();
                numUnused = ins.ReadUInt16();
                numItem = ins.ReadUInt16();
                numStructPiece = ins.ReadUInt16();
                numStructUknC = ins.ReadUInt16();
                numStructUknD = ins.ReadUInt16();
                numStructUknE = ins.ReadUInt16();
                numStructItem = ins.ReadUInt16();

                ukn46 = ins.ReadBytes(0x3A);
            }
        }

        public struct StructA
        {
            public byte[] ukn { private set; get; }

            public StructA(InputStream ins)
            {
                ukn = ins.ReadBytes(0x20);
            }
        }
        public struct StructB
        {
            public Vector4 ukn00 { private set; get; }
            public Vector4 ukn10 { private set; get; }
            public Vector4 ukn20 { private set; get; }
            public Vector4 ukn30 { private set; get; }

            public StructB(InputStream ins)
            {
                ukn00 = ins.ReadVector4s();
                ukn10 = ins.ReadVector4s();
                ukn20 = ins.ReadVector4s();
                ukn30 = ins.ReadVector4s();
            }

            public void Write(OutputStream ous)
            {
                ous.WriteVector4(ukn00);
                ous.WriteVector4(ukn10);
                ous.WriteVector4(ukn20);
                ous.WriteVector4(ukn30);
            }
        }
        public struct DataA
        {
            public byte[] ukn { private set; get; }

            public DataA(InputStream ins)
            {
                ukn = ins.ReadBytes(0x340);
            }
        }
        public struct Piece
        {
            public Vector4 Position { private set; get; }             //Position of the transformed mesh
            public Vector4 Rotation { private set; get; }             //Rotation of the transformed mesh
            public Vector4 Scale { private set; get; }                //Scale of the transformed mesh
            public byte[] mesh { private set; get; }                  //Mesh used for rendering
            public byte[] collsionMesh { private set; get; }          //Mesh used for collsions
            public Vector4 meshAABBMin { private set; get; }          //Minimum point of a bounding box for the mesh
            public Vector4 meshAABBMax { private set; get; }          //Maximum point of a bounding box for the mesh
            public Vector4 collisionMeshAABBMin { private set; get; } //Minimum point of a bounding box for the collision mesh
            public Vector4 collisionMeshAABBMax { private set; get; } //Maximum point of a bounding box for the collision mesh

            public Piece(InputStream ins)
            {
                Position = ins.ReadVector4s();
                Rotation = ins.ReadVector4s();
                Scale = ins.ReadVector4s();
                mesh = ins.ReadBytes(8);
                collsionMesh = ins.ReadBytes(8);

                meshAABBMin = ins.ReadVector4s();
                meshAABBMax = ins.ReadVector4s();
                collisionMeshAABBMin = ins.ReadVector4s();
                collisionMeshAABBMax = ins.ReadVector4s();
            }

            public void Write(OutputStream ous)
            {
                ous.WriteVector4(Position);
                ous.WriteVector4(Rotation);
                ous.WriteVector4(Scale);
                ous.Write(mesh);
                ous.Write(collsionMesh);
                ous.WriteVector4(meshAABBMin);
                ous.WriteVector4(meshAABBMax);
                ous.WriteVector4(collisionMeshAABBMin);
                ous.WriteVector4(collisionMeshAABBMax);
            }
        }
        public struct StructC
        {
            public byte[] ukn { private set; get; }

            public StructC(InputStream ins)
            {
                ukn = ins.ReadBytes(0x40);
            }
        }
        public struct DataB
        {
            public byte[] ukn { private set; get; }

            public DataB(InputStream ins)
            {
                ukn = ins.ReadBytes(0x200);
            }
        }
        public struct StructD
        {
            public Vector4 Position { private set; get; }
            public Vector4 Rotation { private set; get; }
            public Vector4 Scale { private set; get; }

            public byte[] ukn { private set; get; }

            public StructD(InputStream ins)
            {
                Position = ins.ReadVector4s();
                Rotation = ins.ReadVector4s();
                Scale = ins.ReadVector4s();

                ukn = ins.ReadBytes(0x50);
            }

            public void Write(OutputStream ous)
            {
                ous.WriteVector4(Position);
                ous.WriteVector4(Rotation);
                ous.WriteVector4(Scale);

                ous.Write(ukn);
            }
        }
        public struct StructE
        {
            public Vector4 Position { private set; get; }
            public Vector4 Rotation { private set; get; }
            public Vector4 Scale { private set; get; }

            public byte[] ukn { private set; get; }

            public StructE(InputStream ins)
            {
                Position = ins.ReadVector4s();
                Rotation = ins.ReadVector4s();
                Scale = ins.ReadVector4s();

                ukn = ins.ReadBytes(0x30);
            }

            public void Write(OutputStream ous)
            {
                ous.WriteVector4(Position);
                ous.WriteVector4(Rotation);
                ous.WriteVector4(Scale);

                ous.Write(ukn);
            }
        }
        public struct Item
        {
            public Vector4 Position { private set; get; }
            public Vector4 Rotation { private set; get; }
            public Vector4 Scale { private set; get; }

            public byte[] ukn { private set; get; }

            public Item(InputStream ins)
            {
                Position = ins.ReadVector4s();
                Rotation = ins.ReadVector4s();
                Scale = ins.ReadVector4s();

                ukn = ins.ReadBytes(0x10);
            }

            public void Write(OutputStream ous)
            {
                ous.WriteVector4(Position);
                ous.WriteVector4(Rotation);
                ous.WriteVector4(Scale);
                ous.Write(ukn);
            }
        }

        public struct PartPiece
        {
            public byte[] omd { private set; get; }

            public PartPiece(InputStream ins)
            {
                uint size = ins.ReadUInt32();
                ins.Seek(-4, SeekOrigin.Current);

                omd = ins.ReadBytes((int) size); 
            }
        }
        public struct PartPieceCollision
        {
            public byte[] csk { private set; get; }

            public PartPieceCollision(InputStream ins)
            {
                ins.Seek(20, SeekOrigin.Current);
                uint size = ins.ReadUInt32();
                ins.Seek(-24, SeekOrigin.Current);

                csk = ins.ReadBytes((int)size);
            }
        }
        public struct PartObject
        {
            public byte[] omd { private set; get; }
            public byte[] tx2 { private set; get; }

            public PartObject(InputStream ins)
            {
                uint size;

                size = ins.ReadUInt32();
                ins.Seek(-4, SeekOrigin.Current);
                omd = ins.ReadBytes((int)size);

                size = ins.ReadUInt32();
                ins.Seek(-4, SeekOrigin.Current);
                tx2 = ins.ReadBytes((int)size);
            }
        }
        public struct PartInteractable
        {
            public byte[] om2 { private set; get; }
            public byte[] tx2 { private set; get; }

            public PartInteractable(InputStream ins)
            {
                uint size;

                size = ins.ReadUInt32();
                ins.Seek(-4, SeekOrigin.Current);
                om2 = ins.ReadBytes((int)size);

                size = ins.ReadUInt32();
                ins.Seek(-4, SeekOrigin.Current);
                tx2 = ins.ReadBytes((int)size);
            }
        }
        public struct PartObjectCollision
        {
            public byte[] csk { private set; get; }

            public PartObjectCollision(InputStream ins)
            {
                ins.Seek(20, SeekOrigin.Current);
                uint size = ins.ReadUInt32();
                ins.Seek(-24, SeekOrigin.Current);

                csk = ins.ReadBytes((int)size);
            }
        }
        public struct PartAnimation
        {
            public byte[] mix { private set; get; }

            public PartAnimation(InputStream ins)
            {
                uint size = ins.ReadUInt32();
                ins.Seek(-4, SeekOrigin.Current);

                mix = ins.ReadBytes((int)size);
            }
        }
        public struct PartItem
        {
            public byte[] omd { private set; get; }
            public byte[] tx2 { private set; get; }

            public PartItem(InputStream ins)
            {
                uint size;

                size = ins.ReadUInt32();
                ins.Seek(-4, SeekOrigin.Current);
                omd = ins.ReadBytes((int)size);

                size = ins.ReadUInt32();
                ins.Seek(-4, SeekOrigin.Current);
                tx2 = ins.ReadBytes((int)size);
            }
        }
        public struct PartTexture
        {
            public byte[] tx2 { private set; get; }

            public PartTexture(InputStream ins, Header header)
            {
                tx2 = ins.ReadBytes((int)(header.offChunkSound - header.offChunkTexture));
            }
        }
        public struct PartSound
        {
            public byte[] bh { private set; get; }
            public byte[] bd { private set; get; }

            public PartSound(InputStream ins)
            {
                uint bhLength = ins.ReadUInt32();
                uint bhOffset = ins.ReadUInt32();
                uint bdLength = ins.ReadUInt32();
                uint bdOffset = ins.ReadUInt32();

                ins.Seek((int)bhOffset, SeekOrigin.Begin);
                bh = ins.ReadBytes((int)bhLength);

                ins.Seek((int)bdOffset, SeekOrigin.Begin);
                bd = ins.ReadBytes((int)bdLength);
            }
        }
        #endregion

        #region Map Data
        private Header header;

        private List<StructA> structAs;
        private List<StructB> structBs;
        private DataA dataA;
        private List<Piece> pieceDat;
        private List<StructC> structCs;
        private DataB dataB;
        private List<StructD> structDs;
        private List<StructE> structEs;
        private List<Item> itemDat;

        private List<PartPiece> chunkPiece;
        private List<PartPieceCollision> chunkPieceCollision;
        private List<PartObject> chunkObject;
        private List<PartInteractable> chunkInteractable;
        private List<PartObjectCollision> chunkObjectCollision;
        private List<PartAnimation> chunkAnimation;
        private List<PartItem> chunkItem;

        private PartTexture mapTexture;
        private PartSound mapSound;
        #endregion

        public static MAP FromFile(string filename)
        {
            //Create a new map object
            MAP map = new MAP();

            //Read all data from the file
            using (InputStream ins = new InputStream(filename))
            {
                //Read Header
                map.header = new Header(ins);

                //Read 32 Struct A
                map.structAs = new List<StructA>();

                for(uint i = 0; i < 32; ++i)
                    map.structAs.Add(new StructA(ins));

                //Read 64 Struct B
                map.structBs = new List<StructB>();

                for (uint i = 0; i < 64; ++i)
                    map.structBs.Add(new StructB(ins));

                //Read DataA
                map.dataA = new DataA(ins);

                //Read map layout
                map.pieceDat = new List<Piece>();

                for (uint i = 0; i < map.header.numStructPiece; ++i)
                    map.pieceDat.Add(new Piece(ins));

                //Read Struct Cs
                map.structCs = new List<StructC>();

                for(uint i = 0; i < map.header.numStructUknC; ++i)
                    map.structCs.Add(new StructC(ins));

                //Read DataB
                map.dataB = new DataB(ins);

                //Read Struct Ds
                map.structDs = new List<StructD>();

                for (uint i = 0; i < map.header.numStructUknD; ++i)
                    map.structDs.Add(new StructD(ins));

                //Read Struct Es
                map.structEs = new List<StructE>();

                for (uint i = 0; i < map.header.numStructUknE; ++i)
                    map.structEs.Add(new StructE(ins));

                //Read item layout
                map.itemDat = new List<Item>();

                for (uint i = 0; i < map.header.numStructItem; ++i)
                    map.itemDat.Add(new Item(ins));

                //
                //READ CHUNKS
                //

                //Piece Chunk
                map.chunkPiece = new List<PartPiece>();
                ins.Seek(map.header.offChunkPiece, SeekOrigin.Begin);
                for (uint i = 0; i < map.header.numPiece; ++i)
                    map.chunkPiece.Add(new PartPiece(ins));

                //Piece Collision Chunk
                map.chunkPieceCollision = new List<PartPieceCollision>();
                ins.Seek(map.header.offChunkPieceCollision, SeekOrigin.Begin);
                for (uint i = 0; i < map.header.numPieceCollision; ++i)
                    map.chunkPieceCollision.Add(new PartPieceCollision(ins));

                //Object Chunk
                map.chunkObject = new List<PartObject>();
                ins.Seek(map.header.offChunkObject, SeekOrigin.Begin);
                for (uint i = 0; i < map.header.numObject; ++i)
                    map.chunkObject.Add(new PartObject(ins));

                //Interactable Chunk
                map.chunkInteractable = new List<PartInteractable>();
                ins.Seek(map.header.offChunkInteractable, SeekOrigin.Begin);
                for (uint i = 0; i < map.header.numInteractable; ++i)
                    map.chunkInteractable.Add(new PartInteractable(ins));

                //Object Collision Chunk
                map.chunkObjectCollision = new List<PartObjectCollision>();
                ins.Seek(map.header.offChunkObjectCollision, SeekOrigin.Begin);
                for(uint i = 0; i < map.header.numObjectCollision; ++i)
                    map.chunkObjectCollision.Add(new PartObjectCollision(ins));

                //Animation chunk
                map.chunkAnimation = new List<PartAnimation>();
                ins.Seek(map.header.offChunkAnimation, SeekOrigin.Begin);
                for (uint i = 0; i < map.header.numAnimation; ++i)
                    map.chunkAnimation.Add(new PartAnimation(ins));

                //Item Chunk
                map.chunkItem = new List<PartItem>();
                ins.Seek(map.header.offChunkItem, SeekOrigin.Begin);
                for (uint i = 0; i < map.header.numItem; ++i)
                    map.chunkItem.Add(new PartItem(ins));

                ins.Seek(map.header.offChunkTexture, SeekOrigin.Begin);
                map.mapTexture = new PartTexture(ins, map.header);

                ins.Seek(map.header.offChunkSound, SeekOrigin.Begin);
                map.mapSound = new PartSound(ins);
            }

            return map;
        }

        public void Save(string path)
        {
            //Create directories
            Directory.CreateDirectory(path + "piece");
            Directory.CreateDirectory(path + "piece\\collision");
            Directory.CreateDirectory(path + "interactable");
            Directory.CreateDirectory(path + "object");
            Directory.CreateDirectory(path + "object\\collision");
            Directory.CreateDirectory(path + "animation");
            Directory.CreateDirectory(path + "item");

            //Save Chunked Files (ez)
            for(int i = 0; i < chunkPiece.Count; ++i)
                OutputStream.WriteFile(path + "piece\\mesh_" + i.ToString("D4") + ".omd", chunkPiece[i].omd);

            for(int i = 0; i < chunkPieceCollision.Count; ++i)
                OutputStream.WriteFile(path + "piece\\collision\\mesh_" + i.ToString("D4") + ".csk", chunkPieceCollision[i].csk);

            for(int i = 0; i < chunkObject.Count; ++i)
            {
                OutputStream.WriteFile(path + "object\\mesh_" + i.ToString("D4") + ".omd", chunkObject[i].omd);
                OutputStream.WriteFile(path + "object\\texture_" + i.ToString("D4") + ".tx2", chunkObject[i].tx2);
            }

            for(int i = 0; i < chunkInteractable.Count; ++i)
            {
                OutputStream.WriteFile(path + "interactable\\mesh_" + i.ToString("D4") + ".om2", chunkInteractable[i].om2);
                OutputStream.WriteFile(path + "interactable\\texture_" + i.ToString("D4") + ".tx2", chunkInteractable[i].tx2);
            }

            for (int i = 0; i < chunkObjectCollision.Count; ++i)
                OutputStream.WriteFile(path + "object\\collision\\mesh_" + i.ToString("D4") + ".csk", chunkObjectCollision[i].csk);

            for (int i = 0; i < chunkAnimation.Count; ++i)
                OutputStream.WriteFile(path + "animation\\anim_" + i.ToString("D4") + ".mix", chunkAnimation[i].mix);

            for (int i = 0; i < chunkItem.Count; ++i)
            {
                OutputStream.WriteFile(path + "item\\mesh_" + i.ToString("D4") + ".omd", chunkItem[i].omd);
                OutputStream.WriteFile(path + "item\\texture_" + i.ToString("D4") + ".tx2", chunkItem[i].tx2);
            }

            OutputStream.WriteFile(path + "texture.tx2", mapTexture.tx2);
            OutputStream.WriteFile(path + "sfx.bh", mapSound.bh);
            OutputStream.WriteFile(path + "sfx.bd", mapSound.bd);

            //Save DATs
            using(OutputStream ous = new OutputStream(path + "structa.dat"))
            {
                foreach(StructA obj in structAs)
                {
                    ous.Write(obj.ukn);
                }
            }

            using (OutputStream ous = new OutputStream(path + "structb.dat"))
            {
                foreach (StructB obj in structBs)
                {
                    obj.Write(ous);
                }
            }

            using(OutputStream ous = new OutputStream(path + "structc.dat"))
            {
                foreach(StructC obj in structCs)
                {
                    ous.Write(obj.ukn);
                }
            }

            using(OutputStream ous = new OutputStream(path + "structd.dat"))
            {
                foreach(StructD obj in structDs)
                {
                    obj.Write(ous);
                }
            }

            using(OutputStream ous = new OutputStream(path + "structe.dat"))
            {
                foreach(StructE obj in structEs)
                {
                    obj.Write(ous);
                }
            }

            using(OutputStream ous = new OutputStream(path + "layout.dat"))
            {
                foreach(Piece obj in pieceDat)
                {
                    obj.Write(ous);
                }
            }

            using(OutputStream ous = new OutputStream(path + "item.dat"))
            {
                foreach(Item obj in itemDat)
                {
                    obj.Write(ous);
                }
            }

            OutputStream.WriteFile(path + "data1.bin", dataA.ukn);
            OutputStream.WriteFile(path + "data2.bin", dataB.ukn);
        }
    }
}
