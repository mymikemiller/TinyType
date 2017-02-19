using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using TinyTypeAndroid;
using AndroidLib;
using AndroidTTR;
using TinyTypeLib;

namespace AndroidIME
{
    // If it's HardwareAccellerated, there are problems drawing gradients with arcs.
    [Activity(Label = "TinyType Demo", MainLauncher = true, Icon = "@drawable/icon", HardwareAccelerated = false)]
    public class MainActivity : Activity
    {
        private TTRView mTTRView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            View v = LayoutInflater.Inflate(Resource.Layout.Main, null);
            SetContentView(v);
            //SetContentView(Resource.Layout.Main);

            Button openIMESelectionBotton = v.FindViewById<Button>(Resource.Id.openIMESelection);
            openIMESelectionBotton.Click += OpenIMESelection_Click;

            Button toggleViewButton = v.FindViewById<Button>(Resource.Id.toggleView);
            toggleViewButton.Click += ToggleView_Click;

            InputView inputView = (InputView)v.FindViewById(Resource.Id.inputView);
            inputView.ActionChosen += OnActionChosen;

            mTTRView = (TTRView)v.FindViewById(Resource.Id.ttrView);

        }

        private void OnActionChosen(object sender, ActionChosenEventArgs e)
        {
            EditText editText = (EditText)FindViewById(Resource.Id.editText);
            string final = ActionResponder.ActionChosen(e.Action, editText.Text);
            editText.Text = final;
            editText.SetSelection(editText.Text.Length);

            if (e.Action.Type == InputAction.ActionType.TypeCharacter)
            {
                mTTRView.TypeLetter(e.Action.Character);
            }
        }

        void OpenIMESelection_Click(object sender, EventArgs e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.ShowInputMethodPicker();
        }
        void ToggleView_Click(object sender, EventArgs e)
        {
            InputView view = FindViewById<InputView>(Resource.Id.inputView);
            view.Visibility = view.Visibility == ViewStates.Visible ? ViewStates.Invisible : ViewStates.Visible;
        }
    }
}

