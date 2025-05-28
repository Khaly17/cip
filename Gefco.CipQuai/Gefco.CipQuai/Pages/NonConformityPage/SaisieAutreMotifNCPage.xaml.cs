using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.NonConformityPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaisieAutreMotifNCPage
	{
        private readonly NCViewModel _ncViewModel;
        private readonly MotifNCViewModel _motifNCModel;

	    public SaisieAutreMotifNCPage(NCViewModel ncViewModel, MotifNCViewModel motifNCModel)
		{
		    InitializeComponent();
            _ncViewModel = ncViewModel;
            _motifNCModel = motifNCModel;
		    BindingContext = ncViewModel;
		}
        private void OnAppearing(object sender, EventArgs e)
	    {
	        MyEntry.Focus();
	    }
	    protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
	        if (!_ncViewModel.IsValidAutreMotifNC || !_ncViewModel.IsSuccessInput)
	        {
	            _ncViewModel.AutreMotifNC = null;
	            _motifNCModel.IsChecked = false;
	        }
        }

    }
}