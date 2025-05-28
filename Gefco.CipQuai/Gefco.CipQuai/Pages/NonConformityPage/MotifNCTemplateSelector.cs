using System;
using System.Collections.Generic;
using System.Text;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.NonConformityPage;
using Xamarin.Forms;

namespace Gefco.CipQuai.NonConformityPage
{
    public class MotifNCTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var name = ((MotifNCViewModel)item).MotifNC.Name;
            switch (name)
            {
                case "Autre":
                    return OtherItemTemplate;
            }
            return RegularItemTemplate;
        }

        public DataTemplate RegularItemTemplate { get; set; }
        public DataTemplate OtherItemTemplate { get; set; }
    }
}
