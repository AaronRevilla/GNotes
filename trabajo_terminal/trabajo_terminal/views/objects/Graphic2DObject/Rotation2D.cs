using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    class Rotation2D:Transformation2D, INotifyPropertyChanged
    {
        private double angle;
        public double Angle
        {
            get { return angle; }
            set
            {
                if (angle != value)
                {
                    angle = value;
                    NotifyPropertyChanged("Angle");
                }
            
            }
        }

        public Rotation2D(ObservableCollection<Transformation2D> transformations, Figure baseFigure)
            : base(transformations, baseFigure )
        {

        }

        public override void DoTransformation()
        {
            Transformation2D lastEnabled = null;
            int index = transformations.IndexOf(this);
            for (int i = index-1; i >= 0; i--)
            {
                lastEnabled = transformations.ElementAt(i);
                if (lastEnabled.IsActive)
                    break;
            }

            ObservableCollection<Point> paux;
            if (lastEnabled != null)
                paux = lastEnabled.TransformatedFigure.LPoints;
            else
                paux = baseFigure.LPoints;

            ObservableCollection<Point> newPoints=new ObservableCollection<Point>();
            
            for (int i = 0; i < paux.Count(); i++)
            {
                Point temp = new Point();
                double px = paux.ElementAt(i).X;
                double py = paux.ElementAt(i).Y;
                temp.X = Math.Round((Math.Cos(angle * Math.PI / 180) * (px)) - (Math.Sin(angle * Math.PI / 180) * (py)), 3);
                temp.Y = Math.Round((Math.Sin(angle * Math.PI / 180) * (px)) + (Math.Cos(angle * Math.PI / 180) * (py)), 3);
                newPoints.Add(temp);
            }
           transformatedFigure=new Figure(newPoints,Color);
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
