using System;
using System.Collections.Generic;

using FormatKFIV.Asset;
using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    public class FFParamItemName : FIFormat<Param>
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
            Validator = FFParamItemName.FileIsValid
        };

        /// <summary>Validates a file to see if it is this format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    ins.Seek(60, System.IO.SeekOrigin.Begin);
                    validFile = validFile & (ins.ReadUInt32() == 1);
                    ins.Seek(60, System.IO.SeekOrigin.Current);
                    validFile = validFile & (ins.ReadUInt32() == 100);
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

        public Param LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(filepath))
            {
                return LoadItemNameParam(ins);
            }
        }

        public Param LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(buffer))
            {
                return LoadItemNameParam(ins);
            }
        }

        public static Param LoadItemNameParam(InputStream ins)
        {
            Param paramOut = new Param();

            //Define Name Param Layout
            Param.ParamLayout paramLayout = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Name", DataType = Param.ParamColumnFormat.DTString },
                    new Param.ParamColumn { Name = "Unknown 0x2E", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Icon ID", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown 0x32", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown 0x34", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown 0x36", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown 0x38", DataType = Param.ParamColumnFormat.DTFloat },
                    new Param.ParamColumn { Name = "Unknown 0x3C", DataType = Param.ParamColumnFormat.DTUInt32 }
                }
            };

            //Define Name Parameter Pages
            paramOut.Pages = new List<Param.ParamPage>()
            {
                new Param.ParamPage { name = "Items", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Weapons", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Shields", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Helmets", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Cuirass'", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Gauntlets", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Greaves", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Rings", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Bracelets", rows = new List<Param.ParamRow>(), layout = paramLayout },
                new Param.ParamPage { name = "Amulets", rows = new List<Param.ParamRow>(), layout = paramLayout },
            };

            //Read Name Parameter Data
            try
            {
                //Item Names
                for(int i = 0; i < 150; ++i)
                {
                    paramOut.Pages[0].AddRow(ReadParamsRow(ins));
                }

                //Weapon Names
                for(int i = 0; i < 70; ++i)
                {
                    paramOut.Pages[1].AddRow(ReadParamsRow(ins));
                }

                //Shield Names
                for (int i = 0; i < 30; ++i)
                {
                    paramOut.Pages[2].AddRow(ReadParamsRow(ins));
                }

                //Helmet Names
                for(int i = 0; i < 25; ++i)
                {
                    paramOut.Pages[3].AddRow(ReadParamsRow(ins));
                }

                //Cuirass Names
                for (int i = 0; i < 25; ++i)
                {
                    paramOut.Pages[4].AddRow(ReadParamsRow(ins));
                }

                //Gauntlet Names
                for (int i = 0; i < 25; ++i)
                {
                    paramOut.Pages[5].AddRow(ReadParamsRow(ins));
                }

                //Greave Names
                for (int i = 0; i < 25; ++i)
                {
                    paramOut.Pages[6].AddRow(ReadParamsRow(ins));
                }

                //Ring Names
                for (int i = 0; i < 10; ++i)
                {
                    paramOut.Pages[7].AddRow(ReadParamsRow(ins));
                }

                //Bracelet Names
                for (int i = 0; i < 10; ++i)
                {
                    paramOut.Pages[8].AddRow(ReadParamsRow(ins));
                }
                 
                //Amulet Names
                for (int i = 0; i < 10; ++i)
                {
                    paramOut.Pages[9].AddRow(ReadParamsRow(ins));
                }
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


        private static Param.ParamRow ReadParamsRow(InputStream ins)
        {
            string name        = ins.ReadFixedKFIVString(23);
            ushort unknown0x2e = ins.ReadUInt16();
            ushort iconID      = ins.ReadUInt16();
            ushort unknown0x32 = ins.ReadUInt16();
            ushort unknown0x34 = ins.ReadUInt16();
            ushort unknown0x36 = ins.ReadUInt16();
            float  unknown0x38 = ins.ReadSingle();
            uint   unknown0x3c = ins.ReadUInt32();

            return new Param.ParamRow(name, unknown0x2e, iconID, unknown0x32, unknown0x34, unknown0x36, unknown0x38, unknown0x3c);
        }
    }
}
