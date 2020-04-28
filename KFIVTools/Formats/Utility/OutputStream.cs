using System.IO;

using KFIV.Utility.Math;

namespace KFIV.Utility.IO
{
    public class OutputStream : BinaryWriter
    {
        public OutputStream(string path) : base(File.Open(path, FileMode.Create))
        { }

        public static void WriteFile(string path, byte[] bytes)
        {
            using(OutputStream os = new OutputStream(path))
            {
                os.Write(bytes);
            }
        }

        public void WriteVector4(Vector4 vec4)
        {
            base.Write(vec4.x);
            base.Write(vec4.y);
            base.Write(vec4.z);
            base.Write(vec4.w);
        }
    }
}
