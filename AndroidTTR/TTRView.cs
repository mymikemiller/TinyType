using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTRLib;

namespace AndroidTTR
{
    public class TTRView : View
    {
        private Game Game { get; set; }
        Line mLine;

        public TTRView(Context context) :
            base(context)
        {
            Initialize();
        }
        public TTRView(Context context, IAttributeSet attributeSet) :
            base(context)
        {
            Initialize();
        }

        private void Initialize()
        {
            Game = new Game();
            mLine = new Line(Game.CurrentLevel.FullString);
        }

        public void TypeLetter(char letter)
        {
            mLine.TypeLetter(letter);
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            Paint paint = new Paint();
            paint.Color = Color.Black;
            paint.SetStyle(Paint.Style.Fill);

            Rect rect = new Rect();
            this.GetDrawingRect(rect);
            canvas.DrawRect(rect, paint);

            // For now, we'll scroll letters in the TTRView, but we really want to be scrolling them in the ScrollView contained in the TTRView, within or above the InputView


            DrawLine(canvas, mLine);

        }

        private void DrawLine(Canvas canvas, Line line)
        {
            int index = 0;

            string text = line.CorrectText;
            DrawText(canvas, text, index, Color.Lime);
            index += text.Length;

            text = line.IncorrectText;
            DrawText(canvas, text, index, Color.Red);
            index += text.Length;

            text = line.NextText;
            DrawText(canvas, text, index, Color.White);
            index += text.Length;
        }

        private void DrawText(Canvas canvas, string text, int index, Color color)
        {
            foreach(char c in text)
            {
                DrawCharacter(canvas, c, index++, color);
            }
        }

        private void DrawCharacter(Canvas canvas, char character, int index, Color color)
        {
            Paint paint = new Paint();
            paint.Color = color;

            paint.SetStyle(Paint.Style.Stroke);
            paint.TextSize = this.Height * .8f;

            canvas.DrawText(character.ToString(), index * paint.TextSize, this.Height * .9f, paint);
        }
    }
}
