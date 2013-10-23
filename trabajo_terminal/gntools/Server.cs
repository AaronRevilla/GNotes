using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using GNTools;


namespace GNTools
{
    public class Server : INotifyPropertyChanged //Server ahi esta
    {
        public int clientCounter = 0;

        public Object Parent
        {
            get;
            set;
        }
        private TcpListener dataListener;
        private TcpListener fileListener;

        private int dataPort=9090;
        private int filePort=9091;

        public int DataPort { get { return dataPort; } set { dataPort = value; } }
        public int FilePort { get { return filePort; } set { filePort = value; } }

        private LinkedList<Socket> dataClientsQueue;
        public LinkedList<Socket> DataClientsQueue { get { return dataClientsQueue; } }


        private LinkedList<Socket> fileClientsQueue;
        public LinkedList<Socket> FileClientsQueue { get { return fileClientsQueue; } }

        private void AddNewFileClient(Socket newClient)
        {

            if (fileClientsQueue != null)
            {
                fileClientsQueue.AddLast(newClient);
                NotifyPropertyChanged("NewFileClient");
            }
            else
            {
                newClient.Dispose();
                newClient.Close();
            }

        }
        private void AddNewDataClient(Socket newClient)
        {

            if (dataClientsQueue != null)
            {
                dataClientsQueue.AddLast(newClient);
                NotifyPropertyChanged("NewDataClient");
            }
            else
            {
                newClient.Dispose();
                newClient.Close();
            }

        }


        private bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    NotifyPropertyChanged("IsRunning");
                    if (IsRunning == true)
                        Start();
                    else
                        Stop();
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

        public void Start()
        {

            dataClientsQueue = new LinkedList<Socket>();
            fileClientsQueue = new LinkedList<Socket>();

            IPEndPoint endPointData = new IPEndPoint(IPAddress.Any, dataPort);
            IPEndPoint endPointFile = new IPEndPoint(IPAddress.Any, filePort);

            dataListener = new TcpListener(endPointData);
            fileListener = new TcpListener(endPointFile);


            try
            {
                dataListener.Start();
                fileListener.Start();

         

                Utils.WriteLine("DataServer is Running");
                Utils.WriteLine("FileServer is Running");

                Thread thread1 = new Thread(new ThreadStart(AcceptNewDataConnections));
                Thread thread2 = new Thread(new ThreadStart(AcceptNewFileConnections));
                thread1.Start();
                thread2.Start();

            }
            catch (SocketException e)
            {
                IsRunning = false;
                Error = String.Format("Error when trying to start the server:\n{0}", e.Message);
                Utils.WriteLine("SocketException:{0}", e.Message);
            }
               
        }
        public void Stop()
        {
            IsRunning = false;
            if (dataListener != null)
            {
                dataListener.Stop();
                dataListener.Server.Dispose();
                dataListener.Server.Close();
                
            }
            if (fileListener != null)
            {
                fileListener.Stop();
                fileListener.Server.Dispose();
                fileListener.Server.Close(0);
                
            }

            if (dataClientsQueue != null)
            {
                foreach(Socket aux in dataClientsQueue){
                    aux.Dispose();
                    aux.Close();
                }
                dataClientsQueue.Clear();
            }

            if (FileClientsQueue != null)
            {
                foreach (Socket aux in FileClientsQueue)
                {
                    aux.Dispose();
                    aux.Close();
                }
                fileClientsQueue.Clear();
            }

         

            Utils.WriteLine("DataServer has been stoped");
            Utils.WriteLine("FileServer has been stoped");
        }

        private void AcceptNewDataConnections()
        {
            while (IsRunning == true)
            {
                try
                {
                    Socket client = dataListener.AcceptSocket();
                    {
                        AddNewDataClient(client);
                        Utils.WriteLine("Nuevo cliente");
                    }

                }
                catch (SocketException e)
                {
                    if (IsRunning == true) // Disconnected due to error
                    {
                        IsRunning = false;
                        Error = String.Format("Repository server connection has been lost:\n{0}", e.Message);
                        return;
                    }
                    Utils.WriteLine("SocketException:{0}", e.Message);
                }
            }
        }

        private void AcceptNewFileConnections()
        {
            while (IsRunning == true)
            {
                try
                {
                    Socket client = fileListener.AcceptSocket();
                    {
                        AddNewFileClient(client);
                    }

                }
                catch (SocketException e)
                {
                    if (IsRunning == true) // Disconnected due to error
                    {
                        IsRunning = false;
                        Error = String.Format("Repository server connection has been lost:\n{0}", e.Message);
                        return;
                    }
                    Utils.WriteLine("SocketException:{0}", e.Message);
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
