using System;
using System.ComponentModel;
using Android.Graphics.Drawables;
using Android.Util;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PickerRenderer = Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer))]

namespace Gefco.CipQuai.Droid.Renderers
{
    public class BindablePickerRenderer : PickerRenderer
    {
        public BindablePickerRenderer() : base(MainActivity.Instance)
        {
        }

        /// <summary>
        ///     Raises the element changed event.
        /// </summary>
        /// <param name="e">Event arg.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null || Control == null)
                return;

            var bindablePicker = e.NewElement as BindablePicker;
            var customBG = new GradientDrawable();
            customBG.SetColor(Element.BackgroundColor.ToAndroid());
            customBG.SetCornerRadius(bindablePicker.CornerRadius);
            var borderWidth = bindablePicker.BorderWidth;
            customBG.SetStroke(borderWidth, bindablePicker.BorderColor.ToAndroid());
            Control.SetBackground(customBG);

            //if (this.Element.BackgroundColor != default(Color))
            //{
            //    this.Control.SetBackgroundColor(this.Element.BackgroundColor.ToAndroid());
            //}
            Control.SetPadding((int) (bindablePicker.Padding.Left * Resources.DisplayMetrics.Density), (int) (bindablePicker.Padding.Top * Resources.DisplayMetrics.Density), (int) (bindablePicker.Padding.Right * Resources.DisplayMetrics.Density), (int) (bindablePicker.Padding.Bottom * Resources.DisplayMetrics.Density));
            //this.Control.SetTypeface(Typeface.CreateFromAsset(Forms.Context.Assets, "RobotoCondensed-Regular.ttf"), TypefaceStyle.Normal);
            Control.SetTextSize(ComplexUnitType.Dip, (float) bindablePicker.FontSize);

            if (!string.IsNullOrEmpty(bindablePicker?.Placeholder))
            {
                Control.Hint = bindablePicker.Placeholder;
                Control.SetHintTextColor(bindablePicker.PlaceholderColor.ToAndroid());
            }
            Control.Text = bindablePicker.SelectedItem?.ToString();
            bindablePicker.PropertyChanged += BindablePickerOnPropertyChanged;
        }

        private void BindablePickerOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
        }
    }
}