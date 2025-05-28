using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    public class ExtImage : SvgCachedImage
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExtImage" /> class.
        /// </summary>
        public ExtImage()
        {
            // this.SetBinding(ExtButton.IsEnabledProperty, new Binding("CanExecute", BindingMode.OneWay));
            SubscribeClickToRaiseCommand();
        }

        #endregion

        /// <summary>
        ///     Raises the html changed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void WhenIsImageOpaqueChanged(BindableObject sender, object oldValue, object newValue)
        {
            var ctrl = (ExtImage)sender;
            ctrl.Opacity = (bool)newValue ? 0.25 : 1.0;
        }

        /// <summary>
        ///     Subscribes the click to raise command.
        /// </summary>
        private void SubscribeClickToRaiseCommand()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                if (Command != null && Command.CanExecute(CommandParameter))
                {
                    uint duration = 100;
                    await this.ScaleTo(.75, duration);
                    await this.ScaleTo(1, duration);

                    Command.Execute(CommandParameter);
                }
            };

            GestureRecognizers.Add(tapGestureRecognizer);
        }

        #region Bindable property

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ExtImage), null);

        public static BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ExtImage), null);

        public static BindableProperty IsImageOpaqueProperty = BindableProperty.Create(nameof(IsImageOpaque), typeof(bool), typeof(ExtImage), false, propertyChanged: WhenIsImageOpaqueChanged);

        #endregion

        #region Property

        /// <summary>
        ///     Gets or sets the command clicked.
        /// </summary>
        /// <value>The command clicked.</value>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        ///     Gets or sets the command parameter.
        /// </summary>
        /// <value>The command parameter.</value>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this image is opaque.
        /// </summary>
        /// <value><c>true</c> if this instance is image opaque; otherwise, <c>false</c>.</value>
        public bool IsImageOpaque
        {
            get => (bool)GetValue(IsImageOpaqueProperty);
            set => SetValue(IsImageOpaqueProperty, value);
        }

        #endregion
    }
}
