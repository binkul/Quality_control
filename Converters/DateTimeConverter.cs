using System;
using System.Globalization;
using System.Windows.Data;

namespace Quality_Control.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        private const string _format = "dd-MM-yyyy";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;

            return date.ToString(_format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return System.Convert.ToDateTime(value.ToString());
            //return DateTime.ParseExact(tmp _format, culture);
        }
    }
}
