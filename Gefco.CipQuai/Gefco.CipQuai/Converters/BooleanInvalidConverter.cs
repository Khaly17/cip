using System;
using System.Globalization;
using Gefco.CipQuai.DoubleDeckPage;
using Xamarin.Forms;

namespace Gefco.CipQuai.Converters
{
    public class BooleanInvalidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? (Color)App.Current.Resources["InvalidColor"] : (Color)App.Current.Resources["ValidColor"];
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class NullValidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? (Color)App.Current.Resources["NullValidColor"] : (Color)App.Current.Resources["InvalidColor"];
            return (Color)App.Current.Resources["NullColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class SyncValidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? (Color)App.Current.Resources["InvalidColor"] : (Color)App.Current.Resources["ColorAccent"];
            return (Color)App.Current.Resources["ColorAccent"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class BooleanValidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b ? (Color)App.Current.Resources["InvalidColor"] : (Color)App.Current.Resources["ValidColor"];
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class ErrorPictureEnumSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ErrorPictureEnum b && parameter is ErrorPictureEnum target)
                return b != target ? Color.Transparent : (Color)App.Current.Resources["ValidColor"];
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class PictureEnumSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PictureEnum b && parameter is PictureEnum target)
                return b != target ? Color.Transparent : (Color)App.Current.Resources["ValidColor"];
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}