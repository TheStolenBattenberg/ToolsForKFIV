using System;
using System.IO;
namespace ToolsForKFIV
{
    public class SystemResource : Resource
    {
        //Data
        private string _virtualPath;
        private string _osPath;

        //Accessors
        public string RelativePath
        {
            get
            {
                return _virtualPath;
            }
        }
        public string Path
        {
            get
            {
                return _osPath;
            }
        }

        //Constructor
        public SystemResource(string virtualPath, string osPath)
        {
            _virtualPath = virtualPath;
            _osPath = osPath;
        }

        public bool GetBuffer(out byte[] buffer)
        {
            if(!File.Exists(_osPath))
            {
                Logger.LogError($"Bad File Path: {_osPath}!!");
                buffer = null;
                return false;
            }

            Logger.LogInfo($"SystemResource is being retrieved from {_osPath}");

            byte[] fileBuffer;
            using (BinaryReader binr = new BinaryReader(File.OpenRead(_osPath)))
            {
                fileBuffer = binr.ReadBytes((int)binr.BaseStream.Length);
            }

            buffer = fileBuffer;
            return true;
        }
    }
}
