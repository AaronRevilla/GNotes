using DevExpress.Xpf.Editors;
using GNTools;
using GraphicNotes.Views.Workspace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphicNotes.Views.Objects
{
    class LineObject : BaseObject, INotifyPropertyChanged
    {

        private Board innerBoard;
        private Board outerBoard;
        private ObjectBar objectBar;
        private Popup editPopup;
        private Popup dataPopup;
        private CanvasWorkspaceContainer canvasWorkspaceContainer;
        private Point point1;
        private Point point2;
        private CheckEdit check;
        private ComboBoxEdit combo;
        private ObservableCollection<Point> resultPoints;
        private ObservableCollection<Point> userPoints;

        public enum LineAlgorithm
        {
            DDA,
            Naive,
            Bresenham
        }

        private LineAlgorithm currentAlgorithm = LineAlgorithm.Bresenham;
        public LineAlgorithm CurrentAlgorithm
        {
            get { return currentAlgorithm; }
            set
            {
                if (currentAlgorithm != value)
                {
                    currentAlgorithm = value;
                    NotifyPropertyChanged("CurrentAlgorithm");
                }
            }
        }

        private bool isShowingAnswer = false;
        public bool IsShowingAnswer
        {
            get { return isShowingAnswer; }
            set
            {
                if (isShowingAnswer != value)
                {
                    isShowingAnswer = value;
                    NotifyPropertyChanged("IsShowingAnswer");
                }
            }
        }

        public int X1
        {
            get { return (int)point1.X; }
            set
            {
                if ((int)point1.X != value)
                {
                    point1.X = value;
                    NotifyPropertyChanged("X1");
                    innerBoard.SetRange((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
                    outerBoard.SetRange((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
                    DrawAll();
                }
            }
        }

        public int Y1
        {
            get { return (int)point1.Y; }
            set
            {
                if ((int)point1.Y != value)
                {
                    point1.Y = value;
                    NotifyPropertyChanged("Y1");
                    innerBoard.SetRange((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
                    outerBoard.SetRange((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
                    DrawAll();
                }
            }
        }

        public int X2
        {
            get { return (int)point2.X; }
            set
            {
                if ((int)point2.X != value)
                {
                    point2.X = value;
                    NotifyPropertyChanged("X2");
                    innerBoard.SetRange((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
                    outerBoard.SetRange((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
                    DrawAll();
                }
            }
        }

        public int Y2
        {
            get { return (int)point2.Y; }
            set
            {
                if ((int)point2.Y != value)
                {
                    point2.Y = value;
                    NotifyPropertyChanged("Y2");
                    innerBoard.SetRange((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
                    outerBoard.SetRange((int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
                    DrawAll();
                }
            }
        }

        static LineObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(LineObject), new FrameworkPropertyMetadata(typeof(LineObject)));
        }

        public LineObject()
        {

            resultPoints = new ObservableCollection<Point>();
            userPoints = new ObservableCollection<Point>();
            userPoints.CollectionChanged += userPoints_CollectionChanged;


        }

        void userPoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Point p in e.NewItems)
                    {
                        innerBoard.DrawUserPoint(p);
                        outerBoard.DrawUserPoint(p);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    innerBoard.DeleteUserPoints();
                    innerBoard.DrawUserPoints(userPoints);
                    outerBoard.DeleteUserPoints();
                    outerBoard.DrawUserPoints(userPoints);
                    break;
            }
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            (this.Template.FindName("PART_GridContainer", this) as Grid).DataContext = this;
            //Board
            innerBoard = this.Template.FindName("PART_InnerBoard", this) as Board;
            outerBoard = this.Template.FindName("PART_OuterBoard", this) as Board;
            innerBoard.MouseLeftButtonUp += innerBoard_MouseLeftButtonUp;
            outerBoard.MouseLeftButtonUp += innerBoard_MouseLeftButtonUp;
            innerBoard.SizeChanged += innerBoard_SizeChanged;
            outerBoard.SizeChanged += innerBoard_SizeChanged;

            //ObjectBar
            objectBar = this.Template.FindName("PART_ObjectBar", this) as ObjectBar;
            objectBar.Loaded += objectBar_Loaded;

            //CanvasWorkspace
            canvasWorkspaceContainer = UIUtils.FindAncestor(VisualTreeHelper.GetParent(this), typeof(CanvasWorkspaceContainer)) as CanvasWorkspaceContainer;

            //EditPopup
            editPopup = this.Template.FindName("PART_EditPopup", this) as Popup;
            editPopup.Closed += editPopup_Closed;

            //DataPopup
            (this.Template.FindName("PART_ShowPopupButton", this) as Button).Click += LineObject_Click;
            dataPopup = this.Template.FindName("PART_DataPopup", this) as Popup;
            dataPopup.MouseLeave += dataPopup_MouseLeave;

            //Point1 and Point2
            (this.Template.FindName("PART_X1Field", this) as SpinEdit).SetBinding(SpinEdit.TextProperty, new Binding("X1") { Mode = BindingMode.TwoWay });
            (this.Template.FindName("PART_Y1Field", this) as SpinEdit).SetBinding(SpinEdit.TextProperty, new Binding("Y1") { Mode = BindingMode.TwoWay });
            (this.Template.FindName("PART_X2Field", this) as SpinEdit).SetBinding(SpinEdit.TextProperty, new Binding("X2") { Mode = BindingMode.TwoWay });
            (this.Template.FindName("PART_Y2Field", this) as SpinEdit).SetBinding(SpinEdit.TextProperty, new Binding("Y2") { Mode = BindingMode.TwoWay });

            //ComboBoxEdit Algorithm
            combo = this.Template.FindName("PART_AlgorithmField", this) as ComboBoxEdit;
            (combo).ItemsSource = (typeof(LineAlgorithm).GetEnumValues());
            (combo).SetBinding(ComboBoxEdit.SelectedItemProperty, new Binding("CurrentAlgorithm") { Mode = BindingMode.TwoWay });
            combo.EditValueChanged += combo_EditValueChanged;

            X1 = 2; Y1 = 2;
            X2 = 18; Y2 = 18;
            check = this.Template.FindName("PART_AnswerField", this) as CheckEdit;
            check.EditValueChanged += check_EditValueChanged;

        }

        void combo_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (check.IsChecked == true)
            {
                innerBoard.paintAlgorithm(new Point(X1, Y1), new Point(X2, Y2), combo.Text);
                outerBoard.paintAlgorithm(new Point(X1, Y1), new Point(X2, Y2), combo.Text);
            }
        }

        void check_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (check.IsChecked == true)
                Console.WriteLine(combo.Text);
            else
                Console.WriteLine("ola ke no ase");
        }

        void dataPopup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            dataPopup.IsOpen = false;
        }

        void LineObject_Click(object sender, RoutedEventArgs e)
        {
            dataPopup.IsOpen = true;
            Mouse.Capture(dataPopup, CaptureMode.Element);
        }

        void innerBoard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Board board = sender as Board;
            board.DeleteUserPoints();
            board.DrawUserPoints(userPoints);
        }

        void innerBoard_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Board board = sender as Board;
            Point pixelPoint = e.GetPosition(board);
            Point? aux = board.GetCartesianPoint(pixelPoint);
            if (aux.HasValue)
            {
                try
                {
                    Point tmp = userPoints.Single(p => p.X == aux.Value.X && p.Y == aux.Value.Y);
                    userPoints.Remove(tmp);
                }
                catch (Exception ex)
                {
                    userPoints.Add(aux.Value);
                }
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



        private void DrawAll()
        {
            innerBoard.DeleteUserPoints();
            innerBoard.DrawBoard();
            innerBoard.DrawUserPoints(userPoints);

            outerBoard.DeleteUserPoints();
            outerBoard.DrawBoard();
            outerBoard.DrawUserPoints(userPoints);
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
