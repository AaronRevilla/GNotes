using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace GraphicNotes.Views.Objects
{

    class MatrixTransformation3D : Transformation3D, INotifyPropertyChanged
    {

        public MatrixTransformation3D(ObservableCollection<Transformation3D> transformations, Model3D baseFigure)
            : base(transformations, baseFigure)
        {
            x = new double[9];
            val= new string[9];
            for (int i = 0; i < 9; i++) {
                val[i] = "";
                x[i] = 0;
            }
        }
        double[] x;

        public double[] X
        {
            get { return x; }
            set { x = value; }
        }
        string[] val;

        public string[] Val
        {
            get { return val; }
            set { val = value; }
        }

        public override void DoTransformation()
        {
            ObservableCollection<Point3D> paux;
            paux = baseModel.LPoints;
            Transformation3D lastEnabled = null;
            int index = transformations.IndexOf(this);
            for (int i = index - 1; i >= 0; i--)
            {
                lastEnabled = transformations.ElementAt(i);
                if (lastEnabled.IsActive)
                    break;
            }

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
                temp.X = px * x[0] + py * x[1] + pz * x[2] +  x[3];
                temp.Y = px * x[4] + py * x[5] + pz * x[6] +  x[7];
                temp.Z = px * x[8] + py * x[9] + pz * x[10] + x[11];
                newPoints.Add(temp);
            }

        }

        private double[] getValues()
        {
            double[] values = new double[9];
            for (int j = 0; j < 9; j++)
            {
                if (val[j].Contains("sin"))
                {
                    int indx = val[j].IndexOf('(');
                    int indxf = val[j].IndexOf(')');
                    double v = Double.Parse(val[j].Substring(indx + 1, indxf - indx - 1));
                    if (val[j].ElementAt(0) == '-')
                        values[j] = -Math.Sin(v * Math.PI / 180);
                    else
                        values[j] = Math.Sin(v * Math.PI / 180);

                }
                else if (val[j].Contains("cos"))
                {
                    int indx = val[j].IndexOf('(');
                    int indxf = val[j].IndexOf(')');
                    double v = Double.Parse(val[j].Substring(indx + 1, indxf - indx - 1));
                    if (val[j].ElementAt(0) == '-')
                        values[j] = -Math.Cos(v * Math.PI / 180);
                    else
                        values[j] = Math.Cos(v * Math.PI / 180);

                }
                else
                {
                    values[j] = Double.Parse(val[j]);
                }
            }
            return values;

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
