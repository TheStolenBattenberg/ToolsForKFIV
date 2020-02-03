using System.IO;
using System.Collections.Generic;

using KFIV.Utility.IO;
using KFIV.Utility.Type;

namespace KFIV.Format.MAP
{
    public class MAP
    {
        #region Format Types
        public struct Header
        {
            public uint lenFile { private set; get; }
            public uint offChunkGeometry   { private set; get; }
            public uint offChunkUknA       { private set; get; }
            public uint offChunkObjA       { private set; get; }
            public uint offChunkObjB       { private set; get; }
            public uint offChunkUknB       { private set; get; }
            public uint offChunkUknC       { private set; get; }
            public uint offChunkUknD       { private set; get; }
            public uint offChunkItem       { private set; get; }
            public uint offChunkPage       { private set; get; }
            public uint offChunkSound      { private set; get; }
            public ushort numChunkGeometry { private set; get; }
            public ushort numChunkUknA     { private set; get; }
            public ushort numChunkObjA     { private set; get; }
            public ushort numChunkObjB     { private set; get; }
            public ushort numChunkUknB     { private set; get; }
            public ushort numChunkUknC     { private set; get; }
            public ushort numChunkUknD     { private set; get; }
            public ushort numChunkItem     { private set; get; }
            public ushort numChunkPage     { private set; get; }
            public ushort numChunkSound    { private set; get; }

            public Header(InputStream bin)
            {
                lenFile = bin.ReadUInt32();
                offChunkGeometry = bin.ReadUInt32();
                offChunkUknA = bin.ReadUInt32();
                offChunkObjA = bin.ReadUInt32();
                offChunkObjB = bin.ReadUInt32();
                offChunkUknB = bin.ReadUInt32();
                offChunkUknC = bin.ReadUInt32();
                offChunkUknD = bin.ReadUInt32();
                offChunkItem = bin.ReadUInt32();
                offChunkPage = bin.ReadUInt32();
                offChunkSound = bin.ReadUInt32();
                numChunkGeometry = bin.ReadUInt16();
                numChunkUknA = bin.ReadUInt16();
                numChunkObjA = bin.ReadUInt16();
                numChunkObjB = bin.ReadUInt16();
                numChunkUknB = bin.ReadUInt16();
                numChunkUknC = bin.ReadUInt16();
                numChunkUknD = bin.ReadUInt16();
                numChunkItem = bin.ReadUInt16();
                numChunkPage = bin.ReadUInt16();
                numChunkSound = bin.ReadUInt16();
            }
        }
        public struct ChunkGeometry
        {
            public byte[] omd { private set; get; }

            public ChunkGeometry(InputStream bin)
            {
                int size;

                size = bin.ReadInt32();
                bin.Seek(-4, SeekOrigin.Current);
                omd = bin.ReadBytes(size);
            }
        }
        public struct ChunkUknA
        { 
            public byte[] uf0 { private set; get; }

            public ChunkUknA(InputStream bin)
            {
                int size;

                bin.Seek(20, SeekOrigin.Current);
                size = bin.ReadInt32();
                bin.Seek(-24, SeekOrigin.Current);
                uf0 = bin.ReadBytes(size);
            }
        }
        public struct ChunkObjectA
        {
            public byte[] omd { private set; get; }
            public byte[] tx2 { private set; get; }

            public ChunkObjectA(InputStream bin)
            {
                int size;

                size = bin.ReadInt32();
                bin.Seek(-4, SeekOrigin.Current);
                omd = bin.ReadBytes(size);

                size = bin.ReadInt32();
                bin.Seek(-4, SeekOrigin.Current);
                tx2 = bin.ReadBytes(size);
            }
        }
        public struct ChunkObjectB
        {
            public byte[] om2 { private set; get; }
            public byte[] tx2 { private set; get; }

            public ChunkObjectB(InputStream bin)
            {
                int size;

                size = bin.ReadInt32();
                bin.Seek(-4, SeekOrigin.Current);
                om2 = bin.ReadBytes(size);

                size = bin.ReadInt32();
                bin.Seek(-4, SeekOrigin.Current);
                tx2 = bin.ReadBytes(size);
            }
        }
        public struct ChunkUknB
        {
            public byte[] uf0 { private set; get; }

            public ChunkUknB(InputStream bin)
            {
                int size;

                bin.Seek(20, SeekOrigin.Current);
                size = bin.ReadInt32();
                bin.Seek(-24, SeekOrigin.Current);
                uf0 = bin.ReadBytes(size);
            }
        }
        public struct ChunkUknC
        {
            public byte[] uf1 { private set; get; }

            public ChunkUknC(InputStream bin)
            {
                int size;

                size = bin.ReadInt32();
                bin.Seek(-4, SeekOrigin.Current);
                uf1 = bin.ReadBytes(size);
            }
        }
        public struct ChunkUknD 
        { }
        public struct ChunkItem
        { 
            public byte[] omd { private set; get; }
            public byte[] tx2 { private set; get; }

            public ChunkItem(InputStream bin)
            {
                int size;

                size = bin.ReadInt32();
                bin.Seek(-4, SeekOrigin.Current);
                omd = bin.ReadBytes(size);

                size = bin.ReadInt32();
                bin.Seek(-4, SeekOrigin.Current);
                tx2 = bin.ReadBytes(size);
            }
        }
        public struct ChunkPage
        { 
            public byte[] tx2 { private set; get; }

            public ChunkPage(InputStream bin)
            {
                int size;

                bin.Seek(1, SeekOrigin.Current);
                size = (bin.ReadByte() << 12) + 80; //TX2 + Header size
                bin.Seek(-2, SeekOrigin.Current);

                tx2 = bin.ReadBytes(size);
            }
        }
        public struct ChunkSound
        { 
            public byte[] BH { private set; get; }
            public byte[] BD { private set; get; }

            public ChunkSound(InputStream bin)
            {
                int bhLen, bhOff;
                bhLen = bin.ReadInt32();
                bhOff = bin.ReadInt32();

                int bdLen, bdOff;
                bdLen = bin.ReadInt32();
                bdOff = bin.ReadInt32();

                bin.Seek(bhOff, SeekOrigin.Begin);
                BH = bin.ReadBytes(bhLen);

                bin.Seek(bdOff, SeekOrigin.Begin);
                BD = bin.ReadBytes(bdLen);
            }
        
        }

        #endregion
        #region Data
        public List<ChunkGeometry> chunkGeometry;
        public List<ChunkUknA>     chunkUknA;
        public List<ChunkObjectA>  chunkObjectA;
        public List<ChunkObjectB>  chunkObjectB;
        public List<ChunkUknB>     chunkUknB;
        public List<ChunkUknC>     chunkUknC;
        public List<ChunkUknD>     chunkUknD;
        public List<ChunkItem>     chunkItem;
        public List<ChunkPage>     chunkPage;
        public List<ChunkSound>    chunkSound;

        #endregion

        public static MAP FromFile(string path)
        {
            MAP map = new MAP();

            using (InputStream bin = new InputStream(path))
            {
                //Header
                Header header = new Header(bin);

                //Geometry Chunk
                bin.Seek(header.offChunkGeometry, SeekOrigin.Begin);
                map.chunkGeometry = new List<ChunkGeometry>();
                for (uint i = 0; i < header.numChunkGeometry; ++i)
                    map.chunkGeometry.Add(new ChunkGeometry(bin));

                //Unknown chunk A
                bin.Seek(header.offChunkUknA, SeekOrigin.Begin);
                map.chunkUknA = new List<ChunkUknA>();
                for (uint i = 0; i < header.numChunkUknA; ++i)
                    map.chunkUknA.Add(new ChunkUknA(bin));

                //Object A chunk
                bin.Seek(header.offChunkObjA, SeekOrigin.Begin);
                map.chunkObjectA = new List<ChunkObjectA>();
                for (uint i = 0; i < header.numChunkObjA; ++i)
                    map.chunkObjectA.Add(new ChunkObjectA(bin));

                //Object B chunk
                bin.Seek(header.offChunkObjB, SeekOrigin.Begin);
                map.chunkObjectB = new List<ChunkObjectB>();
                for (uint i = 0; i < header.numChunkObjB; ++i)
                    map.chunkObjectB.Add(new ChunkObjectB(bin));

                //Unknown chunk B
                bin.Seek(header.offChunkUknB, SeekOrigin.Begin);
                map.chunkUknB = new List<ChunkUknB>();
                for (uint i = 0; i < header.numChunkUknB; ++i)
                    map.chunkUknB.Add(new ChunkUknB(bin));

                //Unknown chunk C
                bin.Seek(header.offChunkUknC, SeekOrigin.Begin);
                map.chunkUknC = new List<ChunkUknC>();
                for (uint i = 0; i < header.numChunkUknC; ++i)
                    map.chunkUknC.Add(new ChunkUknC(bin));

                //Unknown chunk D always empty

                //Item chunk
                bin.Seek(header.offChunkItem, SeekOrigin.Begin);
                map.chunkItem = new List<ChunkItem>();
                for (uint i = 0; i < header.numChunkItem; ++i)
                    map.chunkItem.Add(new ChunkItem(bin));

                //TexPage chunk (count broken somehow)
                bin.Seek(header.offChunkPage, SeekOrigin.Begin);
                map.chunkPage = new List<ChunkPage>();
                ///for (uint i = 0; i < header.numChunkPage; ++i)
                    map.chunkPage.Add(new ChunkPage(bin));

                //Sound chunk (count broken somehow... means BH + BD?)
                bin.Seek(header.offChunkSound, SeekOrigin.Begin);
                map.chunkSound = new List<ChunkSound>();
                //for (uint i = 0; i < header.numChunkSound; ++i)
                    map.chunkSound.Add(new ChunkSound(bin));
    
            }

            return map;
        }

        public void Save(string path)
        {
            //Create Directories
            Directory.CreateDirectory(path + "C0Geometry");
            Directory.CreateDirectory(path + "C1UknownA");
            Directory.CreateDirectory(path + "C2ObjectA");
            Directory.CreateDirectory(path + "C3ObjectB");
            Directory.CreateDirectory(path + "C4UknownB");
            Directory.CreateDirectory(path + "C5UknownC");
            Directory.CreateDirectory(path + "C7Item");
            Directory.CreateDirectory(path + "C8Page");
            Directory.CreateDirectory(path + "C9Sound");

            uint index = 0;

            //Save Geometry
            index = 0;
            foreach(ChunkGeometry c in chunkGeometry)
            {
                OutputStream.WriteFile(path + "C0Geometry\\geopart_" + index.ToString("D4") + ".omd", c.omd);
                index++;
            }

            //Save Unknown A
            index = 0;
            foreach (ChunkUknA c in chunkUknA)
            {
                OutputStream.WriteFile(path + "C1UknownA\\unknown_" + index.ToString("D4") + ".uf0", c.uf0);
                index++;
            }

            //Save Object A
            index = 0;
            foreach (ChunkObjectA c in chunkObjectA)
            {
                OutputStream.WriteFile(path + "C2ObjectA\\object_" + index.ToString("D4") + ".omd", c.omd);
                OutputStream.WriteFile(path + "C2ObjectA\\object_" + index.ToString("D4") + ".tx2", c.tx2);
                index++;
            }

            //Save Object B
            index = 0;
            foreach (ChunkObjectB c in chunkObjectB)
            {
                OutputStream.WriteFile(path + "C3ObjectB\\object_" + index.ToString("D4") + ".om2", c.om2);
                OutputStream.WriteFile(path + "C3ObjectB\\object_" + index.ToString("D4") + ".tx2", c.tx2);
                index++;
            }

            //Save Unknown B
            index = 0;
            foreach (ChunkUknB c in chunkUknB)
            {
                OutputStream.WriteFile(path + "C4UknownB\\unknown_" + index.ToString("D4") + ".uf0", c.uf0);
                index++;
            }

            //Save Unknown C
            index = 0;
            foreach (ChunkUknC c in chunkUknC)
            {
                OutputStream.WriteFile(path + "C5UknownC\\unknown_" + index.ToString("D4") + ".uf1", c.uf1);
                index++;
            }

            //Save Item
            index = 0;
            foreach (ChunkItem c in chunkItem)
            {
                OutputStream.WriteFile(path + "C7Item\\item_" + index.ToString("D4") + ".omd", c.omd);
                OutputStream.WriteFile(path + "C7Item\\item_" + index.ToString("D4") + ".tx2", c.tx2);
                index++;
            }

            //Save Page
            index = 0;
            foreach (ChunkPage c in chunkPage)
            {
                OutputStream.WriteFile(path + "C8Page\\page_" + index.ToString("D4") + ".tx2", c.tx2);
                index++;
            }

            //Save sound
            index = 0;
            foreach (ChunkSound c in chunkSound)
            {
                OutputStream.WriteFile(path + "C9Sound\\bank_" + index.ToString("D4") + ".bh", c.BH);
                OutputStream.WriteFile(path + "C9Sound\\bank_" + index.ToString("D4") + ".bd", c.BD);
                index++;
            }
        }
    }
}
