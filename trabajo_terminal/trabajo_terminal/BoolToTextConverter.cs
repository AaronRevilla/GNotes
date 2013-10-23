using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GraphicNotes
{
    public class BoolToTextConverter : IValueConverter
    {
        private String trueText = "";
        private String falseText = "";

        public BoolToTextConverter() { }

        public BoolToTextConverter(String trueText, String falseText)
        {
            this.trueText = trueText;
            this.falseText = falseText;
        }
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Console.WriteLine("Converter working");
            if (value == null)
                return falseText;

            bool val = (bool)value;
            if (val == true)
                return trueText;
            else
                return falseText;

        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            String val = value.ToString();
            if (val.Equals(trueText))
                return trueText;
            else
                return falseText;
        }
    }
}
