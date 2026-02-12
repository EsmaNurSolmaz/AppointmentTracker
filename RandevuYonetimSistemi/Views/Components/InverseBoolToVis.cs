using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RandevuYonetimSistemi.Views.Components
{
    public class InverseBoolToVis : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && b ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
