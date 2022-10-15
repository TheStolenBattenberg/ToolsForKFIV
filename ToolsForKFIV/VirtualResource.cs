using System;
using System.Collections.Generic;
using System.Text;

namespace ToolsForKFIV
{
    public class VirtualResource : Resource
    {
        //Data
        private string _virtualPath;
        private byte[] _buffer;

        //Accessors
        public string RelativePath
        {
            get { return _virtualPath; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
        }

        //Constructor
        public VirtualResource(string path, byte[] buffer)
        {
            _virtualPath = path;
            _buffer = buffer;
        }
    }
}
