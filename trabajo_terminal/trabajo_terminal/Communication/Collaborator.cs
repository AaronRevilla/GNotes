using GNTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GraphicNotes.Communication
{
    public class Collaborator:INotifyPropertyChanged
    {

        private DataConnectionHandler connection;
        public DataConnectionHandler Connection { get { return connection; } }

        private LinkedList<XElement> newMessages;
        public LinkedList<XElement> NewMessages { get { return newMessages; } }

        private void AddNewMessage(XElement msg)
        {

            if (newMessages != null)
            {
                newMessages.AddLast(msg);
                NotifyPropertyChanged("NewMessage");
            }

        }

        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private String id;
        public String ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        public Collaborator(String id)
        {
            ID = id;
        }

        public Collaborator(String id, DataConnectionHandler client):this(id)
        {
            this.connection = client;
            this.connection.PropertyChanged += connection_PropertyChanged;
        }

        void connection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName){
                case "NewMessage":
                    XElement lastMessage = connection.NewMessages.ElementAt(0);
                    connection.NewMessages.Remove(lastMessage);
                    AddNewMessage(lastMessage);
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
