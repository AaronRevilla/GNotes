using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    class Scalation2D : Transformation2D, INotifyPropertyChanged
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
        public Scalation2D(ObservableCollection<Transformation2D> transformations, Figure baseFigure)
            : base(transformations, baseFigure)
        {

        }

        public override void DoTransformation()
        {
            Transformation2D lastEnabled = null;
            int index = transformations.IndexOf(this);
            for (int i = index - 1; i >= 0; i--)
            {
                lastEnabled = transformations.ElementAt(i);
                if (lastEnabled.IsActive)
                    break;
            }

            ObservableCollection<Point> paux;
            if (lastEnabled != null && lastEnabled.IsActive)
                paux = lastEnabled.TransformatedFigure.LPoints;
            else
                paux = baseFigure.LPoints;

            ObservableCollection<Point> newPoints = new ObservableCollection<Point>();

            for (int i = 0; i < paux.Count(); i++)
            {
                Point temp = new Point();
                double px = paux.ElementAt(i).X;
                double py = paux.ElementAt(i).Y;
                temp.X = px * this.Px;
                temp.Y = py * this.Py;
                newPoints.Add(temp);
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
