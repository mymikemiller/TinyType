using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyTypeLib
{
    public struct PointF
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public int XInt { get { return (int)X; } }
        public int YInt { get { return (int)Y; } }

        public PointF(float x, float y) :
            this()
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ")";
        }
    }
}
