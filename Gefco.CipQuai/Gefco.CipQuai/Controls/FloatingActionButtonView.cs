using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    public enum FloatingActionButtonSize
    {
        Normal,
        Mini
    }

    /// <summary>
    ///     Used only in Android, to present the floating button in the list view. See the accompanying custom renderer in the
    ///     Android platform project.
    /// </summary>
    public class FloatingActionButtonView : View
    {
        public delegate void AttachToListViewDelegate(ListView listView);

        public delegate void ShowHideDelegate(bool animate = true);

        public static readonly BindableProperty ImageNameProperty = BindableProperty.Create(nameof(ImageName), typeof(string), typeof(FloatingActionButtonView), string.Empty);

        public static readonly BindableProperty ColorNormalProperty = BindableProperty.Create(nameof(ColorNormal), typeof(Color), typeof(FloatingActionButtonView), Color.White);

        public static readonly BindableProperty ColorPressedProperty = BindableProperty.Create(nameof(ColorPressed), typeof(Color), typeof(FloatingActionButtonView), Color.White);

        public static readonly BindableProperty ColorRippleProperty = BindableProperty.Create(nameof(ColorRipple), typeof(Color), typeof(FloatingActionButtonView), Color.White);

        public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(FloatingActionButtonSize), typeof(FloatingActionButtonView), FloatingActionButtonSize.Normal);

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(FloatingActionButtonView), true);

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FloatingActionButtonView), null);

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FloatingActionButtonView), null);

        public string ImageName
        {
            get => (string) GetValue(ImageNameProperty);
            set => SetValue(ImageNameProperty, value);
        }

        public Color ColorNormal
        {
            get => (Color) GetValue(ColorNormalProperty);
            set => SetValue(ColorNormalProperty, value);
        }

        public Color ColorPressed
        {
            get => (Color) GetValue(ColorPressedProperty);
            set => SetValue(ColorPressedProperty, value);
        }

        public Color ColorRipple
        {
            get => (Color) GetValue(ColorRippleProperty);
            set => SetValue(ColorRippleProperty, value);
        }

        public FloatingActionButtonSize Size
        {
            get => (FloatingActionButtonSize) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public bool HasShadow
        {
            get => (bool) GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        public ShowHideDelegate Show { get; set; }
        public ShowHideDelegate Hide { get; set; }
        public Action<object, EventArgs> Clicked { get; set; }


        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
    }
}