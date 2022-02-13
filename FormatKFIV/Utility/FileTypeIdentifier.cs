using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FormatKFIV.Utility
{
    public static class FileTypeIdentifier
    {
        /// <summary>File Format Type Enum. Provides an ID for each file format type (image, model etc...)</summary>
        public enum FileFormatType
        {
            Unknown   = 0,
            Database  = 1,
            Ini       = 2,
            Map       = 3,
            Font      = 4,
            MIDI      = 5,
            Wave      = 6,
            Image     = 7,
            Model     = 8,
            Animation = 9,
            Archive   = 10,
        }

        /// <summary>Get the format type of a file using it's extension.</summary>
        /// <param name="filepath">Filepath. Must contain extension.</param>
        /// <returns>The identified format type</returns>
        public static FileFormatType IdentifyByExtension(string filepath)
        {
            //Special case. By full name.
            switch(filepath.ToLower())
            {
                case "rev_para.dat": return FileFormatType.Database;
            }

            //By file extension
            switch(Path.GetExtension(filepath).ToLower())
            {
                case ".prm": return FileFormatType.Database;
                case ".ini": return FileFormatType.Ini;
                case ".map": return FileFormatType.Map;
                case ".tmr": return FileFormatType.Font;
                case ".sq":  return FileFormatType.MIDI;
                case ".hd":  return FileFormatType.Wave;
                case ".bd":  return FileFormatType.Wave;

                case ".tmx":
                case ".tx2":
                case ".tm2":
                    return FileFormatType.Image;

                case ".mi2":
                case ".ico":
                case ".omd":
                case ".om2":
                    return FileFormatType.Model;

                case ".mix":
                    return FileFormatType.Animation;

                case ".mod":
                case ".arm":
                case ".chr":
                    return FileFormatType.Archive;

            }

            return FileFormatType.Unknown;
        }
    }
}
