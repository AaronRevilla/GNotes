using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace GraphicNotes.Views.Objects
{
    public abstract class Transformation2D
    {

        protected Figure transformatedFigure;

        public Figure TransformatedFigure
        {
            get { return transformatedFigure; }
            set { transformatedFigure = value; }
        }
        private Color color = Colors.Black;
        private Boolean isActive=true;
        protected Figure baseFigure;

        public Figure BaseFigure
        {
            get { return baseFigure; }
            set { baseFigure = value; }
        }
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
        protected ObservableCollection<Transformation2D> transformations;
        public ObservableCollection<Transformation2D> Transformations
        {
            get { return transformations; }
            set { transformations = value; }
        }

        public Transformation2D(ObservableCollection<Transformation2D> transformations, Figure baseFigure)
        {

            this.transformations = transformations;
            this.baseFigure=baseFigure;
            color = NewRandomColor();
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
