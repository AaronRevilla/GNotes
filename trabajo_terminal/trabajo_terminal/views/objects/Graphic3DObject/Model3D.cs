using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GraphicNotes.Views.Objects
{
    public class Model3D
    {
        private Color color;
        ObservableCollection<Point3D> lPoints;

        public ObservableCollection<Point3D> LPoints
        {
            get { return lPoints; }
            set { lPoints = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Model3D()
        {
            color = Colors.Black;
            lPoints = new ObservableCollection<Point3D>();
        }

        public Model3D(ObservableCollection<Point3D> form)
        {
            this.lPoints = form;
            color = Colors.Black;
        }

        public Model3D(ObservableCollection<Point3D> form, Color c)
        {
            this.lPoints = form;
            this.color = c;
        }

    }
}
