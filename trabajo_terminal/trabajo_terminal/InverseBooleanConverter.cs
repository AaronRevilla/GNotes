using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GraphicNotes
{
    class InverseBooleanConverter:IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Console.WriteLine("Converter working");
            if (value == null)
                return true;

            bool val = (bool)value;
            return !val;

        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            bool val = (bool)value;
            return !val;
        }
    }
}
