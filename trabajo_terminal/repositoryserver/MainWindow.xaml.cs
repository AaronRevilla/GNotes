using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using GNTools;
using DevExpress.Xpf.Editors;
using System.Net.Sockets;

namespace RepositoryServer
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private XMLServerHandler xmlServerHandler;
        public XMLServerHandler XmlServerHandler { get { return xmlServerHandler; } }

        public Dictionary<String,RepositoryUserCounter> repositories = new Dictionary<String,RepositoryUserCounter>();

        public Server server=new Server();
        public Server Server { get { return server; } }

        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.ApplicationThemeName = "Office2013";
            this.DataContext = this;



            xmlServerHandler = new XMLServerHandler();
            xmlServerHandler.LoadXMLServer();
            xmlServerHandler.XmlDocument.Changed += XmlDocument_Changed;
            

            server = new Server();
            server.PropertyChanged += server_PropertyChanged;
            
        }

        void XmlDocument_Changed(object sender, System.Xml.Linq.XObjectChangeEventArgs e)
        {
            XObject xObject=(XObject) sender;
            switch (e.ObjectChange)
            {
                case XObjectChange.Value:
                    if (xObject.NodeType == XmlNodeType.Attribute && xObject.Parent.Name.ToString().Equals("configuration"))
                    {
                        xmlServerHandler.SaveChanges();
                    }
                    break;
            }

        }

        void server_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Server tmpServer = sender as Server;
            if (tmpServer!= null)
            switch (e.PropertyName)
            {
                case "Error":
                    MessageBox.Show(tmpServer.Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    tmpServer.Error = "";
                    break;
                case "IsRunning":
                    serverConfigPanel.IsEnabled = !tmpServer.IsRunning;
                    
                    break;
                case "NewDataClient":
                    Socket client = server.DataClientsQueue.ElementAt(0);
                    Utils.WriteLine(client.RemoteEndPoint.ToString().Replace('.',':'));
                    server.DataClientsQueue.Remove(client);
                    DataConnectionHandler connection = new DataConnectionHandler(client, server);
                    Synchronizer synchronizer=new Synchronizer();
                    MessageDispatcher md = new MessageDispatcher(connection, xmlServerHandler, repositories, synchronizer);
                    connection.Start();
                    break;
            }
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RepositoryForm rf = new RepositoryForm();
            rf.DataContext = xmlServerHandler;
            rf.ShowDialog();
            if (rf.DialogResult == true)
            {

                xmlServerHandler.AddRepository(rf.Repository);
            }
        }

        private void bDeleteRepository_Clik(object sender, RoutedEventArgs e)
        {

            DependencyObject dp = sender as Button;
            while (dp != null)
            {

                if (dp.GetType().Equals(typeof(ListViewItem)))
                    break;

                dp = VisualTreeHelper.GetParent(dp);
            }



            if (dp != null)
            {
                int index = repositoryListField.ItemContainerGenerator.IndexFromContainer(dp);
                XElement aux=(XElement)repositoryListField.Items.GetItemAt(index);
                CredentialsForm cf = new CredentialsForm(aux);
                cf.ShowDialog();
                if (cf.DialogResult == true)
                    xmlServerHandler.DeleteRepository(aux);
                    
            }

        }

        private void bEditRepository_Clik(object sender, RoutedEventArgs e)
        {

             DependencyObject dp = sender as Button;
            while (dp != null)
            {

                if (dp.GetType().Equals(typeof(ListViewItem)))
                    break;

                dp = VisualTreeHelper.GetParent(dp);
            }



            if (dp != null)
            {
                int index = repositoryListField.ItemContainerGenerator.IndexFromContainer(dp);
                XElement aux = (XElement)repositoryListField.Items.GetItemAt(index);
                RepositoryForm rf = new RepositoryForm(aux);
                rf.ShowDialog();
                if (rf.DialogResult == true)
                {
                    xmlServerHandler.EditRepository(rf.Repository);
                }
            }

        }

        private void dataPortField_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            SpinEdit spindEdit = sender as SpinEdit;
            if (server != null)
            {
                server.DataPort = Int32.Parse(spindEdit.EditValue.ToString());
            }
        }

        private void filePortField_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            SpinEdit spindEdit = sender as SpinEdit;
            if (server != null)
            {
                server.FilePort = Int32.Parse(spindEdit.EditValue.ToString());
            }
        }
    }
}
