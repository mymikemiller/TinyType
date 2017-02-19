using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using TinyTypeLib;
using TinyTypeAndroid;

namespace AndroidLib
{
    public static class Drawing
    {
        
        public static void ClearClip(Canvas canvas, ViewInfo viewInfo)
        {
            canvas.ClipRect(new Android.Graphics.Rect(viewInfo.Left, viewInfo.Top, viewInfo.Right, viewInfo.Bottom), Region.Op.Replace);
        }


        public static void DrawGradientPath(Canvas canvas, List<TinyTypeLib.PointF> path, int gradientSize, Color color)
        {
            if (path.Count < 3)
            {
                throw new Exception("Path must have at least three points");
            }

            // Add the first two points to the end to make it easier to assess direction
            int originalCount = path.Count;
            path.Add(path[0]);
            path.Add(path[1]);

            for (int i = 0; i < originalCount; i++ )
            {
                Line line = new Line(path[i], path[i + 1]);
                DrawGradientLine(canvas, line, gradientSize, color);
            }
        }

        public static void DrawGradientCap(Canvas canvas, TinyTypeLib.Rect middleSquare, int quadrant, int gradientSize, Color color)
        {
            Paint paint = new Paint();
            paint.SetStyle(Paint.Style.Fill);
            Color transparent = color;
            transparent.A = 0;

            Point center = new Point(
                quadrant == 1 || quadrant == 4 ? middleSquare.Right : middleSquare.Left,
                quadrant == 1 || quadrant == 2 ? middleSquare.Top : middleSquare.Bottom);

            int startAngle = (4 - quadrant) * 90;

            paint.SetShader(new RadialGradient(center.X, center.Y, gradientSize, color, transparent, Shader.TileMode.Mirror));
            canvas.DrawArc(new RectF(
                center.X - gradientSize,
                center.Y - gradientSize,
                center.X + gradientSize,
                center.Y + gradientSize), startAngle, 90, true, paint);
            
        }

        // gradientSize is the distance from the line to half-way through the gradient's fade
        // The direction of the gradient is controlled by the ordering of the points. If you're walking along the line from Start to End, the gradient will go toward the left.
        public static void DrawGradientLine(Canvas canvas, Line line, int gradientSize, Color color)
        {
            /*
            bool horizontal = line.IsHorizontal();
            int direction; // 0 = down or right, -1 = up or left
            if (horizontal)
            {
                direction = line.End.X > line.Start.X ? -1 : 1;
            }
            else
            {
                direction = line.End.Y > line.Start.Y ? 1 : -1;
            }
            int gradientHeight = horizontal ? gradientSize : 0;
            int gradientWidth = horizontal ? 0 : gradientSize;
            */

            Line perpendicular = line.GetPerpendicular(gradientSize);

            int gradientLength = gradientSize * 2;
            Paint paint = new Paint();
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeCap = Paint.Cap.Butt;
            Color transparent = AndroidUtil.GetTransparentColor(color);

            //Line gradientLine = new Line(line.Start, new TinyTypeLib.PointF(line.End.X, line.End.Y + gradientLength));
            //gradientLine = gradientLine.AdjustLength(line.Length);


            // Draw the original line we were sent
            paint.StrokeWidth = 2;
            paint.Color = Color.Violet;
            canvas.DrawLine(line.Start.X, line.Start.Y, line.End.X, line.End.Y, paint);

            // Here we need to solve for the gradient's start point, or the location of B. We'll solve for angle C and length a to find that out.
            double c = line.Length;
            double b = gradientLength;

            /*
             * B____c_____A
             *  \        /
             *    a\    /b
             *        \/
             *        C=90 
             */
        
            // Solve for angle B in radians. B = sin-1(b/c)
            double B = Math.Asin(b / c);

            // Solve for length a. a^2+b^2 = c^2, so a = sqrt(c^2-b^2)
            double a = Math.Sqrt((c * c) - (b * b));


            paint.Color = Color.Green;
            Line l = new Line(line.Start, B, a);
            canvas.DrawLine(l.Start.X, l.Start.Y, l.End.X, l.End.Y, paint);

            paint.Color = Color.Blue;
            Line gradientLine = new Line(l.End, line.End);
            canvas.DrawLine(gradientLine.Start.X, gradientLine.Start.Y, gradientLine.End.X, gradientLine.End.Y, paint);
             
            

            /*
            paint.Color = Color.Blue;
            canvas.DrawLine(line.End.X, line.End.Y, gradientLine.End.X, gradientLine.End.Y, paint);
            paint.Color = Color.Orange;
            canvas.DrawLine(line.Start.X, line.End.Y, gradientLine.End.X, gradientLine.End.Y, paint);
            //paint.StrokeWidth = gradientSize * 20;

            */

            paint.StrokeWidth = gradientLength;
            paint.SetShader(new LinearGradient(gradientLine.Start.X, gradientLine.Start.Y, gradientLine.End.X, gradientLine.End.Y, Color.Red, Color.Black, Shader.TileMode.Mirror));
            canvas.DrawLine(line.Start.X, line.Start.Y, line.End.X, line.End.Y, paint);
        }

        // if sweepAngle is negative, the gradient will fade inward, if it's positive, outward.
        public static void DrawGradientArc(Canvas canvas, TinyTypeLib.PointF center, float radius, float startAngle, float sweepAngle, int gradientSize, Color color)
        {
            //center = new TinyTypeLib.PointF(center.X + 30, center.Y);

            float radiusToUse = sweepAngle < 0 ? radius : radius + gradientSize;
            float gradientPercent = gradientSize / (float)radius;
            int[] colors;
            float[] positions;

            Paint paint = new Paint();
            paint.SetStyle(Paint.Style.FillAndStroke);

            if (sweepAngle > 0)
            {
                // We need to clip out the middle of the circle so the mirrored gradient doesn't show
                Path cp = new Path();
                cp.AddCircle(center.X, center.Y, radius, Path.Direction.Cw);
                canvas.ClipPath(cp, Region.Op.Difference);

            }

            colors = new int[] { AndroidUtil.GetTransparentColor(color), AndroidUtil.GetTransparentColor(color), color };
            positions = new float[] { 0f, 1 - gradientPercent, 1f };

            paint.SetShader(new RadialGradient(center.X, center.Y, radius, colors, positions, Shader.TileMode.Mirror));
            RectF square = new RectF(center.X - radiusToUse, center.Y - radiusToUse, center.X + radiusToUse, center.Y + radiusToUse);
            canvas.DrawArc(square, startAngle, sweepAngle, true, paint);


            /* Draw circles for reference
            Paint circlePaint = new Paint();
            circlePaint.Color = Color.Black;
            circlePaint.SetStyle(Paint.Style.Stroke);
            canvas.DrawCircle(center.X, center.Y, radius, circlePaint);
            canvas.DrawCircle(center.X, center.Y, radiusToUse, circlePaint);
            */
        }
    }
}