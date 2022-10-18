using System;
using System.Collections.Generic;
using System.Text;

namespace FormatKFIV.Utility
{
    public class Vector4f
    {
        //Properties
        public Vector4f Zero
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
        public float W
        {
            get { return _w; }
        }

        //Members
        private static Vector4f _Zero = new Vector4f(0, 0, 0, 0);
        private float _x, _y, _z, _w;


        public Vector3f ToVector3()
        {
            return new Vector3f(X, Y, Z);
        }

        public Vector4f(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
    }
}
