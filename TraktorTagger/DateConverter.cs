using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TraktorTagger
{
    [ValueConversion(typeof(DateTime?), typeof(string))]
    class DateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            DateTime? dt = (DateTime?)value;
            string format= (string)parameter;

            if(dt.HasValue) return dt.Value.ToString(format);
            else return null;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string dateStr = (string)value;

            if(String.IsNullOrEmpty(dateStr)) return null;
            else return DateTime.Parse(dateStr);
        }
    }
}
