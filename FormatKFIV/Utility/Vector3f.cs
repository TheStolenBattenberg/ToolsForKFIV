using System;
using System.Collections.Generic;
using System.Text;

namespace FormatKFIV.Utility
{
    public class Vector3f
    {
        //Properties
        public Vector3f Zero
        {
            get { return _Zero; }
        }
        public float X
        {
            get { return _x; }
        }
        public float Y
        {
            get { return _y; }
        }
        public float Z
        {
            get { return _z; }
        }

        //Members
        private static Vector3f _Zero = new Vector3f(0, 0, 0);
        private float _x, _y, _z;

        public Vector3f(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public static Vector3f Subtract(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Vector3f Cross(Vector3f v1, Vector3f v2)
        {
            float CX = v1.Y * v2.Z - v1.Z * v2.Y;
            float CY = v1.Z * v2.X - v1.X * v2.Z;
            float CZ = v1.X * v2.Y - v1.Y * v2.X;

            return new Vector3f(CX, CY, CZ);
        }
        public static Vector3f Normalize(Vector3f v)
        {
            float length = Magnitude(v);

            return new Vector3f(v.X / length, v.Y / length, v.Z / length);
        }
        public static float Magnitude(Vector3f v)
        {
            return MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        }
        public static float Dot(Vector3f v1, Vector3f v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
        public static Vector3f Average(params Vector3f[] vlist)
        {
            float Xaccum = 0, Yaccum = 0, Zaccum = 0;

            foreach(Vector3f v in vlist)
            {
                Xaccum += v.X;
                Yaccum += v.Y;
                Zaccum += v.Z;
            }

            return new Vector3f(Xaccum / vlist.Length, Yaccum / vlist.Length, Zaccum / vlist.Length);
        }

        public override string ToString()
        {
            return $"[x: {X}, y: {Y}, z: {Z}]";
        }
    }
}
