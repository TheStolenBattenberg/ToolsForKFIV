using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using KFIV.Utility.IO;

namespace KFIV.Format.TX2
{
    public class MapTX2
    {
        #region Format Types
        public struct PixelHeader
        {
            public byte[] DMA   { private set; get; }

            public byte[] ukn20 { private set; get; }
            public ulong ukn28 { private set; get; }    //Always '00 00 00 00 00 00 00 E0'
            public byte[] ukn30 { private set; get; }   //Probably image colour properties?
            public byte bpp { private set; get; }
            public ulong ukn38 { private set; get; }    //Always '00 00 00 00 00 00 00 50'
            public byte[] ukn40 { private set; get; }
            public ulong ukn48 { private set; get; }    //Always '00 00 00 00 00 00 00 51'
            public uint width { private set; get; }     //Image width
            public uint height { private set; get; }    //Image height
            public ulong ukn58 { private set; get; }    //Always '00 00 00 00 00 00 00 52'
            public byte[] ukn60 { private set; get; }
            public ulong ukn68 { private set; get; }    //Always '00 00 00 00 00 00 00 53'
            public byte[] ukn70 { private set; get; }

            public PixelHeader(InputStream ins)
            {
                uint test = ins.ReadUInt32();
                if (test == 0)
                    ins.Seek(12, SeekOrigin.Current);
                else
                    ins.Seek(-4, SeekOrigin.Current);

                DMA = ins.ReadBytes(32);

                ukn20 = ins.ReadBytes(8);
                ukn28 = ins.ReadUInt64();
                ukn30 = ins.ReadBytes(7);
                bpp = ins.ReadByte();
                ukn38 = ins.ReadUInt64();
                ukn40 = ins.ReadBytes(8);
                ukn48 = ins.ReadUInt64();
                width = ins.ReadUInt32();
                height = ins.ReadUInt32();
                ukn58 = ins.ReadUInt64();
                ukn60 = ins.ReadBytes(8);
                ukn68 = ins.ReadUInt64();
                ukn70 = ins.ReadBytes(16);
            }

        }
        public struct PixelData
        {
            public byte[] pixels;
            public uint width;
            public uint height;
            public byte bpp;

            public PixelData(InputStream ins, PixelHeader pixh)
            {
                int imageSize = (int) (pixh.width * pixh.height) * 4;

                pixels = SwizzleFilter(ins.ReadBytes(imageSize), pixh.width, pixh.height);
                //pixels = ins.ReadBytes(imageSize);

                width = pixh.width;
                height = pixh.height;
                bpp = pixh.bpp;
            }
        }

        #endregion

        #region TX2 Data
        public List<PixelData> image;

        #endregion
        public static MapTX2 FromFile(string filename)
        {
            //Create new TX2
            MapTX2 tx2 = new MapTX2();
            tx2.image = new List<PixelData>();

            //Open InputStream
            using (InputStream ins = new InputStream(filename))
            {
                //Create pixel objects
                PixelHeader header;

                //Loop until end of file
                while(ins.Tell() != ins.Size())
                {
                    header = new PixelHeader(ins);

                    tx2.image.Add(new PixelData(ins, header));
                }
            }

            return tx2;
        }
        public static MapTX2 FromStream(byte[] stream)
        {
            //Create new TX2
            MapTX2 tx2 = new MapTX2();
            tx2.image = new List<PixelData>();

            //Open InputStream
            using (InputStream ins = new InputStream(stream))
            {
                //Create pixel objects
                PixelHeader header;

                //Loop until end of file
                while (ins.Tell() != ins.Size())
                {
                    header = new PixelHeader(ins);

                    tx2.image.Add(new PixelData(ins, header));
                }
            }

            return tx2;
        }

        public void Save(string filepath)
        {
            //Loop through each image
            uint index = 0;
            foreach(PixelData img in image)
            {
                //Create new Bitmap
                using (Bitmap bm = new Bitmap((int)img.width, (int)img.height, PixelFormat.Format32bppArgb))
                {
                    BitmapData bmd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, bm.PixelFormat);
                    Marshal.Copy(img.pixels, 0, bmd.Scan0, img.pixels.Length);
                    bm.UnlockBits(bmd);

                    bm.Save(filepath + "texture_" + index.ToString("D4") + ".png", ImageFormat.Png);
                }

                index++;
            }
        }

        //DeSwizzle. Based on work here: https://github.com/marco-calautti/Rainbow/     
        private static byte[] SwizzleFilter(byte[] img, uint width, uint height)
        {
            //Width 16
            //Height 8
            //BitDepth = 32

            //Buffer
            byte[] buffer = new byte[width * height * 4];
            int w = (int) (width * 32) / 8;
            int lineSize = (16 * 32) / 8;

            int i = 0;

            for (int y = 0; y < height; y += 8)
            {
                for (int x = 0; x < w; x += lineSize)
                {
                    for (int tileY = y; tileY < y + 8; tileY++)
                    {
                        for (int tileX = x; tileX < x + lineSize; tileX++)
                        {
                            byte data = img[i++];

                            if (tileX >= w || tileY >= height)
                                continue;

                            buffer[tileY * w + tileX] = data;
                        }
                    }
                }
            }

            return buffer;
        }

    }
}
