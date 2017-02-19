using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using TinyTypeLib;
using AndroidLib;

namespace TinyTypeAndroid
{
    public class ActionChosenEventArgs : EventArgs
    {
        public InputAction Action { get; set; }
    }

    public class InputView : View, View.IOnTouchListener
    {
        public event EventHandler<ActionChosenEventArgs> ActionChosen;
        ViewInfo mViewInfo;

        Arrangement mUpperCaseArrangement = new Arrangement();
        Arrangement mLowerCaseArrangement = new Arrangement("ietar «ondhumslybc'gpwkfjqzvx!,?");

        Arrangement mNumbersAndSymbolsArrangement = new Arrangement("1234567890@#$%^&*(){}-.=+~\"/:;<>");
        
        enum ArrangementMode { LowerCase, UpperCase, NumbersAndSymbols }
        ArrangementMode mMode;
        ArrangementMode Mode
        {
            get
            {
                return mMode;
            }
            set
            {
                mMode = value;
            }
        }

        bool mLockMode = false;
        bool LockMode { 
            get
            {
                return mLockMode;
            }
            set 
            {
                mLockMode = value;
            }
        }

        char mPreviousCharacter = char.MinValue;

        static Dictionary<KeyValuePair<int, int>, Signature> sSignatureLocations;

        public Arrangement Arrangement
        {
            get
            {
                switch (mMode)
                {
                    case ArrangementMode.UpperCase:
                        return mUpperCaseArrangement;
                    case ArrangementMode.NumbersAndSymbols:
                        return mNumbersAndSymbolsArrangement;
                    default:
                        return mLowerCaseArrangement;
                }
            }
        }

        public bool DisplayPoles
        {
            get { return false; }
        }

        public InputView(Context context) :
            base(context)
        {
            Initialize();
        }
        public InputView(Context context, IAttributeSet attributeSet) :
            base(context)
        {
            Initialize();
        }

        private void Initialize()
        {
            mMode = ArrangementMode.UpperCase;

            mUpperCaseArrangement = new Arrangement(mLowerCaseArrangement.GetLayoutString().ToUpper().Replace('«', 'ʬ'));

            // The DisplayMetrics method uses the size of the full screen, but we need to adjust for the notification bar so we instead set the size in OnSizeChanged. Width and Height aren't available in Initialize()
            //DisplayMetrics dm = Resources.DisplayMetrics;
            //mViewInfo = new ViewInfo(dm.WidthPixels, dm.HeightPixels);
            

            sSignatureLocations = new Dictionary<KeyValuePair<int, int>, Signature>();
            // Top
            sSignatureLocations.Add(new KeyValuePair<int, int>(1, 0), new Signature(2, Direction.Clockwise, 1));
            sSignatureLocations.Add(new KeyValuePair<int, int>(2, 0), new Signature(2, Direction.Clockwise, 2));
            sSignatureLocations.Add(new KeyValuePair<int, int>(3, 0), new Signature(2, Direction.Clockwise, 3));
            sSignatureLocations.Add(new KeyValuePair<int, int>(4, 0), new Signature(2, Direction.Clockwise, 4));
            sSignatureLocations.Add(new KeyValuePair<int, int>(6, 0), new Signature(1, Direction.CounterClockwise, 4));
            sSignatureLocations.Add(new KeyValuePair<int, int>(7, 0), new Signature(1, Direction.CounterClockwise, 3));
            sSignatureLocations.Add(new KeyValuePair<int, int>(8, 0), new Signature(1, Direction.CounterClockwise, 2));
            sSignatureLocations.Add(new KeyValuePair<int, int>(9, 0), new Signature(1, Direction.CounterClockwise, 1));

            //Bottom
            sSignatureLocations.Add(new KeyValuePair<int, int>(1, 10), new Signature(3, Direction.CounterClockwise, 1));
            sSignatureLocations.Add(new KeyValuePair<int, int>(2, 10), new Signature(3, Direction.CounterClockwise, 2));
            sSignatureLocations.Add(new KeyValuePair<int, int>(3, 10), new Signature(3, Direction.CounterClockwise, 3));
            sSignatureLocations.Add(new KeyValuePair<int, int>(4, 10), new Signature(3, Direction.CounterClockwise, 4));
            sSignatureLocations.Add(new KeyValuePair<int, int>(6, 10), new Signature(4, Direction.Clockwise, 4));
            sSignatureLocations.Add(new KeyValuePair<int, int>(7, 10), new Signature(4, Direction.Clockwise, 3));
            sSignatureLocations.Add(new KeyValuePair<int, int>(8, 10), new Signature(4, Direction.Clockwise, 2));
            sSignatureLocations.Add(new KeyValuePair<int, int>(9, 10), new Signature(4, Direction.Clockwise, 1));

            // Left
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 1), new Signature(2, Direction.CounterClockwise, 1));
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 2), new Signature(2, Direction.CounterClockwise, 2));
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 3), new Signature(2, Direction.CounterClockwise, 3));
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 4), new Signature(2, Direction.CounterClockwise, 4));
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 6), new Signature(3, Direction.Clockwise, 4));
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 7), new Signature(3, Direction.Clockwise, 3));
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 8), new Signature(3, Direction.Clockwise, 2));
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 9), new Signature(3, Direction.Clockwise, 1));

            // Right
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 1), new Signature(1, Direction.Clockwise, 1));
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 2), new Signature(1, Direction.Clockwise, 2));
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 3), new Signature(1, Direction.Clockwise, 3));
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 4), new Signature(1, Direction.Clockwise, 4));
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 6), new Signature(4, Direction.CounterClockwise, 4));
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 7), new Signature(4, Direction.CounterClockwise, 3));
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 8), new Signature(4, Direction.CounterClockwise, 2));
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 9), new Signature(4, Direction.CounterClockwise, 1));

            // Corners
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 0), Signature.TopLeft);
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 0), Signature.TopRight);
            sSignatureLocations.Add(new KeyValuePair<int, int>(0, 10), Signature.BottomLeft);
            sSignatureLocations.Add(new KeyValuePair<int, int>(10, 10), Signature.BottomRight);

            this.SetOnTouchListener(this);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            mViewInfo = new ViewInfo(w, h);
        }


        protected override void OnDraw(Canvas canvas)
        {
            DrawOutline(canvas);
            DrawLetters(canvas);
            DrawMiddleSquare(canvas);
        }

        // Draws the outer 4 quadrants. Use DrawMiddleSquare to draw that.
        private void DrawOutline(Canvas canvas)
        {
            int largeGradientSize = 40;
            int smallGradientSize = 20;

            int gradientSize = mViewInfo.Position == Position.Q1 ? largeGradientSize : smallGradientSize;
            Drawing.DrawGradientPath(canvas, mViewInfo.Q1Border, gradientSize, GetPositionColor(Position.Q1));
            Drawing.DrawGradientArc(canvas, mViewInfo.MiddleSquare.Center, mViewInfo.MiddleSquare.Width / 2f, 270, 90, gradientSize, GetPositionColor(Position.Q1));
            //Drawing.DrawGradientCap(canvas, mViewInfo.MiddleSquare, 1, mViewInfo.Position == Position.Q1 ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Q1));

            Drawing.DrawGradientPath(canvas, mViewInfo.Q2Border, mViewInfo.Position == Position.Q2 ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Q2));
            //Drawing.DrawGradientCap(canvas, mViewInfo.MiddleSquare, 2, mViewInfo.Position == Position.Q2 ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Q2));

            Drawing.DrawGradientPath(canvas, mViewInfo.Q3Border, mViewInfo.Position == Position.Q3 ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Q3));
            //Drawing.DrawGradientCap(canvas, mViewInfo.MiddleSquare, 3, mViewInfo.Position == Position.Q3 ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Q3));

            Drawing.DrawGradientPath(canvas, mViewInfo.Q4Border, mViewInfo.Position == Position.Q4 ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Q4));
            //Drawing.DrawGradientCap(canvas, mViewInfo.MiddleSquare, 4, mViewInfo.Position == Position.Q4 ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Q4));
        }

        private void DrawMiddleSquare(Canvas canvas)
        {
            /*
            Paint paint = new Paint();
            paint.StrokeWidth = 1;

            // The middle square is painted transparent to cover the lines from the previously drawn rectangles.
            
            paint.SetStyle(Paint.Style.Fill);
            paint.Color = new Color(0, 0, 0, 0);
            canvas.DrawRect(AndroidUtil.GetRect(mViewInfo.MiddleSquare), paint);
            */

            int largeGradientSize = 40;
            int smallGradientSize = 20;

            Drawing.DrawGradientArc(canvas, mViewInfo.MiddleSquare.Center, mViewInfo.MiddleSquare.Width / 2f, 0, -360, mViewInfo.Position == Position.Middle ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Middle));

            //Drawing.DrawGradientPath(canvas, mViewInfo.MiddleSquareBorder, mViewInfo.Position == Position.Middle ? largeGradientSize : smallGradientSize, GetPositionColor(Position.Middle));

            /*
            paint.SetStyle(mViewInfo.Position == Position.Middle ? Paint.Style.FillAndStroke : Paint.Style.Stroke);
            paint.Color = new Color(0, 0, 0);
            canvas.DrawRect(AndroidUtil.GetRect(mViewInfo.MiddleSquare), paint);
            */

        }

        private void DrawLetters(Canvas canvas)
        {
            Paint letterPaint = new Paint();
            letterPaint.StrokeWidth = 1;
            letterPaint.SetStyle(Paint.Style.Stroke);
            


            int rectWidth = (int)(mViewInfo.Width / 11.0);
            int rectHeight = (int)(mViewInfo.Height / 11.0);

            letterPaint.TextSize = Math.Min(rectHeight, 80);


            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    letterPaint.SetStyle(Paint.Style.Stroke);
                    //canvas.DrawRect(new Android.Graphics.Rect(i * rectWidth, j * rectHeight, (i + 1) * rectWidth, (j + 1) * rectHeight), paint);

                    KeyValuePair<int, int> index = new KeyValuePair<int, int>(i, j);
                    if (sSignatureLocations.ContainsKey(index))
                    {
                        Signature sig = sSignatureLocations[index];
                        char chr = Arrangement.Layout[sig].Character;
                        char chrToMeasure = chr == ' ' ? 'X' : chr;

                        int x = i * rectWidth;
                        int y = j * rectHeight;
                        Android.Graphics.Rect rect = new Android.Graphics.Rect(x, y, x + rectWidth, y + rectHeight);

                        // The actual character is used for horizontal positioning
                        Android.Graphics.Rect characterBounds = new Android.Graphics.Rect();
                        letterPaint.GetTextBounds(chrToMeasure.ToString(), 0, 1, characterBounds);
                        // A standard character is used for vertical positioning so all the characters line up
                        Android.Graphics.Rect xBounds = new Android.Graphics.Rect();
                        letterPaint.GetTextBounds("X", 0, 1, xBounds);
                        // Center the character in the rectangle
                        int textLeft = rect.Left + (int)((rect.Width() - characterBounds.Width()) / 2.0);
                        int textBottom = rect.Bottom - (int)((rect.Height() - xBounds.Width()) / 2.0);
                        // Lift the bottom up away from the border if we're on the bottom row
                        //int textBottom = rect.Bottom - (j == 10 ? 10 : 0);


                        TinyTypeLib.Rect boundsInLocation = new TinyTypeLib.Rect(rect.Left, textBottom - xBounds.Height(), rect.Right, textBottom);

                        if (sig == mViewInfo.CurrentSignature && 
                            sig != Signature.BottomLeft &&
                            sig != Signature.BottomRight &&
                            sig != Signature.TopLeft &&
                            sig != Signature.TopRight)
                        {
                            // Don't draw within the MiddleSquare
                            canvas.ClipRect(new Android.Graphics.Rect(mViewInfo.Left, mViewInfo.Top, mViewInfo.Right, mViewInfo.Bottom));
                            canvas.ClipRect(Converter.TinyTypeRectToAndroidRect(mViewInfo.MiddleSquare), Region.Op.Difference);

                            int gradientSize = 20;

                            Paint linePaint = new Paint();

                            // Draw a line to the circle we're about to draw
                            Line lineToLetter = new Line(mViewInfo.MiddleSquare.Center, boundsInLocation.Center);
                            Line perpendicular = lineToLetter.GetPerpendicular(gradientSize);
                            Color color = GetPositionColor((Position)mViewInfo.CurrentSignature.StartHeading.Quadrant);
                            Color transparent = AndroidUtil.GetTransparentColor(color);
                            linePaint.SetShader(new LinearGradient(perpendicular.Start.X, perpendicular.Start.Y, perpendicular.End.X, perpendicular.End.Y, color, transparent, Shader.TileMode.Mirror));
                            linePaint.SetStyle(Paint.Style.Stroke);
                            linePaint.StrokeWidth = gradientSize * 2;
                            linePaint.StrokeCap = Paint.Cap.Round;
                            canvas.DrawLine(lineToLetter.Start.X, lineToLetter.Start.Y, lineToLetter.End.X, lineToLetter.End.Y, linePaint);
                        }
                        else
                        {
                            //letterPaint.Color = new Color(255, 0, 255);
                        }

                        if (chr != char.MinValue)
                        {
                            // Fill a circle in the background of the circle
                            Paint letterBackgroundPaint = new Paint();
                            Color letterBackgroundColor = GetPositionColor((Position)sig.StartQuadrant);
                            Color letterBackgroundTransparent = AndroidUtil.GetTransparentColor(letterBackgroundColor);
                            float radius = Math.Max(boundsInLocation.Width, boundsInLocation.Height) / 2f;
                            letterBackgroundPaint.SetShader(new RadialGradient(boundsInLocation.Center.X, boundsInLocation.Center.Y, radius, letterBackgroundColor, letterBackgroundTransparent, Shader.TileMode.Mirror));
                            letterBackgroundPaint.SetStyle(Paint.Style.FillAndStroke);
                            canvas.DrawCircle(boundsInLocation.Center.X, boundsInLocation.Center.Y, (float)(Math.Max(boundsInLocation.Width, boundsInLocation.Height) / 2.0), letterBackgroundPaint);

                            letterPaint.SetStyle(Paint.Style.FillAndStroke);
                            canvas.DrawText(chr.ToString(), textLeft, textBottom, letterPaint);
                        }
                    }
                }
            }

            letterPaint.SetStyle(Paint.Style.FillAndStroke);
            letterPaint.Color = GetPositionColor(Position.Middle);
            letterPaint.TextSize = mViewInfo.MiddleSquare.Height * 0.75f;

            // Clear the clip region
            canvas.ClipRect(new Android.Graphics.Rect(mViewInfo.Left, mViewInfo.Top, mViewInfo.Right, mViewInfo.Bottom), Region.Op.Replace);

            // Draw the selected character in the middle of the square
            if (mViewInfo.CurrentSignature != Signature.None)
            {
                char chr = Arrangement.Layout[mViewInfo.CurrentSignature].Character;

                // Center the character in the rectangle
                Android.Graphics.Rect bounds = new Android.Graphics.Rect();
                letterPaint.GetTextBounds(chr.ToString(), 0, 1, bounds);

                int textLeft = mViewInfo.MiddleSquare.Left + (int)((mViewInfo.MiddleSquare.Width - bounds.Width()) / 2.0);
                int textBottom = mViewInfo.MiddleSquare.Bottom - (int)((mViewInfo.MiddleSquare.Height - bounds.Height()) / 2.0);

                canvas.DrawText(chr.ToString(), textLeft, textBottom, letterPaint);
            }

            // Draw the previous character above the center square
            if (mPreviousCharacter != char.MinValue)
            {
                TinyTypeLib.Rect rect = mViewInfo.MiddleSquare.Offset(0, -mViewInfo.MiddleSquare.Height);

                //paint.SetStyle(Paint.Style.Stroke);
                //canvas.DrawRect(Converter.TinyTypeRectToAndroidRect(rect), paint);
                //paint.SetStyle(Paint.Style.FillAndStroke);

                // Center the character in the rectangle
                Android.Graphics.Rect bounds = new Android.Graphics.Rect();
                letterPaint.GetTextBounds(mPreviousCharacter.ToString(), 0, 1, bounds);

                int textLeft = rect.Left + (int)((rect.Width - bounds.Width()) / 2.0);
                int textBottom = rect.Bottom - (int)((rect.Height - bounds.Height()) / 2.0);

                canvas.DrawText(mPreviousCharacter.ToString(), textLeft, textBottom, letterPaint);
            }

        }


        public bool OnTouch(View v, MotionEvent e)
        {
            int x = (int)e.GetX();
            int y = (int)e.GetY();

            Position pos = mViewInfo.GetPosition(x, y);

            switch(e.Action)
            {
                case MotionEventActions.Down:
                    mViewInfo.ClearPath();
                    mViewInfo.AddToPath(pos);
                    Invalidate();
                    break;
                case MotionEventActions.Up:
                    mViewInfo.ClearPath();
                    Invalidate();
                    break;
                case MotionEventActions.Move:
                    if (mViewInfo.LatestPathEntry == Position.None || pos != mViewInfo.LatestPathEntry)
                    {
                        if (pos == TinyTypeLib.Position.Middle)
                        {
                            CompletePath();
                            mViewInfo.ClearPath();
                        }

                        mViewInfo.AddToPath(pos);
                        Invalidate();
                    }
                    break;
            }

            return true;
        }

        private void CompletePath()
        {
            Signature sig = mViewInfo.CurrentSignature;

            InputAction inputAction = Arrangement.GetAction(mViewInfo.CurrentSignature);

            switch (inputAction.Type)
            {
                case InputAction.ActionType.Lock:
                    LockMode = !LockMode;
                    break;
                case InputAction.ActionType.ToggleArrangementMode:
                    // Cycle through ArrangementModes
                    Mode = Mode == ArrangementMode.LowerCase ? ArrangementMode.UpperCase :
                        Mode == ArrangementMode.UpperCase ? ArrangementMode.NumbersAndSymbols :
                        ArrangementMode.LowerCase;
                    break;
                default:
                    // For everything else, just raise an event with the selected action
                    ActionChosenEventArgs args = new ActionChosenEventArgs();
                    args.Action = inputAction;
                    OnActionChosen(args);
                    break;
            }
        }

        private string ModifyInput(string input)
        {
            /*
            // If the user typed a second space, switch to a period and a space
            if (input.EndsWith("  "))
            {
                input = mString.Substring(0, mString.Length - 2) + ". ";
            }

            // If the user typed "i ", capitalize it for them
            if (input.Equals("i ") || input.EndsWith(" i ")) // The leading space in the second check verifies that they didn't just type a word that ends in "i"
            {
                input = input.Substring(0, input.Length - 2) + "I ";
            }
            */
            return input;
        }

        protected virtual void OnActionChosen(ActionChosenEventArgs e)
        {
            if (e.Action.Type == InputAction.ActionType.TypeCharacter)
            {
                mPreviousCharacter = e.Action.Character;
            }

            EventHandler<ActionChosenEventArgs> handler = ActionChosen;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private Color GetPositionColor(Position position)
        {
            switch (position)
            {
                case Position.Middle:
                    return Color.Magenta;
                case Position.Q1:
                    return Color.Lime;
                case Position.Q2:
                    return Color.Red;
                case Position.Q3:
                    return Color.Blue;
                case Position.Q4:
                    return Color.Yellow;
                default:
                    throw new Exception("Invalid position: " + position);
            }
        }
    }
}