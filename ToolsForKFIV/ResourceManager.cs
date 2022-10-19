using System;
using System.Collections.Generic;

using FormatKFIV.Asset;
using FormatKFIV.FileFormat;

namespace ToolsForKFIV
{
    /// <summary>Static storage location for all currently loaded/allocated data.</summary>
    public static class ResourceManager
    {
        /// <summary>Store Format Handelers</summary>
        private static List<FIFormat<Model>> formatsModel;
        private static List<FIFormat<Texture>> formatsTexture;
        private static List<FIFormat<Scene>> formatsScene;
        private static List<FIFormat<Param>> formatsParam;

        /// <summary>Stores data files</summary>
        public static VirtualFileSystem vfs = new VirtualFileSystem();

        ///<summary>Stores pretty names</summary>
        public static FFPrettyNames PrettyNamesData = null;

        ///<summary>Stores settings</summary>
        public static Settings settings = null;

        ///<summary>Stores path to exe</summary>
        public static string ProgramDirectory;

        /// <summary>Reference to the main window</summary>
        public static MainWindow winMain;

        /// <summary>Initializes the resource manager. Must be called before any other call to it.</summary>
        /// <param name="programDir">Directory of the exe file.</param>
        public static void Initialize(string programDir)
        {
            ProgramDirectory = programDir;

            //Initialize PrettyNames class
            PrettyNamesData = new FFPrettyNames();
            PrettyNamesData.Initialize();
            PrettyNamesData.LoadPrettyNames(ProgramDirectory + "Resource\\prettynames.csv");

            //Load Settings
            settings = Settings.LoadConfiguration();

            //Initialize Model Handlers
            formatsModel = new List<FIFormat<Model>>();
            formatsModel.Add(new FFModelICO());
            formatsModel.Add(new FFModelOMD());
            formatsModel.Add(new FFModelOM2());
            formatsModel.Add(new FFModelMOD());
            formatsModel.Add(new FFModelCHR());

            formatsModel.Add(new FFModelOBJ());

            //Initialize Texture Handlers
            formatsTexture = new List<FIFormat<Texture>>();
            formatsTexture.Add(new FFTextureTX2());
            formatsTexture.Add(new FFTextureTM2());
            formatsTexture.Add(new FFTextureTMX());

            formatsTexture.Add(new FFTextureTGA());
            formatsTexture.Add(new FFTexturePNG());

            //Initialize Scene Handlers
            formatsScene = new List<FIFormat<Scene>>();
            formatsScene.Add(new FFSceneMAP());

            //Initialize Param Handlers
            formatsParam = new List<FIFormat<Param>>();
            formatsParam.Add(new FFParamReverb());
            formatsParam.Add(new FFParamItemName());
            formatsParam.Add(new FFParamWeapon());
            formatsParam.Add(new FFParamMagic());
        }

        /// <summary>Scan through each registered format to see if a particular one is supported</summary>
        /// <param name="fileExt">The extension of the file to scan</param>
        /// <param name="fileBuffer">Data buffer of the file to scan</param>
        /// <param name="type">(OUT) The returning type of the format, or FEType.None</param>
        /// <param name="handler">(OUT) The returning handler of the format, or null</param>
        /// <returns>True if the format is supported, false otherwise</returns>
        public static bool FormatIsSupported(string fileExt, byte[] fileBuffer, out FEType type, out object handler)
        {
            object formatHandler = null;
            FEType formatType = FEType.None;

            //Scan each model format
            foreach(FIFormat<Model> fmt in formatsModel)
            {
                //Check all extensions this particular format supports...
                string[] extensions = fmt.Parameters.Extensions;
                for (int i = 0; i < extensions.Length; ++i)
                {
                    //First Check - Extension (both with string lower for sanity reasons)
                    if (extensions[i].ToLower() != fileExt.ToLower())
                    {
                        continue;
                    }

                    //Second Check - Validator Delegate call
                    if (!fmt.Parameters.Validator(fileBuffer))
                    {
                        continue;
                    }

                    Logger.LogInfo("Found handler for: " + fileExt);
                    Logger.LogInfo("Handler Class Name: " + fmt.GetType().Name);

                    formatHandler = fmt;
                    formatType = FEType.Model;

                    goto FoundFormatHandler;
                }
            }

            //Scan each texture format
            foreach(FIFormat<Texture> fmt in formatsTexture)
            {
                //Check all extensions this particular format supports...
                string[] extensions = fmt.Parameters.Extensions;
                for (int i = 0; i < extensions.Length; ++i)
                {
                    //First Check - Extension (both with string lower for sanity reasons)
                    if (extensions[i].ToLower() != fileExt.ToLower())
                    {
                        continue;
                    }

                    //Second Check - Validator Delegate call
                    if (!fmt.Parameters.Validator(fileBuffer))
                    {
                        continue;
                    }

                    Logger.LogInfo("Found handler for: " + fileExt);
                    Logger.LogInfo("Handler Class Name: " + fmt.GetType().Name);

                    formatHandler = fmt;
                    formatType = FEType.Texture;

                    goto FoundFormatHandler;
                }
            }

            //Scan each scene format
            foreach (FIFormat<Scene> fmt in formatsScene)
            {
                //Check all extensions this particular format supports...
                string[] extensions = fmt.Parameters.Extensions;
                for (int i = 0; i < extensions.Length; ++i)
                {
                    //First Check - Extension (both with string lower for sanity reasons)
                    if (extensions[i].ToLower() != fileExt.ToLower())
                    {
                        continue;
                    }

                    //Second Check - Validator Delegate call
                    if (!fmt.Parameters.Validator(fileBuffer))
                    {
                        continue;
                    }

                    Logger.LogInfo("Found handler for: " + fileExt);
                    Logger.LogInfo("Handler Class Name: " + fmt.GetType().Name);

                    formatHandler = fmt;
                    formatType = FEType.Scene;

                    goto FoundFormatHandler;
                }
            }

            //Scan each param format
            foreach (FIFormat<Param> fmt in formatsParam)
            {
                //Check all extensions this particular format supports...
                string[] extensions = fmt.Parameters.Extensions;
                for (int i = 0; i < extensions.Length; ++i)
                {
                    //First Check - Extension (both with string lower for sanity reasons)
                    if (extensions[i].ToLower() != fileExt.ToLower())
                    {
                        continue;
                    }

                    //Second Check - Validator Delegate call
                    if (!fmt.Parameters.Validator(fileBuffer))
                    {
                        continue;
                    }

                    Logger.LogInfo("Found handler for: " + fileExt);
                    Logger.LogInfo("Handler Class Name: " + fmt.GetType().Name);

                    formatHandler = fmt;
                    formatType = FEType.Param;

                    goto FoundFormatHandler;
                }
            }

            goto NoFoundFormatHandler;  //Exclusively here to piss you off.
            NoFoundFormatHandler:       //^^
            Logger.LogWarn("Couldn't find a format handler for: " + fileExt);

            type = formatType;
            handler = formatHandler;
            return false;

            FoundFormatHandler:         //Don't get lost in the gotos. You get send here from the loops to avoid redudent code.
            type = formatType;
            handler = formatHandler;
            return true;
        }

        public static FIFormat<Texture>[] GetExportableTextureFormats()
        {
            List<FIFormat<Texture>> formats = new List<FIFormat<Texture>>();

            //Scan for exportable texture formats
            foreach (FIFormat<Texture> fmt in formatsTexture)
            {
                if (!fmt.Parameters.AllowExport)
                    continue;
                formats.Add(fmt);
            }

            //Conversion to array and cleanup
            FIFormat<Texture>[] formatsArray = formats.ToArray();
            formats.Clear();

            return formatsArray;
        }
        public static FIFormat<Model>[] GetExportableModelFormats()
        {
            List<FIFormat<Model>> formats = new List<FIFormat<Model>>();

            //Scan for exportable texture formats
            foreach (FIFormat<Model> fmt in formatsModel)
            {
                if (!fmt.Parameters.AllowExport)
                    continue;
                formats.Add(fmt);
            }

            //Conversion to array and cleanup
            FIFormat<Model>[] formatsArray = formats.ToArray();
            formats.Clear();

            return formatsArray;
        }
    }
}
