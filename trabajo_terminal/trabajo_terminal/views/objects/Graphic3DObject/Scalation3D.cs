using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace GraphicNotes.Views.Objects
{
    class Scalation3D : Traslation3D, INotifyPropertyChanged
    {
        private double px;
        public double Px
        {
            get { return px; }
            set { px = value; }
        }
        private double py;
        public double Py
        {
            get { return py; }
            set { py = value; }
        }
        private double pz;
        public double Pz
        {
            get { return pz; }
            set { pz = value; }
        }
        
        public Scalation3D(ObservableCollection<Transformation3D> transformations, Model3D baseFigure)
            : base(transformations, baseFigure )
        {

        }

        public override void DoTransformation()
        {
            Transformation3D lastEnabled = null;
            int index = transformations.IndexOf(this);
            for (int i = index - 1; i >= 0; i--)
            {
                lastEnabled = transformations.ElementAt(i);
                if (lastEnabled.IsActive)
                    break;
            }
            ObservableCollection<Point3D> paux;
            if (lastEnabled != null && lastEnabled.IsActive)
                paux = lastEnabled.TransformatedFigure.LPoints;
            else
                paux = baseModel.LPoints;

            ObservableCollection<Point3D> newPoints = new ObservableCollection<Point3D>();
            for (int i = 0; i < paux.Count(); i++)
            {
                Point3D temp = new Point3D();
                double x = paux.ElementAt(i).X;
                double y = paux.ElementAt(i).Y;
                double z = paux.ElementAt(i).Z;
                temp.X = x * Px;
                temp.Y = y * Py;
                temp.Z = z * Pz;
                newPoints.Add(temp);
            }
            transformatedFigure.LPoints = newPoints;
            transformatedFigure = new Model3D(newPoints, Color);
        }
    }
}
