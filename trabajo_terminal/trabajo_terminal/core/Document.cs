using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpf.Docking;
using System.Windows;
using GraphicNotes.Views.Workspace;
using DevExpress.Xpf.Editors;
using System.Windows.Controls;
using System.Windows.Media;
using GNTools;
using System.Net.Sockets;
using GraphicNotes.Communication;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq.Expressions;


namespace GraphicNotes.Core
{
    public class Document : INotifyPropertyChanged
    {
        private Visibility visibility=Visibility.Visible;
        public Visibility Visibility
        {
            get { return visibility;}
            set { visibility = value; NotifyPropertyChanged("Visibility"); }
        }



        public int dataPort;
        public int filePort;
        public String password = "";
        public String Password
        {
            get { return password; }
            set
            {
                if (!password.Equals(value))
                {
                    password = value;
                }
            }
        }
        public String name = "";
        public String Name
        {
            get { return name; }
            set
            {
                if (!name.Equals(value))
                {
                    name = value;
                }
            }
        }

        private Server server;
        public Server Server { get { return server; } }

        private Client client;
        public Client Client { get { return client; } }

        private SMessageDispatcher sMessageDispatcher;
        public SMessageDispatcher SMessageDispatcher { get { return sMessageDispatcher; } }

        private DocumentPanel documentPanel;
        public DocumentPanel DocumentPanel{
         get{ return documentPanel; }
         set{ documentPanel=value;  }
        }

        private bool isCollaborating = false;
        public bool IsCollaborating
        {
            get { return isCollaborating; }
            set
            {
                if (isCollaborating != value)
                {
                    isCollaborating = value;
                }
            }
        }


        public Document(String name)
        {
            this.Name = name;
            documentPanel = new DocumentPanel();
            documentPanel.DataContext = this;
            documentPanel.Caption = Name;
            documentPanel.ClosingBehavior = ClosingBehavior.ImmediatelyRemove;
            CanvasWorkspaceContainer cwc = new CanvasWorkspaceContainer();
            cwc.Loaded+=cwc_Loaded;
            documentPanel.Content = cwc;
            documentPanel.SetBinding(DocumentPanel.VisibilityProperty, new Binding("Visibility"));

            server = new Server();
            server.Parent = this;
            server.PropertyChanged += server_PropertyChanged;

            client = new Client();
            client.Parent = this;
            client.PropertyChanged += client_PropertyChanged;

            
        }

        void client_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Client auxClient = sender as Client;
            switch(e.PropertyName){
                case "State":
                    if (auxClient.State == CommonNetworkEnums.ClientState.Connected)
                    {
                        Visibility = Visibility.Visible;
                    }
                    else if (auxClient.State == CommonNetworkEnums.ClientState.Disconnected)
                    {
                        UIUtils.SetPropertyThreadSafe(documentPanel, ()=>documentPanel.Closed, true);
                    
                    }
                    break;
            }
        }

        void cwc_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasWorkspaceContainer cwc=sender as CanvasWorkspaceContainer;
 	        sMessageDispatcher=new SMessageDispatcher(cwc.Template.FindName("PART_CanvasWorkspace", cwc) as CanvasWorkspace);
        }

        void server_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsRunning":
                    if (server.IsRunning)
                    {
                        sMessageDispatcher.Collaborators.Clear();
                        server.clientCounter = 0;
                    }
                    break;
                case "NewDataClient":
                    Socket client = server.DataClientsQueue.ElementAt(0);
                    server.DataClientsQueue.Remove(client);
                    DataConnectionHandler connection = new DataConnectionHandler(client, server);
                    Collaborator collaborator = new Collaborator((++server.clientCounter).ToString("X6"));
                    sMessageDispatcher.Collaborators.Add(collaborator);
                    connection.Start();
                    break;
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

    
    
    }
}
