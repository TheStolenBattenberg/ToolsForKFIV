using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using STA.Format.STZ;

namespace STZUnpack
{
    class Program
    {
        static void Main(string[] args)
        {
            STZ stz = new STZ(args[0]);
        }
    }
}
