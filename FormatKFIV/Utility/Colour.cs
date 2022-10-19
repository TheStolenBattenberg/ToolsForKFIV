using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace FormatKFIV.Utility
{
    /// <summary>Colour class stores a colour. Pretty obvious right?</summary>
    public struct Colour
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public static Colour FromARGB(byte A, byte R, byte G, byte B)
        {
            Colour outC = new Colour();
            outC.R = R;
            outC.G = G;
            outC.B = B;
            outC.A = A;
            return outC;
        }
        public static Colour FromColor(Color c)
        {
            Colour outC = new Colour();
            outC.R = c.R;
            outC.G = c.G;
            outC.B = c.B;
            outC.A = c.A;
            return outC;
        }
        
        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }
}
