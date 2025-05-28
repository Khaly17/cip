using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.BestPracticePage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaisieDescriptionBPPage
	{
        private readonly BPViewModel _bpViewModel;

	    public SaisieDescriptionBPPage(BPViewModel bpViewModel)
		{
		    InitializeComponent();
            _bpViewModel = bpViewModel;
		    BindingContext = bpViewModel;
		}
        private void OnAppearing(object sender, EventArgs e)
	    {
	        MyEntry.Focus();
	    }

    }
}