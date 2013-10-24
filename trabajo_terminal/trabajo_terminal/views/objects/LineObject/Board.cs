using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace GraphicNotes.Views.Objects
{
    class Board : FrameworkElement
    {

        const double HorizontalXCaptionMargin = 8;
        private const double CaptionHeight = 15;
        const double VerticalXCaptionMargin = 3;
        const double HotizontalYCaptionMargin = 3;
        const double BottomMargin = 21;


        private VisualCollection visualElements;

        private LinkedList<DrawingVisual> boardElements;
        private LinkedList<DrawingVisual> userPointsElements;
        private LinkedList<DrawingVisual> resultPointsElements;

        int xMin = 0;
        int xMax = 20;

        int yMin = 0;
        int yMax = 20;

        int xLength;
        int yLength;

        double leftMargin;

        public Board()
        {
            xLength = xMax - xMin + 1;
            yLength = yMax - yMin + 1;
            visualElements = new VisualCollection(this);
            boardElements = new LinkedList<DrawingVisual>();
            userPointsElements = new LinkedList<DrawingVisual>();
            resultPointsElements = new LinkedList<DrawingVisual>();
            SetRange(2, 2, 18, 18);

        }


        protected override void OnRender(DrawingContext dc)
        {
            DrawBoard();
        }

        public void SetRange(int x1, int y1, int x2, int y2)
        {
            if (x1 > x2)
            {
                xMax = x1 + 2; xMin = x2 - 2;
            }
            else
            {
                xMax = x2 + 2; xMin = x1 - 2;
            }

            if (y1 > y2)
            {
                yMax = y1 + 2; yMin = y2 - 2;
            }
            else
            {
                yMax = y2 + 2; yMin = y1 - 2;
            }

            xLength = xMax - xMin + 1;
            yLength = yMax - yMin + 1;

            DrawBoard();
        }

        public void DrawBoard()
        {
            RemoveBoard();
            DrawBackground();
            DrawYCaptions();
            DrawXCaptions();
            DrawGrid();
            //DrawAxes();
        }

        public void DrawYCaptions()
        {

            double width = this.ActualWidth;
            double height = this.ActualHeight;

            double dy = (height - BottomMargin) / yLength;

            LinkedList<FormattedText> captions = new LinkedList<FormattedText>();
            for (int i = yMax; i >= yMin; i--)
            {
                captions.AddLast(GetFormattedText(i.ToString(), FlowDirection.LeftToRight));
            }

            double maxWidth = captions.Max(x => x.Width);
            leftMargin = maxWidth + HotizontalYCaptionMargin * 2;


            DrawingVisual text;  /// Draw the numbers of Y axe
            double offset = (dy - CaptionHeight) / 2;
            int n = (int)Math.Round((height - BottomMargin) / CaptionHeight);
            if (n >= yLength)
            {

                for (int i = yMax; i >= yMin; i--)
                {
                    FormattedText caption = captions.ElementAt(yMax - i);
                    double x = maxWidth + HotizontalYCaptionMargin - caption.Width;
                    text = DrawText(caption, new Point(x, dy * (yMax - i) + offset));
                    visualElements.Add(text);
                    boardElements.AddLast(text);
                }
            }
            else
            {

                int step = (int)Math.Round((double)yLength / n, 0);
                step = step < 2 ? 2 : step;
                step = step == yLength ? yLength - 1 : step;
                for (int i = yMax; i >= yMin; i = i - step)
                {
                    FormattedText caption = captions.ElementAt(yMax - i);
                    double x = maxWidth + HotizontalYCaptionMargin - caption.Width;
                    text = DrawText(caption, new Point(x, dy * (yMax - i) + offset));
                    visualElements.Add(text);
                    boardElements.AddLast(text);
                }
            }
        }

        public void DrawXCaptions()
        {
            double width = this.ActualWidth;
            double height = this.ActualHeight;

            double dx = (width - leftMargin) / xLength;
            double dy = (height - BottomMargin) / yLength;

            LinkedList<FormattedText> captions = new LinkedList<FormattedText>();
            for (int i = xMin; i <= xMax; i++)
            {
                captions.AddLast(GetFormattedText(i.ToString(), FlowDirection.LeftToRight));
            }

            double maxWidth = captions.Max(x => x.Width);


            DrawingVisual text;
            double offset = 0;
            double y = height - CaptionHeight - VerticalXCaptionMargin;
            int n = (int)Math.Round((width - leftMargin) / (maxWidth + HorizontalXCaptionMargin), 0);
            if (n >= xLength)
            {

                for (int i = xMin; i <= xMax; i++)
                {
                    FormattedText caption = captions.ElementAt(i - xMin);
                    offset = dx / 2 - caption.Width / 2;
                    double x = dx * (i - xMin) + leftMargin;
                    text = DrawText(caption, new Point(x + offset, y));
                    visualElements.Add(text);
                    boardElements.AddLast(text);
                }
            }
            else
            {
                n = n == 0 ? 2 : n;
                int step = (int)Math.Round((double)xLength / n, 0);
                step = step < 2 ? 2 : step;
                step = step == xLength ? xLength - 1 : step;
                for (int i = (int)xMin; i <= xMax; i = i + step)
                {
                    FormattedText caption = captions.ElementAt(i - xMin);
                    offset = dx / 2 - caption.Width / 2;
                    double x = dx * (i - xMin) + leftMargin;
                    text = DrawText(caption, new Point(x + offset, y));
                    visualElements.Add(text);
                    boardElements.AddLast(text);
                }
            }
        }

        public void DrawGrid()
        {

            double width = this.ActualWidth;
            double height = this.ActualHeight;

            double dx = (width - leftMargin) / xLength;
            double dy = (height - BottomMargin) / yLength;

            Color color = Brushes.SkyBlue.Color;

            for (int y = 0; y < yLength + 1; y++)  //Draw horizontal lines
            {
                DrawingVisual line = DrawLine(new Point(leftMargin, dy * y), new Point(width, dy * y), color, 1);
                visualElements.Add(line);
                boardElements.AddLast(line);
            }

            for (int x = 0; x < xLength + 1; x++)  //Draw vertical lines
            {
                DrawingVisual line = DrawLine(new Point(dx * x + leftMargin, 0), new Point(dx * x + leftMargin, height - BottomMargin), color, 1);
                visualElements.Add(line);
                boardElements.AddLast(line);
            }

            DrawAxes();
        }

        public void DrawAxes()
        {


            double width = this.ActualWidth;
            double height = this.ActualHeight;

            DrawingVisual line = DrawLine(new Point(leftMargin, 0), new Point(leftMargin, height - BottomMargin), Colors.Red, 3); // Y Axe
            visualElements.Add(line);
            boardElements.AddLast(line);

            line = DrawLine(new Point(leftMargin, height - BottomMargin), new Point(width, height - BottomMargin), Colors.Red, 3);  // X Axe
            visualElements.Add(line);
            boardElements.AddLast(line);
        }

        private void RemoveBoard()
        {
            foreach (DrawingVisual aux in boardElements)
            {
                visualElements.Remove(aux);
            }
            boardElements.Clear();
        }

        public void DrawUserPoints(ObservableCollection<Point> points)
        {
            foreach (Point aux in points)
            {
                DrawUserPoint(aux);
            }
        }

        public void DrawUserPoint(Point p)
        {
            if (p.X < xMin || p.X > xMax || p.Y < yMin || p.Y > yMax)
                return;

            double width = this.ActualWidth;
            double height = this.ActualHeight;

            double dx = (width - leftMargin) / xLength;
            double dy = (height - BottomMargin) / yLength;

            double x = (p.X - xMin) * dx + leftMargin;
            double y = (yMax - p.Y) * dy;

            Brush brush = new SolidColorBrush(Color.FromArgb(0xFA, 0xFF, 0x99, 0x00));
            Pen pen = new Pen(Brushes.SkyBlue, 2);
            if (dx > 0 && dy > 0)
            {

                DrawingVisual dv = DrawSquare(brush, pen, new Rect(x, y, dx, dy));
                visualElements.Add(dv);
                userPointsElements.AddLast(dv);

            }
        }

        public void DeleteUserPoints()
        {
            foreach (DrawingVisual aux in userPointsElements)
            {
                visualElements.Remove(aux);
            }
            userPointsElements.Clear();
        }

        private DrawingVisual DrawLine(Point p1, Point p2, Color color, double thicknes)
        {
            Pen pen = new Pen(new SolidColorBrush(color), thicknes);

            DrawingVisual line = new DrawingVisual();  // Y Axe
            using (DrawingContext dc = line.RenderOpen())
            {
                dc.DrawLine(pen, p1, p2);
            }
            return line;
        }

        private void DrawBackground()
        {

            DrawingVisual background = DrawSquare(Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            visualElements.Add(background);
            boardElements.AddLast(background);
        }

        private DrawingVisual DrawSquare(Brush brush, Pen pen, Rect rect)
        {
            DrawingVisual square = new DrawingVisual();
            using (DrawingContext dc = square.RenderOpen())
            {
                dc.DrawRectangle(brush, pen, rect);
            }
            return square;
        }

        private DrawingVisual DrawText(FormattedText txt, Point p1)
        {

            DrawingVisual text = new DrawingVisual();  // Y Axe
            using (DrawingContext dc = text.RenderOpen())
            {
                dc.DrawText(txt, p1);
            }
            return text;
        }

        private FormattedText GetFormattedText(String txt, FlowDirection flowDirection)
        {
            FormattedText formattedText = new FormattedText(txt,
                                    CultureInfo.GetCultureInfo("en-us"),
                                    flowDirection,
                                    new Typeface("Verdana"),
                                    10,
                                    Brushes.Black);
            return formattedText;
        }

        public Point? GetCartesianPoint(Point p)
        {
            double width = this.ActualWidth;
            double height = this.ActualHeight;

            double dx = (width - leftMargin) / xLength;
            double dy = (height - BottomMargin) / yLength;
            int x = (int)((p.X - leftMargin) / dx);
            int y = (int)(p.Y / dy);
            if (x >= 0 && y < yLength)
            {
                return new Point(xMin + x, yMax - y);
            }
            return null;
        }

        protected override Visual GetVisualChild(int index)
        {
            return visualElements[index];
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return visualElements.Count;
            }
        }

        private LinkedList<Point> doNaive(Point p1, Point p2)
        {
            LinkedList<Point> points = new LinkedList<Point>();
            int x1 = (int)p1.X;
            int x2 = (int)p2.X;
            int y1 = (int)p1.Y;
            int y2 = (int)p2.Y;

            int dx = x2 - x1;
            int dy = y2 - y1;

            if (dx != 0)
            {
                points.AddLast(p1);
                double m = (double)dy / (double)dx;
                double b = y1 - m * x1;
                if (x2 > x1)
                    dx = 1;
                else
                    dx = -1;
                while (x1 != x2)
                {
                    x1 += dx;
                    y1 = (int)Math.Round((m * x1 + b));
                    points.AddLast(new Point(x1, y1));
                }
            }
            else
            {
                if (y2 > y1)
                {
                    for (int i = y1; i <= y2; i++)
                    {
                        points.AddLast(new Point(x1, i));
                    }
                }
                else
                {
                    for (int i = y2; i <= y1; i++)
                    {
                        points.AddLast(new Point(x1, i));
                    }
                }
            }
            return points;
        }

        public LinkedList<Point> doBresenham(Point p0, Point p1)
        {
            /*
             *  Constantes para el algoritmo Bresenham
             * 
             *  dx
             *  dy
             *  m = dy/dx
             *  2dy
             *  2(dy - dx)
             *  k=0
             *  p[k] = (x[k],y[k])
             *  
             */
            int x, y, dx, dy, p, incE, incNE, x0, y0, x1, y1, stepx, stepy, indicePuntos;
            LinkedList<Point> puntos = new LinkedList<Point>();
            x0 = (int)(p0.X);
            x1 = (int)(p1.X);
            y0 = (int)(p0.Y);
            y1 = (int)(p1.Y);

            dy = (y1 - y0);             //constante
            dx = (x1 - x0);             //constante

            if (dy < 0)
            {
                dy = -dy;
                stepy = -1;
            }
            else
            {
                stepy = 1;
            }
            if (dx < 0)
            {
                dx = -dx;
                stepx = -1;
            }
            else
            {
                stepx = 1;
            }
            x = x0;
            y = y0;

            puntos.AddLast(new Point(x0, y0));
            indicePuntos = 1;
            if (dx > dy)
            {
                p = 2 * dy - dx;        //constantes P[0]
                incE = 2 * dy;          //constante 
                incNE = 2 * (dy - dx);  //constante
                while (x != x1)
                {
                    x = x + stepx;
                    if (p < 0)
                    {
                        p = p + incE;   //P[k+1]
                    }
                    else
                    {
                        y = y + stepy;
                        p = p + incNE; //P[k+1]
                    }
                    puntos.AddLast(new Point(x, y));
                }
            }
            else
            {
                p = 2 * dx - dy;        //constante P0
                incE = 2 * dx;          //constante
                incNE = 2 * (dx - dy);  //constante
                while (y != y1)
                {
                    y = y + stepy;
                    if (p < 0)
                    {
                        p = p + incE;   //P[k+1]
                    }
                    else
                    {
                        x = x + stepx;
                        p = p + incNE;  //P[k+1]
                    }
                    puntos.AddLast(new Point(x, y));
                }
            }
            return puntos;
        }

        public LinkedList<Point> doDDA(Point p0, Point p1)
        {
            /*
             *  Constantes para el algoritmo DDA
             *  dx
             *  dy
             *  m= dx/dy
             *  k=0
             *  x[k] =  x[k-1] + (1/m)
             *  y[k] =  y[k-1] + m
             */
            LinkedList<Point> points = new LinkedList<Point>();
            //points.AddLast(p0);

            int xi = (int)p0.X;
            int yi = (int)p0.Y;
            int xf = (int)p1.X;
            int yf = (int)p1.Y;

            int indicePunto = 0;

            int len = 0;
            int longx = 0;
            int longy = 0;
            int aux_x = 0;
            int aux_y = 0;
            int i = 0;
            int indicePuntos = 0;
            int y = 0;

            float x = 0;
            float b = 0;
            float y1 = 0;
            float aux_y1 = 0;
            float m = 0;

            longx = xf - xi;
            longy = yf - yi;

            m = (((float)yf - (float)yi) / ((float)xf - (float)xi));                     //constante
            /////////////////////////////////////////////////////////////////////////////
            //////CASO BASE LINEA HORIZONTAL/////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////
            if (yi == yf)
            {
                if (xi > xf)
                {
                    aux_x = xi;
                    xi = xf;
                    xf = aux_x;
                }
                for (indicePunto = xi; indicePunto < xf; indicePunto++)
                {
                    points.AddLast(new Point(indicePunto, yi));
                }
                points.AddLast(p1);
                return points;
            }
            /////////////////////////////////////////////////////////////////////////////
            //////CASO BASE LINEA VERTIAL////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////
            else if (xi == xf)
            {
                if (yi > yf)
                {
                    aux_y = yi;
                    yi = yf;
                    yf = aux_y;
                }
                for (indicePunto = yi; indicePunto < yf; indicePunto++)
                {
                    points.AddLast(new Point(xi, indicePunto));
                }
                points.AddLast(p1);
                return points;
            }
            /////////////////////////////////////////////////////////////////////////////
            //////CASO BASE LINEA DIAGONAL///////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////
            else if ((m == 1) || (m == -1))
            {
                if (xi > xf)
                {
                    aux_x = xi;
                    xi = xf;
                    xf = aux_x;
                    aux_y = yi;
                    yi = yf;
                    yf = aux_y;
                }
                if (m == 1)
                {

                    for (indicePunto = xi; indicePunto < xf; indicePunto++)
                    {
                        points.AddLast(new Point(xi, yi));
                        xi++;
                        yi++;
                    }
                }
                else
                {
                    for (indicePunto = xi; indicePunto < xf; indicePunto++)
                    {
                        points.AddLast(new Point(xi, yi));
                        xi++;
                        yi--;
                    }
                }
                points.AddLast(p1);
                return points;
            }
            ////////////////////////////////////////////////////////
            //////////////////obtener magnitud//////////////////////
            ////////////////////////////////////////////////////////
            if (longx < 0)
            {
                longx = longx * -1;
            }
            if (longy < 0)
            {
                longy = longy * -1;
            }
            ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////

            if (longx >= longy)
            {
                ///////////////////////////////////////////////
                /////en caso de que P1 sea mas chico que P0////
                ///////////////////////////////////////////////
                if (xf < xi)
                {
                    //Se invierten los puntos//
                    aux_x = xf;
                    aux_y = yf;
                    xf = xi;
                    yf = yi;
                    xi = aux_x;
                    yi = aux_y;
                    len = xf - xi;
                    if (len < 0)
                    {
                        len = len * (-1);
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////
                x = (float)xi;
                b = ((float)yi) - (m * x);
                len = longx;

                y1 = (m * x) + b;
                //points.AddLast(new Point3D(x, round(y1), 0));
                aux_y1 = y1;
                for (i = 0; i < len; i++)
                {
                    x = x + 1;
                    y1 = aux_y1 + m;    //incremento en y
                    points.AddLast(new Point(x - 1, round(y1)));
                    aux_y1 = y1;
                }

            }
            //////////////////////////////////////////////////////////
            /////////////////////swap the plane///////////////////////
            //////////////////////////////////////////////////////////
            else
            {

                //////////////////////////////////////////en caso de que P1 sea mas chico que P0
                if (yf < yi)
                {
                    aux_x = xf;
                    aux_y = yf;
                    xf = xi;
                    yf = yi;
                    xi = aux_x;
                    yi = aux_y;
                    len = xf - xi;
                    if (len < 0)
                    {
                        len = len * (-1);
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////

                m = (((float)xf - (float)xi) / ((float)yf - (float)yi));
                b = (float)((float)xi - (m * (float)yi));
                y = yi;//(float)yi
                len = yf - yi;
                y1 = (m * y) + b;// y1 = x
                //points.AddLast(new Point3D(round(y1), round(y), 0));
                aux_y1 = y1;
                for (i = 0; i < len; i++)
                {
                    y = y + 1;
                    y1 = aux_y1 + m;  //incremento en y
                    points.AddLast(new Point(round(y1 - 1), round(y - 1)));
                    aux_y1 = y1;
                }
            }
            points.AddLast(p1);
            return points;
        }

        private void drawAnswerPoint(Point p, SolidColorBrush color)
        {

            double width = this.ActualWidth;
            double height = this.ActualHeight;

            double dx = (width - leftMargin) / xLength;
            double dy = (height - BottomMargin) / yLength;

            double x = (p.X - xMin) * dx + leftMargin;
            double y = (yMax - p.Y) * dy;

            Brush brush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0x99, 0x00));
            Pen pen = new Pen(color, 2);
            if (dx > 0 && dy > 0)
            {

                DrawingVisual dv = DrawSquare(brush, pen, new Rect(x, y, dx, dy));
                visualElements.Add(dv);
                userPointsElements.AddLast(dv);

            }
        }

        private static int round(float num)
        {
            int entero = (int)num;
            float aux = num - entero;
            if (aux > 0.49)
            {
                return entero + 1;
            }
            else
            {
                return entero;
            }
        }

        public void paintAlgorithm(Point p1, Point p2, String algorithm)
        {
            LinkedList<Point> algorithm_points = new LinkedList<Point>();
            switch (algorithm)
            {
                case "Naive":
                    algorithm_points = doNaive(p1, p2);
                    for (int i = 0; i < algorithm_points.Count; i++)
                    {
                        drawAnswerPoint(algorithm_points.ElementAt(i), Brushes.Green);
                    }
                    break;
                case "Bresenham":
                    algorithm_points = doBresenham(p1, p2);
                    for (int i = 0; i < algorithm_points.Count; i++)
                    {
                        drawAnswerPoint(algorithm_points.ElementAt(i), Brushes.Purple);
                    }
                    break;
                case "DDA":
                    algorithm_points = doDDA(p1, p2);
                    for (int i = 0; i < algorithm_points.Count; i++)
                    {
                        drawAnswerPoint(algorithm_points.ElementAt(i), Brushes.Yellow);
                    }
                    break;

            }
        }
    }
}