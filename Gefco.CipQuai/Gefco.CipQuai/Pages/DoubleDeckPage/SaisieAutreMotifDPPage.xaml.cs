using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.DoubleDeckPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaisieAutreMotifDPPage
	{
        private readonly DPViewModel _dpViewModel;
        private readonly MotifDPViewModel _motifDPModel;

	    public SaisieAutreMotifDPPage(DPViewModel dpViewModel, MotifDPViewModel motifDPModel)
		{
		    InitializeComponent();
            _dpViewModel = dpViewModel;
            _motifDPModel = motifDPModel;
		    BindingContext = dpViewModel;
		}
        private void OnAppearing(object sender, EventArgs e)
	    {
	        MyEntry.Focus();
	    }
	    protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
	        if (!_dpViewModel.IsValidAutreMotifDP || !_dpViewModel.IsSuccessInput)
	        {
	            _dpViewModel.AutreMotifDP = null;
	            _motifDPModel.IsChecked = false;
	        }
        }

    }
}