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

namespace TinyTypeAndroid
{
    public static class AndroidUtil
    {
        public static Android.Graphics.Rect GetRect(TinyTypeLib.Rect rect)
        {
            return new Android.Graphics.Rect(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public static Color GetTransparentColor(Color color)
        {
            color.A = 0;
            return color;
        }
    }
}