using System;

using FormatKFIV.TypePlayStation;
using FormatKFIV.Utility;
using FormatKFIV.Asset;

namespace FormatKFIV.FileFormat
{
    /// <summary>Texture Container serving multiple textures or a single texture. Depends how FromSoft felt.</summary>
    public class FFTextureTX2 : FIFormat<Texture>
    {
        #region Format Structures
        /// <summary>Contains information on a TX2 file</summary>
        public struct TX2Header
        {
            /// <summary>Total length of the file.</summary>
            public uint length;
            /// <summary>Number of textures inside the container</summary>
            public uint numTexture;
            public uint pad0x08;
            public uint pad0x0C;
        }

        /// <summary>Contains offset to TX2GsData, and wastes space.</summary>
        public struct TX2Texture
        {
            /// <summary>Offset to TX2GsTransfer. Relative to file start.</summary>
            public uint gsTransferOffset;
            public uint pad0x04;
            public uint pad0x08;
            public uint pad0x0C;
        }

        /// <summary>Only used above the image segmant of a texture, palettes don't get one. Directly afterwards is a TX2GsData Structure</summary>
        public struct TX2GsTransfer
        {
            /// <summary>Maybe DMA related</summary>
            public ulong Unknown0x00;
            /// <summary>Maybe DMA related</summary>
            public ulong Unknown0x08;
            /// <summary>Maybe DMA related</summary>
            public ulong Unknown0x10;
            /// <summary>Maybe DMA related</summary>
            public ulong Unknown0x18;
            
            //Format of Unknown0x00 and Unknown0x08 combined
            //ushort  QWC; //Quadword count 
            //ubyte   ???;
            //ubyte   ???;
            //uint[3] padding?;
            //
            //Format of Unknown0x10 and Unknown0x18 combined is identical to this,
            //but the padding (maybe) comes before the actual parameters.

            // This is 100% something from SCE because it's used in totally unrelated PS2 games.

            //SPECULATION:
            //  Unknown0x00 and Unknown0x08 probably set up for transfer into main memory
            //  Unknown0x10 and Unknown0x18 are transfered by the above tags, with the reset of the texture data.
            //  It can be assumed that the latter two are then used to transfer from main memory to GS memory?..
        }

        /// <summary>Used to store pixels and commands to send them to the GS. stores either a palette or image (index or colour)</summary>
        public struct TX2GsData
        {
            /// <summary>GIF Tag for sending register information</summary>
            public sceGifTag gifTag0;
            /// <summary>TexFlush Register Data (Usually whatever, Fromsoft likes to use 0xFFFFFFFF00000000</summary>
            public sceGsTexflush texFlushData;
            /// <summary>TexFlush Register Address</summary>
            public ulong texFlushAddr;
            /// <summary>Bitbltbuf Register Data</summary>
            public sceGsBitbltbuf bitbltbufData;
            /// <summary>Bitbltbuf Register Address</summary>
            public ulong bitbltbufAddr;
            /// <summary>Trxpos Register Data</summary>
            public sceGsTrxpos trxposData;
            /// <summary>Trxpos Register Address</summary>
            public ulong trxposAddr;
            /// <summary>Trxreg Register Data</summary>
            public sceGsTrxreg trxregData;
            /// <summary>Trxreg Register Address</summary>
            public ulong trxregAddr;
            /// <summary>Trxreg Register Data</summary>
            public sceGsTrxdir trxdirData;
            /// <summary>Trxdir Register Address</summary>
            public ulong trxdirAddr;
            /// <summary>GIF Tag for sending gs pixels</summary>
            public sceGifTag gifTag1;

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
                ".tx2",
                ".tmr"
            },
            Type = FEType.Texture,
            AllowExport = false,
            Validator = FFTextureTX2.FileIsValid
        };

        /// <summary>Validates a file to see if it is TX2 Format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    ins.Seek(4, System.IO.SeekOrigin.Begin);
                    uint numPic = ins.ReadUInt32();
                    ins.Seek(16, System.IO.SeekOrigin.Begin);
                    uint comp = ins.ReadUInt32();

                    return (16 + (16 * numPic)) == comp;
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

        /// <summary>Load a TX2 from buffered storage</summary>
        /// <param name="buffer">A byte buffer containing the TX2</param>
        /// <param name="ret2">(Optional, null) N/A</param>
        /// <param name="ret3">(Optional, null) N/A</param>
        /// <param name="ret4">(Optional, null) N/A</param>
        /// <returns>The resulting Texture file, or null if something bad happens</returns>
        public Texture LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            //We don't use these, so we null them right away.
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(buffer))
            {
                return ImportTX2(ins);
            }
        }

        /// <summary>Load a TX2 from the file system</summary>
        /// <param name="filepath">Filepath to a TX2</param>
        /// <param name="ret2">(Optional, null) N/A</param>
        /// <param name="ret3">(Optional, null) N/A</param>
        /// <param name="ret4">(Optional, null) N/A</param>
        /// <returns>The resulting Texture file, or null if something bad happens</returns>
        public Texture LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            //We don't use these, so we null them right away.
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(filepath))
            {
                return ImportTX2(ins);
            }
        }

        /// <summary>Imports a TX2</summary>
        /// <param name="ins">The InputStream of TX2 data</param>
        /// <returns>The resulting texture data</returns>
        public Texture ImportTX2(InputStream ins)
        {
            Texture ResultingTexture = new Texture();

            long insBasePos = ins.Position();

            try
            {
                //Read Header
                TX2Header tx2Header = new TX2Header
                {
                    length = ins.ReadUInt32(),
                    numTexture = ins.ReadUInt32(),
                    pad0x08 = ins.ReadUInt32(),
                    pad0x0C = ins.ReadUInt32()
                };

                //Read Textures
                TX2Texture[] tx2Textures = new TX2Texture[tx2Header.numTexture];
                for (int i = 0; i < tx2Header.numTexture; ++i)
                {
                    tx2Textures[i] = new TX2Texture
                    {
                        gsTransferOffset = ins.ReadUInt32(),
                        pad0x04 = ins.ReadUInt32(),
                        pad0x08 = ins.ReadUInt32(),
                        pad0x0C = ins.ReadUInt32()
                    };
                }

                //Read TX2GsTransfer/TX2GsData
                foreach (TX2Texture texRef in tx2Textures)
                {
                    //Jump to location of texture in file
                    ins.Jump(insBasePos + texRef.gsTransferOffset);

                    //Read TX2GsTransfer
                    TX2GsTransfer tx2Gst = new TX2GsTransfer
                    {
                        Unknown0x00 = ins.ReadUInt64(),
                        Unknown0x08 = ins.ReadUInt64(),
                        Unknown0x10 = ins.ReadUInt64(),
                        Unknown0x18 = ins.ReadUInt64()
                    };

                    //Read first TX2GsData (ID = Image Data, Always present)
                    TX2GsData tx2GsID = new TX2GsData
                    {
                        gifTag0 = ins.ReadPS216ByteRegister()._sceGifTag,          //Register Transfer Tag
                        texFlushData = ins.ReadPS28ByteRegister()._sceGsTexflush,       //REGISTERS :)
                        texFlushAddr = ins.ReadUInt64(),
                        bitbltbufData = ins.ReadPS28ByteRegister()._sceGsBitbltbuf,
                        bitbltbufAddr = ins.ReadUInt64(),
                        trxposData = ins.ReadPS28ByteRegister()._sceGsTrxpos,
                        trxposAddr = ins.ReadUInt64(),
                        trxregData = ins.ReadPS28ByteRegister()._sceGsTrxreg,
                        trxregAddr = ins.ReadUInt64(),
                        trxdirData = ins.ReadPS28ByteRegister()._sceGsTrxdir,
                        trxdirAddr = ins.ReadUInt64(),
                        gifTag1 = ins.ReadPS216ByteRegister()._sceGifTag                //Data Transfer Tag
                    };

                    //Store Image data
                    Texture.ImageBuffer imgBuffer = new Texture.ImageBuffer();
                    imgBuffer.Width = tx2GsID.trxregData.RRW;      //Copy image width/height
                    imgBuffer.Height = tx2GsID.trxregData.RRH;
                    imgBuffer.Length = tx2GsID.gifTag1.NLOOP * 16;  //Calculate buffer width using nloop * 16 (nloop = num quad words in packet)
                    imgBuffer.Format = Texture.PSMtoColourMode(tx2GsID.bitbltbufData.DPSM);
                    imgBuffer.data = ins.ReadBytes((int)imgBuffer.Length);
                    imgBuffer.UID = ((0x0000 & 0xFFFF) << 16) | (tx2GsID.bitbltbufData.DBP & 0xFFFF);

                    //Flip
                    Texture.FlipV(ref imgBuffer.data, imgBuffer.Format, (int)imgBuffer.Width, (int)imgBuffer.Height);

                    //Fix 4BPP endianness
                    if (imgBuffer.Format == Texture.ColourMode.M4)
                    {
                        Texture.Fix4BPPIndicesEndianness(ref imgBuffer.data);
                    }

                    //Load CLUT data (if applicable)
                    if (imgBuffer.Format == Texture.ColourMode.M4 || imgBuffer.Format == Texture.ColourMode.M8)
                    {
                        //Is there even a CLUT present?
                        //gif is done in chains, so if it's not the last gifTag1 IS NOT the last gif packet
                        //there is most likely a palette afterwards.
                        if (tx2GsID.gifTag1.EOP == 0)
                        {
                            //Read second TX2GsData (ID = Image Data, Always present)
                            TX2GsData tx2GsCD = new TX2GsData
                            {
                                gifTag0 = ins.ReadPS216ByteRegister()._sceGifTag,          //Register Transfer Tag
                                texFlushData = ins.ReadPS28ByteRegister()._sceGsTexflush,       //REGISTERS :)
                                texFlushAddr = ins.ReadUInt64(),
                                bitbltbufData = ins.ReadPS28ByteRegister()._sceGsBitbltbuf,
                                bitbltbufAddr = ins.ReadUInt64(),
                                trxposData = ins.ReadPS28ByteRegister()._sceGsTrxpos,
                                trxposAddr = ins.ReadUInt64(),
                                trxregData = ins.ReadPS28ByteRegister()._sceGsTrxreg,
                                trxregAddr = ins.ReadUInt64(),
                                trxdirData = ins.ReadPS28ByteRegister()._sceGsTrxdir,
                                trxdirAddr = ins.ReadUInt64(),
                                gifTag1 = ins.ReadPS216ByteRegister()._sceGifTag                 //Data Transfer Tag
                            };

                            //Store CLUT data
                            Texture.ClutBuffer clutBuffer = new Texture.ClutBuffer();
                            clutBuffer.Width = tx2GsCD.trxregData.RRW;
                            clutBuffer.Height = tx2GsCD.trxregData.RRH;
                            clutBuffer.Length = tx2GsCD.gifTag1.NLOOP * 16;
                            clutBuffer.Format = Texture.PSMtoColourMode(tx2GsCD.bitbltbufData.DPSM);
                            clutBuffer.data = ins.ReadBytes((int)clutBuffer.Length);

                            imgBuffer.UID = ((tx2GsCD.bitbltbufData.DBP & 0xFFFF) << 16) | (tx2GsID.bitbltbufData.DBP & 0xFFFF);

                            //Unswizzle the clut. Need to find a way to check if it IS swizzled
                            if (imgBuffer.Format == Texture.ColourMode.M8)
                            {
                                //Can only currently unswizzle 8BPP data... and according to
                                //the sce docs, it doesn't even make sense to unswizzle 4bpp data.
                                Texture.PS2UnswizzleImageData(ref clutBuffer.data, clutBuffer.Format, (int)clutBuffer.Width, (int)clutBuffer.Height);
                            }

                            //BGRA -> RGBA
                            Texture.ConvColourSpaceBGRAtoRGBA(ref clutBuffer.data, clutBuffer.Format, true);

                            //Put CLUT Info into ImageBuffer
                            imgBuffer.ClutCount = 1;
                            imgBuffer.ClutIDs = new int[1];

                            //Put ClutBuffer into texture, and resulting index into ImageBuffer
                            imgBuffer.ClutIDs[0] = ResultingTexture.PutClut(clutBuffer);
                        }
                        else
                        {
                            Console.WriteLine("Texture with ColourMode M4/M8 but it had no palette.");
                        }
                    }
                    else
                    {
                        //No Clut is present, so these very spesific parameters are set that if you change
                        //will probably make everything code related explode but the universe implode or some shit like that.
                        imgBuffer.ClutCount = 0;
                        imgBuffer.ClutIDs = null;
                    }

                    //Put the ImageBuffer into the texture
                    ResultingTexture.PutSubimage(imgBuffer);

                    //Return from just read Texture
                    ins.Return();
                }


                ins.Seek(insBasePos + tx2Header.length, System.IO.SeekOrigin.Begin);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                return null;
            }

            return ResultingTexture;
        }

        public void SaveToFile(string filepath, Texture data)
        {
            throw new NotImplementedException();
        }
    }
}
