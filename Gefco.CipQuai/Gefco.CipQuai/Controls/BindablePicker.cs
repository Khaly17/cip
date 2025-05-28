using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    public class BindablePicker : Picker
    {
        #region Fields

        /// <summary>
        ///     The placeholder property.
        /// </summary>
        public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(BindablePicker), "");

        /// <summary>
        ///     The placeholder color property.
        /// </summary>
        public static BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(BindablePicker), Color.Default);

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the command parameter.
        /// </summary>
        /// <value>The command parameter.</value>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        ///     Gets or sets the color of the placeholder.
        /// </summary>
        /// <value>The color of the placeholder.</value>
        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public float CornerRadius { get; set; } = 10;
        public int BorderWidth { get; set; } = 2;

        public static BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(BindablePicker), new Thickness(7));
        public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BindablePicker), default(Color));

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        #endregion

        #region Methods

        #endregion

        public BindablePicker()
        {
        }

    }
}