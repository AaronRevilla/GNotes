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

            Brush brush = new SolidColorBrush(Color.FromArgb(0xFA,0xFF, 0x99, 0x00));
            Pen pen = new Pen(Brushes.SkyBlue, 1);
            if (dx > 0 && dy > 0)
            {
                
                    DrawingVisual dv = DrawSquare(brush, pen, new Rect(x, y, dx, dy));
                    visualElements.Add(dv);
                    userPointsElements.AddLast(dv);
                
            }
        }

        public void DeleteUserPoints()
        {
            foreach(DrawingVisual aux in userPointsElements){
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

        private void DrawBackground(){

            DrawingVisual background = DrawSquare(Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));
            visualElements.Add(background);
            boardElements.AddLast(background);
        }

        private DrawingVisual DrawSquare(Brush brush, Pen pen, Rect rect)
        {
            DrawingVisual square = new DrawingVisual();  
            using (DrawingContext dc = square.RenderOpen())
            {
                dc.DrawRectangle(brush ,pen, rect);
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
            int y=(int)(p.Y/dy);
            if (x >= 0 && y < yLength)
            {
                return new Point(xMin+x,yMax-y);
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


    }
}