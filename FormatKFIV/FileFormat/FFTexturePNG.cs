using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using FormatKFIV.Utility;
using FormatKFIV.Asset;


namespace FormatKFIV.FileFormat
{
    /// <summary>Texture Container serving a single texture.</summary>
    public class FFTexturePNG : FIFormat<Texture>
    {
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".png",
            },
            Type = FEType.Texture,
            Validator = null,

            AllowExport = true,
            FormatDescription = "Portable Network Graphics (PNG)",
            FormatFilter = "Portable Network Graphics (*.png)|*.png"
        };
        #endregion

        /// <summary>Writes (a) PNG file(s) to disc</summary>
        /// <param name="filepath">The file path (including filename)</param>
        /// <param name="texture">The texture data to write</param>
        public void SaveToFile(string filepath, Texture data)
        {
            //In the case of multiple files, we need to split filepath into name and dir
            string fName = Path.GetFileNameWithoutExtension(filepath);
            string fPath = filepath.Replace(Path.GetFileName(filepath), "");
            string fID = "";

            for (var i = 0; i < data.SubimageCount; ++i)
            {
                //Get Nullable SubImage and ensure it's not null.
                Texture.ImageBuffer? imgBufferNullposs = data.GetSubimage(i);
                if (!imgBufferNullposs.HasValue)
                {
                    Console.WriteLine("Invalid Texture");
                    return;
                }
                Texture.ImageBuffer imgBuffer = imgBufferNullposs.Value;

                //Get File ID
                if(imgBuffer.Name == null)
                {
                    fID = "_f" + i.ToString("D4");
                }
                else
                {
                    fID = "_" + imgBuffer.Name;
                }

                //Clut Count changes a lot here.
                if (imgBuffer.ClutCount == 1)
                {
                    SavePNG((fPath + fName + fID) + ".PNG", imgBuffer, data.GetClut(imgBuffer.ClutIDs[0]).Value);
                }
                else
                if (imgBuffer.ClutCount > 1)
                {
                    for (int j = 0; j < imgBuffer.ClutCount; ++i)
                    {
                        SavePNG((fPath + fName + fID) + "_clut" + j.ToString("D4") + ".PNG", imgBuffer, data.GetClut(imgBuffer.ClutIDs[0]).Value);
                    }
                }
                else
                {
                    SavePNG((fPath + fName + fID) + ".PNG", imgBuffer, null);
                }
            }
        }

        private void SavePNG(string path, Texture.ImageBuffer image, Texture.ClutBuffer? clut)
        {
            //Ensure the image has a CLUT if it is a mapped type.
            if ((image.Format == Texture.ColourMode.M4 | image.Format == Texture.ColourMode.M8) && !clut.HasValue)
            {
                Console.WriteLine("Attempted to write mapped image without a clut?..");
                return;
            }

            //Get System.Drawing pixel format
            PixelFormat bmFormat = PixelFormat.Undefined;
            switch(image.Format)
            {
                case Texture.ColourMode.D32: bmFormat = PixelFormat.Format32bppArgb;   break;
                case Texture.ColourMode.D24: bmFormat = PixelFormat.Format24bppRgb;    break;
                case Texture.ColourMode.D16: bmFormat = PixelFormat.Format16bppRgb555; break;
                case Texture.ColourMode.M8:  bmFormat = PixelFormat.Format8bppIndexed; break;
                case Texture.ColourMode.M4:  bmFormat = PixelFormat.Format4bppIndexed; break;
            }

            //Write file to disc
            using (Bitmap bm = new Bitmap((int)image.Width, (int)image.Height, bmFormat))
            {
                //Copy CLUT to bm.
                if(clut.HasValue)
                {
                    ColorPalette bmPalette = bm.Palette;

                    //CLUT must be 32 bit RGB colours.
                    if(clut.Value.Format != Texture.ColourMode.D32)
                    {
                        Console.WriteLine("CLUT is not Direct32 mode. Cannot write PNG.");
                        return;
                    }

                    if (image.Format == Texture.ColourMode.M8)
                    {
                        using (OutputStream ousClut = new OutputStream(path + ".clut"))
                        {
                            ousClut.Write(clut.Value.data);
                        }
                    }

                    //Copy CLUT
                    for (int i = 0; i < clut.Value.Length; i += 4)
                    {
                        //Make Colour from our CLUT
                        bmPalette.Entries[(i / 4)] = Color.FromArgb(clut.Value.data[i + 3], clut.Value.data[i + 2], clut.Value.data[i + 1], clut.Value.data[i + 0]);
                    }

                    bm.Palette = bmPalette;
                }

                if (image.Format == Texture.ColourMode.M8)
                {
                    using (OutputStream ousImg = new OutputStream(path + ".img"))
                    {
                        ousImg.Write(image.data);
                    }
                }


                //Copy Direct or Mapped Image
                BitmapData bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, bmFormat);
                Marshal.Copy(image.data, 0, bmData.Scan0, (int)image.Length);
                bm.UnlockBits(bmData);

                //Save file to disc
                bm.Save(path, ImageFormat.Png);
            }
        }

        public Texture LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }
        public Texture LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            throw new NotImplementedException();
        }
    }
}
