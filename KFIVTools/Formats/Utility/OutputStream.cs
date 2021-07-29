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

        public long Tell()
        {
            return base.BaseStream.Position;
        }

        public void WriteVector4(Vector4 vec4)
        {
            base.Write(vec4.x);
            base.Write(vec4.y);
            base.Write(vec4.z);
            base.Write(vec4.w);
        }
        public void WriteVector3(Vector3 vec3)
        {
            base.Write(vec3.x);
            base.Write(vec3.y);
            base.Write(vec3.z);
        }
        public void WriteVector2(Vector2 vec2)
        {
            base.Write(vec2.x);
            base.Write(vec2.y);
        }
    }
}
