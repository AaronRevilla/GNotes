using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using GNTools;

namespace GraphicNotes.Views.Adorners
{
    class LockedChrome:Control
    {
        static LockedChrome()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(LockedChrome), new FrameworkPropertyMetadata(typeof(LockedChrome)));
        }

        public LockedChrome()
        {

            
        }
    }
}
