using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TraktorTagger
{

    [ValueConversion(typeof(Key), typeof(string))]
    public class KeyEnumConverter : IValueConverter
    {   
        //convert string to
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null) return null;
            else return (KeyEnumStringConverter.ConvertToString((Key)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (KeyEnumStringConverter.ConvertFromString((string)value));
        }
    }
}
