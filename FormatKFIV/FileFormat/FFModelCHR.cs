using System;
using System.Collections.Generic;

using FormatKFIV.Utility;
using FormatKFIV.Asset;

namespace FormatKFIV.FileFormat
{
    public class FFModelCHR : FIFormat<Model>
    {
        #region Format Structures
        public struct CHRHeader
        {
            public uint unknown0x00;
            public uint offOM2Model;
            public uint offMIXAnimation;
            public uint offTX2Texture;
            public uint offOMDModel;
            public uint offHDSound;
            public uint offBDSound;
            public uint lenBDSound;
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
                ".chr",
            },
            Type = FEType.Model,
            AllowExport = false,
            Validator = FFModelCHR.FileIsValid
        };

        /// <summary>Validates a file to see if it is KFIV character file</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    //no it doesn't lol.
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

        public Model LoadFromMemory(byte[] buffer, out object ret2, out object ret3, out object ret4)
        {
            Model outOM2;
            Texture outTX2;

            using (InputStream ins = new InputStream(buffer))
            {
                outOM2 = ImportCHR(ins, out outTX2);
            }

            ret2 = outTX2;
            ret3 = null;
            ret4 = null;
            return outOM2;
        }

        public Model LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            Model outOM2;
            Texture outTX2;

            using (InputStream ins = new InputStream(filepath))
            {
                outOM2 = ImportCHR(ins, out outTX2);
            }

            ret2 = outTX2;
            ret3 = null;
            ret4 = null;
            return outOM2;
        }

        public Model ImportCHR(InputStream ins, out Texture tx2)
        {
            Model outOm2 = null;
            Texture outTx2 = null;


            
            try
            {
                CHRHeader header = new CHRHeader
                {
                    unknown0x00 = ins.ReadUInt32(),
                    offOM2Model = ins.ReadUInt32(),
                    offMIXAnimation = ins.ReadUInt32(),
                    offTX2Texture = ins.ReadUInt32(),
                    offOMDModel = ins.ReadUInt32(),
                    offHDSound = ins.ReadUInt32(),
                    offBDSound = ins.ReadUInt32(),
                    lenBDSound = ins.ReadUInt32()
                };

                //Read OMD for transformation info
                ins.Jump(header.offOMDModel);
                FFModelOMD.OMDModel omdModel = FFModelOMD.ImportOMDFromStream(ins);
                ins.Return();

                //Read OM2
                FFModelOM2 om2Importer = new FFModelOM2();
                ins.Jump(header.offOM2Model);
                outOm2 = om2Importer.ImportOM2(ins, omdModel);
                ins.Return();

                //Read TX2
                FFTextureTX2 tx2Importer = new FFTextureTX2();
                ins.Jump(header.offTX2Texture);
                outTx2 = tx2Importer.ImportTX2(ins);
                ins.Return();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.StackTrace);
                tx2 = null;
                return null;
            }

            tx2 = outTx2;
            return outOm2;
        }

        public void SaveToFile(string filepath, Model data)
        {
            throw new NotImplementedException();
        }
    }
}
