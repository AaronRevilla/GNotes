using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GNTools
{
    public class MessageDispatcher:INotifyPropertyChanged
    {
        private Synchronizer synchronizer;
        private DataConnectionHandler connection;
        private XMLRepositoryHandler currentRepository;
        private XMLRepositoryHandler localRepository;
        private XMLServerHandler xmlServerHandler;
        private Dictionary<String, RepositoryUserCounter> repositories;

        public MessageDispatcher(DataConnectionHandler connection, XMLServerHandler xmlServerHandler, Dictionary<String, RepositoryUserCounter> repositories, Synchronizer synchronizer)
        {
            this.connection = connection;
            this.connection.PropertyChanged += connection_PropertyChanged;
            this.synchronizer = synchronizer;
            this.xmlServerHandler = xmlServerHandler;
            this.repositories = repositories;
        }

        public MessageDispatcher(DataConnectionHandler connection,  Synchronizer synchronizer)
        {
            this.connection = connection;
            this.connection.PropertyChanged += connection_PropertyChanged;
            this.synchronizer = synchronizer;
        }

        void connection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "NewMessage":
                    XElement message = connection.NewMessages.First();
                    connection.NewMessages.RemoveFirst();
                    CommonNetworkEnums.MessageType type = (CommonNetworkEnums.MessageType)Enum.Parse(typeof(CommonNetworkEnums.MessageType), message.Attribute("type").Value);
                    Utils.WriteLine("MessageArrived:"+message.ToString());
                    switch (type)
                    {
                        case CommonNetworkEnums.MessageType.Synchronization:
                            ProccessSynchronizationRequest(message);
                            break;
                    }
                    
                    break;
                case "IsListening":
                    if (currentRepository != null)
                        CloseRepository();
                    break;
            }
        }

        public void ProccessSynchronizationRequest(XElement message)
        {
            
            CommonNetworkEnums.SynchronizationActions action = (CommonNetworkEnums.SynchronizationActions)Enum.Parse(typeof(CommonNetworkEnums.SynchronizationActions), message.Attribute("action").Value);
            switch (action)
            {

                case CommonNetworkEnums.SynchronizationActions.ClientRequest:
                        XElement repository=message.Element("parameters").Descendants("add").Single(x=>x.Attribute("key").Value.Equals("repository"));
                        XElement repositoryName=message.Element("parameters").Descendants("add").Single(x=>x.Attribute("key").Value.Equals("repositoryName"));
                        XElement password=message.Element("parameters").Descendants("add").Single(x=>x.Attribute("key").Value.Equals("password"));

                    try{
                        XElement repositoryS=xmlServerHandler.XmlDocument.Root.Element("repositories").Elements("repository").Single(x=>x.Attribute("name").Value.Equals(repositoryName.Attribute("value").Value));
                        if (bool.Parse(repositoryS.Attribute("enabled").Value) == true)
                        {
                            SetCurrentRepository(repositoryName.Attribute("value").Value);
                            if (currentRepository != null)
                            {
                                XElement xmlDoc = currentRepository.XmlDocument.Root;
                                if (bool.Parse(repository.Attribute("value").Value) == true)
                                {
                                    XElement messageResponse = synchronizer.Synchronize(ref xmlDoc,XElement.Parse(repository.Value));
                                    connection.SendMessage(messageResponse);
                                }
                                else
                                {
                                    XElement messageResponse = synchronizer.Synchronize(ref xmlDoc, null);
                                    connection.SendMessage(messageResponse);
                                }
                                currentRepository.XmlDocument =new XDocument( xmlDoc);
                                currentRepository.SaveChanges();
                            }
                            else
                            {
                                connection.SendMessageError(CommonNetworkEnums.MessageType.Synchronization.ToString(), CommonNetworkEnums.SynchronizationActions.Error.ToString(), "The repository  is disable.");
                            }
                            CloseRepository();
                        }
                        else
                        {
                            connection.SendMessageError(CommonNetworkEnums.MessageType.Synchronization.ToString(), CommonNetworkEnums.SynchronizationActions.Error.ToString(), "The repository  is disable.");
                        }

                    }catch(InvalidOperationException e){
                        connection.SendMessageError(CommonNetworkEnums.MessageType.Synchronization.ToString(), CommonNetworkEnums.SynchronizationActions.Error.ToString(), "The repository name and/or password provided are incorrect");
                    }
                    break;
                case CommonNetworkEnums.SynchronizationActions.ServerResponse:

                    break;
                case CommonNetworkEnums.SynchronizationActions.Error:
                        String error = message.Element("parameters").Descendants("add").Single(x => x.Attribute("key").Value.Equals("description")).Value;
                        synchronizer.Error = error;
                        synchronizer.State = "";
                        synchronizer.IsSynchronizing = false;
                    break;
            }
            
        }

    private void SetCurrentRepository(String repositoryName)
        {
            if (currentRepository != null)
            {
                if (!currentRepository.XmlDocument.Root.Attribute("name").Value.Equals(repositoryName))
                {
                    CloseRepository();
                    SetCurrentRepository(repositoryName);
                }
            }
            else //if repository is null
            {
                RepositoryUserCounter tmp = null;
                if (repositories.TryGetValue(repositoryName, out tmp))
                {
                    tmp.Counter = tmp.Counter + 1;
                    currentRepository = tmp.Repository;
                }
                else
                {

                    XMLRepositoryHandler aux = new XMLRepositoryHandler(CommonNetworkEnums.Storage.Remote, repositoryName);
                    if (aux.IsLoaded !=false)
                    {
                        RepositoryUserCounter rc = new RepositoryUserCounter();
                        rc.Counter = rc.Counter + 1;
                        rc.Repository = aux;
                        rc.PropertyChanged += ReposiotryUserCounter_PropertyChanged;
                        repositories.Add(repositoryName, rc);
                        currentRepository = aux;
                    }
                }
            }
        }
    void ReposiotryUserCounter_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        RepositoryUserCounter aux = sender as RepositoryUserCounter;
        if (e.PropertyName.Equals("Close"))
        {
            if (repositories != null)
                repositories.Remove(aux.Repository.XmlDocument.Root.Attribute("name").Value);
        }
    }

    private void CloseRepository()
    {
        if (currentRepository != null)
        {
            RepositoryUserCounter aux;
            if (repositories.TryGetValue(currentRepository.XmlDocument.Root.Attribute("name").Value, out aux))
            {
                aux.Counter = aux.Counter - 1;
                currentRepository = null;
            }
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
