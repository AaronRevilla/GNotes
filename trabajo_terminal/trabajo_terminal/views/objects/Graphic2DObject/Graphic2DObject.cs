
using GraphicNotes.Views.Workspace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace GraphicNotes.Views.Objects
{
    class Graphic2DObject:BaseObject
    {
        private Canvas2D innerCanvas;
        private Canvas2D outerCanvas;
        private ObjectBar objectBar;
        private Popup editPopup;
        private ListView transformationsListView;
        public ListView TransformationsListView
        {
            get { return transformationsListView; }
            set { transformationsListView = value; }
        }
        private CanvasWorkspaceContainer canvasWorkspaceContainer;
        private ObservableCollection<Transformation2D> transformations;
        public ObservableCollection<Transformation2D> Transformations
        {
            get { return transformations; }
            set { transformations = value; }
        }
        private Figure mainFigure;

        private ObservableCollection<Figure> figures;

        public ObservableCollection<Figure> Figures
        {
            get { return figures; }
            set { figures = value; }
        }

        static Graphic2DObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Graphic2DObject), new FrameworkPropertyMetadata(typeof(Graphic2DObject)));
        }

        public Graphic2DObject()
        {
            mainFigure = new Figure();
            transformations = new ObservableCollection<Transformation2D>();
            figures = new ObservableCollection<Figure>();
            figures.Add(mainFigure);
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            innerCanvas = this.Template.FindName("PART_InnerCanvas", this) as Canvas2D;
            outerCanvas = this.Template.FindName("PART_OuterCanvas", this) as Canvas2D;
            objectBar = this.Template.FindName("PART_ObjectBar", this) as ObjectBar;
            objectBar.Loaded += objectBar_Loaded;
            canvasWorkspaceContainer = FindAncestor(VisualTreeHelper.GetParent(this), typeof(CanvasWorkspaceContainer)) as CanvasWorkspaceContainer;
            editPopup = this.Template.FindName("PART_EditPopup",this) as Popup;
            editPopup.Closed += editPopup_Closed;
            innerCanvas.MouseLeftButtonUp += innerCanvas_MouseLeftButtonUp;
            outerCanvas.MouseLeftButtonUp += innerCanvas_MouseLeftButtonUp;

            (this.Template.FindName("PART_NewRotationButton", this) as Button).Click += NewRotationButton_Click;
            (this.Template.FindName("PART_NewTranslationButton", this) as Button).Click += NewTraslationButton_Click;
            (this.Template.FindName("PART_NewScaleButton", this) as Button).Click += NewScalationButton_Click;
            (this.Template.FindName("PART_NewMatrixButton", this) as Button).Click += NewMatrixButton_Click; 
            (this.Template.FindName("PART_DoItButton", this) as Button).Click+=DoItButton_Click;
            (this.Template.FindName("PART_GridContainer", this) as Grid).DataContext=this;
            transformationsListView = this.Template.FindName("PART_TransformationsListView", this) as ListView;

            innerCanvas.MouseRightButtonUp += innerCanvas_MouseRightButtonUp;
            outerCanvas.MouseRightButtonUp += innerCanvas_MouseRightButtonUp;
            outerCanvas.SizeChanged += outerCanvas_SizeChanged;
            innerCanvas.SizeChanged += outerCanvas_SizeChanged;
            
        }

        void innerCanvas_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Canvas2D aux = sender as Canvas2D;
            Point p = e.GetPosition(aux);
            innerCanvas.Clear();
            outerCanvas.Clear();
            transformations.Clear();
            mainFigure = new Figure();
       }

        void outerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas2D tmp = sender as Canvas2D;
            tmp.Clear();

            tmp.PaintTransformations(transformations);
            tmp.paintFigure(mainFigure);
        }

        void NewRotationButton_Click(object sender, RoutedEventArgs e)
        {
            Rotation2D rotation2d = new Rotation2D(transformations,mainFigure);
            transformations.Add(rotation2d);
        }

        void NewTraslationButton_Click(object sender, RoutedEventArgs e)
        {
            Traslation2D traslation2d = new Traslation2D(transformations, mainFigure);
            transformations.Add(traslation2d);
        }
        void NewScalationButton_Click(object sender, RoutedEventArgs e)
        {
            Scalation2D scalation2d= new Scalation2D(transformations, mainFigure);
            transformations.Add(scalation2d);
        }
        void NewMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            MatrixTransformation2D matrix2D = new MatrixTransformation2D(transformations, mainFigure);

            transformations.Add(matrix2D);
        }
        void DoItButton_Click(object sender, RoutedEventArgs e)
        {
            innerCanvas.Clear();
            outerCanvas.Clear();
            foreach (Transformation2D aux in transformations)
            {   if(aux.IsActive)
                    aux.DoTransformation();
            }
            outerCanvas.PaintTransformations(transformations);
            innerCanvas.PaintTransformations(transformations);
            outerCanvas.paintFigure(mainFigure);
            innerCanvas.paintFigure(mainFigure);
        }


        void innerCanvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Canvas2D aux = sender as Canvas2D;
            Point p = e.GetPosition(aux);
            innerCanvas.Clear();
            outerCanvas.Clear();
            mainFigure.LPoints.Add(aux.pixelToPoint(p));
            outerCanvas.paintFigure(mainFigure);
            innerCanvas.paintFigure(mainFigure);
            
        }

        void editPopup_Closed(object sender, EventArgs e)
        {
            canvasWorkspaceContainer.IsEditing = false;
        }


        void objectBar_Loaded(object sender, RoutedEventArgs e)
        {
           objectBar.EditButton.Click+=EditButton_Click;
        }

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            editPopup.IsOpen = true;
            canvasWorkspaceContainer.IsEditing = true;
        }
      

        private DependencyObject FindAncestor(DependencyObject control, Type type)
        {
            DependencyObject ancestor = VisualTreeHelper.GetParent(control);
            if (ancestor.GetType() == type)
            {
                return ancestor;
            }
            else
            {
                return FindAncestor(ancestor,type);
            }

        }
    }
}



