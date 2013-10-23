using GraphicNotes.Views.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using SharpGL.WPF;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Xpf.Editors;
using System.Windows.Data;
using SharpGL;
using System.Windows.Media.Media3D;

namespace GraphicNotes.Views.Objects
{
    class FigureObject : BaseObject, INotifyPropertyChanged
    {
        private Popup editPopup;
        private ObjectBar objectBar;
        private CanvasWorkspaceContainer canvasWorkspaceContainer;
        private OpenGLControl innerOpenGL;
        private OpenGLControl outerOpenGL;
        private Model3D baseModel;
        private ObservableCollection<Point3D> list;
        float xTras = -0.0f;
        float yTras = -0.0f;
        float zTras = -15.0f;
        float blenderObj = 0;

        public enum AxisName
        {
            X,
            Y,
            Z
        }

        private AxisName currentAxis = AxisName.X;
        public AxisName CurrentAxis
        {
            get { return currentAxis; }
            set
            {
                if (currentAxis!= value)
                {
                    currentAxis = value;
                    NotifyPropertyChanged("CurrentAlgorithm");
                }
            }
        }
        static FigureObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FigureObject), new FrameworkPropertyMetadata(typeof(FigureObject)));
           
        }

        public FigureObject() {
            transformations = new ObservableCollection<Transformation3D>();
            baseModel = new Model3D();
            list = new ObservableCollection<Point3D>();
           
            baseModel.LPoints = list;
        }

        private ObservableCollection<Transformation3D> transformations;
        public ObservableCollection<Transformation3D> Transformations
        {
            get { return transformations; }
            set { transformations = value; }
        }

        public override void OnApplyTemplate()
        {
            objectBar = this.Template.FindName("PART_ObjectBar", this) as ObjectBar;
            objectBar.Loaded += objectBar_Loaded;
            canvasWorkspaceContainer = FindAncestor(VisualTreeHelper.GetParent(this), typeof(CanvasWorkspaceContainer)) as CanvasWorkspaceContainer;
            editPopup = this.Template.FindName("PART_EditPopup", this) as Popup;
            editPopup.Closed += editPopup_Closed;
            (this.Template.FindName("PART_NewRotation3DButton", this) as Button).Click += NewRotation3DButton_Click;
            (this.Template.FindName("PART_NewTranslation3DButton", this) as Button).Click += NewTranslation3DButton_Click;
            (this.Template.FindName("PART_NewScale3DButton", this) as Button).Click += NewScalation3DButton_Click;
            (this.Template.FindName("PART_NewMatrix3DButton", this) as Button).Click += NewMatrix3DButton_Click;
            (this.Template.FindName("PART_NewModelButton", this) as Button).Click += FigureObject_Click;
            (this.Template.FindName("PART_DoItButton", this) as Button).Click +=  DoIt_Click;

            this.outerOpenGL = (this.Template.FindName("PART_OuterGL", this) as OpenGLControl);
            this.outerOpenGL.OpenGLDraw += FigureObject_OpenGLDraw;

            this.innerOpenGL = (this.Template.FindName("PART_InnerGL", this) as OpenGLControl);
            this.innerOpenGL.OpenGLDraw += FigureObject_OpenGLDraw;

     
        }



        void FigureObject_OpenGLDraw(object sender, SharpGL.OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;
            paintModel(gl);
        }

        private void paintModel(OpenGL gl)
        {
            //  Get the OpenGL instance that's been passed to us.
            gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            //  Clear the color and depth buffers.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //  Reset the modelview matrix.
            gl.LoadIdentity();
            //  Move the geometry into a fairly central position.
            gl.Translate(xTras, yTras, zTras);
            //  Start drawing triangles.
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0.0f, 1.0f, 0.0f, 1.0f);
            for (int indicePunto = 0; indicePunto < list.Count; indicePunto++)
            {
                gl.Vertex(list.ElementAt(indicePunto).X, list.ElementAt(indicePunto).Y, list.ElementAt(indicePunto).Z);
            }
            gl.End();

            foreach (Transformation3D aux in Transformations)
            {
                //Reset the modelview.
                gl.LoadIdentity();
                //  Move into a more central position.
                gl.Translate(xTras, yTras, zTras);
                //  Rotate the cube.
                gl.Rotate(blenderObj, 0.0f, 1.0f, 0.0f);
                //  Provide the cube colors and geometry.
                gl.Begin(OpenGL.GL_TRIANGLES);

                float red = float.Parse(aux.Color.R.ToString()) / 255;
                float green = float.Parse(aux.Color.G.ToString()) / 255;
                float blue = float.Parse(aux.Color.B.ToString()) / 255;
                Console.WriteLine(red + "  " + green + "  " + blue);
                gl.Color(red, green, blue, 0.5f);
                for (int indicePunto = 0; indicePunto < aux.TransformatedFigure.LPoints.Count; indicePunto++)
                {
                    gl.Vertex(aux.TransformatedFigure.LPoints.ElementAt(indicePunto).X,
                        aux.TransformatedFigure.LPoints.ElementAt(indicePunto).Y,
                        aux.TransformatedFigure.LPoints.ElementAt(indicePunto).Z);
                }
                gl.End();
            }
            gl.Flush();
        }


        //private void paintModel(OpenGL gl)
        //{
        //    //  Reset the modelview matrix.
        //    gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        //    gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        //    gl.PushMatrix();
        //    //  Move the geometry into a fairly central position.
        //    gl.LoadIdentity();   
        //    gl.Translate(xTras, yTras, zTras);
        //    //  Draw a pyramid. First, rotate the modelview matrix.
        //    gl.Rotate(blenderObj, 0.0f, 1.0f, 0.0f);
        //    //  Start drawing triangles.
        //    gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        //    gl.Begin(OpenGL.GL_POLYGON);
        //    gl.Color(0.0f, 1.0f, 0.0f, 1.0f);
        //    for (int indicePunto = 0; indicePunto < list.Count; indicePunto++)
        //    {
        //        gl.Vertex(list.ElementAt(indicePunto).X, list.ElementAt(indicePunto).Y, list.ElementAt(indicePunto).Z);
        //    }
       
        //    if (transformations.Count > 0)
        //    {
        //        foreach (Transformation3D t in transformations)
        //        {
        //            gl.Color(0.0f, 1.0f, 0.0f, 1.0f);
        //            for (int indicePunto = 0; indicePunto < t.TransformatedFigure.LPoints.Count; indicePunto++)
        //            {
                  
        //                gl.Vertex(t.TransformatedFigure.LPoints.ElementAt(indicePunto).X, t.TransformatedFigure.LPoints.ElementAt(indicePunto).Y, t.TransformatedFigure.LPoints.ElementAt(indicePunto).Z);
        //            }
                    
        //        }
        //    }
        //    gl.End();
        //    gl.PopMatrix();
        //    gl.Flush();
        //}

        void FigureObject_Click(object sender, RoutedEventArgs e)
        {
            readFile();
            editPopup.IsOpen = true;
        }

        private void readFile()
        {
            FileReader archivo = null;

            int numCaras = 0;
            int numPuntos = 0;
            var ofd = new Microsoft.Win32.OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == false) return;
            archivo = new FileReader(ofd.FileName);
            numCaras = archivo.numLines();
            for (int indiceLinea = 0; indiceLinea < numCaras; indiceLinea++)
            {
                string[] puntosCadena = archivo.getLine(indiceLinea + 1).Split(' ');
                numPuntos = puntosCadena.Count() / 3;

                int i = 0;

                for (int indicePunto = 0; indicePunto < numPuntos; indicePunto++)
                {
                    float x = (float)Convert.ToDouble(puntosCadena.ElementAt(i++));
                    float y = (float)Convert.ToDouble(puntosCadena.ElementAt(i++));
                    float z = (float)Convert.ToDouble(puntosCadena.ElementAt(i++));

                    list.Add(new Point3D(x, y, z));
                }
            }
            baseModel.LPoints = list;
        }

        void DoIt_Click(object sender, RoutedEventArgs e)
        {
            foreach (Transformation3D aux in transformations)
            {
                if (aux.IsActive)
                    aux.DoTransformation();
            }
        }

        void editPopup_Closed(object sender, EventArgs e)
        {
            canvasWorkspaceContainer.IsEditing = false;
        }
        void objectBar_Loaded(object sender, RoutedEventArgs e)
        {
            objectBar.EditButton.Click += EditButton_Click;
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
                return FindAncestor(ancestor, type);
            }

        }
        void NewRotation3DButton_Click(object sender, RoutedEventArgs e)
        {
            Rotation3D rot = new Rotation3D(transformations, baseModel);
            transformations.Add(rot);
        }

        void NewTranslation3DButton_Click(object sender, RoutedEventArgs e)
        {
            Transformation3D tra = new Traslation3D(transformations, baseModel);
            transformations.Add(tra);
        }

        void NewScalation3DButton_Click(object sender, RoutedEventArgs e)
        {
            Scalation3D sca = new Scalation3D(transformations, baseModel);
            transformations.Add(sca);
        }

        void NewMatrix3DButton_Click(object sender, RoutedEventArgs e)
        {
            MatrixTransformation3D sca = new MatrixTransformation3D(transformations, baseModel);
            transformations.Add(sca);
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
