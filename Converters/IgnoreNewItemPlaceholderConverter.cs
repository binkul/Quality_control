using System;
using System.Globalization;
using System.Windows.Data;

namespace Quality_Control.Converters
{
    public class IgnoreNewItemPlaceholderConverter : IValueConverter
    {
        private const string newItemPlaceholderName = "{NewItemPlaceholder}";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.ToString().Equals(newItemPlaceholderName))
                return null;
            else
                return value;
        }
    }
}
