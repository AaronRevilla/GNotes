using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace GraphicNotes
{
    class ApplicationThemeProvider : DependencyObject
    {
        public string ApplicationThemeName
        {
            get { return (string)GetValue(ApplicationThemeNameProperty); }
            set { SetValue(ApplicationThemeNameProperty, value); }
        }
        public static readonly DependencyProperty ApplicationThemeNameProperty =
            DependencyProperty.Register("ApplicationThemeName", typeof(string), typeof(ApplicationThemeProvider), new PropertyMetadata("", null,
                                        new CoerceValueCallback((d, o) =>
                                        {
                                            return ThemeManager.ApplicationThemeName;
                                        })));

        public ApplicationThemeProvider()
        {
            ThemeManager.ThemeChanged += ThemeManager_ApplicationThemeChanged;
        }

        void ThemeManager_ApplicationThemeChanged(System.Windows.DependencyObject sender, ThemeChangedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                InvalidateProperty(ApplicationThemeNameProperty);
            }), DispatcherPriority.Background);
        }
    }
}
