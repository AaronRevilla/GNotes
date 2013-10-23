using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    class MatrixTransformation2D : Transformation2D, INotifyPropertyChanged
    {
        public MatrixTransformation2D(ObservableCollection<Transformation2D> transformations, Figure baseFigure)
            : base(transformations, baseFigure)
        {
            x = new double[9];
            for (int i = 0; i < 9; i++) {

                x[i] = 0;
            }
        }

        double[] x;

        public double[] X
        {
            get { return x; }
            set { x = value; }
        }

        public override void DoTransformation()
        {
         
            ObservableCollection<Point> paux;
            paux = baseFigure.LPoints;
            ObservableCollection<Point> newPoints = new ObservableCollection<Point>();

            for (int i = 0; i < paux.Count(); i++)
            {
                double px = paux.ElementAt(i).X;
                double py = paux.ElementAt(i).Y;
                Point p = new Point();
                p.X = x[0] * px + x[1] * py + x[2];
                p.Y = x[3] * px + x[4] * py + x[5];
                newPoints.Add(p);
            }
            transformatedFigure = new Figure(newPoints, Color);
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
