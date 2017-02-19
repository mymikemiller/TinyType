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
using Android.Views.InputMethods;
using TinyTypeLib;


namespace AndroidLib
{
    public static class ActionResponder
    {

        public static string ActionChosen(InputAction action, string source)
        {
            string final = source;

            switch (action.Type)
            {
                case InputAction.ActionType.TypeCharacter:
                    final += action.Character;
                    //final = ModifyInput(final);
                    /*
                    bool forceUpperNext = final.Length == 0 || final.EndsWith(". ");
                    // Switch to lower case after each letter (unless we're in capslock. Don't ever switch away from CapsLock until the user manually switches)
                    if (mMode != ArrangementMode.CapsLock)
                    {
                        mMode = forceUpperNext ? ArrangementMode.UpperCase : ArrangementMode.LowerCase;
                    }
                     */
                    break;
                case InputAction.ActionType.Backspace:
                    // Backspace
                    if (final.Length > 0)
                    {
                        final = final.Substring(0, final.Length - 1);
                    }
                    break;
                case InputAction.ActionType.DeleteWord:
                    // Delete whole word
                    // First remove any trailing spaces
                    
                    while (final.Length > 0 && final[final.Length - 1] == ' ')
                    {
                        final = final.Remove(final.Length - 1);
                    }

                    if (final.Length > 0)
                    {
                        int lastSpace = final.LastIndexOf(' ');
                        if (lastSpace == -1)
                        {
                            final = string.Empty;
                        }
                        else
                        {
                            final = final.Substring(0, lastSpace) + ' ';
                        }
                    }
                     
                    break;
            }

            return final;
        }

        public static void ActionChosen(InputAction action, IInputConnection inputConnection)
        {
            //Java.Lang.String str = new Java.Lang.String(e.Key.ToString());
            //CurrentInputConnection.CommitText(str, 1);
            switch (action.Type)
            {
                case InputAction.ActionType.TypeCharacter:
                    /*
                    mString += letter;

                    mString = ModifyInput(mString);

                    bool forceUpperNext = mString.Length == 0 || mString.EndsWith(". ");
                    // Switch to lower case after each letter (unless we're in capslock. Don't ever switch away from CapsLock until the user manually switches)
                    if (mMode != ArrangementMode.CapsLock)
                    {
                        mMode = forceUpperNext ? ArrangementMode.UpperCase : ArrangementMode.LowerCase;
                    }
                     */
                    inputConnection.CommitText(action.Character.ToString(), 1);
                    break;
                case InputAction.ActionType.Backspace:
                    // Backspace
                    inputConnection.SendKeyEvent(new KeyEvent(KeyEventActions.Down, Android.Views.Keycode.Del));
                    break;
                case InputAction.ActionType.DeleteWord:
                    // Delete whole word
                    // First remove any trailing spaces
                    /*
                    while (mString.Length > 0 && mString[mString.Length - 1] == ' ')
                    {
                        mString = mString.Remove(mString.Length - 1);
                    }

                    if (mString.Length > 0)
                    {
                        int lastSpace = mString.LastIndexOf(' ');
                        if (lastSpace == -1)
                        {
                            mString = string.Empty;
                        }
                        else
                        {
                            mString = mString.Substring(0, lastSpace) + ' ';
                        }
                    }
                     */
                    break;
            }
            //KeyEvent ke = new KeyEvent(KeyEventActions.Down, e.Keycode);
            //CurrentInputConnection.SendKeyEvent(ke);
        }
    }
}