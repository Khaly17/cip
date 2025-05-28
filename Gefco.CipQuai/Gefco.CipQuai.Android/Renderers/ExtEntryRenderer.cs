using System.ComponentModel;
using Android.Graphics.Drawables;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtEntry), typeof(ExtEntryRenderer))]

namespace Gefco.CipQuai.Droid.Renderers
{
    /// <summary>
    /// Entry custom renderer.
    /// </summary>
    public class ExtEntryRenderer : EntryRenderer
    {
        public ExtEntryRenderer() : base(MainActivity.Instance)
        {

        }
        private bool RemoveBorder { get; set; }

        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">Event arg.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var element = this.Element as ExtEntry;

            if (e.OldElement != null || element == null || this.Control == null)
            {
                return;
            }

            if (Control != null)
            {
                GradientDrawable customBG = new GradientDrawable();
                customBG.SetColor(this.Element.BackgroundColor.ToAndroid());
                customBG.SetCornerRadius(element.CornerRadius);
                int borderWidth = element.BorderWidth;
                customBG.SetStroke(borderWidth, element.BorderColor.ToAndroid());
                this.Control.SetBackground(customBG);
            }
            //if (this.Element.BackgroundColor != default(Color))
            //{
            //    this.Control.SetBackgroundColor(this.Element.BackgroundColor.ToAndroid());
            //}

            if (!string.IsNullOrEmpty(this.Element.Placeholder))
            {
                this.Control.SetHintTextColor(Color.Gray.ToAndroid());
            }

            this.RemoveBorder = element.RemoveBorder;
            if (this.RemoveBorder)
            {
                //this.Control. = UITextBorderStyle.None;
            }
            this.Control.SetPadding((int)(element.Padding.Left * Resources.DisplayMetrics.Density), (int)(element.Padding.Top * Resources.DisplayMetrics.Density), (int)(element.Padding.Right * Resources.DisplayMetrics.Density), (int)(element.Padding.Bottom * Resources.DisplayMetrics.Density));
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control != null)
            {
                var element = this.Element as ExtEntry;
                GradientDrawable customBG = new GradientDrawable();
                customBG.SetColor(this.Element.BackgroundColor.ToAndroid());
                customBG.SetCornerRadius(element.CornerRadius);
                int borderWidth = element.BorderWidth;
                customBG.SetStroke(borderWidth, element.BorderColor.ToAndroid());
                this.Control.SetBackground(customBG);
            }
        }
    }
}