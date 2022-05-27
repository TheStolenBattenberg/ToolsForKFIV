using System;
using System.Collections.Generic;
using System.Text;

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

        /// <summary>Validates a file to see if it is PS2 ICO Format</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    ins.Seek(60, System.IO.SeekOrigin.Begin);
                    int someVal = ins.ReadInt32();
                    ins.Seek(60, System.IO.SeekOrigin.Current);
                    int someVal2 = ins.ReadInt32();

                    validFile = (someVal == 1 && someVal2 == 100);

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

            //Create Param Layout
            Param.ParamLayout layout = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[7],
            };
            layout.Columns[0] = new Param.ParamColumn
            {
                Name = "Name",
                DataType = Param.ParamColumnFormat.DTString,
            };
            layout.Columns[1] = new Param.ParamColumn
            {
                Name = "Icon ID",
                DataType = Param.ParamColumnFormat.DTInt16,
            };
            layout.Columns[2] = new Param.ParamColumn
            {
                Name = "Unknown0x32",
                DataType = Param.ParamColumnFormat.DTInt16,
            };
            layout.Columns[3] = new Param.ParamColumn
            {
                Name = "Unknown0x34",
                DataType = Param.ParamColumnFormat.DTInt16,
            };
            layout.Columns[4] = new Param.ParamColumn
            {
                Name = "Unknown0x36",
                DataType = Param.ParamColumnFormat.DTInt16,
            };
            layout.Columns[5] = new Param.ParamColumn
            {
                Name = "Unknown0x38",
                DataType = Param.ParamColumnFormat.DTFloat,
            };
            layout.Columns[6] = new Param.ParamColumn
            {
                Name = "Durability",
                DataType = Param.ParamColumnFormat.DTInt32
            };
            paramOut.SetLayout(layout);

            try
            {
                int count;

                //Item Page
                Param.ParamPage itemNameItems = new Param.ParamPage
                {
                    pageName = "Item Name",
                    pageRows = new List<Param.ParamRow>(),
                };
                //Equipment Page
                Param.ParamPage itemNameEquips = new Param.ParamPage
                {
                    pageName = "Equipment Name",
                    pageRows = new List<Param.ParamRow>(),
                };

                count = 150;
                do
                {
                    //Read a row of data
                    string vItemName = ins.ReadFixedKFIVString(24);
                    short vItemID = ins.ReadInt16();
                    short vItemUkn0x32 = ins.ReadInt16();
                    short vItemUkn0x34 = ins.ReadInt16();
                    short vItemUkn0x36 = ins.ReadInt16();
                    float vItemUkn0x38 = ins.ReadSingle();
                    int vItemUnk0x3c = ins.ReadInt32();

                    if (count > 0)
                    {
                        itemNameItems.AddRow(
                            new Param.ParamRow(vItemName, vItemID, vItemUkn0x32, vItemUkn0x34, vItemUkn0x36, vItemUkn0x38, vItemUnk0x3c));

                        count--;
                    }
                    else
                    {
                        itemNameEquips.AddRow(
                            new Param.ParamRow(vItemName, vItemID, vItemUkn0x32, vItemUkn0x34, vItemUkn0x36, vItemUkn0x38, vItemUnk0x3c));
                    }
                } while (!ins.IsEndOfStream());

                paramOut.AddPage(itemNameItems);
                paramOut.AddPage(itemNameEquips);

            } catch (Exception Ex)
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
