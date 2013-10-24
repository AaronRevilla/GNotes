using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GraphicNotes.Views.Objects;
using GraphicNotes.Views.Adorners;
using GraphicNotes.Core;
using DevExpress.Xpf.Docking;
using GNTools;

namespace GraphicNotes.Views.Workspace
{
    public class CanvasWorkspace:Canvas
    {
        private Document document;

        public CanvasWorkspace()
        {
            this.Loaded += CanvasWorkspace_Loaded;
        }

        void CanvasWorkspace_Loaded(object sender, RoutedEventArgs e)
        {
            document = this.DataContext as Document;
        }

        private Point? dragStartPoint = null;

        public IEnumerable<BaseObject> SelectedElements
        {
            get
            {
                var selectedElements = from element in this.Children.OfType<BaseObject>()
                                    where element.State == BaseObject.ObjectState.Unlocked
                                    select element;
                
                return selectedElements;
            }
        }

        public void DeselectAll()
        {
            foreach (BaseObject item in this.SelectedElements)
            {
                item.Deselect();
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                this.dragStartPoint = new Point?(e.GetPosition(this));
                this.DeselectAll();
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                this.dragStartPoint = null;
            }

            if (this.dragStartPoint.HasValue)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    RubberbandAdorner adorner = new RubberbandAdorner(this, this.dragStartPoint);
                    if (adorner != null)
                    {
                        adornerLayer.Add(adorner);
                    }
                }

                e.Handled = true;
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {

            base.OnDrop(e);
            int? type = (int)(e.Data.GetData("ObjectType"));
            if (type!=null)
            {
                if (type ==(int)Enums.Objects.Text)
                {
                    TextObject ob = new TextObject() { Width = 65, Height = 65 };
                    Point position = e.GetPosition(this);
                    CanvasWorkspace.SetLeft(ob, Math.Max(0, position.X - ob.Width / 2));
                    CanvasWorkspace.SetTop(ob, Math.Max(0, position.Y - ob.Height / 2));
                    this.Children.Add(ob);
                    this.DeselectAll();
                    ob.Loaded += ob_Loaded;
                }
                else if (type == (int)Enums.Objects.Table)
                {
                    
                    
                    TableForm tf = new TableForm();
                    tf.ShowDialog();
                    if (tf.DialogResult == true)
                    {
                        TableObject ob = new TableObject(tf.Columns, tf.Rows) { Width = 65, Height = 65 };
                        Point position = e.GetPosition(this);
                        CanvasWorkspace.SetLeft(ob, Math.Max(0, position.X - ob.Width / 2));
                        CanvasWorkspace.SetTop(ob, Math.Max(0, position.Y - ob.Height / 2));
                        this.Children.Add(ob);
                        ob.Loaded += ob_Loaded;
                    }
                }
                else if (type == (int)Enums.Objects.Plot)
                {
                    PlotObject ob = new PlotObject() { Width = 200, Height = 200 };
                    Point position = e.GetPosition(this);
                    CanvasWorkspace.SetLeft(ob, Math.Max(0, position.X - ob.Width / 2));
                    CanvasWorkspace.SetTop(ob, Math.Max(0, position.Y - ob.Height / 2));
                    this.Children.Add(ob);
                    ob.Loaded += ob_Loaded;
                }
                else if (type == (int)Enums.Objects.Picture)
                {
                    PictureObject ob = new PictureObject() { Width = 200, Height = 200 };
                    Point position = e.GetPosition(this);
                    CanvasWorkspace.SetLeft(ob, Math.Max(0, position.X - ob.Width / 2));
                    CanvasWorkspace.SetTop(ob, Math.Max(0, position.Y - ob.Height / 2));
                    this.Children.Add(ob);
                    ob.Loaded += ob_Loaded;
                }
                else if (type == (int)Enums.Objects.Line)
                {
                    LineObject ob = new LineObject() { Width = 200, Height = 200 };
                    Point position = e.GetPosition(this);
                    CanvasWorkspace.SetLeft(ob, Math.Max(0, position.X - ob.Width / 2));
                    CanvasWorkspace.SetTop(ob, Math.Max(0, position.Y - ob.Height / 2));
                    this.Children.Add(ob);
                    ob.Loaded += ob_Loaded;
                }
                else if (type == (int)Enums.Objects.Figure3D)
                {
                    FigureObject ob = new FigureObject() { Width = 200, Height = 200 };
                    Point position = e.GetPosition(this);
                    CanvasWorkspace.SetLeft(ob, Math.Max(0, position.X - ob.Width / 2));
                    CanvasWorkspace.SetTop(ob, Math.Max(0, position.Y - ob.Height / 2));
                    this.Children.Add(ob);
                    ob.Loaded += ob_Loaded;
                }

                else if (type == (int)Enums.Objects.Video)
                {
                    VideoObject ob = new VideoObject() { Width = 200, Height = 200 };
                    Point position = e.GetPosition(this);
                    CanvasWorkspace.SetLeft(ob, Math.Max(0, position.X - ob.Width / 2));
                    CanvasWorkspace.SetTop(ob, Math.Max(0, position.Y - ob.Height / 2));
                    this.Children.Add(ob);
                    ob.Loaded += ob_Loaded;
                }

                else if (type == (int)Enums.Objects.Graphic2D)
                {
                    Graphic2DObject ob = new Graphic2DObject() { Width = 200, Height = 200 };
                    Point position = e.GetPosition(this);
                    CanvasWorkspace.SetLeft(ob, Math.Max(0, position.X - ob.Width / 2));
                    CanvasWorkspace.SetTop(ob, Math.Max(0, position.Y - ob.Height / 2));
                    this.Children.Add(ob);
                    ob.Loaded += ob_Loaded;
                }
                
            }

             e.Handled = true;
            
        }

        void ob_Loaded(object sender, RoutedEventArgs e)
        {
            BaseObject baseObject = sender as BaseObject;
            this.DeselectAll();
            baseObject.Select();
            baseObject.Loaded -= ob_Loaded;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            foreach (UIElement element in Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }

            // add some extra margin
            size.Width += 10;
            size.Height += 10;
            return size;
        }



    }
}
