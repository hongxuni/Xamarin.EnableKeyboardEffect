﻿using Android.Content;
using Android.Support.Design.Widget;
using Android.Views.InputMethods;
using Android.Widget;
using System;
using System.Linq;
using Xamarin.EnableKeyboardEffect.Platform.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ResolutionGroupName("Xamarin.EnableKeyboardEffect")]
[assembly: ExportEffect(typeof(KeyboardEnableEffect), nameof(KeyboardEnableEffect))]
namespace Xamarin.EnableKeyboardEffect.Platform.Droid
{
    public class KeyboardEnableEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                EditText editText;
                # region Material Design
                if (Control is TextInputLayout inputLayout)
                {
                    editText = inputLayout.EditText;
                }
                #endregion
                else if (Control is EditText)
                {
                    editText = (EditText)Control;
                }
                else
                {
                    return;
                }
                editText.ShowSoftInputOnFocus = EnableKeyboardEffect.GetEnableKeyboard(Element);
               
                if (!editText.ShowSoftInputOnFocus)
                {
                    editText.FocusChange += HideMethod;
                }
                editText.RequestFocus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        protected override void OnDetached()
        {
            try
            {
                if (Element == null || Control == null)
                {
                    return;
                }

                EditText editText;

                #region Material Design
                if (Control is TextInputLayout inputLayout)
                {
                    editText = inputLayout.EditText;
                }
                #endregion
                else if (Control is EditText)
                {
                    editText = (EditText)Control;
                }
                else
                {
                    return;
                }

                var visibilityEffect = Element.Effects.OfType<Xamarin.EnableKeyboardEffect.KeyboardEnableEffect>().FirstOrDefault();
                if (visibilityEffect != null)
                {
                    return;
                }

                editText.ShowSoftInputOnFocus = EnableKeyboardEffect.GetEnableKeyboard(Element);

                editText.FocusChange -= HideMethod;

                var imm = (InputMethodManager)Effects.Activity?.GetSystemService(Context.InputMethodService);
                //Comment out as it is using an obsolete method
                //imm?.ShowSoftInputFromInputMethod(Control.WindowToken, ShowFlags.Implicit);

                imm?.ShowSoftInput(Control, ShowFlags.Implicit);
                editText.RequestFocus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void HideMethod(object sender, View.FocusChangeEventArgs e)
        {
            try
            {
                //hide keyboard for current focused control.
                var imm = (InputMethodManager)Effects.Activity?.GetSystemService(Context.InputMethodService);
                imm?.HideSoftInputFromWindow(Control.WindowToken, HideSoftInputFlags.None);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}