using DevExpress.Xpf.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GraphicNotes.DocumentServer.RibbonBarCommands
{
    class ManageDocumentServersCommand:ICommand
    {
     
        public ManageDocumentServersCommand()
        {
            
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DockLayoutManager dlm = parameter as DockLayoutManager;
            if (dlm != null)
            {
                BaseLayoutItem panel = dlm.GetItem("serverManagementPanel");
                if(panel!=null)
                 dlm.DockController.Restore(panel);
            }
        }




        public event EventHandler CanExecuteChanged;
    }
}
