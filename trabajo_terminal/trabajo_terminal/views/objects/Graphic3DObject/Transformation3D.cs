using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace GraphicNotes.Views.Objects
{
    public abstract class Transformation3D
    {
       protected Model3D transformatedFigure;
       protected Model3D baseModel;
       public Model3D TransformatedFigure
        {
            get { return transformatedFigure; }
            set { transformatedFigure = value; }
        }
       private Color color = Colors.Black;
       private Boolean isActive = true;
       public Boolean IsActive
       {
           get { return isActive; }
           set { isActive = value; }
       }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        protected ObservableCollection<Transformation3D> transformations;
        public ObservableCollection<Transformation3D> Transformations
        {
            get { return transformations; }
            set { transformations = value; }
        }

        public Transformation3D(ObservableCollection<Transformation3D> transformations, Model3D baseFigure)
        {

            this.transformations = transformations;
            this.baseModel = baseFigure;
            color = NewRandomColor();
            transformatedFigure = new Model3D();
        }

        public Color NewRandomColor(){
            Random r = new Random(DateTime.Now.Millisecond);
            byte Red = (byte)r.Next(0,256);
            byte Blue = (byte)r.Next(0, 256);
            byte Green = (byte)r.Next(0, 256);
            return Color.FromArgb(255, Red, Green, Blue);
        }


        public abstract void DoTransformation();
     
    }
}
