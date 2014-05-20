#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace Hoover.Converters
{
    /// <summary>
    /// This converter is to convert a true boolean value to a visible visibility, and 
    /// for a false boolean value it returns a collapsed visibility.
    /// </summary>
    public sealed class BoolToNegateVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return Visibility.Collapsed;

            return (bool) value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}