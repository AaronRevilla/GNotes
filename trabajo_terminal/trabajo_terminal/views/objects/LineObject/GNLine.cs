using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    public class GNLine
    {
        private Point point1=new Point(0,0);
        public Point Point1
        {
            get { return point1; }
            set { point1 = value; }
        }
        private Point point2 = new Point(0, 0);
        public Point Point2
        {
            get { return point2; }
            set { point2 = value; }
        }
        private Algorithm algorith=Algorithm.Bresenham;

        public GNLine() { }
        public GNLine(Point p1, Point p2){
            this.point1 = p1;
            this.point2 = p2;
        }

        public GNLine(Point p1, Point p2, Algorithm alg): this(p1, p2)
        {
            algorith = alg;
        }
    }

    public enum Algorithm
    {
        Naive,
        DDA,
        Bresenham
    }
}
