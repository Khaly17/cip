using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    /// <summary>
    ///     All kind of link types.
    /// </summary>
    public enum TypesLink
    {
        /// <summary>
        ///     The default link : execute the command binded.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     The email.
        /// </summary>
        Email,

        /// <summary>
        ///     The phone number.
        /// </summary>
        PhoneNumber,

        /// <summary>
        ///     The location.
        /// </summary>
        Location
    }

    /// <summary>
    ///     "Xamarin.Forms.Label" extended to get a kind of hyperlink.
    /// </summary>
    public class ExtHyperLink : Label
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExtHyperLink" /> class.
        /// </summary>
        public ExtHyperLink()
        {
            SubscribeClickToRaiseCommand();
        }

        #endregion

        /// <summary>
        ///     Subscribes the click to raise command.
        /// </summary>
        private void SubscribeClickToRaiseCommand()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                var args = new CancelEventArgs();
                Clicked?.Invoke(this, args);
                if (args.Cancel)
                    return;
                uint duration = 100;
                await this.ScaleTo(.75, duration);
                await this.ScaleTo(1, duration);

                if (TypeLink == TypesLink.Default)
                {
                    Command?.Execute(CommandParameter);
                }
                else if (TypeLink == TypesLink.PhoneNumber)
                {
                    var numTel = Text.Replace(".", string.Empty);
                    numTel = numTel.Replace(" ", string.Empty);
                    numTel = numTel.Replace("-", string.Empty);
                    Device.OpenUri(new Uri("tel://" + numTel));
                }
                else if (TypeLink == TypesLink.Email)
                {
                    Device.OpenUri(new Uri("mailto:" + Text));
                }
                else if (TypeLink == TypesLink.Location)
                {
                    Device.OpenUri(new Uri("geo:" + Text));
                }
                else
                {
                    throw new NotImplementedException("SubscribeClickToRaiseCommand -> TypeLink not implemented");
                }
            };
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        #region Bindable property

        /// <summary>
        ///     The type link property.
        /// </summary>
        public static BindableProperty TypeLinkProperty = BindableProperty.Create(nameof(TypeLink), typeof(TypesLink), typeof(ExtHyperLink), TypesLink.Default);

        /// <summary>
        ///     The command clicked property.
        /// </summary>
        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ExtHyperLink), null);
        public static BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ExtHyperLink), null);

        #endregion

        #region Property

        /// <summary>
        ///     Gets or sets the type link.
        /// </summary>
        /// <value>The type link.</value>
        public TypesLink TypeLink
        {
            get => (TypesLink)GetValue(TypeLinkProperty);
            set => SetValue(TypeLinkProperty, value);
        }

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
        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public event EventHandler<CancelEventArgs> Clicked;

        #endregion
    }
}
