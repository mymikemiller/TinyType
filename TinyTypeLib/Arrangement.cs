using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyTypeLib
{
    public class Arrangement
    {
        public Dictionary<Signature, InputAction> Layout { get; private set; }

        public Arrangement(string orderedCharacters)
        {
            if (orderedCharacters.Length != 32)
            {
                throw new Exception("32 characters are necessary for an arrangement. You specified " + orderedCharacters.Length);
            }
            Layout = new Dictionary<Signature, InputAction>();

            Signature signature = new Signature(1, Direction.Clockwise, 1);

            for (int i = 0; i < orderedCharacters.Length; ++i)
            {
                char chr = orderedCharacters[i];

                InputAction inputAction = new InputAction(chr);
                
                // Some special cases
                if (chr.Equals('«'))
                {
                    inputAction.Type = InputAction.ActionType.Backspace;
                }
                else if (chr.Equals('ʬ'))
                {
                    inputAction.Type = InputAction.ActionType.DeleteWord;
                }

                Layout.Add(signature, inputAction);

                int nextStartQuadrant = signature.StartQuadrant;
                Direction nextDirection = signature.Direction;
                int nextTravel = signature.Travel;

                nextDirection = signature.Direction == Direction.Clockwise ? Direction.CounterClockwise : Direction.Clockwise;

                if ((i + 1) % 8 == 0)
                {
                    nextTravel++;
                }
                if ((i + 1) % 2 == 0)
                {
                    nextStartQuadrant = signature.StartQuadrant == 4 ? 1 : nextStartQuadrant + 1;
                }

                signature = new Signature(nextStartQuadrant, nextDirection, nextTravel);
            }

            // Add special signatures
            Layout.Add(Signature.TopLeft, new InputAction());
            Layout.Add(Signature.TopRight, new InputAction());//InputAction.ActionType.ToggleArrangementMode));
            Layout.Add(Signature.BottomLeft, new InputAction());//InputAction.ActionType.Lock));
            Layout.Add(Signature.BottomRight, new InputAction());
        }

        public Arrangement()
        {
            Layout = new Dictionary<Signature, InputAction>();
        }

        public override string ToString()
        {
            return GetLayoutString();
        }
        public string GetLayoutString()
        {
            string layoutString = "";
            Signature signature = new Signature(1, Direction.Clockwise, 1);

            for (int i = 0; i < 32; ++i)
            {
                layoutString += Layout[signature].Character;

                int nextStartQuadrant = signature.StartQuadrant;
                Direction nextDirection = signature.Direction;
                int nextTravel = signature.Travel;

                nextDirection = signature.Direction == Direction.Clockwise ? Direction.CounterClockwise : Direction.Clockwise;

                if ((i + 1) % 8 == 0)
                {
                    nextTravel++;
                }
                if ((i + 1) % 2 == 0)
                {
                    nextStartQuadrant = signature.StartQuadrant == 4 ? 1 : nextStartQuadrant + 1;
                }

                signature = new Signature(nextStartQuadrant, nextDirection, nextTravel);
            }

            return layoutString;
        }

        /*
        public Signature GetSignature(char chr)
        {
            return Layout.First(l => l.Value == chr).Key;
        }*/

        public string GetPlusRepresentation()
        {
            string rep = "";

            // Top pole
            rep += "       " + GetAction(new Signature(2, Direction.Clockwise, 4)).Character.ToString();
            rep += " " + GetAction(new Signature(1, Direction.CounterClockwise, 4)).Character.ToString() + "\n";
            rep += "       " + GetAction(new Signature(2, Direction.Clockwise, 3)).Character.ToString();
            rep += " " + GetAction(new Signature(1, Direction.CounterClockwise, 3)).Character.ToString() + "\n";
            rep += "       " + GetAction(new Signature(2, Direction.Clockwise, 2)).Character.ToString();
            rep += " " + GetAction(new Signature(1, Direction.CounterClockwise, 2)).Character.ToString() + "\n";
            rep += "       " + GetAction(new Signature(2, Direction.Clockwise, 1)).Character.ToString();
            rep += " " + GetAction(new Signature(1, Direction.CounterClockwise, 1)).Character.ToString() + "\n";

            // Left and right poles
            rep += GetAction(new Signature(2, Direction.CounterClockwise, 4)).Character.ToString() + " ";
            rep += GetAction(new Signature(2, Direction.CounterClockwise, 3)).Character.ToString() + " ";
            rep += GetAction(new Signature(2, Direction.CounterClockwise, 2)).Character.ToString() + " ";
            rep += GetAction(new Signature(2, Direction.CounterClockwise, 1)).Character.ToString() + "   ";
            rep += GetAction(new Signature(1, Direction.Clockwise, 1)).Character.ToString() + " ";
            rep += GetAction(new Signature(1, Direction.Clockwise, 2)).Character.ToString() + " ";
            rep += GetAction(new Signature(1, Direction.Clockwise, 3)).Character.ToString() + " ";
            rep += GetAction(new Signature(1, Direction.Clockwise, 4)).Character.ToString() + "\n";

            rep += GetAction(new Signature(3, Direction.Clockwise, 4)).Character.ToString() + " ";
            rep += GetAction(new Signature(3, Direction.Clockwise, 3)).Character.ToString() + " ";
            rep += GetAction(new Signature(3, Direction.Clockwise, 2)).Character.ToString() + " ";
            rep += GetAction(new Signature(3, Direction.Clockwise, 1)).Character.ToString() + "   ";
            rep += GetAction(new Signature(4, Direction.CounterClockwise, 1)).Character.ToString() + " ";
            rep += GetAction(new Signature(4, Direction.CounterClockwise, 2)).Character.ToString() + " ";
            rep += GetAction(new Signature(4, Direction.CounterClockwise, 3)).Character.ToString() + " ";
            rep += GetAction(new Signature(4, Direction.CounterClockwise, 4)).Character.ToString() + "\n";

            // Bottom pole
            rep += "       " + GetAction(new Signature(3, Direction.CounterClockwise, 1)).Character.ToString();
            rep += " " + GetAction(new Signature(4, Direction.Clockwise, 1)).Character.ToString() + "\n";
            rep += "       " + GetAction(new Signature(3, Direction.CounterClockwise, 2)).Character.ToString();
            rep += " " + GetAction(new Signature(4, Direction.Clockwise, 2)).Character.ToString() + "\n";
            rep += "       " + GetAction(new Signature(3, Direction.CounterClockwise, 3)).Character.ToString();
            rep += " " + GetAction(new Signature(4, Direction.Clockwise, 3)).Character.ToString() + "\n";
            rep += "       " + GetAction(new Signature(3, Direction.CounterClockwise, 4)).Character.ToString();
            rep += " " + GetAction(new Signature(4, Direction.Clockwise, 4)).Character.ToString() + "\n";

            return rep;
        }

        public InputAction GetAction(Signature signature, InputAction defaultAction = new InputAction())
        {
            switch (signature.Gesture)
            {
                case Signature.Gestures.SwipeUp:
                    return new InputAction(InputAction.ActionType.Upper);
                case Signature.Gestures.SwipeDown:
                    return new InputAction(InputAction.ActionType.Lower);
                case Signature.Gestures.SwipeLeft:
                    return new InputAction(InputAction.ActionType.Backspace);
                case Signature.Gestures.SwipeRight:
                    return new InputAction(InputAction.ActionType.Symbols);
                default:
                    if (Layout.ContainsKey(signature))
                    {
                        return Layout[signature];
                    }
                    else
                    {
                        return defaultAction;
                    }
            }

            

        }

        public string GetSquareRepresentation()
        {
            string rep = "";

            // Top line
            rep += " ";
            rep += GetAction(new Signature(2, Direction.Clockwise, 1)).Character.ToString() + " ";
            rep += GetAction(new Signature(2, Direction.Clockwise, 2)).Character.ToString() + " ";
            rep += GetAction(new Signature(2, Direction.Clockwise, 3)).Character.ToString() + " ";
            rep += GetAction(new Signature(2, Direction.Clockwise, 4)).Character.ToString() + " ";
            rep += GetAction(new Signature(1, Direction.CounterClockwise, 4)).Character.ToString() + " ";
            rep += GetAction(new Signature(1, Direction.CounterClockwise, 3)).Character.ToString() + " ";
            rep += GetAction(new Signature(1, Direction.CounterClockwise, 2)).Character.ToString() + " ";
            rep += GetAction(new Signature(1, Direction.CounterClockwise, 1)).Character.ToString() + " ";
            rep += " " + "\n";

            // Left and right sides
            rep += GetAction(new Signature(2, Direction.CounterClockwise, 1)).Character.ToString() + "               ";
            rep += GetAction(new Signature(1, Direction.Clockwise, 1)).Character.ToString() + "\n";
            rep += GetAction(new Signature(2, Direction.CounterClockwise, 2)).Character.ToString() + "               ";
            rep += GetAction(new Signature(1, Direction.Clockwise, 2)).Character.ToString() + "\n";
            rep += GetAction(new Signature(2, Direction.CounterClockwise, 3)).Character.ToString() + "               ";
            rep += GetAction(new Signature(1, Direction.Clockwise, 3)).Character.ToString() + "\n";
            rep += GetAction(new Signature(2, Direction.CounterClockwise, 4)).Character.ToString() + "      _|_      ";
            rep += GetAction(new Signature(1, Direction.Clockwise, 4)).Character.ToString() + "\n";

            rep += GetAction(new Signature(3, Direction.Clockwise, 4)).Character.ToString() + "       |       ";
            rep += GetAction(new Signature(4, Direction.CounterClockwise, 4)).Character.ToString() + "\n";
            rep += GetAction(new Signature(3, Direction.Clockwise, 3)).Character.ToString() + "               ";
            rep += GetAction(new Signature(4, Direction.CounterClockwise, 3)).Character.ToString() + "\n";
            rep += GetAction(new Signature(3, Direction.Clockwise, 2)).Character.ToString() + "               ";
            rep += GetAction(new Signature(4, Direction.CounterClockwise, 2)).Character.ToString() + "\n";
            rep += GetAction(new Signature(3, Direction.Clockwise, 1)).Character.ToString() + "               ";
            rep += GetAction(new Signature(4, Direction.CounterClockwise, 1)).Character.ToString() + "\n";

            // Bottom line
            rep += " ";
            rep += GetAction(new Signature(3, Direction.CounterClockwise, 1)).Character.ToString() + " ";
            rep += GetAction(new Signature(3, Direction.CounterClockwise, 2)).Character.ToString() + " ";
            rep += GetAction(new Signature(3, Direction.CounterClockwise, 3)).Character.ToString() + " ";
            rep += GetAction(new Signature(3, Direction.CounterClockwise, 4)).Character.ToString() + " ";
            rep += GetAction(new Signature(4, Direction.Clockwise, 4)).Character.ToString() + " ";
            rep += GetAction(new Signature(4, Direction.Clockwise, 3)).Character.ToString() + " ";
            rep += GetAction(new Signature(4, Direction.Clockwise, 2)).Character.ToString() + " ";
            rep += GetAction(new Signature(4, Direction.Clockwise, 1)).Character.ToString() + " ";
            rep += " ";
            return rep;
        }
    }
}
