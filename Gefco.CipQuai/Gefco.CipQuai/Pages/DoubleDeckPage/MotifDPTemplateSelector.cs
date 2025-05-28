using System;
using System.Collections.Generic;
using System.Text;
using Gefco.CipQuai.ApiClient.Models;
using Xamarin.Forms;

namespace Gefco.CipQuai.DoubleDeckPage
{
    public class MotifDPTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is MotifDPViewModel model)
            if (model.MotifDP.IsNbDP)
                return DPCasseeItemTemplate;
            else if (model.MotifDP.IsOther)
                {
                return OtherItemTemplate;
            }
            return RegularItemTemplate;
        }

        public DataTemplate RegularItemTemplate { get; set; }
        public DataTemplate DPCasseeItemTemplate { get; set; }
        public DataTemplate OtherItemTemplate { get; set; }
    }
}
