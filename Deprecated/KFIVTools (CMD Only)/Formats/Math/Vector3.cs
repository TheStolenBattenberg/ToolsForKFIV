using System;

namespace KFIV.Utility.Maths
{

    public class Vector3
    {
        public float x;
        public float y;
        public float z;

        public static Vector3 Zero
        {
            get
            {
                return new Vector3(0f, 0f, 0f);
            }
        }
        public static Vector3 One
        {
            get
            {
                return new Vector3(1f, 1f, 1f);
            }
        }

        public Vector4 AsQuaternion
        {
            get
            {
                double qx, qy, qz, qw;

                qx = Math.Sin(z / 2) * Math.Cos(y / 2) * Math.Cos(x / 2) - Math.Cos(z / 2) * Math.Sin(y / 2) * Math.Sin(x / 2);
                qy = Math.Cos(z / 2) * Math.Sin(y / 2) * Math.Cos(x / 2) + Math.Sin(z / 2) * Math.Cos(y / 2) * Math.Sin(x / 2);
                qz = Math.Cos(z / 2) * Math.Cos(y / 2) * Math.Sin(x / 2) - Math.Sin(z / 2) * Math.Sin(y / 2) * Math.Cos(x / 2);
                qw = Math.Cos(z / 2) * Math.Cos(y / 2) * Math.Cos(x / 2) + Math.Sin(z / 2) * Math.Sin(y / 2) * Math.Sin(x / 2);

                return new Vector4((float)qx, (float)qy, (float)qz, (float)qw);
            }
        }


        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vector3 Subtract(Vector3 a, Vector3 b)
        {
            Vector3 vec = Zero;

            //Subtract B from A
            vec.x = a.x - b.x;
            vec.y = a.y - b.y;
            vec.z = a.z - b.z;

            return vec;
        }
        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            Vector3 vec = Zero;

            //Cross Product Calculation
            vec.x = a.y * b.z - a.z * b.y;
            vec.y = a.z * b.x - a.x * b.z;
            vec.z = a.x * b.y - a.y * b.x;

            return vec;
        }
        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
    }
}
