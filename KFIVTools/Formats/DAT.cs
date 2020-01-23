using System;
using System.IO;
using System.Collections.Generic;

namespace KFIV.Format.DAT1
{
    public class DAT
    {
        #region Format Types
        public struct Header
        {
            public uint fileLength { private set; get; }
            public uint fileCount  { private set; get; }

            public Header(BinaryReader bin)
            {
                fileLength = bin.ReadUInt32();
                fileCount  = bin.ReadUInt32();

                bin.ReadBytes(56);  //Read padding
            }
        }
        public struct File
        {
            public string name { private set; get; }
            public uint length { private set; get; }
            public uint lengthPadded { private set; get; }
            public uint offset { private set; get; }

            public byte[] bytes;

            public File(BinaryReader bin)
            {
                name = new string(bin.ReadChars(52)).TrimEnd((char)0x0).Replace("/", "\\");
                length = bin.ReadUInt32();
                lengthPadded = bin.ReadUInt32();
                offset = bin.ReadUInt32();

                //Seek to offset
                long oldOffset = bin.BaseStream.Position;
                bin.BaseStream.Seek(offset, SeekOrigin.Begin);

                //Read file bytes
                bytes = bin.ReadBytes((int) length);

                //Seek to previous position
                bin.BaseStream.Seek(oldOffset, SeekOrigin.Begin);
            }
            
            public void Save(string path)
            {
                Save(path, name);
            }

            public void Save(string path, string name)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path + name));

                using(BinaryWriter bin = new BinaryWriter(System.IO.File.Open(path + name, FileMode.Create)))
                {
                    bin.Write(bytes);
                }
            }
        }

        #endregion
        #region Data
        private Header header;
        private List<File> files;

        #endregion

        public static DAT FromFile(string path)
        {
            //Open File
            BinaryReader bin;
            try
            {
                bin = new BinaryReader(System.IO.File.Open(path, FileMode.Open));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            DAT dat = new DAT();

            //Read DAT Header
            dat.header = new Header(bin);
            dat.files = new List<File>((int) dat.header.fileCount);

            //Read DAT Files
            for (uint i = 0; i < dat.header.fileCount; ++i)
                dat.files.Add(new File(bin));

            //Close BinaryReader
            bin.Close();

            return dat;
        }

        public uint Count { 
            get { return header.fileCount; }
        }
        public File this[uint i]
        {
            get { return files[(int) i]; }
        }
    }
}
