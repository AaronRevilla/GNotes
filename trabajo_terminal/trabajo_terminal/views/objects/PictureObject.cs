using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    class PictureObject:BaseObject
    {
        static PictureObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PictureObject), new FrameworkPropertyMetadata(typeof(PictureObject)));
        }
    }
}
