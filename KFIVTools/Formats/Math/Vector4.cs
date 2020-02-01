namespace KFIV.Utility.Math
{
    public class Vector4
    {
        #region Data
        public float x;
        public float y;
        public float z;
        public float w;

        #endregion

        public Vector3 AsVec3
        {
            get
            {
                return new Vector3(x, y, z);
            }
        }
        public static Vector4 Zero
        {
            get { return new Vector4(0f, 0f, 0f, 0f); }
        }
        public static Vector4 One
        {
            get { return new Vector4(1f, 1f, 1f, 1f); }
        }

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public static Vector4 Subtract(Vector4 a, Vector4 b)
        {
            Vector4 c = Vector4.Zero;

            c.x = a.x - b.x;
            c.y = a.y - b.y;
            c.z = a.z - b.z;
            c.w = a.w - b.w;

            return c;
        }

    }
}
