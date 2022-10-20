using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FormatKFIV.Asset
{
    /// <summary>Provides storage for texture type data.</summary>
    public class Texture
    {
        /// <summary>Colour Modes enum</summary>
        public enum ColourMode
        {
            /// <summary>Unknown colour mode. Provided for sanity checking.</summary>
            Unknown     = -2,
            /// <summary>Unsupported colour mode. Provided for sanity checking.</summary>
            Unsupported = -1,
            /// <summary>4-Bit Mapped. Only valid for Image Data</summary>
            M4  = 0,
            /// <summary>8-Bit Mapped. Only valid for Image Data</summary>
            M8  = 1,
            /// <summary>16-Bit Direct.</summary>
            D16 = 2,
            /// <summary>24-Bit Direct.</summary>
            D24 = 3,
            /// <summary>32-Bit Direct.</summary>
            D32 = 4
        }

        /// <summary>TX2 Pixel Storage Mode Enum</summary>
        public enum PS2PSM
        {
            PSMCT32 = 0,
            PSMCT24 = 1,
            PSMCT16 = 2,
            PSMCT16S = 10,
            PSMT8 = 19,
            PSMT4 = 20,
            PSMT4HL = 26,
            PSMT8H = 27,
            PSMT4HH = 44,
            PSMZ32 = 48,
            PSMZ24 = 49,
            PSMZ16 = 50,
            PSMZ16S = 58
        }

        /// <summary>Storage for Image Data</summary>
        public struct ImageBuffer
        {
            /// <summary>Texture Name</summary>
            public string Name;
            /// <summary>Actual width of image</summary>
            public uint Width;
            /// <summary>Actual height of image</summary>
            public uint Height;
            /// <summary>Actual length of data</summary>
            public uint Length;
            /// <summary>Colour mode of the image (bpp)</summary>
            public ColourMode Format;
            /// <summary>Number of CLUT/Palette</summary>
            public uint ClutCount;
            /// <summary>IDs of the images possible CLUTs/Palettes</summary>
            public int[] ClutIDs;
            /// <summary>Byte buffer with image data</summary>
            public byte[] data;
        }

        /// <summary>Storage for Clut Data</summary>
        public struct ClutBuffer
        {
            /// <summary>Actual width of image</summary>
            public uint Width;
            /// <summary>Actual height of image</summary>
            public uint Height;
            /// <summary>Actual length of data</summary>
            public uint Length;
            /// <summary>Colour mode of the clut. Cannot be 'I4' or 'I8'</summary>
            public ColourMode Format;
            /// <summary>Byte buffer with clut data</summary>
            public byte[] data;
        }

        //Properties
        public int SubimageCount
        {
            get { return subimages.Count; }
        }
        public int ClutCount
        {
            get { return cluts.Count; }
        }

        //Members
        private List<ImageBuffer> subimages;
        private List<ClutBuffer>  cluts;

        /// <summary>Basic constructor for Texture.</summary>
        public Texture()
        {
            subimages = new List<ImageBuffer>();
            cluts  = new List<ClutBuffer>();
        }

        public void SetSubimageName(int index, string name)
        {
            ImageBuffer buff = subimages[index];
            buff.Name = name;
            subimages[index] = buff;
        }

        public byte[] GetSubimageAsRGBA(int index)
        {
            //Get Subimage
            ImageBuffer? nullableImage = GetSubimage(index);
            if (!nullableImage.HasValue)
            {
                Console.WriteLine("Couldn't Get Image (null)");
                return null;
            }
            ImageBuffer subimage = nullableImage.Value;

            //Build RGBA Texture
            byte[] rgbaBuffer = null;

            ClutBuffer cb;

            switch (subimage.Format)
            {
                case ColourMode.M4:
                    if (subimage.ClutCount <= 0)
                    {
                        Console.WriteLine("Subimage has no clut, and cannot be converted to RGBA.");
                        return null;
                    }

                    //Get CLUT
                    cb = cluts[subimage.ClutIDs[0]];

                    rgbaBuffer = new byte[subimage.Length * 8];

                    for (int i = 0; i < subimage.Length * 2; i += 2)
                    {
                        //Get pixel index from 4bpp mapped image
                        byte bpp4Pix1 = (byte)(subimage.data[i / 2] & 0xF);
                        byte bpp4Pix2 = (byte)((subimage.data[i / 2] >> 4) & 0xF);

                        //Add Pixels
                        rgbaBuffer[(4 * (i)) + 0] = cb.data[(4 * bpp4Pix1) + 0];
                        rgbaBuffer[(4 * (i)) + 1] = cb.data[(4 * bpp4Pix1) + 1];
                        rgbaBuffer[(4 * (i)) + 2] = cb.data[(4 * bpp4Pix1) + 2];
                        rgbaBuffer[(4 * (i)) + 3] = cb.data[(4 * bpp4Pix1) + 3];
                        rgbaBuffer[(4 * (i + 1)) + 0] = cb.data[(4 * bpp4Pix2) + 0];
                        rgbaBuffer[(4 * (i + 1)) + 1] = cb.data[(4 * bpp4Pix2) + 1];
                        rgbaBuffer[(4 * (i + 1)) + 2] = cb.data[(4 * bpp4Pix2) + 2];
                        rgbaBuffer[(4 * (i + 1)) + 3] = cb.data[(4 * bpp4Pix2) + 3];
                    }
                    break;

                case ColourMode.M8:
                    if (subimage.ClutCount <= 0)
                    {
                        Console.WriteLine("Subimage has no clut, and cannot be converted to RGBA.");
                        return null;
                    }

                    rgbaBuffer = new byte[(4 * subimage.Width) * subimage.Height];

                    //Get CLUT
                    cb = cluts[subimage.ClutIDs[0]];

                    for (int i = 0; i < subimage.Length; ++i)
                    {
                        //Get pixel index from 4bpp mapped image
                        byte bpp8Pix = (byte)(subimage.data[i] & 0xFF);

                        //Add Pixels
                        rgbaBuffer[(4 * i) + 0] = cb.data[(4 * bpp8Pix) + 0];
                        rgbaBuffer[(4 * i) + 1] = cb.data[(4 * bpp8Pix) + 1];
                        rgbaBuffer[(4 * i) + 2] = cb.data[(4 * bpp8Pix) + 2];
                        rgbaBuffer[(4 * i) + 3] = cb.data[(4 * bpp8Pix) + 3];

                    }
                    break;

                case ColourMode.D16:

                    rgbaBuffer = new byte[subimage.Length * 2];

                    for (int i = 0; i < subimage.Length / 2; ++i)
                    {
                        //Get pixel index from 4bpp mapped image
                        ushort bbp16pix = (ushort)((subimage.data[(2 * i) + 1] << 8) | subimage.data[(2 * i) + 0]);

                        //Add Pixels
                        rgbaBuffer[(4 * i) + 0] = (byte)(((bbp16pix >> 00) & 0x1f) << 3);
                        rgbaBuffer[(4 * i) + 1] = (byte)(((bbp16pix >> 05) & 0x1f) << 3);
                        rgbaBuffer[(4 * i) + 2] = (byte)(((bbp16pix >> 10) & 0x1f) << 3);
                        rgbaBuffer[(4 * i) + 3] = 255;

                    }
                    break;

                case ColourMode.D32:
                    rgbaBuffer = subimage.data;
                    break;
            }

            return rgbaBuffer;
        }

        /// <summary>Return a ImageBuffer from Texture</summary>
        /// <param name="index">The index of the subimage to return</param>
        /// <returns>A ImageBuffer if found, or null. Do I feel lucky?</returns>
        public ImageBuffer? GetSubimage(int index)
        {
            //Sanity UP
            if (index < 0 || index > SubimageCount)
            {
                Console.WriteLine("Texture:GetSubimage -> Invalid Subimage Index");
                return null;
            }
            return subimages[index];
        }

        /// <summary>Return a ClutBuffer from texture</summary>
        /// <param name="index">The index of the clut to return</param>
        /// <returns>A ClutBuffer if found, or null. Well do ya', punk?</returns>
        public ClutBuffer? GetClut(int index)
        {
            //Sanity UP
            if (index < 0 || index > ClutCount)
            {
                Console.WriteLine("Texture:GetClut -> Invalid Clut Index");
                return null;
            }
            return cluts[index];
        }

        /// <summary>Turns a contained subimage into a bitmap</summary>
        /// <param name="index">The subimage index</param>
        /// <returns>A System.Drawing Bitmap</returns>
        public Bitmap GetBitmap(int index)
        {
            //Get Subimage
            ImageBuffer? subImageNullable = GetSubimage(index);
            if (!subImageNullable.HasValue)
            {
                Console.WriteLine("Null Subimage.");
                return null;
            }
            ImageBuffer subImage = subImageNullable.Value;

            //Get System.Drawing pixel format
            PixelFormat bmFormat = PixelFormat.Undefined;
            switch (subImage.Format)
            {
                case ColourMode.D32: bmFormat = PixelFormat.Format32bppArgb; break;
                case ColourMode.D24: bmFormat = PixelFormat.Format24bppRgb; break;
                case ColourMode.D16: bmFormat = PixelFormat.Format16bppRgb555; break;
                case ColourMode.M8: bmFormat = PixelFormat.Format8bppIndexed; break;
                case ColourMode.M4: bmFormat = PixelFormat.Format4bppIndexed; break;
            }

            //Create new Bitmap
            Bitmap bm = new Bitmap((int)subImage.Width, (int)subImage.Height, bmFormat);

            //Copy CLUT to bm. Will always use first clut if more than one is present. :(
            if (subImage.ClutCount > 0)
            {
                //Get CLUT from subimage
                ClutBuffer? clutNullable = GetClut(subImage.ClutIDs[0]);
                if (!clutNullable.HasValue)
                {
                    Console.WriteLine("Null Clut in mapped image");
                    return null;
                }
                ClutBuffer subimageClut = clutNullable.Value;

                //Get Bitmap Palette
                ColorPalette bmPalette = bm.Palette;

                //CLUT must be 32 bit RGB colours.
                if (subimageClut.Format != Texture.ColourMode.D32)
                {
                    Console.WriteLine("CLUT is not Direct32 mode.");
                    return null;
                }

                //Copy CLUT
                for (int i = 0; i < subimageClut.Length; i += 4)
                {
                    //Make Colour from our CLUT
                    bmPalette.Entries[(i / 4)] = Color.FromArgb(subimageClut.data[i + 3], subimageClut.data[i + 2], subimageClut.data[i + 1], subimageClut.data[i + 0]);
                }

                bm.Palette = bmPalette;
            }

            //Copy Direct or Mapped Image
            BitmapData bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, bmFormat);
            Marshal.Copy(subImage.data, 0, bmData.Scan0, (int)subImage.Length);
            bm.UnlockBits(bmData);

            return bm;
        }

        /// <summary>Put an ImageBuffer into the texture subimages</summary>
        /// <param name="imageBuffer">The ImageBuffer</param>
        /// <returns>Returns the index of the added ImageBuffer</returns>
        public int PutSubimage(ImageBuffer imageBuffer)
        {
            subimages.Add(imageBuffer);
            return SubimageCount - 1;
        }

        /// <summary>Put an ClutBuffer into the texture cluts</summary>
        /// <param name="clutBuffer">The ClutBuffer</param>
        /// <returns>Returns the index of the added ClutBuffer</returns>
        public int PutClut(ClutBuffer clutBuffer)
        {
            cluts.Add(clutBuffer);
            return ClutCount - 1;
        }

        /// <summary>Converts a PS2 Colour Mode to TFKFIV Native Colour Mode</summary>
        /// <param name="psm">PS2 PSM Type</param>
        /// <returns>A ColourMode enum entry</returns>
        public static ColourMode PSMtoColourMode(uint psm)
        {
            switch(psm)
            {
                //PSMCT32, PSMZ32
                case 0:
                case 48:
                    return ColourMode.D32;

                //PSMCT24, PSMZ24
                case 1:
                case 49:
                    return ColourMode.D24;

                //PSMCT16, PSMCT16S, PSMZ16, PSMZ16S
                case 2:
                case 10:
                case 50:
                case 58:
                    return ColourMode.D16;

                //PSMT8
                case 19:
                    return ColourMode.M8;

                //PSMT4
                case 20:
                    return ColourMode.M4;

                case 26:
                case 27:
                case 44:
                    return ColourMode.Unsupported;
            }

            return ColourMode.Unknown;
        }

        /// <summary>Unswizzles a PS2 CLUT/Image Data</summary>
        /// <param name="data">byte buffer of colours/indices passed by reference</param>
        /// <param name="cMode">colour mode of the data</param>
        public static void PS2UnswizzleImageData(ref byte[] data, ColourMode cMode, int width, int height)
        {
            int BColourLen = 0; //Length of colour (in bytes)
            switch(cMode)
            {
                case ColourMode.D32:
                    BColourLen = 4;
                    break;
                case ColourMode.D24:
                    BColourLen = 3;
                    break;
                case ColourMode.D16:
                    BColourLen = 2;
                    break;

                case ColourMode.M8:
                    PS2Unswizzle8BPP(ref data, width, height);
                    return;

                case ColourMode.M4:
                    PS2Unswizzle4BPP(ref data, width, height);
                    return;

                default:
                    Console.WriteLine("Can't Unswizzle this colour mode: " + cMode.ToString());
                    return;

            }

            int BTileLen   = BColourLen * 8; //Length of tile (in bytes)
            int BBlockLen  = BTileLen * 4;   //Length of block (in bytes)
            int BBlockNum  = 8;              //Number of blocks

            for (int i = 0; i < BBlockNum; ++i)
            {
                int offTile2 = (BBlockLen * i) + BTileLen * 1;
                int offTile3 = (BBlockLen * i) + BTileLen * 2;

                for (int j = 0; j < BTileLen; j += BColourLen)
                {
                    for(int k = 0; k < BColourLen; ++k)
                    {
                        //Switch colours around.
                        byte buffer = data[(offTile2 + j) + k];
                        data[(offTile2 + j) + k] = data[(offTile3 + j) + k];
                        data[(offTile3 + j) + k] = buffer;
                    }
                }
            }
        }

        private static readonly byte[] bpp8UnswizzleMatrix = { 0, 16, 8, 24 };
        private static readonly byte[] bpp4UnswizzleMatrix = { 0, 16, 2, 18, 17, 1, 19, 3 };

        private static void PS2Unswizzle8BPP(ref byte[] output, int width, int height)
        {
            //Special thanks to Gh0stBlade for sharing the 8bpp unswizzling code from their TR projects,
            //and Mumm-Ra for telling me 8bpp swizzled indices were actually a thing over on the Xentax discord server.
            //Also loginmen for the last loop bit, still not sure what it does.

            //Copy output array to avoid weirdness.
            byte[] input = new byte[output.Length];
            Array.Copy(output, 0, input, 0, output.Length);

            //Unswizzle in both Y (j) and X (i)
            for (int j = 0; j < height; ++j)
            {
                for (int i = 0; i < width; ++i)
                {
                    int blockLocation = (j & (~0xF)) * width + (i & (~0xF)) * 2;
                    int swapSelector = (((j + 2) >> 2) & 0x1) * 4;
                    int posY = (((j & (~3)) >> 1) + (j & 1)) & 0x7;
                    int column_location = posY * width * 2 + ((i + swapSelector) & 0x7) * 4;
                    int byte_num = ((j >> 1) & 1) + ((i >> 2) & 2);     // 0,1,2,3
                    output[(j * width) + i] = input[blockLocation + column_location + byte_num];
                }
            }

            for(int i = 0; i < (width * height); ++i)
            {
                output[i] = (byte)((output[i] & ~0x18) | bpp8UnswizzleMatrix[(output[i] & 0x18) >> 3]);
            }
        }
        private static void PS2Unswizzle4BPP(ref byte[] output, int width, int height)
        {
            //https://gist.github.com/Fireboyd78/1546f5c86ebce52ce05e7837c697dc72

            byte[] bpp8Buffer = new byte[output.Length * 2];
            int Dst, Src;

            //Turn 4BPP indices into 8bpp indices
            Src = 0;
            Dst = 0;
            while (Src < output.Length)
            {
                int bpp4Pixel = output[Src];

                bpp8Buffer[Dst + 0] = (byte)((bpp4Pixel >> 0) & 0xF);
                bpp8Buffer[Dst + 1] = (byte)((bpp4Pixel >> 4) & 0xF);

                Dst += 2;
                Src++;
            }

            //Do 8BPP Unswizzle on this data
            PS2Unswizzle8BPP(ref bpp8Buffer, width, height);

            //Turn our unswizzled 8BPP indices back into 4bpp indices
            Src = 0;
            Dst = 0;
            while (Dst < output.Length)
            {
                byte bpp8Pix1 = bpp8Buffer[Src + 0];
                byte bpp8Pix2 = bpp8Buffer[Src + 1];

                output[Dst] = (byte)(((bpp8Pix1 & 0xF) << 0) | ((bpp8Pix2 & 0xF) << 4));

                Src += 2;
                Dst++;
            }
        }

        public static ClutBuffer GenerateGreyscaleClut(ColourMode indexBpp)
        {
            //Format
            uint width = 0, height = 0;

            //What type of clut are we generating?
            switch(indexBpp)
            {
                case ColourMode.M4:
                    width = 8;
                    height = 2;
                    break;

                case ColourMode.M8:
                    width  = 16;
                    height = 16;
                    break;
            }

            ClutBuffer cb = new ClutBuffer
            {
                Width = width,
                Height = height,
                Length = (4 * width) * height,
                Format = ColourMode.D32, 
            };
            cb.data = new byte[cb.Length];

            int colourStride = (int)(256 / (cb.Length / 4));

            for(int i = 0; i < cb.Length; i+=4)
            {
                cb.data[i + 0] = (byte)((colourStride * (i/4)) & 0xFF);
                cb.data[i + 1] = cb.data[i + 0];
                cb.data[i + 2] = cb.data[i + 0];
                cb.data[i + 3] = 0xFF;
            }

            return cb; 
        }

        /// <summary>Converts an array of colours from BGR to RGB format</summary>
        /// <param name="data">byte buffer of colours passed by reference</param>
        /// <param name="cMode">colour mode of the colour data (D16, D24 and D32)</param>
        /// <param name="fixPS2Alpha">fix PS2s half alpha (when cMode equals D32 only)</param>
        public static void ConvColourSpaceBGRAtoRGBA(ref byte[] data, ColourMode cMode, bool fixPS2Alpha)
        {
            switch(cMode)
            {
                case ColourMode.D32:
                    for (int i = 0; i < data.Length; i += 4)
                    {
                        //R -> BUFFER, B -> R, BUFFER -> B
                        byte buffer = data[i + 0];
                        data[i + 0] = data[i + 2];
                        data[i + 2] = buffer;

                        //PS2 alpha is 0x00 - 0x80, we expand it to regular alpha values here. Could be optimized.
                        data[i + 3] = (byte)(fixPS2Alpha == false ? 255 : Math.Min(data[i + 3] * 2, 255));

                        
                    }
                    break;

                case ColourMode.D24:
                    for (int i = 0; i < data.Length; i += 3)
                    {
                        //R -> BUFFER, B -> R, BUFFER -> B
                        byte buffer = data[i + 0];
                        data[i + 0] = data[i + 2];
                        data[i + 2] = buffer;
                    }
                    break;

                case ColourMode.D16:
                    for (int i = 0; i < data.Length; i += 2)
                    {
                        ushort buffer2 = (ushort)((data[i + 1] << 8) | (data[i + 0] & 0xFF));

                        //Copy R, G, B
                        byte bufR = (byte)((buffer2 >> 00) & 0x1F);
                        byte bufG = (byte)((buffer2 >> 05) & 0x1F);
                        byte bufB = (byte)((buffer2 >> 10) & 0x1F);

                        //Swap R, G, B
                        buffer2 = (ushort)((bufR << 10) | (bufG << 5) | (bufB << 0));
                        data[i + 0] = (byte)(buffer2 & 0xFF);
                        data[i + 1] = (byte)((buffer2 >> 8) & 0xFF);
                    }
                    break;
            }
        }

        /// <summary>Flips an array of colours vertically</summary>
        /// <param name="data">byte buffer of colours/indices passed by reference</param>
        /// <param name="cMode">mode of the colour/index data</param>
        /// <param name="width">Width of 1 row of data</param>
        /// <param name="height">Height of 1 column of data</param>
        public static void FlipV(ref byte[] data, ColourMode cMode, int width, int height)
        {
            //How wide is our colour in bytes?
            float colourLen = 0;
            switch(cMode)
            {
                case ColourMode.D32:
                    colourLen = 4;
                    break;
                case ColourMode.D24:
                    colourLen = 3;
                    break;
                case ColourMode.D16:
                    colourLen = 2;
                    break;
                case ColourMode.M8:
                    colourLen = 1;
                    break;

                case ColourMode.M4:
                    colourLen = 0.5f;
                    break;
            }

            //How wide is a row of pixels
            int rowLength = (int)(colourLen * width);

            //Initialize the buffer, and do the flip with many copies.
            byte[] rowBuffer1 = new byte[rowLength];
            for (int i = 0; i < height; ++i)
            {
                int id1 = rowLength * i;
                int id2 = rowLength * (height - i - 1);

                //Top -> Buffer, Bottom -> Top, Buffer -> Top
                Array.Copy(data, id1, rowBuffer1, 0, rowLength);
                Array.Copy(data, id2, data, id1, rowLength);
                Array.Copy(rowBuffer1, 0, data, id2, rowLength);
            }
        }

        /// <summary>Rotates 4BPP colour indices to fix endian artifacts.</summary>
        /// <param name="data">byte buffer of indices passed by reference</param>
        public static void Fix4BPPIndicesEndianness(ref byte[] data)
        {
            for(int i = 0; i < data.Length; ++i)
            {
                byte buffer = data[i];
                data[i] = (byte)((((buffer >> 4) & 0xF) | ((buffer & 0xF) << 4)) & 0xFF);
            }
        }
    }
}