using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    public class ExtEntry : Entry
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExtEntry" /> class.
        /// </summary>
        public ExtEntry()
        {
            Completed += (sender, eventArgs) => { OnCompleted(); };
            Instances.Add(this);
            Focused += OnFocused;
            Unfocused += OnFocused;
        }

        private void OnFocused(object sender, FocusEventArgs focusEventArgs)
        {
            IsValidAndFocused = !IsInvalid && IsFocused;
        }

        #endregion

        /// <summary>
        ///     When the entry is completed.
        /// </summary>
        /// <returns>The completed.</returns>
        private void OnCompleted()
        {
            if (Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        #region Bindable property

        /// <summary>
        ///     The remove border property.
        /// </summary>

        #region IsInvalid

        static readonly BindablePropertyKey IsInvalidPropertyKey = BindableProperty.CreateReadOnly(nameof(IsInvalid), typeof(bool), typeof(ExtEntry), false);

        public static readonly BindableProperty IsInvalidProperty = IsInvalidPropertyKey.BindableProperty;
        public bool IsInvalid
        {
            get => (bool)GetValue(IsInvalidProperty);
            set => SetValue(IsInvalidPropertyKey, value);
        }

        #endregion

        #region IsValidAndFocused

        static readonly BindablePropertyKey IsValidAndFocusedPropertyKey = BindableProperty.CreateReadOnly(nameof(IsValidAndFocused), typeof(bool), typeof(ExtEntry), false);

        public static readonly BindableProperty IsValidAndFocusedProperty = IsValidAndFocusedPropertyKey.BindableProperty;
        public bool IsValidAndFocused
        {
            get => (bool)GetValue(IsValidAndFocusedProperty);
            set => SetValue(IsValidAndFocusedPropertyKey, value);
        }

        #endregion

        #region Padding

        public static BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ExtEntry), new Thickness(4));
        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        #endregion

        public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ExtEntry), default(Color));
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        /// <summary>
        ///     The command property.
        /// </summary>
        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ExtEntry));

        /// <summary>
        ///     The command parameter property.
        /// </summary>
        public static BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ExtEntry));

        #endregion

        #region Property

        public static BindableProperty RemoveBorderProperty = BindableProperty.Create(nameof(RemoveBorder), typeof(bool), typeof(ExtEntry), false);

        /// <summary>
        ///     Gets or sets the remove border.
        /// </summary>
        /// <value>The remove border.</value>
        public bool RemoveBorder
        {
            get => (bool)GetValue(RemoveBorderProperty);
            set => SetValue(RemoveBorderProperty, value);
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
        /// <value>The command parameter.</value>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static List<ExtEntry> Instances { get; } = new List<ExtEntry>();

        #region CornerRadius

        static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(ExtEntry), 2f);

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion
        public int BorderWidth { get; set; } = 2;

        public bool IsEditingOutBlocked { get; set; }

        public static void UnfocusAll()
        {
            foreach (var entry in Instances)
            {
                entry.Unfocus();
            }
        }



        #endregion
    }
}
