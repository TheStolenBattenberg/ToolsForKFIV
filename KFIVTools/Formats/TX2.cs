using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace KFIV.Format.TX2
{
    public class TX2
    {
        #region Format Types
        public struct Header
        {
            public uint fileLength { private set; get; }
            public uint numImage   { private set; get; }
            public byte[] unknown8 { private set; get; }

            public Header(BinaryReader bin)
            {
                fileLength = bin.ReadUInt32();
                numImage = bin.ReadUInt32();
                unknown8 = bin.ReadBytes(8);
            }
        }
        public class PixelBuffer
        {
            public byte[] unknown0  { private set; get; }
            public byte   bpp       { private set; get; }
            public byte[] unknown48 { private set; get; }
            public uint   width     { private set; get; }
            public uint   height    { private set; get; }
            public byte[] unknown68 { private set; get; }
            public byte[] pixels    { private set; get; }

            public bool HasClut
            {
                get { return bpp > 0xF; }
            }

            public PixelBuffer(BinaryReader bin)
            {
                unknown0 = bin.ReadBytes(71);
                bpp = bin.ReadByte();
                unknown48 = bin.ReadBytes(24);
                width = bin.ReadUInt32();
                height = bin.ReadUInt32();
                unknown68 = bin.ReadBytes(40);
                pixels = null;

                switch(bpp)
                {
                    case 0x14: pixels = read4bpp(bin); break;
                    case 0x13: pixels = read8bpp(bin); break;
                    case 0x02: pixels = read16bpp(bin); break;
                    case 0x01: pixels = read24bpp(bin); break;
                    case 0x00: pixels = read32bpp(bin); break;
                }             
            }

            private byte[] read4bpp(BinaryReader bin)
            {
                return bin.ReadBytes((int)((width / 2) * height));
            }
            private byte[] read8bpp(BinaryReader bin)
            {
                return bin.ReadBytes((int) (width * height));
            }
            private byte[] read16bpp(BinaryReader bin)
            {
                return bin.ReadBytes((int)(2 * (width * height)));
            }
            private byte[] read24bpp(BinaryReader bin)
            {
                return bin.ReadBytes((int)(3 * (width * height)));
            }
            private byte[] read32bpp(BinaryReader bin)
            {
                byte[] bytes = new byte[(int)(4 * (width * height))];

                uint k = 0;
                for(uint i = 0; i < width; ++i)
                {
                    for(uint j = 0; j < height; ++j)
                    {
                        bytes[k + 2] = bin.ReadByte();
                        bytes[k + 1] = bin.ReadByte();
                        bytes[k + 0] = bin.ReadByte();
                        bytes[k + 3] = (byte) (bin.ReadByte() | 0xFF);

                        k += 4;
                    }
                }

                return bytes;
            }
        }
        public class ClutBuffer
        {
            public byte[] unknown0  { private set; get; }
            public uint width       { private set; get; }
            public uint height      { private set; get; }
            public byte[] unknown48 { private set; get; }
            public Color[] colours  { private set; get; }

            public ClutBuffer(BinaryReader bin)
            {
                unknown0 = bin.ReadBytes(64);
                width = bin.ReadUInt32();
                height = bin.ReadUInt32();
                unknown48 = bin.ReadBytes(40);
                colours = null;

                switch(width * height)
                {
                    case 16:  colours = readclut4(bin); break;
                    case 256: colours = readclut8(bin); break;
                }
            }

            private Color[] readclut4(BinaryReader bin)
            {
                Color[] temp = new Color[16];

                //read colours
                byte ca, cr, cg, cb;

                for(uint x = 0; x < 16; ++x)
                {
                    cr = bin.ReadByte();
                    cg = bin.ReadByte();
                    cb = bin.ReadByte();
                    ca = bin.ReadByte();

                    //just ignore alpha by masking
                    temp[x] = Color.FromArgb(ca | 0xFF, cr, cg, cb);
                }

                return temp;
            }
            private Color[] readclut8(BinaryReader bin)
            {
                Color[] temp = new Color[256];
                Color[] sort = new Color[256];

                //read colours
                byte ca, cr, cg, cb;
                for (uint x = 0; x < 256; ++x)
                {
                    cr = bin.ReadByte();
                    cg = bin.ReadByte();
                    cb = bin.ReadByte();
                    ca = bin.ReadByte();

                    //just ignore alpha by masking
                    temp[x] = Color.FromArgb(ca | 0xFF, cr, cg, cb);
                }

                //https://github.com/marco-calautti/Rainbow/
                //https://www.dropbox.com/s/4wfvq86qk3h6v0n/GS_Users_Manual.pdf (2.7 CLUT Buffer)

                //Unpack pallette 
                uint ps = 8;
                uint ss = 2;
                uint cs = 8;
                uint bk = 2;
                uint k = 0;

                for (uint p = 0; p < ps; p++)
                {
                    for (uint b = 0; b < bk; b++)
                    {
                        for (uint s = 0; s < ss; s++)
                        {
                            for (uint c = 0; c < cs; c++)
                            {
                                sort[k++] = temp[p * cs * ss * bk + b * cs + s * ss * cs + c];
                            }
                        }
                    }
                }

                return sort;
            }
        }
        public class Image
        {
            public uint offset     { private set; get; }
            public byte[] unknown4 { private set; get; }

            public PixelBuffer pixelBuffer { private set; get; }
            public ClutBuffer  clutBuffer  { private set; get; }

            public Image(BinaryReader bin)
            {
                offset   = bin.ReadUInt32();
                unknown4 = bin.ReadBytes(12);

                //Read actual image data
                long co = bin.BaseStream.Position;
                bin.BaseStream.Seek(offset, SeekOrigin.Begin);

                pixelBuffer = new PixelBuffer(bin);
                clutBuffer  = null;

                if (pixelBuffer.HasClut)
                    clutBuffer = new ClutBuffer(bin);

                bin.BaseStream.Seek(co, SeekOrigin.Begin);
            }

            public void Save(string path)
            {
                PixelFormat fmt = PixelFormat.Undefined;

                switch(pixelBuffer.bpp)
                {
                    case 0x14: fmt = PixelFormat.Format4bppIndexed;   break;
                    case 0x13: fmt = PixelFormat.Format8bppIndexed;   break;
                    case 0x02: fmt = PixelFormat.Format16bppArgb1555; break;
                    case 0x01: fmt = PixelFormat.Format24bppRgb;      break;
                    case 0x00: fmt = PixelFormat.Format32bppArgb;     break; 
                }

                using (Bitmap bm = new Bitmap((int)pixelBuffer.width, (int)pixelBuffer.height, fmt))
                {
                    //Copy CLUT
                    if(pixelBuffer.HasClut)
                    {
                        ColorPalette pal = bm.Palette;

                        for(uint i = 0; i < clutBuffer.colours.Length; ++i)
                        {
                            pal.Entries[i] = clutBuffer.colours[i];
                        }

                        bm.Palette = pal;
                    }

                    //Copy Image
                    BitmapData bmd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, fmt);
                        Marshal.Copy(pixelBuffer.pixels, 0, bmd.Scan0, pixelBuffer.pixels.Length);
                    bm.UnlockBits(bmd);

                    bm.Save(path, ImageFormat.Png);
                }
            }
        }

        #endregion
        #region Data
        private Header header;
        private List<Image> images;

        #endregion

        public static TX2 FromFile(string path)
        {
            //Open File
            BinaryReader bin;
            try
            {
                bin = new BinaryReader(File.Open(path, FileMode.Open));
            } catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
            TX2 tx2 = new TX2();

            //Read TX2 Header
            tx2.header = new Header(bin);
            tx2.images = new List<Image>((int) tx2.header.numImage);

            //Read TX2 Images
            for (uint i = 0; i < tx2.header.numImage; ++i)
                tx2.images.Add(new Image(bin));

            //Close BinaryReader
            bin.Close();

            return tx2;
        }

        public uint Count {
            get { return header.numImage; }
        }
        public Image this[uint i]
        {
            get { return images[(int) i]; }
        }
    }
}
