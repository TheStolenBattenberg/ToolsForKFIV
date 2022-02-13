using System;
using System.IO;

using FormatKFIV.Utility;
using FormatKFIV.Asset;

namespace FormatKFIV.FileFormat
{
    /// <summary>Texture Container serving a single texture.</summary>
    public class FFTextureTGA : FIFormat<Texture>
    {
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".tga",
            },
            Type = FEType.Texture,
            Validator = null,

            AllowExport = true,
            FormatDescription = "Truevision Advanced Raster Graphics Adapter (TGA/TARGA)",
            FormatFilter = "Truevision TGA (*.tga)|*.tga"
        };
        #endregion

        /// <summary>Writes a TGA file to disc</summary>
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
                //Calculate File ID string
                if (data.SubimageCount > 1)
                    fID = "_f" + i.ToString("D4");

                //Get Nullable SubImage and ensure it's not null.
                Texture.ImageBuffer? imgBufferNullposs = data.GetSubimage(i);

                if (!imgBufferNullposs.HasValue)
                {
                    Console.WriteLine("Invalid Texture");
                    return;
                }

                Texture.ImageBuffer imgBuffer = imgBufferNullposs.Value;

                //Clut Count changes a lot here.
                if (imgBuffer.ClutCount == 1)
                {
                    SaveTGA((fPath + fName + fID) + ".TGA", imgBuffer, data.GetClut(imgBuffer.ClutIDs[0]).Value);
                }
                else
                if (imgBuffer.ClutCount > 1)
                {
                    for (int j = 0; j < imgBuffer.ClutCount; ++i)
                    {
                        SaveTGA((fPath + fName + fID) + "_clut" + j.ToString("D4") + ".TGA", imgBuffer, data.GetClut(imgBuffer.ClutIDs[0]).Value);
                    }
                }
                else
                {
                    SaveTGA((fPath + fName + fID) + ".TGA", imgBuffer, null);
                }
            }
        }

        private static void SaveTGA(string path, Texture.ImageBuffer image, Texture.ClutBuffer? clut)
        {
            //Ensure the image has a CLUT if it is a mapped type.
            if ((image.Format == Texture.ColourMode.M4 | image.Format == Texture.ColourMode.M8) && !clut.HasValue)
            {
                Console.WriteLine("Attempted to write mapped image without a clut?..");
                return;
            }

            Texture.ClutBuffer clutBuffer = clut.Value;

            //Open new OutputStream
            using (OutputStream ous = new OutputStream(path))
            {
                //Write Header
                ous.WriteByte(0);   //Identification Field Length
                ous.WriteByte((byte) ((image.Format == Texture.ColourMode.M4 | image.Format == Texture.ColourMode.M8) ? 1 : 0));   //Colour Map Type
                ous.WriteByte((byte) ((image.Format == Texture.ColourMode.M4 | image.Format == Texture.ColourMode.M8) ? 1 : 2));   //Image Data Type

                //Write Colour Map Specification
                ous.WriteUInt16(0); //Clut Origin
                ous.WriteUInt16((ushort)(clutBuffer.Length / 4));   //CLUT Colour Count
                switch(clutBuffer.Format)
                {
                    case Texture.ColourMode.D32: ous.WriteByte(32); break;  //Colour bit length
                    case Texture.ColourMode.D24: ous.WriteByte(24); break;
                    case Texture.ColourMode.D16: ous.WriteByte(16); break;
                }

                //Write Image Specification
                ous.WriteUInt16(0); //X Origin
                ous.WriteUInt16(0); //Y Origin
                ous.WriteUInt16((ushort)image.Width);   //Width
                ous.WriteUInt16((ushort)image.Height);  //Height
                switch (image.Format)
                {
                    case Texture.ColourMode.D32: ous.WriteByte(32); break;
                    case Texture.ColourMode.D24: ous.WriteByte(24); break;
                    case Texture.ColourMode.D16: ous.WriteByte(16); break;
                    case Texture.ColourMode.M8:  ous.WriteByte(8);  break;
                    case Texture.ColourMode.M4:  ous.WriteByte(4);  break;  //lol guess what? I actually read the TGA spec PROPERLY.
                }
                ous.WriteByte(0);   //Colour Map Type

                //Write Colour Map if applicable
                if((image.Format == Texture.ColourMode.M4 | image.Format == Texture.ColourMode.M8) == true)
                {
                    ous.Write(clutBuffer.data);
                }

                //Write Image Data
                ous.Write(image.data);
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
