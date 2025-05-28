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
	public partial class VerifierCodePinPage
	{
        private readonly ViewModel _viewModel;

	    public VerifierCodePinPage(ViewModel viewModel)
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