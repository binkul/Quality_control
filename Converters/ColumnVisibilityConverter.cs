using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Quality_Control.Converters
{
    public class ColumnVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = parameter.ToString();
            var columns = value as List<string>;

            if (columns.Contains(name))
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
