using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyTypeLib
{
    public enum Direction { None, CounterClockwise, Clockwise }

    public struct Heading
    {
        public int Quadrant { get; private set; }
        public Direction Direction { get; private set; }

        public Heading(int quadrant, Direction direction)
            : this()
        {
            Quadrant = quadrant;
            Direction = direction;
        }

        public static int GetQuadrantOpposite(int quadrant)
        {
            return GetQuadrantInDirection(quadrant, Direction.Clockwise, 1);
        }
        public static int GetQuadrantAdjacent(int quadrant, Direction direction)
        {
            return GetQuadrantInDirection(quadrant, direction, 1);
        }
        public static int GetQuadrantInDirection(int quadrant, Direction direction, int distance)
        {
            // Use a Signature to figure this out
            Signature s = new Signature(new Heading(quadrant, direction), distance);
            return s.EndQuadrant;
        }
        public static bool operator ==(Heading a, Heading b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Quadrant == b.Quadrant &&
                a.Direction == b.Direction;
        }

        public static bool operator !=(Heading a, Heading b)
        {
            return !(a == b);
        }
    }
}
