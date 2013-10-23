using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace GraphicNotes.Views.Objects
{
    class PlotObject:BaseObject
    {
        //public PlotModel plotModel=new PlotModel("Plot");


        public PlotModel Model
        {
            get { return (PlotModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
          DependencyProperty.Register("Model", typeof(PlotModel),
                                      typeof(PlotObject),
                                      new FrameworkPropertyMetadata(new PlotModel()));

        static PlotObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotObject), new FrameworkPropertyMetadata(typeof(PlotObject)));
        }

        public PlotObject(){
            this.Loaded += new RoutedEventHandler(this.PlotObject_Loaded);

        }

        private void PlotObject_Loaded(object sender, RoutedEventArgs e)
        {
            CreateEmptyPlot();
            Console.WriteLine("Plot Loaded");
        }

        public void CreateEmptyPlot()
        {

            PlotModel  model= new PlotModel("");
            model.Background = OxyColors.Blue;
            model.Padding = new OxyThickness(0,model.Padding.Top,model.Padding.Right,0);
            model.PlotMargins = new OxyThickness(0, model.ActualPlotMargins.Top, model.ActualPlotMargins.Right, 0);
            model.PlotType = PlotType.Cartesian;
            var ls = new LineSeries("");
            ls.Points.Add(new DataPoint(0,0));
            ls.Points.Add(new DataPoint(1, 1));
            ls.Points.Add(new DataPoint(2, 2));
            ls.Points.Add(new DataPoint(3, 3));
            ls.Points.Add(new DataPoint(4, 4));
            ls.Points.Add(new DataPoint(10, 15));

            model.Series.Add(ls);
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 10, "X Axe") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 15, "Y Axe") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
          
            Model = model;
        }


    }
}
