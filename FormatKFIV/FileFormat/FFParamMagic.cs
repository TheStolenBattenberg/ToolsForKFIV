using System;
using System.Collections.Generic;

using FormatKFIV.Utility;
using FormatKFIV.Asset;


namespace FormatKFIV.FileFormat
{
    public class FFParamMagic : FIFormat<Param>
    {
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".prm",
            },
            Type = FEType.Param,
            AllowExport = false,
            Validator = FFParamMagic.FileIsValid
        };

        /// <summary>Validates a file to see if it is valid</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    validFile = validFile & (ins.ReadUInt16() == 0x82E);
                    ins.Seek(0x10E, System.IO.SeekOrigin.Current);
                    validFile = validFile & (ins.ReadUInt16() == 0x826);
                    ins.Seek(0x10E, System.IO.SeekOrigin.Current);
                    validFile = validFile & (ins.ReadUInt16() == 0x826);
                    ins.Seek(0x10E, System.IO.SeekOrigin.Current);
                    validFile = validFile & (ins.ReadUInt16() == 0x826);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    Console.WriteLine(Ex.StackTrace);
                    return false;
                }
            }

            return validFile;
        }

        #endregion

        public Param LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(buffer))
            {
                return LoadParamMagic(ins);
            }
        }
        public Param LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(filepath))
            {
                return LoadParamMagic(ins);
            }
        }

        public static Param LoadParamMagic(InputStream ins)
        {
            Param paramOut = new Param();

            //Define Magic Param Layout
            Param.ParamLayout paramLayoutBasic = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Name",    DataType = Param.ParamColumnFormat.DTString },
                    new Param.ParamColumn { Name = "Unknown 0x30", DataType = Param.ParamColumnFormat.DTUInt32 },
                    new Param.ParamColumn { Name = "MP Cost", DataType = Param.ParamColumnFormat.DTUInt16 }
                },
            };
            Param.ParamLayout paramLayoutStats = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Slash", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Hit", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Stab", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Fire", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Earth", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Wind", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Water", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Light", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Dark", DataType = Param.ParamColumnFormat.DTUInt16 },
                },
            };

            //Define Magic Param Pages
            paramOut.Pages = new List<Param.ParamPage>()
            {
                new Param.ParamPage { name = "Misc", rows = new List<Param.ParamRow>(), layout = paramLayoutBasic },
                new Param.ParamPage { name = "Stats", rows = new List<Param.ParamRow>(), layout = paramLayoutStats } 
            };

            //Read Magic Param Data
            try
            {
                do
                {
                    string magicName = ins.ReadFixedKFIVString(24);
                    uint unknown0x30 = ins.ReadUInt32();
                    ushort dmgSlash = ins.ReadUInt16();
                    ushort dmgHit = ins.ReadUInt16();
                    ushort dmgStab = ins.ReadUInt16();
                    ushort dmgFire = ins.ReadUInt16();
                    ushort dmgEarth = ins.ReadUInt16();
                    ushort dmgWind = ins.ReadUInt16();
                    ushort dmgWater = ins.ReadUInt16();
                    ushort dmgLight = ins.ReadUInt16();
                    ushort dmgDark = ins.ReadUInt16();

                    ins.Seek(0xB6, System.IO.SeekOrigin.Current);

                    ushort mpCost = ins.ReadUInt16();

                    ins.Seek(0x12, System.IO.SeekOrigin.Current);

                    paramOut.Pages[0].AddRow(new Param.ParamRow(magicName, unknown0x30, mpCost));
                    paramOut.Pages[1].AddRow(new Param.ParamRow(dmgSlash, dmgHit, dmgStab, dmgFire, dmgEarth, dmgWind, dmgWater, dmgLight, dmgDark));

                } while (!ins.IsEndOfStream());
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                return null;
            }

            return paramOut;
        }

        public void SaveToFile(string filepath, Param data)
        {
            throw new NotImplementedException();
        }
    }
}
