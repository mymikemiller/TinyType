using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyTypeLib
{
    public static class Util
    {
        public static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
