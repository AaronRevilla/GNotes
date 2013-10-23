using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GraphicNotes
{
    class EnumToObjectConverter:IValueConverter
    {

        private object[] objects;

        public EnumToObjectConverter(params object[] args)
        {
            this.objects = args;
        }


        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value == null)
                return objects[0];

            var _enum = Cast(value, value.GetType());
            int index=(int) _enum;

            if (index < objects.Length)
                return objects[index];
            else
                return objects[0];

        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }


        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }

    }
}
