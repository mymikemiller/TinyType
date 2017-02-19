using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyTypeLib
{
    public struct Line
    {
        public PointF Start { get; private set; }
        public PointF End { get; private set; }

        public double Length
        {
            get
            {
                return Math.Sqrt(Math.Pow(End.X - Start.X, 2) + Math.Pow(End.Y - Start.Y, 2));
            }
        }

        //radians
        public double Angle
        {
            get
            {
                if (Start.X == End.X && Start.Y < End.Y)
                    return Math.PI * 3 / 2;
                else if (Start.X == End.X && Start.Y > End.Y)
                    return Math.PI / 2;
                else if (Start.X < End.X && Start.Y == End.Y)
                    return 0;
                else if (Start.X > End.X && Start.Y == End.Y)
                    return Math.PI; 
                
                return Math.Asin((End.X - Start.Y) / (End.Y - Start.Y));
            }
        }

        public Line(PointF start, PointF end) :
            this()
        {
            Start = start;
            End = end;
        }
        public Line(float x1, float y1, float x2, float y2) :
            this()
        {
            Start = new PointF(x1, y1);
            End = new PointF(x2, y2);
        }

        // Angle is in radians, clockwise
        public Line(PointF start, double angle, double length) :
            this()
        {
            Start = start;

            double diffX = length * Math.Cos(angle);
            double diffY = length * Math.Sin(angle);

            End = new PointF(start.X + (float)diffX, start.Y - (float)diffY);
        }

        public override string ToString()
        {
            return Start.ToString() + " - " + End.ToString();
        }

        public bool IsVertical()
        {
            return Start.X == End.X;
        }

        public bool IsHorizontal()
        {
            return Start.Y == End.Y;
        }

        // Returns a line in the same direction as this line, beginning at Start, in the direction of End, with the given length
        public Line AdjustLength(double length)
        {
            double lengthMultiplier = length / this.Length;

            double diffX = End.X - Start.X;
            double diffY = End.Y - Start.Y;

            double newDiffX = diffX * lengthMultiplier;
            double newDiffY = diffY * lengthMultiplier;

            Line l = new Line(Start, new PointF((float)(Start.X + newDiffX), (float)(Start.Y + newDiffY)));
            
            return l;
        }

        // Gets a line with the given length perpendicular to this line, starting at the this line's end point.
        public Line GetPerpendicular(int length)
        {
            /*
            float x = Start.X + (End.Y - Start.Y);
            float y = Start.Y + (Start.X - End.X);

            return new Line(Start, new PointF(x, y)).AdjustLength(length);
             */
            
            float x = End.X + (Start.Y - End.Y);
            float y = End.Y + (End.X - Start.X);

            return new Line(End, new PointF(x, y)).AdjustLength(length);
            
        }
    }
}
