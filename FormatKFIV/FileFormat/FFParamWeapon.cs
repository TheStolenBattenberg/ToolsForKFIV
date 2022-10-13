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

        /// <summary>Validates a file to see if it is PS2 ICO Format</summary>
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

            //Define Parameter Layout
            Param.ParamLayout layout = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[9],
            };

            //Define Parameter Columns
            layout.Columns[0] = new Param.ParamColumn
            {
                Name = "Slash Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };
            layout.Columns[1] = new Param.ParamColumn
            {
                Name = "Hit Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };
            layout.Columns[2] = new Param.ParamColumn
            {
                Name = "Stab Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };
            layout.Columns[3] = new Param.ParamColumn
            {
                Name = "Fire Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };
            layout.Columns[4] = new Param.ParamColumn
            {
                Name = "Earth Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };
            layout.Columns[5] = new Param.ParamColumn
            {
                Name = "Wind Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };
            layout.Columns[6] = new Param.ParamColumn
            {
                Name = "Water Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };
            layout.Columns[7] = new Param.ParamColumn
            {
                Name = "Light Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };
            layout.Columns[8] = new Param.ParamColumn
            {
                Name = "Dark Damage",
                DataType = Param.ParamColumnFormat.DTUInt16,
            };

            paramOut.SetLayout(layout);

            //Define Parameter Pages
            Param.ParamPage weaponLvl1 = new Param.ParamPage
            {
                pageName = "Weapon (Level 1)",
                pageRows = new List<Param.ParamRow>(),
            };
            Param.ParamPage weaponLvl2 = new Param.ParamPage
            {
                pageName = "Weapon (Level 2)",
                pageRows = new List<Param.ParamRow>(),
            };
            Param.ParamPage weaponLvl3 = new Param.ParamPage
            {
                pageName = "Weapon (Level 3)",
                pageRows = new List<Param.ParamRow>(),
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
                                weaponLvl1.AddRow(row);
                                continue;
                            case 1:
                                weaponLvl2.AddRow(row);
                                continue;
                            case 2:
                                weaponLvl3.AddRow(row);
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

            paramOut.AddPage(weaponLvl1);
            paramOut.AddPage(weaponLvl2);
            paramOut.AddPage(weaponLvl3);

            return paramOut;
        }

        public void SaveToFile(string filepath, Param data)
        {
            throw new NotImplementedException();
        }
    }
}
