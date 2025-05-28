using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Gefco.CipQuai.Menu;
using Xamarin.Forms;

namespace Gefco.CipQuai.Converters
{
    public class MenuPageItemSelectedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MenuPageItem item)
            {
                if (item.IsSelected)
                    return (Color)App.Current.Resources["MenuSelectedColorForeground"];
                return (Color)App.Current.Resources["MenuNotselectedColorForeground"];
            }
            if (value is bool val)
            {
                return val ? (Color)App.Current.Resources["MenuSelectedColorForeground"] : (Color)App.Current.Resources["MenuNotselectedColorForeground"];
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
