using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace GraphicNotes
{
    public class BarItemsColorConverter : MarkupExtension, IValueConverter
    {
        public BarItemsColorConverter() { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((string)value)
            {
                case Theme.MetropolisDarkName:
                    return Brushes.White;
                case Theme.MetropolisLightName:
                    return Brushes.Orange;
                case Theme.DeepBlueName:
                    return new SolidColorBrush(Color.FromRgb(0x00, 0x33, 0x66));
                case Theme.VS2010Name:
                    return Brushes.BlueViolet;
                default:
                    return new SolidColorBrush(Color.FromRgb(0x35, 0x35, 0x35));
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

    }
}
