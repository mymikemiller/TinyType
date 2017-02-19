using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyTypeLib
{
    public struct Signature
    {
        public enum Gestures { Normal, None, SwipeUp, SwipeDown, SwipeLeft, SwipeRight };
        public Gestures Gesture { get; private set; }
        public static Signature None { get { Signature sig = new Signature(); sig.Gesture = Gestures.None; return sig; } }
        public static Signature SwipeUp { get { Signature sig = new Signature(); sig.Gesture = Gestures.SwipeUp; return sig; } }
        public static Signature SwipeDown { get { Signature sig = new Signature(); sig.Gesture = Gestures.SwipeDown; return sig; } }
        public static Signature SwipeLeft { get { Signature sig = new Signature(); sig.Gesture = Gestures.SwipeLeft; return sig; } }
        public static Signature SwipeRight { get { Signature sig = new Signature(); sig.Gesture = Gestures.SwipeRight; return sig; } }

        public static Signature TopLeft { get { return new Signature(2, Direction.None, 0); } }
        public static Signature BottomLeft { get { return new Signature(3, Direction.None, 0); } }
        public static Signature TopRight { get { return new Signature(1, Direction.None, 0); } }
        public static Signature BottomRight { get { return new Signature(4, Direction.None, 0); } }

        public Heading StartHeading { get; private set; }
        public Direction Direction { get { return StartHeading.Direction; } }
        public int StartQuadrant { get { return StartHeading.Quadrant; } }
        public int Travel { get; private set; }
        public int EndQuadrant
        {
            get
            {
                int sign = Direction == Direction.Clockwise ? -1 : 1;
                int endQuadrant = StartQuadrant + (Travel * sign);
                while (endQuadrant < 1)
                {
                    endQuadrant += 4;
                }
                while (endQuadrant > 4)
                {
                    endQuadrant -= 4;
                }
                return endQuadrant;
            }
        }
        public Heading EndHeading { get { return new Heading(EndQuadrant, Direction); } }

        public Signature(Heading heading, int travel)
            : this()
        {
            StartHeading = heading;
            Travel = travel;
            Gesture = Gestures.Normal;
        }
        public Signature(int startQuadrant, Direction direction, int travel)
            : this()
        {
            StartHeading = new Heading(startQuadrant, direction);
            Travel = travel;
            Gesture = Gestures.Normal;
        }

        public override string ToString()
        {
            return "Q" + StartQuadrant + ", " + Travel + " " + Direction.ToString();
        }


        public static bool operator ==(Signature a, Signature b)
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
            bool val = a.Gesture == b.Gesture &&
                (a.Gesture == Gestures.Normal ?
                (a.Direction == b.Direction &&
                a.StartHeading == b.StartHeading &&
                a.Travel == b.Travel) : true);

            return val;
        }

        public static bool operator !=(Signature a, Signature b)
        {
            return !(a == b);
        }

    }
}
