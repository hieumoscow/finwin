using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinWin.Converters
{
    public class StartItemEnableColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (bool)value;
            return (v) ? App.GetResource<Color>("TextGrayColor")
                                : App.GetResource<Color>("TextLightGrayColor");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
