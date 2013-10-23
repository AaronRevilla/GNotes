using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;


namespace GraphicNotes.Repository.Commands
{
    class UpdateCommand:ICommand
    {
   

        public UpdateCommand()
        {
            
        }

        public bool CanExecute(object parameter)
        {
            if (parameter != null)
                return true;
            else
                return false;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("Command");
        }


        public event EventHandler CanExecuteChanged;

    }
}
