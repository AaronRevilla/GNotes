using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicNotes.Views.Objects
{
    class Canvas2D:Canvas
    {
        public Canvas2D()
        {
            this.Loaded += Canvas2D_Loaded;
        }

        void Canvas2D_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateLayout();
        }
        //This methos was overwritten, is a Canvas method
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            DrawLines(dc);
            drawAxis(dc);
            numerateAxis(dc);
        }
        //This method draw all the lines defined wich represents all the coordinates
        private void DrawLines(DrawingContext dc)
        {
            Pen pen = new Pen(Brushes.SkyBlue, 1);
            double dx = this.ActualWidth / 50;
            double dy = this.ActualHeight / 50;
            for (int i = 0; i < 51; i++)
            {
                dc.DrawLine(pen, new Point(dx * i, 0), new Point(dx * i, this.ActualHeight));
            }
            for (int i = 0; i < 51; i++)
            {
                dc.DrawLine(pen, new Point(0, dy * i), new Point(this.ActualWidth, dy * i));
            }
        }
        //This method wirte all the numbres in the board
        private void numerateAxis(DrawingContext dc)
        {
            double dx = this.ActualWidth / 50;
            double dy = this.ActualHeight / 50;
            double halfx = this.ActualWidth / 2;
            double halfy = this.ActualHeight / 2;
            FormattedText ft;
            for (int i = 0; i < 50; i++)
            {
                double coordinate = (i * dx) - halfx;
                ft = doText(Math.Round(coordinate / dx) + "");
                dc.DrawText(ft, new Point(dx * i, halfy));
            }

            for (int i = 0; i < 50; i++)
            {
                double coordinate = halfy - (i * dy);
                ft = doText(Math.Round(coordinate / dy) + "");
                dc.DrawText(ft, new Point(halfx, dy * i));
            }
        }
        //This method is used by numerateAxis method
        private FormattedText doText(String txt)
        {
            FormattedText formattedText = new FormattedText(txt,
                                    CultureInfo.GetCultureInfo("en-us"),
                                    FlowDirection.LeftToRight,
                                    new Typeface("Verdana"),
                                    8,
                                    Brushes.Black);
            return formattedText;
        }
        //This method clear the board
        public void Clear()
        {
            this.Children.Clear();
        }
        //this method is used by drawAxis method to draw the X axis
        private void DrawXaxis(DrawingContext dc)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            dc.DrawLine(pen, new Point(0, (this.ActualHeight / 2)), new Point(this.ActualWidth, (this.ActualHeight / 2)));
        }
        //this method is used by drawAxis method to draw the Y axis
        private void DrawYaxis(DrawingContext dc)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            dc.DrawLine(pen, new Point((this.ActualWidth / 2), 0), new Point((this.ActualWidth / 2), this.ActualHeight));
        }
        //this metod draws all the axis in the board
        private void drawAxis(DrawingContext dc)
        {
            DrawXaxis(dc);
            DrawYaxis(dc);
        }

        public void paintFigure(Figure figure)
        {
            ObservableCollection<Line> lines = drawFigure(normalize(figure.LPoints));
            for (int i = 0; i < lines.Count(); i++)
            {
                Line laux = lines.ElementAt(i);
                laux.HorizontalAlignment = HorizontalAlignment;
                laux.VerticalAlignment = VerticalAlignment;
                laux.Stroke = Brushes.Black;
                this.Children.Add(laux);
            }
        }

        public void paintDotFigure(Figure figure)
        {
            ObservableCollection<Line> lines = drawFigure(normalize(figure.LPoints));
            SolidColorBrush Brush = new SolidColorBrush();
            Brush.Color = figure.Color;
            for (int i = 0; i < lines.Count(); i++)
            {
                Line laux = lines.ElementAt(i);
                laux.HorizontalAlignment = HorizontalAlignment;
                laux.VerticalAlignment = VerticalAlignment;
                laux.Stroke = Brush;
                laux.StrokeDashArray = new DoubleCollection() { 2 };
                this.Children.Add(laux);
            }
        }

        public void PaintTransformations(ObservableCollection<Transformation2D> trans)
        {
            foreach (Transformation2D aux in trans)
            {
                if(aux.TransformatedFigure!=null && aux.IsActive)
                    paintDotFigure(aux.TransformatedFigure);

            }

        }


        public ObservableCollection<Line> drawFigure(ObservableCollection<Point> points)
        {
            ObservableCollection<Line> polygon = new ObservableCollection<Line>();

            for (int i = 0; i < points.Count(); i++)
            {

                Line line = new Line();
                line.Stroke = Brushes.DarkCyan;
                if ((i + 1) < points.Count())
                {
                    line.X1 = points.ElementAt(i).X;
                    line.Y1 = points.ElementAt(i).Y;
                    line.X2 = points.ElementAt(i + 1).X;
                    line.Y2 = points.ElementAt(i + 1).Y;

                }
                else
                {
                    line.X1 = points.ElementAt(i).X;
                    line.Y1 = points.ElementAt(i).Y;
                    line.X2 = points.ElementAt(0).X;
                    line.Y2 = points.ElementAt(0).Y;
                }


                line.StrokeThickness = 2;
                polygon.Add(line);

            }

            return polygon;
        }

        private ObservableCollection<Point> restartPoints(ObservableCollection<Point> p)
        {

            double dx = this.ActualWidth / 50;
            double dy = this.ActualHeight / 50;
            double halfx = this.ActualWidth / 2;
            double halfy = this.ActualHeight / 2;
            ObservableCollection<Point> pts = new ObservableCollection<Point>();
            for (int i = 0; i < p.Count(); i++)
            {
                double px = p.ElementAt(i).X;
                double py = p.ElementAt(i).Y;
                Point temp = new Point();
                double paux = Math.Abs(px - halfx);
                if (px < halfx)
                    temp.X = -Math.Round(paux / dx, 0);
                else
                    temp.X = Math.Round(paux / dx, 0);
                double pauy = Math.Abs(py - halfy);
                if (py < halfy)
                    temp.Y = Math.Round(pauy / dy, 0);
                else
                    temp.Y = -Math.Round(pauy / dy, 0);
                pts.Add(temp);
            }

            return pts;
        }

        public Point pixelToPoint(Point p)
        {
            double dx = this.ActualWidth / 50;
            double dy = this.ActualHeight / 50;
            double halfx = this.ActualWidth / 2;
            double halfy = this.ActualHeight / 2;
            Point temp = new Point();
            double paux = p.X - halfx;
            temp.X = paux / dx;

            double pauy = p.Y - halfy;
            temp.Y = -pauy / dy;

            return temp;

        }

        public Point pointToPixel(Point p)
        {
            double dx = this.ActualWidth / 50;
            double dy = this.ActualHeight / 50;
            double halfx = this.ActualWidth / 2;
            double halfy = this.ActualHeight / 2;

            Point temp = new Point();
            if (p.X > 0)
            {
                temp.X = (halfx) + (p.X * dx);
            }
            else
            {
                temp.X = (halfx) - (Math.Abs(p.X) * dx);
            }
            if (p.Y > 0)
            {
                temp.Y = halfy - (Math.Abs(p.Y) * dy);
            }
            else
            {
                temp.Y = halfy + Math.Abs(p.Y * dy);
            }
            return temp;

        }

        public ObservableCollection<Point> normalize(ObservableCollection<Point> p)
        {
            double dx = this.ActualWidth / 50;
            double dy = this.ActualHeight / 50;
            double halfx = this.ActualWidth / 2;
            double halfy = this.ActualHeight / 2;
            double px, py;
            ObservableCollection<Point> newlist = new ObservableCollection<Point>();
            for (int i = 0; i < p.Count(); i++)
            {
                px = p.ElementAt(i).X;
                py = p.ElementAt(i).Y;
                Point temp = new Point();
                if (px > 0)
                {
                    temp.X = (halfx) + (px * dx);
                }
                else
                {
                    temp.X = (halfx) - (Math.Abs(px) * dx);
                }
                if (py > 0)
                {
                    temp.Y = halfy - (Math.Abs(py) * dy);
                }
                else
                {
                    temp.Y = halfy + Math.Abs(py * dy);
                }
                newlist.Add(temp);
            }
            return newlist;
        }
    }
}
