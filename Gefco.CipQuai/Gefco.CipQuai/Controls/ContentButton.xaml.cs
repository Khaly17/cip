using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContentButton
    {
        public ContentButton()
        {
            InitializeComponent();
            Padding = 0;
        }

        public int BorderWidth { get; set; } = 1;

        public event EventHandler Tapped;

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContentButton), null, BindingMode.OneWay, null, null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public bool AnimateOnTap { get; set; } = true;

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            try
            {
                uint duration = 120;
                Tapped?.Invoke(this, new EventArgs());
                if (AnimateOnTap)
                {
                    await this.ScaleTo(.75, duration);
                    await this.ScaleTo(1, duration);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}