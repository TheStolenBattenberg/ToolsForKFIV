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
            public uint offChunkChest      { private set; get; }
            public uint offChunkObject     { private set; get; }
            public uint offChunkUknB       { private set; get; }
            public uint offChunkUknC       { private set; get; }
            public uint offChunkUknD       { private set; get; }
            public uint offChunkItem       { private set; get; }
            public uint offChunkPage       { private set; get; }
            public uint offChunkSound      { private set; get; }
            public ushort numChunkGeometry { private set; get; }
            public ushort numChunkUknA     { private set; get; }
            public ushort numChunkChest    { private set; get; }
            public ushort numChunkObject   { private set; get; }
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
                offChunkChest = bin.ReadUInt32();
                offChunkObject = bin.ReadUInt32();
                offChunkUknB = bin.ReadUInt32();
                offChunkUknC = bin.ReadUInt32();
                offChunkUknD = bin.ReadUInt32();
                offChunkItem = bin.ReadUInt32();
                offChunkPage = bin.ReadUInt32();
                offChunkSound = bin.ReadUInt32();
                numChunkGeometry = bin.ReadUInt16();
                numChunkUknA = bin.ReadUInt16();
                numChunkChest = bin.ReadUInt16();
                numChunkObject = bin.ReadUInt16();
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
        { }
        public struct ChunkChest
        {
            public byte[] omd { private set; get; }
            public byte[] tx2 { private set; get; }

            public ChunkChest(InputStream bin)
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
        public struct ChunkObject
        {
            public byte[] om2 { private set; get; }
            public byte[] tx2 { private set; get; }

            public ChunkObject(InputStream bin)
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
        { }
        public struct ChunkUknC
        { }
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
        { }
        public struct ChunkSound
        {

        }

        #endregion
        #region Data
        public List<ChunkGeometry> chunkGeometry;
        public List<ChunkUknA>     chunkUknA;
        public List<ChunkChest>    chunkChest;
        public List<ChunkObject>   chunkObject;
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

                //Read Chunks
                bin.Seek(header.offChunkGeometry, SeekOrigin.Begin);
                map.chunkGeometry = new List<ChunkGeometry>();
                for (uint i = 0; i < header.numChunkGeometry; ++i)
                    map.chunkGeometry.Add(new ChunkGeometry(bin));

                bin.Seek(header.offChunkChest, SeekOrigin.Begin);
                map.chunkChest = new List<ChunkChest>();
                for (uint i = 0; i < header.numChunkChest; ++i)
                    map.chunkChest.Add(new ChunkChest(bin));

                bin.Seek(header.offChunkObject, SeekOrigin.Begin);
                map.chunkObject = new List<ChunkObject>();
                for (uint i = 0; i < header.numChunkObject; ++i)
                    map.chunkObject.Add(new ChunkObject(bin));

                bin.Seek(header.offChunkItem, SeekOrigin.Begin);
                map.chunkItem = new List<ChunkItem>();
                for (uint i = 0; i < header.numChunkItem; ++i)
                    map.chunkItem.Add(new ChunkItem(bin));
    
            }

            return map;
        }
    }
}
