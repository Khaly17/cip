using System;
using System.Text.RegularExpressions;
using CommonServiceLocator;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Services;
using Newtonsoft.Json;
using Telerik.XamarinForms.Chart;
using Xamarin.Forms;

namespace Gefco.CipQuai.DoubleDeckPage
{
    public partial class DoubleDeckPage : ContentPage
    {
        public DoubleDeckPage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance(typeof(ViewModel));
        }

        public ViewModel ViewModel => BindingContext as ViewModel;

        private void FirstNameEntry_OnFocused(object sender, FocusEventArgs e)
        {
            FirstNameEntry.IsValidAndFocused = !ViewModel.IsInvalid;
        }

        private void FirstNameEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Validate();
        }

        public void Reset()
        {
            ViewModel.Reset();
        }
    }
}
