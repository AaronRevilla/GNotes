using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using GraphicNotes.Core.RibbonBarCommands;
using GraphicNotes.DocumentServer.RibbonBarCommands;
using GraphicNotes.Repository.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace GraphicNotes
{
    partial class MainWindow : Window, IDisposable
    {
        private NewDocumentCommand newDocumentCommand;

        private ManageDocumentServersCommand manageDocumentServersCommand;
        private NewGroupCommand newGroupCommand;
        private UpdateCommand updateCommand;

        private void SetCommands()
        {

            //New Document Server 
            manageDocumentServersCommand = new ManageDocumentServersCommand();
            bManageDocumentServers.Command = manageDocumentServersCommand;
            bManageDocumentServers.CommandParameter = dockManager1;

            //New Group
            newGroupCommand = new NewGroupCommand();
            bNewGroup.Command = newGroupCommand;
            bNewGroup.CommandParameter = null;
        }
    }
}
