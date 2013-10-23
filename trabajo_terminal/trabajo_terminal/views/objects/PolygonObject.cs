using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    class PolygonObject:BaseObject
    {
        static PolygonObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PolygonObject), new FrameworkPropertyMetadata(typeof(PolygonObject)));
        }
    }
}
