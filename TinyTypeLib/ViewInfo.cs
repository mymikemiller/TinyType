using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyTypeLib
{
    public enum Position { None = -1, Middle = 0, Q1 = 1, Q2 = 2, Q3 = 3, Q4 = 4 }

    public class ViewInfo
    {
        List<Position> mPath = new List<Position>();

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Left { get { return 0; } }
        public int Top { get { return 0; } }
        public int Right { get { return Width; } }
        public int Bottom { get { return Height; } }
        //public List<Position> Path { get { return mPath; } private set; } // Instead of this, use AddToPath() and ClearPath()

        public Position LatestPathEntry
        {
            get
            {
                return mPath.Count == 0 ? Position.None : mPath[mPath.Count - 1];
            }
        }


        public Rect MiddleSquare
        {
            get
            {
                int middleSquareSize = (int)(Math.Min(Width, Height) * 0.3);
                int left = (int)((Width - middleSquareSize) / 2.0);
                int top = (int)((Height - middleSquareSize) / 2.0);
                return new Rect(left, top, Width - left, Height - top);
            }
        }

        public int PoleLength
        {
            get
            {
                // The ratio is the size of the spokes compared to the diameter of the center circle
                float ratio = 1;
                return (int)(MiddleSquare.Width * ratio);
            }
        }

        public int LargeGradientSize { get { return 40; } } // This should be dependent on the MiddleSquare width
        public int SmallGradientSize { get { return 20; } }

        public Rect GetPositionRect(Position position)
        {
            switch (position)
            {
                case TinyTypeLib.Position.Middle:
                    return MiddleSquare;
                case TinyTypeLib.Position.Q1:
                    return Q1;
                case TinyTypeLib.Position.Q2:
                    return Q2;
                case TinyTypeLib.Position.Q3:
                    return Q3;
                case TinyTypeLib.Position.Q4:
                    return Q4;
                default:
                    return new Rect(0, 0, 0, 0);
            }
        }

        public Rect Q1
        {
            get
            {
                return new Rect((int)(Width / 2.0), 0, Width - 1, (int)(Height / 2.0));
            }
        }
        public Rect Q2
        {
            get
            {
                return new Rect(0, 0, (int)(Width / 2.0) - 1, (int)(Height / 2.0));
            }
        }
        public Rect Q3
        {
            get
            {
                return new Rect(0, (int)(Height / 2.0) + 1, (int)(Width / 2.0) - 1, Height - 1);
            }
        }
        public Rect Q4
        {
            get
            {
                return new Rect((int)(Width / 2.0), (int)(Height / 2.0) + 1, Width - 1, Height - 1);
            }
        }

        /*
        public List<PointF> MiddleSquareBorder
        {
            get
            {
                return new List<PointF>() { 
                    new PointF(MiddleSquare.Right, MiddleSquare.Top),
                    new PointF(MiddleSquare.Left, MiddleSquare.Top),
                    new PointF(MiddleSquare.Left, MiddleSquare.Bottom),
                    new PointF(MiddleSquare.Right, MiddleSquare.Bottom)
                };
            }
        }

        public List<PointF> Q1Border
        {
            get
            {
                return new List<PointF>() { 
                    new PointF(MiddleSquare.Right, Q1.Bottom),
                    new PointF(Q1.Right, Q1.Bottom),
                    new PointF(Q1.Right, Q1.Top),
                    new PointF(Q1.Left, Q1.Top),
                    new PointF(Q1.Left, MiddleSquare.Top),
                    new PointF(MiddleSquare.Right, MiddleSquare.Top)
                };
            }
        }

        public List<PointF> Q2Border
        {
            get
            {
                return new List<PointF>() { 
                    new PointF(Q2.Right, MiddleSquare.Top),
                    new PointF(Q2.Right, Q2.Top),
                    new PointF(Q2.Left, Q2.Top),
                    new PointF(Q2.Left, Q2.Bottom),
                    new PointF(MiddleSquare.Left, Q2.Bottom),
                    new PointF(MiddleSquare.Left, MiddleSquare.Top)
                };
            }
        }

        public List<PointF> Q3Border
        {
            get
            {
                return new List<PointF>() { 
                    new PointF(MiddleSquare.Left, Q3.Top),
                    new PointF(Q3.Left, Q3.Top),
                    new PointF(Q3.Left, Q3.Bottom),
                    new PointF(Q3.Right, Q3.Bottom),
                    new PointF(Q3.Right, MiddleSquare.Bottom),
                    new PointF(MiddleSquare.Left, MiddleSquare.Bottom)
                };
            }
        }

        public List<PointF> Q4Border
        {
            get
            {
                return new List<PointF>() { 
                    new PointF(Q4.Left, MiddleSquare.Bottom),
                    new PointF(Q4.Left, Q4.Bottom),
                    new PointF(Q4.Right, Q4.Bottom),
                    new PointF(Q4.Right, Q4.Top),
                    new PointF(MiddleSquare.Right, Q4.Top),
                    new PointF(MiddleSquare.Right, MiddleSquare.Bottom)
                };
            }
        }
         * */

        public ViewInfo(int width, int height)
        {
            Width = width;
            Height = height;

        }

        public Position GetPosition(int x, int y)
        {
            if (MiddleSquare.Contains(x,y))
            {
                return Position.Middle;
            } else if (Q1.Contains(x,y))
            {
                return Position.Q1;
            } else if (Q2.Contains(x,y))
            {
                return Position.Q2;
            } else if (Q3.Contains(x,y))
            {
                return Position.Q3;
            } else if (Q4.Contains(x,y))
            {
                return Position.Q4;
            }

            return Position.None;
        }


        public Position Position
        {
            get
            {
                if (mPath.Count < 1)
                    return Position.None;

                return mPath[mPath.Count - 1];
            }
        }

        public void AddToPath(Position pos)
        {
            // Add the pos
            mPath.Add(pos);

            // Sanitize the list by removing backtracks
            while (mPath.Count > 2 && mPath[mPath.Count - 1] == mPath[mPath.Count - 3])
            {
                mPath.RemoveRange(mPath.Count - 2, 2);
            }
        }

        public void ClearPath()
        {
            mPath.Clear();
        }


        public Signature CurrentSignature
        {
            get
            {

                

                if (mPath.Count < 3) // Other than special cases, it takes three positions to make a signature: middle, first quadrant, and second quadrant (to establish a direction)
                {
                    // Special case for corners
                    if (mPath.Count == 2 && mPath[0] == TinyTypeLib.Position.Middle && mPath[1] == TinyTypeLib.Position.Q2)
                    {
                        return Signature.TopLeft;
                    }
                    else if (mPath.Count == 2 && mPath[0] == TinyTypeLib.Position.Middle && mPath[1] == TinyTypeLib.Position.Q3)
                    {
                        return Signature.BottomLeft;
                    }
                    else if (mPath.Count == 2 && mPath[0] == TinyTypeLib.Position.Middle && mPath[1] == TinyTypeLib.Position.Q4)
                    {
                        return Signature.BottomRight;
                    }
                    else if (mPath.Count == 2 && mPath[0] == TinyTypeLib.Position.Middle && mPath[1] == TinyTypeLib.Position.Q1)
                    {
                        return Signature.TopRight;
                    }

                    // Special case for things starting outside the middle
                    if (mPath.SequenceEqual(new List<Position> { TinyTypeLib.Position.Q3, TinyTypeLib.Position.Q2 }) ||
                        mPath.SequenceEqual(new List<Position> { TinyTypeLib.Position.Q4, TinyTypeLib.Position.Q1 }))
                    {
                        return Signature.SwipeUp;
                    }
                    else if (mPath.SequenceEqual(new List<Position> { TinyTypeLib.Position.Q2, TinyTypeLib.Position.Q3 }) ||
                        mPath.SequenceEqual(new List<Position> { TinyTypeLib.Position.Q1, TinyTypeLib.Position.Q4 }))
                    {
                        return Signature.SwipeDown;
                    }

                    return Signature.None;
                }

                if (mPath[0] != TinyTypeLib.Position.Middle)
                {
                    return Signature.None;
                }

                int first = (int)mPath[1];
                int second = (int)mPath[2];

                // Adjust for crossing the 0-4 boundary so the direction can be determined by second > first
                if (first == 4 && second == 1)
                    second = 5;
                if (first == 1 && second == 4)
                    second = 0;

                Direction dir = second > first ? Direction.CounterClockwise : Direction.Clockwise;

                int travel = mPath.Count - 2;

                // don't count the final "middle" position as part of the travel. 
                if (mPath[mPath.Count - 1] == TinyTypeLib.Position.Middle)
                {
                    travel--;
                }
                    
                return GetFinalSignature(first, dir, travel);

            }
        }

        private Signature GetFinalSignature(int startQuadrant, Direction direction, int travel)
        {
            int completeCircles = (travel - 1) / 4;
            bool flipDirection = completeCircles % 2 == 1;

            // after 1 complete circle, move 1 quadrant, then move 1 quadrant after every 2 complete circles
            int quadrantsToMove = completeCircles == 0 ? 0 : 1 + ((completeCircles - 1) / 2);
            int newStartQuadrant = Heading.GetQuadrantInDirection(startQuadrant, direction, quadrantsToMove);
            Direction newDirection = flipDirection ? (direction == Direction.Clockwise ? Direction.CounterClockwise : Direction.Clockwise) : direction;

            /*
            int newTravel = travel;
            while(newTravel > 4)
            {
                newTravel -= 4;
            }*/
            int newTravel = ((travel - 1) % 4) + 1;
            newTravel = flipDirection ? 5 - newTravel : newTravel;


            return new Signature(newStartQuadrant, newDirection, newTravel);
        }
    }
}
