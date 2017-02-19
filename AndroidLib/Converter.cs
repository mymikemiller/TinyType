using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidLib
{
    public static class Converter
    {
        public static Android.Graphics.Rect TinyTypeRectToAndroidRect(TinyTypeLib.Rect rect)
        {
            return new Android.Graphics.Rect(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
    }
}
