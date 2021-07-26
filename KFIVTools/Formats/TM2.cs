using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System;

using KFIV.Utility.IO;

namespace KFIV.Format.TM2
{
    public class TM2
    {
        /**
         * This region contains all structures and types of the MOD Format.
        **/
        #region Format Types
        public struct TM2Header
        {
            public uint  tm2Length;
            public uint  clutOffset;
            public uint  unknown0x08;
            public uint  unknown0x0c;
            public ulong gsTexReg;
            public ulong unknown0x18;

            public int texPSM
            {
                get
                {
                    return (int) (gsTexReg >> 20) & 0x3F;
                }
            }
            public int texWidth
            {
                get
                {
                    return (int) Math.Pow(2, (gsTexReg >> 26) & 0xF);  //Texture width is stored as log2. We can restore with 2^width
                }
            }
            public int texHeight
            {
                get
                {
                    return (int)Math.Pow(2, (gsTexReg >> 30) & 0xF);  //Texture height is stored as log2. We can restore with 2^width
                }
            }
            public int texClutAlpha
            {
                get
                {
                    return (int)(gsTexReg >> 34) & 0x1;
                }
            }
            public int texPixelSize
            {
                get
                {
                    return (int) (clutOffset == 0 ? tm2Length - 0x20 : clutOffset);
                }
            }
            public int texClutSize
            {
                get
                {
                    return (int) (clutOffset == 0 ? 0 : (tm2Length - 0x20) - clutOffset);
                }
            }

            public TM2Header(InputStream ins)
            {
                tm2Length  = ins.ReadUInt32();
                clutOffset = ins.ReadUInt32();
                unknown0x08 = ins.ReadUInt32();
                unknown0x0c = ins.ReadUInt32();
                gsTexReg = ins.ReadUInt64();
                unknown0x18 = ins.ReadUInt64();
            }
        }
        #endregion

        public TM2Header header;
        public byte[] pixelData;
        public byte[] clutData;

        public static TM2 FromFile(string path)
        {
            TM2 tm2;
            using (InputStream ins = new InputStream(path))
            {
                tm2 = ReadTM2Data(ins);
            }

            return tm2;
        }
        public static TM2 FromMemory(byte[] memory)
        {
            TM2 tm2;
            using(InputStream ins = new InputStream(memory))
            {
                tm2 = ReadTM2Data(ins);
            }

            return tm2;
        }

        public void Save(string path)
        {
            //Calculate pixel format, apply fixes
            PixelFormat pFmt = PixelFormat.Format32bppArgb;
            switch (header.texPSM)
            {
                case 0:
                    pFmt = PixelFormat.Format32bppArgb;
                    BGRtoRGB32(ref pixelData, (uint)header.texPixelSize);
                    break;
                case 1:
                    pFmt = PixelFormat.Format24bppRgb;
                    //BGRtoRGB24(ref pixelData, (uint)header.texPixelSize);
                    break;
                case 2:
                    pFmt = PixelFormat.Format16bppArgb1555;
                    break;
                case 19:
                    pFmt = PixelFormat.Format8bppIndexed;
                    CLUT8BPPFix(ref clutData, (uint)header.texClutSize, header.texClutAlpha == 1);
                    break;
                case 20:
                    pFmt = PixelFormat.Format4bppIndexed;
                    CLUT4BPPFix(ref clutData, (uint)header.texClutSize, header.texClutAlpha == 1);
                    PIXL4BPPFix(ref pixelData, (uint)header.texPixelSize);
                    break;
            }

            using (Bitmap bm = new Bitmap(header.texWidth, header.texHeight, pFmt))
            {
                //When image is indexed, copy the clut first.
                if (header.clutOffset != 0)
                {
                    ColorPalette pal = bm.Palette;

                    int palOffset = 0;
                    for (uint i = 0; i < (header.texClutSize / 4); ++i)
                    {
                        pal.Entries[i] = Color.FromArgb(clutData[palOffset + 3], clutData[palOffset + 2], clutData[palOffset + 1], clutData[palOffset + 0]);
                        palOffset += 4;
                    }

                    bm.Palette = pal;
                }

                Console.WriteLine("Image Width: " + bm.Width.ToString());
                Console.WriteLine("Image Height: " + bm.Height.ToString());
                Console.WriteLine("Image PSM: " + header.texPSM.ToString());
                Console.WriteLine("Image Buffer Size: " + pixelData.Length.ToString() + "/" + header.texPixelSize.ToString());

                //Copy Image
                BitmapData bmd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, pFmt);
                Marshal.Copy(pixelData, 0, bmd.Scan0, header.texPixelSize);
                bm.UnlockBits(bmd);

                bm.Save(path + ".png", ImageFormat.Png);
            }
        }
        private static TM2 ReadTM2Data(InputStream ins)
        {
            TM2 tm2 = new TM2();

            //
            //Read Header
            //
            tm2.header = new TM2Header(ins);

            //read pixels
            ins.Seek(0x20, SeekOrigin.Begin);
            tm2.pixelData = ins.ReadBytes(tm2.header.texPixelSize);

            //read clut (if present)
            if(tm2.header.texClutSize > 0)
            {
                ins.Jump(0x20 + tm2.header.clutOffset);
                tm2.clutData = ins.ReadBytes(tm2.header.texClutSize);
                ins.Return();
            }

            return tm2;
        }

        public static void BGRtoRGB32(ref byte[] pixels, uint length)
        {
            //First pass converts BGR to RGB and also fixes wierd PS2 half alpha (1.0 = 0x80)
            for (int i = 0; i < length; i += 4)
            {
                byte R = pixels[i + 0];
                pixels[i + 0] = pixels[i + 2];
                pixels[i + 2] = R;
                pixels[i + 3] = (byte)Math.Min(pixels[i+3] * 2, 255);
            }
        }
        public static void BGRtoRGB24(ref byte[] pixels, uint length)
        {
            //First pass converts BGR to RGB
            for (int i = 0; i < length; i += 3)
            {
                byte R = pixels[i + 0];
                pixels[i + 0] = pixels[i + 2];
                pixels[i + 2] = R;
            }
        }

        public static void PIXL4BPPFix(ref byte[] pixels, uint length)
        {
            for(int i = 0; i < length; ++i)
            {
                byte p = pixels[i];
                pixels[i] = (byte) ((p >> 4) | ((p & 0xF) << 4) & 0xFF);
            }
        }
        public static void CLUT4BPPFix(ref byte[] pixels, uint length, bool fixAlpha)
        {
            //First pass changes colours from BGR to RGB
            for (int i = 0; i < length; i += 4)
            {
                byte R = pixels[i + 0];
                pixels[i + 0] = pixels[i + 2];
                pixels[i + 2] = R;
                pixels[i + 3] = (byte)(fixAlpha == false ? 255 : (byte)Math.Min(pixels[i + 3] * 2, 255));
            }

            //Second pass fixes wierd ordering issues (Is this swizzling?)
            byte tempBuffer;
            for (int i = 0; i < (length / 128); ++i)
            {
                int b2Off = (128 * i) + 32;
                int b3Off = (128 * i) + 64;

                for (int j = 0; j < 32; ++j)
                {
                    tempBuffer = pixels[b2Off + j];
                    pixels[b2Off + j] = pixels[b3Off + j];
                    pixels[b3Off + j] = tempBuffer;
                }
            }
        }
        public static void CLUT8BPPFix(ref byte[] pixels, uint length, bool fixAlpha)
        {
            //First pass changes colours from BGR to RGB
            for (int i = 0; i < length; i += 4)
            {
                byte R = pixels[i + 0];
                pixels[i + 0] = pixels[i + 2];
                pixels[i + 2] = R;
                pixels[i + 3] = (byte) (fixAlpha == false ? 255 : (byte)Math.Min(pixels[i+3] * 2, 255));
            }

            //Second pass fixes wierd ordering issues (Is this swizzling?)
            byte tempBuffer;
            for(int i = 0; i < (length / 128); ++i)
            {
                int b2Off = (128 * i) + 32;
                int b3Off = (128 * i) + 64;

                for(int j = 0; j < 32; ++j)
                {
                    tempBuffer = pixels[b2Off + j];
                    pixels[b2Off + j] = pixels[b3Off + j];
                    pixels[b3Off + j] = tempBuffer;
                }
            }
        }
    }
}
