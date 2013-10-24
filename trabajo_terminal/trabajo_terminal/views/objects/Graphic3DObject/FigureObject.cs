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

		float xPlane = 0.0f;
		float yPlane = 0.0f;
		float zPlane = -6.0f;

		/*************************** var lights *********************************************/
		//color
		float fluzR = 0;
		float fluzG = 0;
		float fluzB = 0;

		public float FluzR { get { return fluzR; } set { fluzR = value; } }
		public float FluzG { get { return fluzG; } set { fluzG = value; } }
		public float FluzB { get { return fluzB; } set { fluzB = value; } }

		//position
		float posicionLuzX = 0;
		float posicionLuzY = 0;
		float posicionLuzZ = 0;

		public float PosicionLuzX { get { return posicionLuzX; } set { posicionLuzX = value; } }
		public float PosicionLuzY { get { return posicionLuzY; } set { posicionLuzY = value; } }
		public float PosicionLuzZ { get { return posicionLuzZ; } set { posicionLuzZ = value; } }
		/**************************************************************************************/

		/*************************** var camera************************************************/
		//postion
		double camaraX = 0;
		double camaraY = 0;
		double camaraZ = 0;

		public double CamaraX { get { return camaraX; } set { camaraX = value; } }
		public double CamaraY { get { return camaraY; } set { camaraY = value; } }
		public double CamaraZ { get { return camaraZ; } set { camaraZ = value; } }

		//angles camera
		double anguloCamX = 0;
		double anguloCamY = 0;
		double anguloCamZ = 0;

		public double AnguloCamX { get { return anguloCamX; } set { anguloCamX = value; } }
		public double AnguloCamY { get { return anguloCamY; } set { anguloCamY = value; } }
		public double AnguloCamZ { get { return anguloCamZ; } set { anguloCamZ = value; } }

		/*************************** var model *************************************************/
		//position
		double modeloX = 0;
		double modeloY = 0;
		double modeloZ = 0;

		public double ModeloX { get { return modeloX; } set { modeloX = value; } }
		public double ModeloY { get { return modeloY; } set { modeloY = value; } }
		public double ModeloZ { get { return modeloZ; } set { modeloZ = value; } }

		//angule model
		double anguloModX = 0;
		double anguloModY = 0;
		double anguloModZ = 0;

		public double AnguloModX { get { return anguloModX; } set { anguloModX = value; } }
		public double AnguloModY { get { return anguloModY; } set { anguloModY = value; } }
		public double AnguloModZ { get { return anguloModZ; } set { anguloModZ = value; } }
		/*************************************************************************************/

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



			this.outerOpenGL = (this.Template.FindName("PART_OuterGL", this) as OpenGLControl);
			this.outerOpenGL.OpenGLInitialized += OpenGLControl_OpenGLInitialized;

            this.innerOpenGL = (this.Template.FindName("PART_InnerGL", this) as OpenGLControl);
			this.innerOpenGL.OpenGLInitialized += OpenGLControl_OpenGLInitialized;



			this.outerOpenGL = (this.Template.FindName("PART_OuterGL", this) as OpenGLControl);
			this.outerOpenGL.Resized += OpenGLControl_Resized;

            this.innerOpenGL = (this.Template.FindName("PART_InnerGL", this) as OpenGLControl);
			this.innerOpenGL.Resized += OpenGLControl_Resized;

     
        }


		private void OpenGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
		{
			//  Enable the OpenGL depth testing functionality.
			args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
			args.OpenGL.Enable(OpenGL.GL_COLOR_MATERIAL);
			args.OpenGL.Enable(OpenGL.GL_LIGHTING);

			/*OpenGL solo permite 8 luces por escena*/
			args.OpenGL.Enable(OpenGL.GL_LIGHT0);

			/*Normaliza los vectores en la escena*/
			args.OpenGL.Enable(OpenGL.GL_NORMALIZE);

		}

		private void OpenGLControl_Resized(object sender, OpenGLEventArgs args)
		{
			OpenGL gl = args.OpenGL;
			gl.MatrixMode(OpenGL.GL_MODELVIEW);
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
			drawCoordinateSystem(1000, 1000, 1000, gl);

            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);

            //  Clear the color and depth buffers.
			clearPlane(gl);

			gl.PushMatrix();

            //  Reset the modelview matrix.
            gl.LoadIdentity();
            //  Move the geometry into a fairly central position.

			// First, transform the camera (viewing matrix) from world space to eye space
			// Notice all values are negated, because we move the whole scene with the
			// inverse of camera transform
			gl.Rotate(-anguloCamZ, 0, 0, 1); // roll
			gl.Rotate(-anguloCamY, 0, 1, 0); // heading
			gl.Rotate(-anguloCamX, 1, 0, 0); // pitch
			gl.Translate(-camaraX, -camaraY, -camaraZ);

			// draw the grid at origin before model transform
			drawCoordinateSystem(1000, 1000, 1000, gl);

			
			// transform the object (model matrix)
			// The result of GL_MODELVIEW matrix will be:
			// ModelView_M = View_M * Model_M
			gl.Translate(modeloX, modeloY, modeloZ);
			gl.Rotate(anguloModX, 1, 0, 0);
			gl.Rotate(anguloModY, 0, 1, 0);
			gl.Rotate(anguloModZ, 0, 0, 1);


			//Luz ambiental
			float[] colorAmbiental = new float[] { 0.2f, 0.2f, 0.2f, 1.0f }; //Color con alpha
			gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, colorAmbiental);

			//Inicializar fuente de luz 0 (Esta fuente de luz es posicionada)
			float[] fuenteLuz0 = new float[] { fluzR, fluzG, fluzB, 1.0f };
			float[] posicionLuz0 = new float[] { posicionLuzX, posicionLuzY, posicionLuzZ, 1.0f }; // posicion en (4,0,8)
			gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, fuenteLuz0);
			gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, posicionLuz0);

			//gl.Translate(xTras, yTras, zTras);
            
			
			//  Start drawing triangles.
			gl.PushMatrix();
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(0.0f, 1.0f, 0.0f, 1.0f);
            for (int indicePunto = 0; indicePunto < list.Count; indicePunto++)
            {
				gl.Normal(list.ElementAt(indicePunto).X, list.ElementAt(indicePunto).Y, list.ElementAt(indicePunto).Z);
                gl.Vertex(list.ElementAt(indicePunto).X, list.ElementAt(indicePunto).Y, list.ElementAt(indicePunto).Z);
            }
            gl.End();
			GC.Collect();
			gl.PopMatrix();

            foreach (Transformation3D aux in Transformations)
            {
                //Reset the modelview.
                //gl.LoadIdentity();
				gl.PushMatrix();
                
				//  Move into a more central position.
                //gl.Translate(xTras, yTras, zTras);
                //  Rotate the cube.
                //gl.Rotate(blenderObj, 0.0f, 1.0f, 0.0f);
                //  Provide the cube colors and geometry.
                gl.Begin(OpenGL.GL_TRIANGLES);

                float red = float.Parse(aux.Color.R.ToString()) / 255;
                float green = float.Parse(aux.Color.G.ToString()) / 255;
                float blue = float.Parse(aux.Color.B.ToString()) / 255;
                Console.WriteLine(red + "  " + green + "  " + blue);
                gl.Color(red, green, blue, 0.5f);
                for (int indicePunto = 0; indicePunto < aux.TransformatedFigure.LPoints.Count; indicePunto++)
                {
						//gl.Normal(getNormal(new Point3D(0, 1, 0), new Point3D(-1, -1, 1), new Point3D(1, -1, 1)));
						gl.Normal(aux.TransformatedFigure.LPoints.ElementAt(indicePunto).X, aux.TransformatedFigure.LPoints.ElementAt(indicePunto).Y, aux.TransformatedFigure.LPoints.ElementAt(indicePunto).Z);
						gl.Vertex(aux.TransformatedFigure.LPoints.ElementAt(indicePunto).X,
                        aux.TransformatedFigure.LPoints.ElementAt(indicePunto).Y,
                        aux.TransformatedFigure.LPoints.ElementAt(indicePunto).Z);
                }
                gl.End();
				GC.Collect();
				gl.PopMatrix();
            }
			gl.End();
			GC.Collect();
			gl.Flush();
			gl.PopMatrix();
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

		private void drawCoordinateSystem(int rangeX, int rangeY, int rangeZ, OpenGL gl)
		{

			clearPlane(gl);

			float xP = (float)rangeX;
			float xN = -xP;

			float yP = (float)rangeY;
			float yN = -yP;

			float zP = (float)rangeZ;
			float zN = -zP;

			//gl.LoadIdentity();
			gl.PushMatrix();
			gl.Translate(xPlane, yPlane, zPlane);

			gl.Begin(OpenGL.GL_LINE_STRIP);
			gl.Color(0.0f, 1.0f, 0.0f);//GREEN X AXIS
			gl.Vertex(xP, 0.0f, 0.0f);
			gl.Vertex(xN, 0.0f, 0.0f);
			gl.End();

			gl.Begin(OpenGL.GL_LINE_STRIP);
			gl.Color(1.0f, 0.0f, 0.0f);//RED Y AXIS
			gl.Vertex(0.0f, yP, 0.0f);
			gl.Vertex(0.0f, yN, 0.0f);
			gl.End();

			gl.Begin(OpenGL.GL_LINE_STRIP);
			gl.Color(0.0f, 0.0f, 1.0f);//BLUE Z AXIS
			gl.Vertex(0.0f, 0.0f, zN);
			gl.Vertex(0.0f, 0.0f, zP);
			gl.End();
			gl.PopMatrix();

			gl.Flush();
		}

		private void clearPlane(OpenGL gl)
		{
			//  Clear the color and depth buffers.
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
		}

		public double[] getNormal(Point3D p1, Point3D p2, Point3D p3)
		{
			Vector3D normal = CalculateNormal(p1, p2, p3);
			double[] vNormal = new double[] { normal.X, normal.Y, normal.Z };
			return vNormal;
		}

		public static Vector3D CalculateNormal(Point3D p0, Point3D p1, Point3D p2)
		{
			Vector3D v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
			Vector3D v1 = new Vector3D(p1.X - p2.X, p1.Y - p2.Y, p2.Z - p1.Z);

			return Vector3D.CrossProduct(v0, v1);
		}
        
      
    }
}
