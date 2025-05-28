using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.ProfilePage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifierLastNamePage
	{
        private readonly ViewModel _viewModel;

	    public ModifierLastNamePage(ViewModel viewModel)
		{
		    InitializeComponent();
            _viewModel = viewModel;
		    BindingContext = viewModel;
		}
        private void OnAppearing(object sender, EventArgs e)
	    {
	        MyEntry.Focus();
	    }

    }
}