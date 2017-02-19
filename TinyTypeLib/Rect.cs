using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyTypeLib
{
    public class Rect
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Right { get; private set; }
        public int Bottom { get; private set; }

        public int Width { get { return Right - Left; } }
        public int Height { get { return Bottom - Top; } }
        public PointF Center { get { return new PointF(Left + Width / 2f, Top + Height / 2f); } }


        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public bool Contains(int x, int y)
        {
            return x >= Left && x <= Right && y >= Top && y <= Bottom;
        }

        public Rect Offset(int x, int y)
        {
            return new Rect(Left + x, Top + y, Right + x, Bottom + y);
        }
    }
}
