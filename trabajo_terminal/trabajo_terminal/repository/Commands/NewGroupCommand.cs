using GraphicNotes.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;


namespace GraphicNotes.DocumentServer.RibbonBarCommands
{
    class NewGroupCommand:ICommand
    {

        public NewGroupCommand()
        {
            
        }

        public bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                Console.WriteLine(parameter);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            
        }




        public event EventHandler CanExecuteChanged;
    }
}
