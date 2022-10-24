using System;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.TypePlayStation;
using FormatKFIV.Utility;
using FormatKFIV.Asset;

namespace FormatKFIV.FileFormat
{
    /// <summary>Container of many TM2 textures. It's a mix of other KFIV formats. </summary>
    public class FFTextureTMX : FIFormat<Texture>
    {
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".tmx",
            },
            Type = FEType.Texture,
            AllowExport = false,
            Validator = FFTextureTMX.FileIsValid
        };

        /// <summary>Validates a file to see if it is FromSoft TM2 Format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    ins.Seek(8, System.IO.SeekOrigin.Begin);
                    uint check1 = ins.ReadUInt32();
                    ins.Seek(12, System.IO.SeekOrigin.Begin);
                    uint check2 = ins.ReadUInt32();
                    ins.Seek(28, System.IO.SeekOrigin.Begin);
                    uint check3 = ins.ReadUInt32();

                    return (check1 == 0 && check2 == 0 && check3 == 0);
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
                //Read TMX Header
                FFTextureTX2.TX2Header tx2Header = new FFTextureTX2.TX2Header
                {
                    length = ins.ReadUInt32(),
                    numTexture = ins.ReadUInt32(),
                    pad0x08 = ins.ReadUInt32(),
                    pad0x0C = ins.ReadUInt32()
                };

                //Read each TM2
                for(int i = 0; i < tx2Header.numTexture; ++i)
                {
                    uint tm2Offset = ins.ReadUInt32();
                    ins.Seek(12, System.IO.SeekOrigin.Current); //Jump 4 bytes forward

                    ins.Jump(tm2Offset);

                    //Read TM2 Header
                    FFTextureTM2.TM2Header tm2Header = new FFTextureTM2.TM2Header
                    {
                        length = ins.ReadUInt32(),
                        clutOffset = ins.ReadUInt32(),
                        unknown0x08 = ins.ReadUInt32(),
                        unknown0x0C = ins.ReadUInt32(),
                        gsTex0 = ins.ReadPS28ByteRegister()._sceGsTex0,
                        unknown0x18 = ins.ReadUInt64()
                    };

                    //Create Image Buffer
                    Texture.ImageBuffer tm2SubImage = new Texture.ImageBuffer
                    {
                        Name = null,
                        Width = (uint)Math.Pow(2, tm2Header.gsTex0.TW),
                        Height = (uint)Math.Pow(2, tm2Header.gsTex0.TH),
                        Length = 0,
                        Format = Texture.PSMtoColourMode(tm2Header.gsTex0.PSM),
                        ClutCount = 0,
                        ClutIDs = null,
                        data = null,
                        UID = ((tm2Header.gsTex0.CBP & 0xFFFF) << 16) | (tm2Header.gsTex0.TBP & 0xFFFF)
                };

                    //Calculate TM2 Image Size
                    int tm2ImageSize = 0;
                    switch(tm2SubImage.Format)
                    {
                        case Texture.ColourMode.M4: 
                            tm2ImageSize = (int)((tm2SubImage.Width >> 1) * tm2SubImage.Height); 
                            break;
                        case Texture.ColourMode.M8: 
                            tm2ImageSize = (int)(tm2SubImage.Width * tm2SubImage.Height); 
                            break;
                        case Texture.ColourMode.D16: 
                            tm2ImageSize = (int)((tm2SubImage.Width * 2) * tm2SubImage.Height);
                            break;
                        case Texture.ColourMode.D24:
                            tm2ImageSize = (int)((tm2SubImage.Width * 3) * tm2SubImage.Height);
                            break;
                        case Texture.ColourMode.D32:
                            tm2ImageSize = (int)((tm2SubImage.Width * 4) * tm2SubImage.Height);
                            break;
                    }

                    //Read TM2 Image Data
                    tm2SubImage.data = ins.ReadBytes(tm2ImageSize);
                    tm2SubImage.Length = (uint)tm2ImageSize;

                    //Fix Image Issues
                    switch(tm2SubImage.Format)
                    {
                        case Texture.ColourMode.M4:
                            Texture.Fix4BPPIndicesEndianness(ref tm2SubImage.data);
                            break;
                        case Texture.ColourMode.D24:
                        case Texture.ColourMode.D32:
                            Texture.ConvColourSpaceBGRAtoRGBA(ref tm2SubImage.data, tm2SubImage.Format, true);
                            break;
                    }

                    //Create Clut Buffer
                    if(tm2Header.clutOffset > 0)
                    {
                        //Figure out colour map width/height
                        uint CMWidth = 0, CMHeight = 0;
                        switch(tm2SubImage.Format)
                        {
                            case Texture.ColourMode.M4:
                                CMWidth = 8;
                                CMWidth = 2;
                                break;
                            case Texture.ColourMode.M8:
                                CMWidth = 16;
                                CMHeight = 16;
                                break;
                        }

                        Texture.ClutBuffer tm2Clut = new Texture.ClutBuffer
                        {
                            Width = CMWidth,
                            Height = CMHeight,
                            Length = tm2Header.length - (tm2Header.clutOffset + 0x20),
                            Format = Texture.PSMtoColourMode(tm2Header.gsTex0.CPSM)
                        };
                        tm2Clut.data = ins.ReadBytes((int)tm2Clut.Length);

                        //Fix Clut Issues
                        if(tm2SubImage.Format == Texture.ColourMode.M8 && tm2Header.gsTex0.CSM == 0)
                            Texture.PS2UnswizzleImageData(ref tm2Clut.data, tm2Clut.Format, (int)tm2Clut.Width, (int)tm2Clut.Height);

                        Texture.ConvColourSpaceBGRAtoRGBA(ref tm2Clut.data, tm2Clut.Format, true);

                        tm2SubImage.ClutCount = 1;
                        tm2SubImage.ClutIDs = new int[1];
                        tm2SubImage.ClutIDs[0] = ResultTexture.PutClut(tm2Clut);
                    }

                    ResultTexture.PutSubimage(tm2SubImage);

                    ins.Return();
                }

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
