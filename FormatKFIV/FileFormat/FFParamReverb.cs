using System;
using System.Collections.Generic;

using FormatKFIV.Utility;
using FormatKFIV.Asset;

namespace FormatKFIV.FileFormat
{
    /// <summary>Provides storage and utility for reading/writing a rev_para.dat file</summary>
    public class FFParamReverb : FIFormat<Param>
    {
        #region Format Parameters
        /// <summary>Returns FF parameters for import/export</summary>
        public FIParameters Parameters { get { return _parameters; } }

        /// <summary>The defined file format parameters.</summary>
        private FIParameters _parameters = new FIParameters
        {
            Extensions = new string[]
            {
                ".dat",
            },
            Type = FEType.Param,
            AllowExport = false,
            Validator = FFParamReverb.FileIsValid
        };

        /// <summary>Validates a file to see if it is valid</summary>
        private static bool FileIsValid(byte[] buffer)
        {
            bool validFile = true;
            using (InputStream ins = new InputStream(buffer))
            {
                try
                {
                    //Check Reverb Param by checking reverb modes.
                    validFile = (ins.ReadUInt32() == 5);
                    ins.Seek(0xC, System.IO.SeekOrigin.Current);
                    validFile = validFile & (ins.ReadUInt32() == 5);
                    ins.Seek(0xC, System.IO.SeekOrigin.Current);
                    validFile = validFile & (ins.ReadUInt32() == 6);
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
                return LoadParamReverb(ins);
            }
        }
        public Param LoadFromFile(string filepath, out object ret2, out object ret3, out object ret4)
        {
            ret2 = null;
            ret3 = null;
            ret4 = null;

            using (InputStream ins = new InputStream(filepath))
            {
                return LoadParamReverb(ins);
            }
        }

        public static Param LoadParamReverb(InputStream ins)
        {
            Param paramOut = new Param();

            //Define Reverb Param Layout
            Param.ParamLayout paramLayout = new Param.ParamLayout
            {
                Columns = new Param.ParamColumn[]
                {
                    new Param.ParamColumn { Name = "Mode", DataType = Param.ParamColumnFormat.DTUInt32 },
                    new Param.ParamColumn { Name = "Depth (L)", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Depth (R)", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Delay", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Feedback", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Volume (L)", DataType = Param.ParamColumnFormat.DTUInt16 },
                    new Param.ParamColumn { Name = "Volume (R)", DataType = Param.ParamColumnFormat.DTUInt16 },
                },
            };

            //Define Reverb Param Pages
            paramOut.Pages = new List<Param.ParamPage>()
            {
                new Param.ParamPage { name = "Settings", rows = new List<Param.ParamRow>(), layout = paramLayout }
            };

            //Read Reverb Parameter Data
            try
            {
                do
                {
                    //Read a single row of data...
                    uint vRevMode = ins.ReadUInt32();
                    ushort vRevDepL = ins.ReadUInt16();
                    ushort vRevDepR = ins.ReadUInt16();
                    ushort vDelay = ins.ReadUInt16();
                    ushort vFeedback = ins.ReadUInt16();
                    ushort vVolL = ins.ReadUInt16();
                    ushort vVolR = ins.ReadUInt16();

                    //Add row to page
                    paramOut.Pages[0].AddRow(new Param.ParamRow(vRevMode, vRevDepL, vRevDepR, vDelay, vFeedback, vVolL, vVolR));

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
