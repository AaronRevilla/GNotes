using GNTools;
using GraphicNotes.Views.Workspace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GraphicNotes.Communication
{
    public class SMessageDispatcher
    {
        private ObservableCollection<Collaborator> collaborators;
        public ObservableCollection<Collaborator> Collaborators { get { return collaborators; } }
        private CanvasWorkspace canvasWorkspace;
        public SMessageDispatcher(CanvasWorkspace canvasWorkspace)
        {
            this.canvasWorkspace = canvasWorkspace;
            collaborators = new ObservableCollection<Collaborator>();
        }

        public void AddCollaborator(Collaborator collaborator)
        {
            collaborator.PropertyChanged += collaborator_PropertyChanged;
            collaborators.Add(collaborator);
        }

        void collaborator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Collaborator collaborator = sender as Collaborator;
            switch (e.PropertyName)
            {
                case "NewMessage":
                    XElement message = collaborator.NewMessages.First();
                    collaborator.NewMessages.RemoveFirst();
                    ProccessMessage(message);
                    break;
            }
        }

        private void ProccessMessage(XElement message)
        {


        }
    }
}
