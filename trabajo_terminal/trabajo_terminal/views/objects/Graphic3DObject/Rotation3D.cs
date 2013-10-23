using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace GraphicNotes.Views.Objects
{
    class Rotation3D : Transformation3D, INotifyPropertyChanged
    {
        private double angle;
        private string axis = "x";
        private FigureObject.AxisName axisname;

        internal FigureObject.AxisName Axisname
        {
            get { return axisname; }
            set { axisname = value; }
        }
        public Rotation3D(ObservableCollection<Transformation3D> transformations, Model3D baseFigure)
            : base(transformations, baseFigure )
        {
           
        }


        public string Axis
        {
            get { return axis; }
            set {
                if (axis != value) {
                    axis = value;
                    NotifyPropertyChanged("Axis");
                }
            }
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
            if (lastEnabled != null)
                paux = lastEnabled.TransformatedFigure.LPoints;
            else
                paux = baseModel.LPoints;

            ObservableCollection<Point3D> rotados = new ObservableCollection<Point3D>();

            switch (axis)
            {
                case "x":
                    for (int i = 0; i < paux.Count; i++)
                    {
                        Point3D aux = new Point3D();
                        aux.Y = (paux.ElementAt(i).Y) * (Math.Cos((angle * Math.PI) / 180)) - (paux.ElementAt(i).Z) * (Math.Sin((angle * Math.PI) / 180));
                        aux.Z = (paux.ElementAt(i).Y) * (Math.Sin((angle * Math.PI) / 180)) + (paux.ElementAt(i).Z) * (Math.Cos((angle * Math.PI) / 180));
                        aux.X = paux.ElementAt(i).X;
                        rotados.Add(aux);
                    }
                    break;
                case "y":
                    for (int i = 0; i < paux.Count; i++)
                    {
                        Point3D aux = new Point3D();
                        aux.Y = paux.ElementAt(i).Y;
                        aux.Z = (paux.ElementAt(i).Z) * (Math.Cos((angle * Math.PI) / 180)) - (paux.ElementAt(i).X) * (Math.Sin((angle * Math.PI) / 180));
                        aux.X = (paux.ElementAt(i).Z) * (Math.Sin((angle * Math.PI) / 180)) + (paux.ElementAt(i).X) * (Math.Cos((angle * Math.PI) / 180));
                        rotados.Add(aux);
                    }
                    break;
                case "z":
                    for (int i = 0; i < paux.Count; i++)
                    {
                        Point3D aux = new Point3D();
                        aux.X = (paux.ElementAt(i).X) * (Math.Cos((angle * Math.PI) / 180)) - (paux.ElementAt(i).Y) * (Math.Sin((angle * Math.PI) / 180));
                        aux.Y = (paux.ElementAt(i).X) * (Math.Sin((angle * Math.PI) / 180)) + (paux.ElementAt(i).Y) * (Math.Cos((angle * Math.PI) / 180));
                        aux.Z = paux.ElementAt(i).Z;
                        rotados.Add(aux);
                    }
                    break;
                default:
                    break;
            }
            transformatedFigure.LPoints = rotados;
            transformatedFigure = new Model3D(rotados, Color);
        }

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
