using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AdventureWork.Converters
{
    public class ThresholdColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal total && total > 100)
                return (Application.Current?.Resources["AccentColor"] as Color) ?? Colors.Orange;

            return (Application.Current?.Resources["TextColor"] as Color) ?? Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
