using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace TinyTypeAndroid
{
    [Activity(Label = "TinyTypeAndroid", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            base.SetContentView(new InputView(this));
        }

        
    }
}

