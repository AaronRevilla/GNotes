using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace GraphicNotes.Views.Objects
{
    public class Figure
    {
        private Color color;
        ObservableCollection<Point> lPoints;

        public ObservableCollection<Point> LPoints
        {
            get { return lPoints; }
            set { lPoints = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Figure()
        {
            color = Colors.Black;
            lPoints = new ObservableCollection<Point>();
        }

        public Figure(ObservableCollection<Point> form)
        {
            this.lPoints = form;
            color = Colors.Black;
        }

        public Figure(ObservableCollection<Point> form, Color c)
        {
            this.lPoints = form;
            this.color = c;
        }
    }
}
