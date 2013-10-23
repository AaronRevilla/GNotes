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
using DevExpress.Xpf.Docking;
using GraphicNotes.Core;
using System.Collections.Specialized;
using GraphicNotes.Core.Forms;

namespace GraphicNotes
{
    partial class MainWindow : Window, IDisposable
    {
        private String connectingState = "";
        public String ConnectingState { get { return connectingState; } set { connectingState = value; NotifyPropertyChanged("ConnectingState"); } }
        private int documentsCounter = 1;

        private Document currentDocument;
        public Document CurrentDocument
        {
            get { return currentDocument; }
            set { currentDocument = value; NotifyPropertyChanged("CurrentDocument"); }
        }

        void dockManager1_DockItemActivated(object sender, DevExpress.Xpf.Docking.Base.DockItemActivatedEventArgs ea)
        {
            if (ea.Item is DocumentPanel)
            {
                CurrentDocument = (ea.Item as DocumentPanel).DataContext as Document;
            }
            else
            {
                CurrentDocument = null;
            }

        }


        void dockManager1_DockItemClosing(object sender, DevExpress.Xpf.Docking.Base.ItemCancelEventArgs e)
        {
            if (e.Item is DocumentPanel)
            {
                DocumentPanel dp = e.Item as DocumentPanel;
                Document document = dp.DataContext as Document;
                if (document.Server.IsRunning)
                    document.Server.IsRunning = false;
            }
        }

        private void bNewDocument_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Document doc = new Document("Document" + documentsCounter++);
            documentGroup1.Items.Add(doc.DocumentPanel);
            this.dockManager1.DockController.Activate(doc.DocumentPanel);
            doc.Server.PropertyChanged += Server_PropertyChanged;
        }

        #region server

        void Server_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Server server = sender as Server;
            switch (e.PropertyName)
            {

                case "Error":
                    MessageBox.Show(server.Error, (server.Parent as Document).Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    server.Error = "";
                    break;
    
            }
        }

        private void bShare_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (currentDocument!=null &&(!currentDocument.Server.IsRunning || !CurrentDocument.IsCollaborating))
            {
                ShareForm sf = new ShareForm();
                sf.ShowDialog();
                if (sf.DialogResult == true)
                {
                    currentDocument.Server.DataPort = sf.DataPort;
                    currentDocument.Server.FilePort = sf.FilePort;
                    currentDocument.Password = sf.Password;
                    currentDocument.Server.IsRunning = true;
                }
            }
        }

        private void bStop_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (currentDocument.Server.IsRunning)
            {
                currentDocument.Server.IsRunning = false;
            }
        }
        #endregion

        #region Client

        private void bConnect_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

                ConnectForm sf = new ConnectForm();
                sf.ShowDialog();
                if (sf.DialogResult == true)
                {
                    Document documentAux=new Document(sf.Ip.ToString());
                    documentAux.password=sf.Password;
                    documentAux.dataPort=sf.DataPort;
                    documentAux.filePort=sf.FilePort;
                    documentAux.Visibility = Visibility.Hidden;


                    documentGroup1.Items.Add(documentAux.DocumentPanel);
                    this.dockManager1.DockController.Activate(documentAux.DocumentPanel);
                    
                    documentAux.Client.DataPort=sf.DataPort;
                    documentAux.Client.PropertyChanged+= auxClient_PropertyChanged;
                    documentAux.Client.Connect(sf.Ip);
                    ConnectingState = "Trying to connect to " + sf.Ip;
                }
        }

        #endregion

        void auxClient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Client client=sender as Client;
            switch (e.PropertyName)
            {
                case "State":
                    if (client.State == CommonNetworkEnums.ClientState.Connected)
                    {
                        ConnectingState = "Validating password...";
                        client.PropertyChanged -= auxClient_PropertyChanged;

                    }
                    break;
                case "Error":
                    client.PropertyChanged -= auxClient_PropertyChanged;
                    ConnectingState ="";
                    MessageBox.Show(client.Error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}
