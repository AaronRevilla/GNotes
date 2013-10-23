using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GNTools
{
    public class XMLRepositoryHandler : INotifyPropertyChanged
    {
        private String path="";
        private XDocument xmlDocument;
        private object locker = new Object();
        public XDocument XmlDocument { get { return xmlDocument; } set { xmlDocument = value; } }
        private CommonNetworkEnums.Storage storageType;

        private String error = "";
        public String Error
        {
            get { return error; }
            set
            {
                error = value;
                if (!String.IsNullOrEmpty(error))
                {
                    NotifyPropertyChanged("Error");
                }
            }
        }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set
            {
                if (isLoaded != value)
                {
                    isLoaded = value;
                    NotifyPropertyChanged("IsLoaded");
                }
            }
        }

        public XMLRepositoryHandler(CommonNetworkEnums.Storage storage, String repositoryName)
        {
            this.storageType = storage;
            switch (storageType)
            {
                case CommonNetworkEnums.Storage.Local:
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GraphicsNotes", "Local", "Repositories", repositoryName, String.Format("{0}.xml", repositoryName));
                    break;
                case CommonNetworkEnums.Storage.Remote:
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GraphicsNotes", "Server", "Repositories", repositoryName, String.Format("{0}.xml", repositoryName));
                    break;
            }
            LoadXMLRepository();
        }

        public XMLRepositoryHandler()
        {
            
        }


        private void LoadXMLRepository()
        {
            try
            {
                xmlDocument = XDocument.Load(path);
                IsLoaded = true;
            }
            catch (Exception)
            {
                Error = "The repository file does not exist or is malformed";
                IsLoaded = false;
            }
        }

        public void UnLoad()
        {
            xmlDocument = null;
            IsLoaded = false;
        }

        public void LoadXMLRepository(CommonNetworkEnums.Storage storage, String repositoryName)
        {
            this.storageType = storage;
            switch (storageType)
            {
                case CommonNetworkEnums.Storage.Local:
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GraphicsNotes", "Local", "Repositories", repositoryName, String.Format("{0}.xml", repositoryName));
                    break;
                case CommonNetworkEnums.Storage.Remote:
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GraphicsNotes", "Server", "Repositories", repositoryName, String.Format("{0}.xml", repositoryName));
                    break;
            }
            LoadXMLRepository();
        }

        public void SaveChanges()
        {
            if (IsLoaded)
            {
                xmlDocument.Save(path);
            }
        }

        public bool ValidateCredentials(String repositoryName, String pass)
        {
            if (IsLoaded == true)
            {
                String name = xmlDocument.Root.Attribute("name").Value;
                String password = xmlDocument.Root.Attribute("password").Value;
                if (name.Equals(repositoryName) && password.Equals(pass))
                    return true;
            }
                return false;
        }

        public static XElement GetRepositorySkeleton()
        {
            return XElement.Parse(@"<repository name="" "" password="" "" enabled="" "">
                                        <groups></groups>
                                        <tasks></tasks>
                                  </repository>
                                    ");

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
