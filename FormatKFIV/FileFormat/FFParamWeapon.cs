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

        #region Format Data
        private static string[] gEquipEffectNames =
        {
            "None",
            "Unknown (0x01)",
            "Damage HP",
            "Increase HP",
            "Decrease HP",
            "Increase MP",
            "Decrease MP",
            "HP Vamparism",
            "MP Vamparism",
            "Increase Physical",
            "Increase Magical"
        };

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
            Param.ParamLayout paramLayoutStatus = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Poison Chance", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Paralyze Chance", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Dark Chance", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Curse Chance", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Slow Chance", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Silence Chance", DataType = Param.ParamColumnFormat.DTUInt16 }
                }
            };
            Param.ParamLayout paramLayoutMagics = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Magic ID", DataType = Param.ParamColumnFormat.DTInt16 },
                    new Param.ParamColumn { Name = "Delay", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown0x04", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown0x06", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown0x08", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown0x0A", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown0x0C", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown0x0E", DataType = Param.ParamColumnFormat.DTUInt16 },
                }
            };
            Param.ParamLayout paramLayoutEquipEffects = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Type", DataType = Param.ParamColumnFormat.DTUInt8 },
                    new Param.ParamColumn { Name = "Name", DataType = Param.ParamColumnFormat.DTString },
                    new Param.ParamColumn { Name = "Amount", DataType = Param.ParamColumnFormat.DTInt8 }
                }
            };
            Param.ParamLayout paramLayoutType = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Ammo Item",   DataType = Param.ParamColumnFormat.DTUInt8 },
                    new Param.ParamColumn { Name = "Attack Type", DataType = Param.ParamColumnFormat.DTUInt8 }
                }
            };
            Param.ParamLayout paramLayoutAttack = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Length (Ft)", DataType = Param.ParamColumnFormat.DTFloat },
                    new Param.ParamColumn { Name = "Length (Cm)", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown 0x7a", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Collider Bounds", DataType = Param.ParamColumnFormat.DTString },
                    new Param.ParamColumn { Name = "Scan Start Frame", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Scan End Frame", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Unknown 0x7c", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Stamina Recharge Delay", DataType = Param.ParamColumnFormat.DTUInt8 },
                    new Param.ParamColumn { Name = "Unknown 0x7f", DataType = Param.ParamColumnFormat.DTUInt8 }
                }
            };

            //Define Weapon Param Pages
            paramOut.Pages = new List<Param.ParamPage>()
            {
                new Param.ParamPage { name = "Stats (Lvl 1)", rows = new List<Param.ParamRow>(), layout = paramLayoutStats },
                new Param.ParamPage { name = "Stats (Lvl 2)", rows = new List<Param.ParamRow>(), layout = paramLayoutStats },
                new Param.ParamPage { name = "Stats (Lvl 3)", rows = new List<Param.ParamRow>(), layout = paramLayoutStats },
                new Param.ParamPage { name = "Status Chance", rows = new List<Param.ParamRow>(), layout = paramLayoutStatus },
                new Param.ParamPage { name = "Magic Type A", rows = new List<Param.ParamRow>(), layout = paramLayoutMagics },
                new Param.ParamPage { name = "Magic Type B", rows = new List<Param.ParamRow>(), layout = paramLayoutMagics },
                new Param.ParamPage { name = "Type", rows = new List<Param.ParamRow>(), layout = paramLayoutType},
                new Param.ParamPage { name = "Equip Effect #1", rows = new List<Param.ParamRow>(), layout = paramLayoutEquipEffects},
                new Param.ParamPage { name = "Equip Effect #2", rows = new List<Param.ParamRow>(), layout = paramLayoutEquipEffects},
                new Param.ParamPage { name = "Attack", rows = new List<Param.ParamRow>(), layout = paramLayoutAttack}
            };

            //Load Parameters
            try
            {
                do
                {
                    //Stats Per Level
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

                    //Status Chance
                    ushort statusPoison = ins.ReadUInt16();
                    ushort statusParalyze = ins.ReadUInt16();
                    ushort statusDark = ins.ReadUInt16();
                    ushort statusCurse = ins.ReadUInt16();
                    ushort statusSlow = ins.ReadUInt16();
                    ushort statusSilence = ins.ReadUInt16();

                    paramOut.Pages[3].AddRow(new Param.ParamRow(statusPoison, statusParalyze, statusDark, statusCurse, statusSlow, statusSilence));

                    //Magic Type A
                    short magicTypeAID = ins.ReadInt16();
                    ushort magicTypeADelay = ins.ReadUInt16();
                    ushort magicUnknown0x46 = ins.ReadUInt16();
                    ushort magicUnknown0x48 = ins.ReadUInt16();
                    ushort magicUnknown0x4a = ins.ReadUInt16();
                    ushort magicUnknown0x4c = ins.ReadUInt16();
                    ushort magicUnknown0x4e = ins.ReadUInt16();
                    ushort magicUnknown0x50 = ins.ReadUInt16();

                    paramOut.Pages[4].AddRow(
                        new Param.ParamRow(
                            magicTypeAID, 
                            magicTypeADelay, 
                            magicUnknown0x46, 
                            magicUnknown0x48,
                            magicUnknown0x4a,
                            magicUnknown0x4c,
                            magicUnknown0x4e,
                            magicUnknown0x50
                        )
                    );

                    //Magic Type B
                    short magicTypeBID = ins.ReadInt16();
                    ushort magicTypeBDelay = ins.ReadUInt16();
                    ushort magicUnknown0x56 = ins.ReadUInt16();
                    ushort magicUnknown0x58 = ins.ReadUInt16();
                    ushort magicUnknown0x5a = ins.ReadUInt16();
                    ushort magicUnknown0x5c = ins.ReadUInt16();
                    ushort magicUnknown0x5e = ins.ReadUInt16();
                    ushort magicUnknown0x60 = ins.ReadUInt16();

                    paramOut.Pages[5].AddRow(
                        new Param.ParamRow(
                            magicTypeBID,
                            magicTypeBDelay,
                            magicUnknown0x56,
                            magicUnknown0x58,
                            magicUnknown0x5a,
                            magicUnknown0x5c,
                            magicUnknown0x5e,
                            magicUnknown0x60
                        )
                    );

                    //Weapon Type
                    byte ammoItemID = ins.ReadByte();
                    byte attackType = ins.ReadByte();

                    paramOut.Pages[6].AddRow(new Param.ParamRow(ammoItemID, attackType));


                    //Equip Effect #1
                    byte equipEffectAId    = ins.ReadByte();
                    byte equipEffectADelay = ins.ReadByte();
                    string equipEffectAName = $"Unknown (0x{equipEffectAId.ToString("X2")})";
                    if (equipEffectAId < gEquipEffectNames.Length)
                    {
                        equipEffectAName = gEquipEffectNames[equipEffectAId];
                    }
                    paramOut.Pages[7].AddRow(new Param.ParamRow(equipEffectAId, equipEffectAName, equipEffectADelay));

                    //Equip Effect #2
                    byte equipEffectBId = ins.ReadByte();
                    byte equipEffectBDelay = ins.ReadByte();
                    string equipEffectBName = $"Unknown (0x{equipEffectBId.ToString("X2")})";

                    if (equipEffectBId < gEquipEffectNames.Length)
                    {
                        equipEffectBName = gEquipEffectNames[equipEffectBId];
                    }

                    paramOut.Pages[8].AddRow(new Param.ParamRow(equipEffectBId, equipEffectBName, equipEffectBDelay));

                    //Attack
                    ushort length = ins.ReadUInt16();
                    ushort unknown0x6a = ins.ReadUInt16();
                    Vector3f hitBox = ins.ReadVector3f();
                    ushort hitScanSf = ins.ReadUInt16();
                    ushort hitScanEf = ins.ReadUInt16();
                    ushort unknown0x7c = ins.ReadUInt16();
                    byte staminaRechargeDelay = ins.ReadByte();
                    byte unknown0x7f = ins.ReadByte();

                    float lengthFt = MathF.Round((float)(length * 0.0328084), 1, MidpointRounding.ToZero);

                    paramOut.Pages[9].AddRow(new Param.ParamRow(lengthFt, length, unknown0x6a, hitBox.ToString(), hitScanSf, hitScanEf, unknown0x7c, staminaRechargeDelay, unknown0x7f));

                    ins.Seek(0x10, System.IO.SeekOrigin.Current);

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
