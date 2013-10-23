using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GNTools
{
    public class Client:INotifyPropertyChanged
    {
        public Object Parent
        {
            get;
            set;
        }

        private TcpClient dataClient;


        public TcpClient DataClient
        {
            get { return dataClient; }
        }
        private String ipAddress;
        private int dataPort;
        public int DataPort { get { return dataPort; } set { dataPort = value; } }


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

        private CommonNetworkEnums.ClientState state = CommonNetworkEnums.ClientState.Disconnected;
        public CommonNetworkEnums.ClientState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    NotifyPropertyChanged("State");
                }
            }
        }

        public void Connect(String ipAddress)
        {
            this.ipAddress = ipAddress;
            this.State = CommonNetworkEnums.ClientState.Connecting;
            Thread connectingThread = new Thread(new ThreadStart(Connect));
            connectingThread.Start();
        }

        private void Connect()
        {

            dataClient = new TcpClient();

            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(this.ipAddress), dataPort);
                dataClient.Connect(endPoint);
                State = CommonNetworkEnums.ClientState.Connected;
            }
            catch (FormatException e)
            {
                State = CommonNetworkEnums.ClientState.Disconnected;
                Error = "Malformed IP Address.";
                return;
            }
            catch (SocketException e)
            {
                State = CommonNetworkEnums.ClientState.Disconnected;
                Error = String.Format("Error when trying to connect to server:\n{0}", e.Message);
                return;
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
