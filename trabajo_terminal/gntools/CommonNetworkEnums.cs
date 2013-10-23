using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNTools
{
    public class CommonNetworkEnums
    {
        

        public enum MessageType
        {
            Synchronization,
            Download,
            Upload
        }

        public enum SynchronizationActions
        {
            Error,
            ClientRequest,
            ServerResponse
        }

        public enum ActionType
        {
            Request,
            Response,
            Error,
        }

        public enum ClientState
        {
            Disconnected,
            Connecting,
            Connected
        }

        public enum Storage
        {
            Local,
            Remote
        }
    }
}
