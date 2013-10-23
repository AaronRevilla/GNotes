using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    class VideoObject:BaseObject
    {



        static VideoObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoObject), new FrameworkPropertyMetadata(typeof(VideoObject)));
        }
    }
}
