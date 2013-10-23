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
    public partial class Graphic2DObjectHandler:ResourceDictionary
    {
        public Graphic2DObjectHandler()
        {
            InitializeComponent();
            
            
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Transformation2D t2d = button.DataContext as Transformation2D;
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

        public void deleteButton_Click(object sender, RoutedEventArgs e) {
            Button element = sender as Button;
      
            Transformation2D item = element.DataContext as Transformation2D;
            ListView actualListView = GetListView(element);
            ObservableCollection<Transformation2D> list = actualListView.ItemsSource as ObservableCollection<Transformation2D>;
            list.RemoveAt(list.IndexOf(item));
            
        }

        public void bringUpButton_Click(object sender, RoutedEventArgs e)
        {
            Button element = sender as Button;
            Transformation2D item = element.DataContext as Transformation2D;
            ListView actualListView = GetListView(element);
            ObservableCollection<Transformation2D> list = actualListView.ItemsSource as ObservableCollection<Transformation2D>;
            int index= list.IndexOf(item);
            if(index>0){
                list.Move(index, index - 1);
            }
        }

        public void bringDownButton_Click(object sender, RoutedEventArgs e)
        {
            Button element = sender as Button;
            Transformation2D item = element.DataContext as Transformation2D;
            ListView actualListView = GetListView(element);
            ObservableCollection<Transformation2D> list = actualListView.ItemsSource as ObservableCollection<Transformation2D>;
            int index = list.IndexOf(item);
            if (index < list.Count-1)
            {
                list.Move(index, index + 1);
            }
         }
    }
}
