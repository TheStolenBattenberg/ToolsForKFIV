using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using FormatKFIV.Utility;

namespace FormatKFIV.FileFormat
{
    /// <summary>Provides utility for reading/writing a FromSoft KFIV INI</summary>
    public class FFINI
    {
        #region Format Structures

        #endregion

        //Properties
        /// <summary>Returns an array of lines.</summary>
        public string[] Lines;

        /// <summary>Load an INI file from a buffer</summary>
        /// <param name="buffer">The buffer to load from</param>
        /// <returns>An FFINI Class</returns>
        public static FFINI LoadFromBuffer(byte[] buffer)
        {
            FFINI ResultingINIFile = new FFINI();

            using(StreamReader sr = new StreamReader(new MemoryStream(buffer), Encoding.UTF8, false))
            {
                List<string> iniLines = new List<string>();
                do
                {
                    iniLines.Add(sr.ReadLine());
                } while (!sr.EndOfStream);

                ResultingINIFile.Lines = iniLines.ToArray();
            }

            return ResultingINIFile;
        }
        
    }
}
