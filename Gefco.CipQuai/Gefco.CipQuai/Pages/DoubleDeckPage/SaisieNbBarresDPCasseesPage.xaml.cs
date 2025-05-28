using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.DoubleDeckPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaisieNbBarresDPCasseesPage
	{
        private readonly DPViewModel _dpViewModel;
        private readonly MotifDPViewModel _motifDPModel;

	    public SaisieNbBarresDPCasseesPage(DPViewModel dpViewModel, MotifDPViewModel motifDPModel)
		{
            this._dpViewModel = dpViewModel;
            _motifDPModel = motifDPModel;
		    InitializeComponent ();
		    BindingContext = dpViewModel;
		}

	    private void ValidateNbDPCassées(object sender, EventArgs e)
	    {
	        
	    }
	    private void OnAppearing(object sender, EventArgs e)
	    {
	        MyEntry.Focus();
	    }

	    private void MyEntry_OnFocused(object sender, FocusEventArgs e)
	    {
	        if (sender is ExtEntry entry)
	        {
	            if (entry.Text == null)
	                return;
	            entry.CursorPosition = 0;
	            entry.SelectionLength = entry.Text.Length;
	        }
	    }

	    protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
	        if (!_dpViewModel.IsValidNbDPCassées || !_dpViewModel.IsSuccessInput)
	        {
	            _dpViewModel.NbDPCassées = null;
	            _motifDPModel.IsChecked = false;
	        }
	    }

    }
}