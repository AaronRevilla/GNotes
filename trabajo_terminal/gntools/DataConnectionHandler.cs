using GNTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GNTools
{
    public class DataConnectionHandler:INotifyPropertyChanged
    {
        private Socket client;
        private Server server;

        private NetworkStream networkStreamClient;

        private LinkedList<XElement> newMessages;
        public LinkedList<XElement> NewMessages{get{return newMessages;}}

        private void AddNewMessage(String msg)
        {
            XElement message = XElement.Parse(msg);

            if (newMessages != null)
            {
                newMessages.AddLast(message);
                NotifyPropertyChanged("NewMessage");
            }

        }
        private bool isListening = true;
        public bool IsListening
        {
            get { return isListening; }
            set
            {
                if (isListening != value)
                {
                    isListening = value;
                    NotifyPropertyChanged("IsListening");
                    if (isListening == false)
                        DisconnectClient();
                }
            }
        }

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

        public DataConnectionHandler(Socket client) // Constructor for client connection
        {
            this.client = client;
        }  

        public DataConnectionHandler(Socket client, Server server){
            this.client=client;
            this.server = server;
            this.server.PropertyChanged += server_PropertyChanged;
        }  //Constructor for server conenction

        void server_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsRunning":
                    if (server.IsRunning == false)
                    {
                        server.PropertyChanged -= server_PropertyChanged;
                        IsListening = false;
                    }
                    break;
            }
        }

        public void Start(){
            InitStreams();
            newMessages = new LinkedList<XElement>();
            Thread listenerTread = new Thread(new ThreadStart(ListenToDataClient));
            listenerTread.Start();
        }

        public void DisconnectClient()
        {
            IsListening = false;
            if (client != null)
            {
                client.Dispose();
                client.Close();
            }
            Utils.WriteLine("Client has disconnected.");
        }

        private void InitStreams()
        {
            networkStreamClient = new NetworkStream(client);
        }


        private void ListenToDataClient()
        {   
            while (isListening == true)
            {
                try
                {
                    
                    String[] messages = GetMessages();
                    foreach(String message in messages)
                        AddNewMessage(message);
                    
                }
                catch (IOException e)
                {
                    if (isListening == true) /// Disconnected due to error
                    {
                        IsListening = false;
                        Error = String.Format("Remote EndPoint has disconnected:\n{0}", e.Message);
                        return;
                    }
                    Utils.WriteLine("---->IOException:{0}", e.Message);
                    return;

                }
                catch (SocketException e)
                {
                    if (isListening == true) /// Disconnected due to error
                    {
                        IsListening = false;
                        Error = String.Format("Remote EndPoint has disconnected:\n{0}", e.Message);
                        return;
                    }

                    Utils.WriteLine("---->SocketException:{0}", e.Message);
                    return;
                }
            }
        }

         public void SendMessage(XElement message)
        {
            String strMessage = message.ToString() + "\u0005";
            byte[] bytes=UnicodeEncoding.Unicode.GetBytes(strMessage);

            try
            {
                int size = client.SendBufferSize;
                if (bytes.Length > size)
                {
                    int offset = 0;
                    while (offset < bytes.Length)
                    {
                        int auxSize = (bytes.Length - offset) > size ? size : (bytes.Length - offset);
                        networkStreamClient.Write(bytes, offset, auxSize);
                        networkStreamClient.Flush();
                        offset = offset + auxSize;
                    }
                }
                else
                {
                    networkStreamClient.Write(bytes, 0, bytes.Length);
                    networkStreamClient.Flush();
                    Utils.WriteLine("Sent message:" + message.Attribute("type").Value);
                }
                
            }
            catch (ObjectDisposedException e)
            {
                if (isListening == true) /// Disconnected due to error
                {
                    IsListening = false;
                    Error = String.Format("Remote EndPoint has disconnected:\n{0}", e.Message);
                    return;
                }
                Utils.WriteLine("ObjectDisposedException:{0}", e.Message);
                return;
            }
            catch (IOException e)
            {
                if (isListening == true) /// Disconnected due to error
                {
                    IsListening = false;
                    Error = String.Format("Remote EndPoint has disconnected:\n{0}", e.Message);
                    return;
                }
                Utils.WriteLine("IOException:{0}", e.Message);
                return;   
            }
        }

         public static XElement GetMessageSkeleton()
         {
             return XElement.Parse(@"
                <message type="""" action="""">
                    <parameters> </parameters>
                </message>
            ");
         }

         public static XElement GetParameterSkeleton()
         {
             return XElement.Parse(@"
                <add key="""" value="""">

                </add>
            ");
         }

         public static XElement GetCustomParameter(String key, String value, XElement data)
         {
             XElement parameter = GetParameterSkeleton();
             parameter.Attribute("key").Value=key;
             parameter.Attribute("value").Value = value;
             if(data!=null)
                 parameter.Add(data);
             return parameter;
         }

         public void SendMessageError(String type, String action, String description)
         {
             XElement message = GetMessageSkeleton();
             message.Attribute("type").Value=type;
             message.Attribute("action").Value=action;
             XElement param1 = GetParameterSkeleton();
             param1.Attribute("key").Value="description";
             param1.Value = description;
             message.Element("parameters").Add(param1); ;
             SendMessage(message);
         }

        String nextMessage = "";
        private String[] GetMessages()
        {
            String data = "" + nextMessage;
            int length;
            int size = client.ReceiveBufferSize;
            byte[] bytes=new byte[size];
            while((length=networkStreamClient.Read(bytes,0,size))>0){
                data = data + UnicodeEncoding.Unicode.GetString(bytes, 0, length);
                if (data.Contains("\u0005"))
                {
                    int n = data.Count(c => c == '\u0005');
                    String[] messages = data.Split(new char[]{'\u0005'}, StringSplitOptions.RemoveEmptyEntries);
                    if (messages.Length - n > 0)
                    {
                        nextMessage = messages[n];
                        return messages.Take(n - 1).ToArray();
                    }
                    else
                    {
                        nextMessage = "";
                        return messages;
                    }
                }
            }
            return new String[] { data };
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
