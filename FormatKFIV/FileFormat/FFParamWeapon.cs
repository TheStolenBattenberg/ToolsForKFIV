using System;
using System.Collections.Generic;

using FormatKFIV.Asset;
using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    public class FFParamWeapon : FIFormat<Param>
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
            Validator = FFParamWeapon.FileIsValid
        };

        /// <summary>Validates a file to see if it of the correct type</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    ins.Seek(2, System.IO.SeekOrigin.Current);
                    ushort hitLvl1 = ins.ReadUInt16();
                    ins.Seek(16, System.IO.SeekOrigin.Current);
                    ushort hitLvl2 = ins.ReadUInt16();
                    ins.Seek(16, System.IO.SeekOrigin.Current);
                    ushort hitLvl3 = ins.ReadUInt16();

                    validFile = (hitLvl1 == 0x14 && hitLvl2 == 0x32 && hitLvl3 == 0x50);
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
                return LoadItemWeaponParams(ins);
            }
        }

        public Param LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(buffer))
            {
                return LoadItemWeaponParams(ins);
            }
        }

        public static Param LoadItemWeaponParams(InputStream ins)
        {
            Param paramOut = new Param();

            //Define Weapon Param Layout for stats
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
                    new Param.ParamColumn { Name = "Dark", DataType = Param.ParamColumnFormat.DTUInt16 }
                }
            };

            //Define Weapon Param Pages
            paramOut.Pages = new List<Param.ParamPage>()
            {
                new Param.ParamPage { name = "Weapon Stats (Lvl 1)", rows = new List<Param.ParamRow>(), layout = paramLayoutStats },
                new Param.ParamPage { name = "Weapon Stats (Lvl 2)", rows = new List<Param.ParamRow>(), layout = paramLayoutStats },
                new Param.ParamPage { name = "Weapon Stats (Lvl 3)", rows = new List<Param.ParamRow>(), layout = paramLayoutStats },
            };

            //Load Parameters
            try
            {
                do
                {
                    for(int i = 0; i < 3; ++i)
                    {
                        ushort slashDmg = ins.ReadUInt16();
                        ushort hitDmg = ins.ReadUInt16();
                        ushort stabDmg = ins.ReadUInt16();
                        ushort fireDmg = ins.ReadUInt16();
                        ushort earthDmg = ins.ReadUInt16();
                        ushort windDmg = ins.ReadUInt16();
                        ushort waterDmg = ins.ReadUInt16();
                        ushort lightDmg = ins.ReadUInt16();
                        ushort darkDmg = ins.ReadUInt16();

                        Param.ParamRow row = new Param.ParamRow(slashDmg, hitDmg, stabDmg, fireDmg, earthDmg, windDmg, waterDmg, lightDmg, darkDmg);
                        switch (i)
                        {
                            case 0:
                                paramOut.Pages[0].AddRow(row);
                                continue;
                            case 1:
                                paramOut.Pages[1].AddRow(row);
                                continue;
                            case 2:
                                paramOut.Pages[2].AddRow(row);
                                continue;
                        }
                    }

                    ins.Seek(0x5a, System.IO.SeekOrigin.Current);
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
