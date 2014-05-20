#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace Hoover.Converters
{
    /// <summary>
    /// Used for negating a boolean value.
    /// </summary>
    public sealed class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return false;

            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return false;

            return !(bool) value;
        }
    }
}
