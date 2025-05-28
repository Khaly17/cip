using Gefco.CipQuai.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Gefco.CipQuai.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}