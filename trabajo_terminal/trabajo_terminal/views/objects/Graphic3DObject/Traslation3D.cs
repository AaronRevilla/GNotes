using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace GraphicNotes.Views.Objects
{
    class Traslation3D : Transformation3D, INotifyPropertyChanged
    {


        private double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        private double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        private double z;

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public Traslation3D(ObservableCollection<Transformation3D> transformations, Model3D baseFigure)
            : base(transformations, baseFigure)
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
                double px = paux.ElementAt(i).X;
                double py = paux.ElementAt(i).Y;
                double pz = paux.ElementAt(i).Z;
                temp.X = px + x;
                temp.Y = py + y;
                temp.Z = pz + z;
                newPoints.Add(temp);
            }
            transformatedFigure.LPoints = newPoints;
            transformatedFigure = new Model3D(newPoints, Color);
        }




        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
