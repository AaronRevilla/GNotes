using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphicNotes.Views.Objects
{
    public partial class Graphic3DObjectHandler
    {
        
        public Graphic3DObjectHandler() {
            InitializeComponent();
        }

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
                if (currentAxis != value)
                {
                    currentAxis = value;
                }
            }
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Transformation3D t2d = button.DataContext as Transformation3D;
            t2d.DoTransformation();
        }

        public ListViewItem GetListViewItem(DependencyObject obj)
        {
            DependencyObject aux = VisualTreeHelper.GetParent(obj);
            if (aux == null)
                return null;

            if (aux is ListViewItem)
            {
                return (aux as ListViewItem);
            }
            else
            {
                return GetListViewItem(aux);
            }


        }

        public ListView GetListView(DependencyObject obj)
        {
            DependencyObject aux = VisualTreeHelper.GetParent(obj);
            if (aux == null)
                return null;

            if (aux is ListView)
            {
                return (aux as ListView);
            }
            else
            {
                return GetListView(aux);
            }


        }

        public void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button element = sender as Button;

            Transformation3D item = element.DataContext as Transformation3D;
            ListView actualListView = GetListView(element);
            ObservableCollection<Transformation3D> list = actualListView.ItemsSource as ObservableCollection<Transformation3D>;
            list.RemoveAt(list.IndexOf(item));

        }

        public void bringUpButton_Click(object sender, RoutedEventArgs e)
        {
            Button element = sender as Button;
            Transformation3D item = element.DataContext as Transformation3D;
            ListView actualListView = GetListView(element);
            ObservableCollection<Transformation3D> list = actualListView.ItemsSource as ObservableCollection<Transformation3D>;
            int index = list.IndexOf(item);
            if (index > 0)
            {
                list.Move(index, index - 1);
            }
        }

        public void bringDownButton_Click(object sender, RoutedEventArgs e)
        {
            Button element = sender as Button;
            Transformation3D item = element.DataContext as Transformation3D;
            ListView actualListView = GetListView(element);
            ObservableCollection<Transformation3D> list = actualListView.ItemsSource as ObservableCollection<Transformation3D>;
            int index = list.IndexOf(item);
            if (index < list.Count - 1)
            {
                list.Move(index, index + 1);
            }
        }

    }
}
