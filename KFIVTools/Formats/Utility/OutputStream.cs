using System.IO;

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
    }
}
