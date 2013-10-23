using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GraphicNotes.Core.RibbonBarCommands
{
    class NewDocumentCommand:ICommand
    {
        private int documentsCounter = 1;

        private ObservableCollection<Document> documents;
        public NewDocumentCommand(ObservableCollection<Document> documents)
        {
            this.documents = documents;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            
        }
    }
}
