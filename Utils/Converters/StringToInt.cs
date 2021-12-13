using System;
using System.Globalization;
using System.Windows.Data;

namespace Inventory_Management.Utils.Converters
{
    public class StringToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
                return intValue.ToString();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sValue = value?.ToString();
            return int.TryParse(sValue, out var parsedInt) ? parsedInt : 0;
        }
    }
}