using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.InputMethodServices;
using Android.Views.InputMethods;
using TinyTypeAndroid;
using TinyTypeLib;
using AndroidLib;

namespace AndroidIME
{
    //[Activity(Label = "TestKeyboard", MainLauncher = true, Icon = "@drawable/icon")]
    [Service(Name = "com.AndroidIME.TinyTypeIME", Permission = "android.permission.BIND_INPUT_METHOD")]
    [IntentFilter(new String[] { "android.view.InputMethod" })]
    [MetaData("android.view.im", Resource = "@xml/method")]
    public class TinyTypeIME : InputMethodService
    {
        ActionResponder mActionResponder;

        public override void OnCreate()
        {
            base.OnCreate();

            mActionResponder = new ActionResponder(CurrentInputConnection);
        }

        public override void OnWindowShown()
        {
            base.OnWindowShown();
            SetCandidatesViewShown(true);
        }

        public override View OnCreateCandidatesView()
        {
            View v = LayoutInflater.Inflate(Resource.Layout.Input, null);

            InputView inputView = (InputView)v.FindViewById(Resource.Id.inputView);
            inputView.ActionChosen += OnActionChosen;

            return v;
        }

        private void OnActionChosen(object sender, ActionChosenEventArgs e)
        {
            
        }

        public override View OnCreateInputView()
        {
            return base.OnCreateInputView();
            /*
            // It seems that just returning new KeyboardView makes it take up the entire height, pushing the contents upward leaving whitespace. If I could use a layout that had 0 height maybe it would work.
            View v = LayoutInflater.Inflate(Resource.Layout.Input, null);
            
            Button button = v.FindViewById<Button>(Resource.Id.MyButton);
            button.Text = "Click me!";
            button.SetOnClickListener(this);

            //button.SetBackgroundColor(new Android.Graphics.Color(50, 50, 50, 50));
            
            return v;
            
            */



            //return new KeyboardView(this);
        }


    }
}

