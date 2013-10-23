using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GraphicNotes.Core;
using DevExpress.Xpf.Core;
using System.Collections.ObjectModel;
using GraphicNotes.Core.RibbonBarCommands;
using System.Collections.Specialized;
using DevExpress.Xpf.Docking;
using System.ComponentModel;
using GraphicNotes.DocumentServer.RibbonBarCommands;
using GraphicNotes.DocumentServer;
using System.Windows.Controls;
using System.Windows.Media;
using GraphicNotes.Configuration;
using GraphicNotes.Repository;
using DevExpress.Xpf.Editors;
using System.Windows.Data;
using GNTools;
using System.Text;
using System.Collections;


namespace GraphicNotes
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable, INotifyPropertyChanged
    {

        private ObservableCollection<Document> documents=new ObservableCollection<Document>();


        public MainWindow()
        {
            InitializeComponent();
            SetLanguageDictionary();
            SetTheme();
            SetCommands();
            SetConfiguration();
            SetBindings();
            InitializeRepositoryModule();
            dockManager1.DockItemActivated += dockManager1_DockItemActivated;
            dockManager1.DockItemClosing += dockManager1_DockItemClosing;


        }

        



        private void SetConfiguration()
        {
            ConfigurationData dc = (new ConfigurationHandler()).GetConfigurationData();
            ThemeManager.ApplicationThemeName = dc.ThemeName;
        }


       
        




        private void bNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            
        }

        private void groupFile_CaptionButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("This is File Open dialog", "File Open Dialog");
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
        ~MainWindow()
        {
            Dispose(false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
           Console.WriteLine("Windows loaded");
           synchronizingPopup.DataContext = this;
           synchronizingPopupContent.DataContext = this;
           synPopupContent.DataContext = this;
            
        }

        //private void serverListField_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    ListView lv = sender as ListView;
        //    if (lv.SelectedIndex > -1)
        //    {
        //        DocServer aux = servers.ElementAt(lv.SelectedIndex);
        //        serverConfigurationForm.DataContext = aux;
        //        if (aux.Running)
        //            serverConfigurationForm.IsEnabled = false;
        //        else
        //            serverConfigurationForm.IsEnabled = true;
        //        bNewGroup.CommandParameter = aux;
        //        groupListField.DataContext = aux;
        //        groupTreeField.DataContext = aux;
        //    }
        //    else
        //    {
        //        serverConfigurationForm.DataContext = null;
        //        bNewGroup.CommandParameter = null;
        //        groupListField.DataContext = null;
        //        groupTreeField.DataContext = null;
        //    }


        //}

        private void groupListField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.SelectedIndex > -1)
            {

            }
            else
            {
            }


        }

        private void groupTreeField_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tv = sender as TreeView;
            Console.WriteLine(sender);
            //Console.WriteLine(tv.SelectedItem.ToString());
        }

        private void bDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject dp = sender as Button;
            while (dp != null)
            {
                Console.WriteLine(dp);

                if (dp.GetType().Equals(typeof(TreeViewItem)))
                    break;

                dp = VisualTreeHelper.GetParent(dp);
            }

           Console.WriteLine(groupTreeField.ItemContainerGenerator.IndexFromContainer(dp));
           
        }

        private void bDeleteTeam_Click(object sender, RoutedEventArgs e)
        {
           
            DependencyObject dp = sender as Button;
            while (dp != null)
            {
                Console.WriteLine(dp);

                if (dp.GetType().Equals(typeof(TreeViewItem)))
                    break;
                
                dp=VisualTreeHelper.GetParent(dp);
            }
            
            

        }

        


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void loadLocalRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void closeSynPopupButton_Click(object sender, RoutedEventArgs e)
        {
            synchronizingPopup.IsModal = false;

        }



    }



}
