using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace GraphicNotes.Views.Objects
{
    class CellBorder:Shape
    {
        public enum Border
        {
            Top=0,
            Right,
            Bottom,
            Left
        }

        public CellBorder()
            : base()
        {
            
            this.Stretch = System.Windows.Media.Stretch.Fill;
        }

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
          DependencyProperty.Register("Size", typeof(double),
                                      typeof(CellBorder),
                                      new FrameworkPropertyMetadata(0.0));

        protected override Geometry DefiningGeometry
        {
            get { return GetGeometry(); }
        }

        private Geometry GetGeometry()
        {

            return Geometry.Parse("M 0,0 h 100 v 100 h -100 v -100 Z");
        }

    }
}
