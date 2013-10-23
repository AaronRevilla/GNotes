using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using GNTools;
using System.ComponentModel;
using GraphicNotes.Configuration;
using System.Xml.Linq;

namespace GraphicNotes
{
    partial class MainWindow : Window, IDisposable
    {

        private Client repositoryClient = new Client();
        public Client RepositoryClient { get { return repositoryClient; } }

        private DataConnectionHandler repositoryConnectionHandler;


        private XMLRepositoryHandler xmlRepositoryHandler = new XMLRepositoryHandler();
        public XMLRepositoryHandler XmlRepositoryHandler { get { return xmlRepositoryHandler; } }

        private Synchronizer synchronizer = new Synchronizer();
        public Synchronizer Synchronizer { get { return synchronizer; } }


        private void InitializeRepositoryModule(){
            repositoryClient.PropertyChanged += repositoryClient_PropertyChanged;
            synchronizer.PropertyChanged += synchronizer_PropertyChanged;
        }

        void synchronizer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsSynchronizing":
                    if(Synchronizer.IsSynchronizing==true)
                        synchronizingPopup.IsModal = true;
                    break;
            }
        }



        void repositoryClient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Client rc = sender as Client;
            if (rc != null)
                switch (e.PropertyName)
                {
                    case "Error":
                        MessageBox.Show(rc.Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        rc.Error = "";
                        break;

                    case "State":
                        if (rc.State == CommonNetworkEnums.ClientState.Connected)
                        {
                            repositoryConnectionHandler = new DataConnectionHandler(repositoryClient.DataClient.Client);
                            repositoryConnectionHandler.PropertyChanged += repositoryConnectionHandler_PropertyChanged;
                            MessageDispatcher md = new MessageDispatcher(repositoryConnectionHandler, synchronizer);
                            repositoryConnectionHandler.Start();
                        }
                        break;
                }
        }

        void repositoryConnectionHandler_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsListening":
                    if (repositoryConnectionHandler.IsListening == false)
                    {
                        repositoryConnectionHandler.PropertyChanged -= repositoryConnectionHandler_PropertyChanged;
                        repositoryClient.State = CommonNetworkEnums.ClientState.Disconnected;
                        synchronizer.Error="Connection lost.";
                        synchronizer.State = "";
                        synchronizer.IsSynchronizing = false;
                    }
                    break;
                case "Error":
                    DataConnectionHandler ch = sender as DataConnectionHandler;
                    MessageBox.Show(ch.Error, "-->Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ch.Error = "";
                    break;
            }
        }

        private void ConnectRepositoryServerButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ipRepositoryField.Text))
            {
                ipRepositoryField.Focus();
                return;
            }

            ConfigurationHandler ch = new ConfigurationHandler();
            repositoryClient.DataPort = Int32.Parse(ch.GetValue("DataPort").ToString());

            if (repositoryClient.State == CommonNetworkEnums.ClientState.Disconnected)
            {
                repositoryClient.Connect(ipRepositoryField.Text);

            }
            else if (repositoryClient.State == CommonNetworkEnums.ClientState.Connected)
            {
                repositoryConnectionHandler.DisconnectClient();

            }

        }

        private void synchronizeButton_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(repositoryNameField.Text))
            {
                repositoryNameField.Focus();
                return;
            }

            if (String.IsNullOrEmpty(repositoryPasswordField.Text))
            {
                repositoryPasswordField.Focus();
                return;
            }

            if (synchronizer.IsSynchronizing == false)
            {
                XElement message = synchronizer.GetSynchronizationMessageRequest(xmlRepositoryHandler, repositoryNameField.Text, repositoryPasswordField.Text);
                if (message != null)
                {
                    repositoryConnectionHandler.SendMessage(message);
                    synchronizer.State = "Waiting for server...";
                }
            }

        }
    }
}
