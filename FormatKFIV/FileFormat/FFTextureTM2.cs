using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.TypePlayStation;
using FormatKFIV.Utility;
using FormatKFIV.Asset;

namespace FormatKFIV.FileFormat
{
    /// <summary>Texture Format from Sony, with a very modified header thanks to FromSoft.</summary>
    public class FFTextureTM2 : FIFormat<Texture>
    {
        #region Format Structures
        public struct TM2Header
        {
            public uint length;
            public uint clutOffset;
            public uint unknown0x08;   //1?
            public uint unknown0x0C;   //0?
            public sceGsTex0 gsTex0;
            public ulong unknown0x18;  //Unknown;
        }

        #endregion
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".tm2",
            },
            Type = FEType.Texture,
            AllowExport = false,
            Validator = FFTextureTM2.FileIsValid
        };

        /// <summary>Validates a file to see if it is FromSoft TM2 Format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    ins.Seek(12, System.IO.SeekOrigin.Begin);
                    uint check1   = ins.ReadUInt32();
                    ins.Seek(24, System.IO.SeekOrigin.Begin);
                    byte check2   = ins.ReadByte();
                    ins.Seek(24, System.IO.SeekOrigin.Begin);
                    ushort check3 = ins.ReadUInt16();

                    return (check1 == 0 && ((check2 == 0x60) | (check3 == 0x260) | (check2 == 0)));
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    Console.WriteLine(Ex.StackTrace);
                    return false;
                }
            }
        }

        #endregion

        public Texture LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(filepath))
            {
                Texture T = ImportTM2(ins);
                if (T != null)
                {
                    return T;
                }
            }
            return null;
        }

        public Texture LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(buffer))
            {
                Texture T = ImportTM2(ins);
                if(T != null)
                {
                    return T;
                }
            }
            return null;
        }

        private Texture ImportTM2(InputStream ins)
        {
            //Create Texture to return.
            Texture ResultTexture = new Texture();

            try
            {
                //Read Header
                TM2Header header = new TM2Header
                {
                    length = ins.ReadUInt32(),
                    clutOffset = ins.ReadUInt32(),
                    unknown0x08 = ins.ReadUInt32(),
                    unknown0x0C = ins.ReadUInt32(),
                    gsTex0 = ins.ReadPS28ByteRegister()._sceGsTex0,
                    unknown0x18 = ins.ReadUInt64()
                };

                //Read Image Data
                //Calculate Image Buffer Size
                int imageSize = 0;
                if(header.clutOffset != 0)
                {
                    imageSize = (int)header.clutOffset;
                }
                else
                {
                    imageSize = (int)header.length - 0x20;
                }

                //Read Image Data
                Texture.ImageBuffer subimage = new Texture.ImageBuffer
                {
                    Name = null,
                    Width = (uint)Math.Pow(2, header.gsTex0.TW),
                    Height = (uint)Math.Pow(2, header.gsTex0.TH),
                    Length = (uint)imageSize,
                    Format = Texture.PSMtoColourMode(header.gsTex0.PSM),
                    ClutCount = 0,
                    ClutIDs = null,
                    data = null,
                    UID = ((header.gsTex0.CBP & 0xFFFF) << 16) | (header.gsTex0.TBP & 0xFFFF)
                };

                subimage.data = ins.ReadBytes(imageSize);

                //Weird shit FS do, do
                if (subimage.Length > subimage.data.Length)
                    subimage.Length = (uint)subimage.data.Length;

                //Fix Weird Images
                switch(subimage.Format)
                {
                    case Texture.ColourMode.M4:
                        Texture.Fix4BPPIndicesEndianness(ref subimage.data);
                        break;

                    case Texture.ColourMode.D16:
                    case Texture.ColourMode.D24:
                    case Texture.ColourMode.D32:
                        Texture.ConvColourSpaceBGRAtoRGBA(ref subimage.data, subimage.Format, true);
                        break;
                }

                //Read Image Clut
                if (header.clutOffset > 0)
                {
                    Texture.ClutBuffer clut;

                    if(subimage.Format == Texture.ColourMode.M4)
                    {
                        clut = new Texture.ClutBuffer
                        {
                            Width = 8,
                            Height = 2,
                            Length = header.length - (header.clutOffset + 0x20),
                            Format = Texture.PSMtoColourMode(header.gsTex0.CPSM)
                        };
                    }
                    else
                    {
                        clut = new Texture.ClutBuffer
                        {
                            Width  = 16,
                            Height = 16,
                            Length = header.length - (header.clutOffset + 0x20),
                            Format = Texture.PSMtoColourMode(header.gsTex0.CPSM)
                        };
                    }

                    clut.data = ins.ReadBytes((int)clut.Length);

                    //Unswizzle
                    if(subimage.Format == Texture.ColourMode.M8 && header.gsTex0.CSM == 0)
                    {
                        Texture.PS2UnswizzleImageData(ref clut.data, clut.Format, (int)clut.Width, (int)clut.Height);
                    }

                    //BGRA -> RGBA
                    Texture.ConvColourSpaceBGRAtoRGBA(ref clut.data, clut.Format, true);

                    subimage.ClutCount = 1;
                    subimage.ClutIDs = new int[1];
                    subimage.ClutIDs[0] = ResultTexture.PutClut(clut);
                }

                ResultTexture.PutSubimage(subimage);

            } catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                return null;
            }

            return ResultTexture;
        }


        public void SaveToFile(string filepath, Texture data)
        {
            throw new NotImplementedException();
        }
    }
}
