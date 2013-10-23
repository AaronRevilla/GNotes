using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GNTools;
using System.ComponentModel;
using System.Xml.Linq;

namespace GNTools
{
    public class Synchronizer:INotifyPropertyChanged
    {

        private LinkedList<XElement> tasksForClient;
        private LinkedList<XElement> tasksForServer;

        private LinkedList<XElement> entriesForClient;
        private LinkedList<XElement> entriesForServer;

        private bool isSynchronizing = false;
        public bool IsSynchronizing
        {
            get { return isSynchronizing; }
            set
            {
                if (isSynchronizing != value)
                {
                    isSynchronizing = value;
                    NotifyPropertyChanged("IsSynchronizing");
                }
            }
        }

        private String state = "";
        public String State
        {
            get { return state; }
            set
            {
                state = value;
                NotifyPropertyChanged("State");
            }
        }

        private String error = "";
        public String Error
        {
            get { return error; }
            set
            {
                error = value;
                NotifyPropertyChanged("Error");
            }
        }

        public XElement GetSynchronizationMessageRequest(XMLRepositoryHandler repository, String repositoryName, String password)
        {
            IsSynchronizing = true;
            Error = "";
            State = "Initializing...";
            repository.LoadXMLRepository(CommonNetworkEnums.Storage.Local, repositoryName);
            XElement message=DataConnectionHandler.GetMessageSkeleton();
            message.Attribute("type").Value=CommonNetworkEnums.MessageType.Synchronization.ToString();
            message.Attribute("action").Value = CommonNetworkEnums.SynchronizationActions.ClientRequest.ToString();
            if (repository.IsLoaded)
            {
                if (repository.ValidateCredentials(repositoryName, password))
                {
                    message.Element("parameters").Add(DataConnectionHandler.GetCustomParameter("repository", "true", repository.XmlDocument.Root));
                }
                else
                {
                    State = "";
                    Error = "Repository name and password provided are incorrect.";
                    IsSynchronizing = false;
                    return null;
                }
            }
            else
            {
                message.Element("parameters").Add(DataConnectionHandler.GetCustomParameter("repository", "false", null));
            }
            message.Element("parameters").Add(DataConnectionHandler.GetCustomParameter("repositoryName", repositoryName, null));
            message.Element("parameters").Add(DataConnectionHandler.GetCustomParameter("password", password, null));
            return message;
        }

        public XElement Synchronize(ref XElement xmlServer, XElement xmlClient)
        {
            tasksForClient = new LinkedList<XElement>();
            tasksForServer = new LinkedList<XElement>();
            entriesForClient = new LinkedList<XElement>();
            entriesForServer = new LinkedList<XElement>();
            XElement message = DataConnectionHandler.GetMessageSkeleton();
            message.Attribute("type").Value = CommonNetworkEnums.MessageType.Synchronization.ToString();
            message.Attribute("action").Value = CommonNetworkEnums.SynchronizationActions.ServerResponse.ToString();
            if (xmlClient == null)
                xmlClient = XMLRepositoryHandler.GetRepositorySkeleton();
            XElement xmlTempS = new XElement(xmlServer);
            XElement xmlTempC = new XElement(xmlServer);
            SynchronizeGroups(xmlTempS,xmlTempC,xmlClient);
            SynchronizeTasks(xmlTempS,xmlTempC,xmlClient);

            xmlServer = xmlTempS;

            XElement paramR = DataConnectionHandler.GetCustomParameter("repository", "repository", xmlTempC);
            message.Element("parameters").Add(paramR);

            if (tasksForClient.Count > 0)
            {
                XElement param = DataConnectionHandler.GetCustomParameter("files", "taskForClient", null);
                foreach (XElement f in tasksForClient)
                    param.Add(f);
                message.Element("parameters").Add(param);
            }
            if (tasksForServer.Count > 0)
            {
                XElement param = DataConnectionHandler.GetCustomParameter("files", "taskForServer", null);
                foreach (XElement f in tasksForServer)
                    param.Add(f);
                message.Element("parameters").Add(param);
            }
            if (entriesForClient.Count > 0)
            {
                XElement param = DataConnectionHandler.GetCustomParameter("files", "entriesForClient", null);
                foreach (XElement f in entriesForClient)
                    param.Add(f);
                message.Element("parameters").Add(param);
            }
            if (entriesForServer.Count > 0)
            {
                XElement param = DataConnectionHandler.GetCustomParameter("files", "entriesForServer", null);
                foreach (XElement f in entriesForServer)
                    param.Add(f);
                message.Element("parameters").Add(param);
            }
            

            return message;

        }

        private void SynchronizeGroups(XElement xmlTempS,XElement xmlTempC, XElement xmlClient )
        {

            foreach (XElement groupC in xmlClient.Descendants("group"))
            {
                XElement groupT = null;
                XElement groupTC = null;
                var match1 = xmlTempS.Descendants("group").Where(x => x.Attribute("name").Value.Equals(groupC.Attribute("name").Value));
                var match1TC = xmlTempC.Descendants("group").Where(x => x.Attribute("name").Value.Equals(groupC.Attribute("name").Value));
                if (match1.Count() > 0)
                {
                    groupT = match1.First();
                    groupTC = match1TC.First();
                    if (bool.Parse(groupC.Attribute("deleted").Value) == true)
                    {
                        groupT.Remove();
                        groupT = null;
                        groupTC.Remove();
                        groupTC = null;
                    }
                }
                else
                {
                    if (bool.Parse(groupC.Attribute("deleted").Value) == false)
                    {
                        groupT = new XElement(groupC);
                        xmlTempS.Element("groups").Add(groupT);

                        groupTC = new XElement(groupC);
                        xmlTempC.Element("groups").Add(groupTC);
                    }
                }
                if (groupT != null)
                    SynchronizeTeams(groupC, groupT, groupTC);
            }
        }

        private void SynchronizeTasks(XElement xmlTempS, XElement xmlTempC, XElement xmlClient)
        {


            foreach (XElement taskC in xmlClient.Descendants("task"))
            {
                var match1 = xmlTempS.Descendants("task").Where(x => x.Attribute("name").Value.Equals(taskC.Attribute("name").Value));
                var match1TC = xmlTempC.Descendants("task").Where(x => x.Attribute("name").Value.Equals(taskC.Attribute("name").Value));
                if (match1.Count() > 0)
                {
                    XElement taskT = match1.First();
                    XElement taskTC = match1TC.First();
                    if (bool.Parse(taskC.Attribute("deleted").Value) == true)
                    {
                        taskT.Remove();
                        taskT = null;
                        taskTC.Remove();
                        taskTC = null;
                    }
                    else
                    {
                        DateTime dateClient, dateTemp;
                        dateClient = DateTime.Parse(taskC.Attribute("date").Value);
                        dateTemp = DateTime.Parse(taskT.Attribute("date").Value);
                        int result = DateTime.Compare(dateClient, dateTemp);
                        if (result > 0)
                        {
                            taskT.Attribute("individual").Value = taskC.Attribute("individual").Value;
                            taskT.Attribute("date").Value = taskC.Attribute("date").Value;
                            taskT.Element("description").Value = taskC.Element("description").Value;

                            taskTC.Attribute("individual").Value = taskC.Attribute("individual").Value;
                            taskTC.Attribute("date").Value = taskC.Attribute("date").Value;
                            taskTC.Element("description").Value = taskC.Element("description").Value;
                        }

                        dateClient = DateTime.Parse(taskC.Attribute("uploaded").Value);
                        dateTemp = DateTime.Parse(taskT.Attribute("uploaded").Value);
                        result = DateTime.Compare(dateClient, dateTemp);
                        if (result > 0) //si fecha mayor a 0 entonces el cliente tiene la version mas reciente del archivo
                        {
                            tasksForServer.AddLast(GetTaskAux(xmlTempS.Attribute("name").Value, taskC.Attribute("name").Value));
                            //taskT.Attribute("uploaded").Value = taskC.Attribute("uploaded").Value; //taskT se queda igual hasta recibir archivo
                            taskTC.Attribute("uploaded").Value = taskC.Attribute("uploaded").Value;
                        }
                        else if (result < 0) //si fecha menor a 0 entonces el servidor tiene la version mas reciente del archivo
                        {

                            tasksForClient.AddLast(GetTaskAux(xmlTempS.Attribute("name").Value, taskT.Attribute("name").Value));
                            taskTC.Attribute("uploaded").Value = taskC.Attribute("uploaded").Value;
                        }
                        //si fecha igual a 0 entonces ambos tienen la misma version
                    }
                }
                else
                {
                    if (bool.Parse(taskC.Attribute("deleted").Value) == false)
                    {
                        XElement taskT = new XElement(taskC);
                        xmlTempS.Element("tasks").Add(taskT);
                        XElement taskTC = new XElement(taskC);
                        xmlTempC.Element("tasks").Add(taskTC);

                        DateTime baseTime = DateTime.Parse("01/01/1970 00:00:00 am");
                        DateTime taskDate = DateTime.Parse(taskT.Attribute("uploaded").Value);
                        if (DateTime.Compare(taskDate, baseTime) > 0){
                            tasksForServer.AddLast(GetTaskAux(xmlTempS.Attribute("name").Value, taskT.Attribute("name").Value));
                            taskT.Attribute("uploaded").Value = baseTime.ToString();
                        }
                    }

                }
            }
            foreach (XElement taskTC in xmlTempC.Descendants("task"))
            {
                var match = xmlClient.Descendants("task").Where(x => x.Attribute("name").Value.Equals(taskTC.Attribute("name").Value));
                if (match.Count() <= 0)
                {
                    DateTime baseTime = DateTime.Parse("01/01/1970 00:00:00 am");
                    DateTime taskDate = DateTime.Parse(taskTC.Attribute("uploaded").Value);
                    if (DateTime.Compare(taskDate, baseTime) > 0)
                    {
                        tasksForClient.AddLast(GetTaskAux(xmlTempS.Attribute("name").Value, taskTC.Attribute("name").Value));
                        taskTC.Attribute("uploaded").Value = baseTime.ToString();
                    }
                }
            }


        }

        private void SynchronizeTeams(XElement groupC, XElement groupT, XElement groupTC)
        {
            foreach (XElement teamC in groupC.Descendants("team"))
            {
                XElement teamT = null;
                XElement teamTC = null;
                var match2 = groupT.Descendants("team").Where(x => x.Attribute("name").Value.Equals(teamC.Attribute("name").Value));
                var match2TC = groupTC.Descendants("team").Where(x => x.Attribute("name").Value.Equals(teamC.Attribute("name").Value));
                if (match2.Count() > 0)
                {
                    teamT = match2.First();
                    teamTC = match2TC.First();
                    if (bool.Parse(teamC.Attribute("deleted").Value) == true)
                    {
                        teamT.Remove();
                        teamT = null;

                        teamTC.Remove();
                        teamTC = null;
                    }
                    else
                    {
                        DateTime dateClient, dateTemp;
                        dateClient = DateTime.Parse(teamC.Attribute("date").Value);
                        dateTemp = DateTime.Parse(teamT.Attribute("date").Value);
                        int result = DateTime.Compare(dateClient, dateTemp);
                        if (result > 0)
                        {
                            teamT.Attribute("password").Value = teamC.Attribute("password").Value;
                            teamT.Attribute("date").Value = teamC.Attribute("date").Value;

                            teamTC.Attribute("password").Value = teamC.Attribute("password").Value;
                            teamTC.Attribute("date").Value = teamC.Attribute("date").Value;
                        }

                    }
                }
                else
                {
                    if (bool.Parse(teamC.Attribute("deleted").Value) == false)
                    {
                        teamT = new XElement(teamC);
                        groupT.Add(teamT);

                        teamTC = new XElement(teamC);
                        groupTC.Add(teamT);
                    }
                }

                if (teamT != null)
                {
                    SynchronizePeople(teamC, teamT, teamTC);
                    SynchronizeTeamEntries(teamC, teamT, teamTC);
                }
            }
        }

        private void SynchronizeTeamEntries(XElement teamC, XElement teamT, XElement teamTC)
        {
            foreach (XElement entryC in teamC.Element("entries").Descendants("entry"))
            {
                XElement entryT = null;
                XElement entryTC = null;
                var match3 = teamT.Element("entries").Descendants("entry").Where(x => x.Attribute("task").Value.Equals(entryC.Attribute("task").Value));
                var match3TC = teamTC.Element("entries").Descendants("entry").Where(x => x.Attribute("task").Value.Equals(entryC.Attribute("task").Value));
                if (match3.Count() > 0)
                {
                    entryT = match3.First();
                    entryTC = match3TC.First();
                    if (bool.Parse(entryC.Attribute("deleted").Value) == true)
                    {
                        entryT.Remove();
                        entryT = null;

                        entryTC.Remove();
                        entryTC = null;
                    }
                    else
                    {
                        DateTime dateClient, dateTemp;
                        dateClient = DateTime.Parse(entryC.Attribute("date").Value);
                        dateTemp = DateTime.Parse(entryT.Attribute("date").Value);
                        int result = DateTime.Compare(dateClient, dateTemp);
                        if (result > 0)
                        {
                            entryT.Element("remark").Value = entryC.Element("remark").Value;
                            entryT.Attribute("date").Value = entryC.Attribute("date").Value;

                            entryTC.Element("remark").Value = entryC.Element("remark").Value;
                            entryTC.Attribute("date").Value = entryC.Attribute("date").Value;
                        }

                        dateClient = DateTime.Parse(entryC.Attribute("uploaded").Value);
                        dateTemp = DateTime.Parse(entryT.Attribute("uploaded").Value);
                        result = DateTime.Compare(dateClient, dateTemp);
                        if (result > 0) //si fecha mayor a 0 entonces el cliente tiene la version mas reciente del archivo
                        {

                            entriesForServer.AddLast(GetEntryAux(GetParent(entryC, "repository").Attribute("name").Value,
                                                                 GetParent(entryC, "group").Attribute("name").Value,
                                                                 GetParent(entryC, "team").Attribute("name").Value,
                                                                 "",
                                                                 entryC.Attribute("task").Value));

                            entryTC.Attribute("uploaded").Value = entryC.Attribute("uploaded").Value;
                        }
                        else if (result < 0) //si fecha menor a 0 entonces el servidor tiene la version mas reciente del archivo
                        {

                            entriesForClient.AddLast(GetEntryAux(GetParent(entryT, "repository").Attribute("name").Value,
                                                                 GetParent(entryT, "group").Attribute("name").Value,
                                                                 GetParent(entryT, "team").Attribute("name").Value,
                                                                 "",
                                                                 entryT.Attribute("task").Value));
                            entryTC.Attribute("uploaded").Value = entryC.Attribute("uploaded").Value;
                        }
                        //si fecha igual a 0 entonces ambos tienen la misma version

                    }
                }
                else
                {

                    if (bool.Parse(entryC.Attribute("deleted").Value) == false)
                    {
                        entryT = new XElement(entryC);
                        teamT.Element("entries").Add(entryT);

                        entryTC = new XElement(entryC);
                        teamTC.Element("entries").Add(entryTC);

                        DateTime baseTime = DateTime.Parse("01/01/1970 00:00:00 am");
                        DateTime taskDate = DateTime.Parse(entryT.Attribute("uploaded").Value);
                        if (DateTime.Compare(taskDate, baseTime) > 0)
                        {
                            entriesForServer.AddLast(GetEntryAux(GetParent(entryT, "repository").Attribute("name").Value,
                                                        GetParent(entryT, "group").Attribute("name").Value,
                                                        GetParent(entryT, "team").Attribute("name").Value,
                                                        "",
                                                        entryT.Attribute("task").Value));
                            entryT.Attribute("uploaded").Value = baseTime.ToString();
                        }
                    }
                }
            }
            foreach (XElement entryT in teamT.Element("entries").Descendants("entry"))
            {
                var match = teamC.Element("entries").Descendants("entry").Where(x => x.Attribute("task").Value.Equals(entryT.Attribute("task").Value));
                if (match.Count() <= 0)
                {
                    DateTime baseTime = DateTime.Parse("01/01/1970 00:00:00 am");
                    DateTime taskDate = DateTime.Parse(entryT.Attribute("uploaded").Value);
                    if (DateTime.Compare(taskDate, baseTime) > 0)
                    {
                        entriesForClient.AddLast(GetEntryAux(GetParent(entryT, "repository").Attribute("name").Value,
                                                        GetParent(entryT, "group").Attribute("name").Value,
                                                        GetParent(entryT, "team").Attribute("name").Value,
                                                        "",
                                                        entryT.Attribute("task").Value));
                        entryT.Attribute("uploaded").Value = baseTime.ToString();
                    }
                }
            }

        }

        private void SynchronizePeople(XElement teamC, XElement teamT, XElement teamTC)
        {

            foreach (XElement personC in teamC.Descendants("person"))
            {
                XElement personT = null;
                XElement personTC = null;
                var match3 = teamT.Descendants("person").Where(x => x.Attribute("id").Value.Equals(personC.Attribute("id").Value));
                var match3TC = teamTC.Descendants("person").Where(x => x.Attribute("id").Value.Equals(personC.Attribute("id").Value));
                if (match3.Count() > 0)
                {
                    personT = match3.First();
                    personTC = match3TC.First();
                    if (bool.Parse(personC.Attribute("deleted").Value) == true)
                    {
                        personT.Remove();
                        personT = null;

                        personTC.Remove();
                        personTC = null;
                    }
                    else
                    {
                        DateTime dateClient, dateTemp;
                        dateClient = DateTime.Parse(personC.Attribute("date").Value);
                        dateTemp = DateTime.Parse(personT.Attribute("date").Value);
                        int result = DateTime.Compare(dateClient, dateTemp);
                        if (result > 0)
                        {
                            personTC.Attribute("name").Value = personC.Attribute("name").Value;
                            personTC.Attribute("password").Value = personC.Attribute("password").Value;
                            personTC.Attribute("date").Value = personC.Attribute("date").Value;

                            personTC.Attribute("name").Value = personC.Attribute("name").Value;
                            personTC.Attribute("password").Value = personC.Attribute("password").Value;
                            personTC.Attribute("date").Value = personC.Attribute("date").Value;
                        }
                    }
                }
                else
                {
                    if (bool.Parse(personC.Attribute("deleted").Value) == false)
                    {
                        personT = new XElement(personC);
                        teamT.Element("members").Add(personT);

                        personTC = new XElement(personC);
                        teamTC.Element("members").Add(personTC);
                    }

                }

                if (personT != null)
                {
                    SynchronizePersonEntries(personC, personT, personTC);
                }
            }
        }

        private void SynchronizePersonEntries(XElement personC, XElement personT, XElement personTC)
        {
            foreach (XElement entryC in personC.Descendants("entry"))
            {
                XElement entryT = null;
                XElement entryTC = null;
                var match4 = personT.Descendants("entry").Where(x => x.Attribute("task").Value.Equals(entryC.Attribute("task").Value));
                var match4TC = personTC.Descendants("entry").Where(x => x.Attribute("task").Value.Equals(entryC.Attribute("task").Value));
                if (match4.Count() > 0)
                {
                    entryT = match4.First();
                    entryTC = match4TC.First();
                    if (bool.Parse(entryC.Attribute("deleted").Value) == true)
                    {
                        entryT.Remove();
                        entryT = null;

                        entryTC.Remove();
                        entryTC = null;
                    }
                    else
                    {
                        DateTime dateClient, dateTemp;
                        dateClient = DateTime.Parse(entryC.Attribute("date").Value);
                        dateTemp = DateTime.Parse(entryT.Attribute("date").Value);
                        int result = DateTime.Compare(dateClient, dateTemp);
                        if (result > 0)
                        {
                            entryT.Element("remark").Value = entryC.Element("remark").Value;
                            entryT.Attribute("date").Value = entryC.Attribute("date").Value;

                            entryTC.Element("remark").Value = entryC.Element("remark").Value;
                            entryTC.Attribute("date").Value = entryC.Attribute("date").Value;
                        }

                        dateClient = DateTime.Parse(entryC.Attribute("uploaded").Value);
                        dateTemp = DateTime.Parse(entryT.Attribute("uploaded").Value);
                        result = DateTime.Compare(dateClient, dateTemp);
                        if (result > 0) //si fecha mayor a 0 entonces el cliente tiene la version mas reciente del archivo
                        {

                            entriesForServer.AddLast(GetEntryAux(GetParent(entryC, "repository").Attribute("name").Value,
                                                                 GetParent(entryC, "group").Attribute("name").Value,
                                                                 GetParent(entryC, "team").Attribute("name").Value,
                                                                 personC.Attribute("name").Value,
                                                                 entryC.Attribute("task").Value));

                            entryTC.Attribute("uploaded").Value = entryC.Attribute("uploaded").Value;
                        }
                        else if (result < 0) //si fecha menor a 0 entonces el servidor tiene la version mas reciente del archivo
                        {

                            entriesForClient.AddLast(GetEntryAux(GetParent(entryT, "repository").Attribute("name").Value,
                                                                 GetParent(entryT, "group").Attribute("name").Value,
                                                                 GetParent(entryT, "team").Attribute("name").Value,
                                                                 personT.Attribute("name").Value,
                                                                 entryT.Attribute("task").Value));

                            entryTC.Attribute("uploaded").Value = entryC.Attribute("uploaded").Value;
                        }
                        //si fecha igual a 0 entonces ambos tienen la misma version
                    }
                }
                else
                {
                    if (bool.Parse(entryC.Attribute("deleted").Value) == false)
                    {
                        entryT = new XElement(entryC);
                        personT.Element("entries").Add(entryT);

                        entryTC = new XElement(entryC);
                        personTC.Element("entries").Add(entryTC);

                        DateTime baseTime = DateTime.Parse("01/01/1970 00:00:00 am");
                        DateTime taskDate = DateTime.Parse(entryT.Attribute("uploaded").Value);
                        if (DateTime.Compare(taskDate, baseTime) > 0)
                        {
                            entriesForServer.AddLast(GetEntryAux(GetParent(entryT, "repository").Attribute("name").Value,
                                                        GetParent(entryT, "group").Attribute("name").Value,
                                                        GetParent(entryT, "team").Attribute("name").Value,
                                                        personT.Attribute("name").Value,
                                                        entryT.Attribute("task").Value));
                            entryT.Attribute("uploaded").Value = baseTime.ToString();
                        }
                    }
                }
            }
            foreach (XElement entryT in personT.Descendants("entry"))
            {
                var match = personC.Descendants("entry").Where(x => x.Attribute("task").Value.Equals(entryT.Attribute("task").Value));
                if (match.Count() <= 0)
                {
                    DateTime baseTime = DateTime.Parse("01/01/1970 00:00:00 am");
                    DateTime taskDate = DateTime.Parse(entryT.Attribute("uploaded").Value);
                    if (DateTime.Compare(taskDate, baseTime) > 0)
                    {
                        entriesForClient.AddLast(GetEntryAux(GetParent(entryT, "repository").Attribute("name").Value,
                                                        GetParent(entryT, "group").Attribute("name").Value,
                                                        GetParent(entryT, "team").Attribute("name").Value,
                                                        personT.Attribute("name").Value,
                                                        entryT.Attribute("task").Value));
                        entryT.Attribute("uploaded").Value = baseTime.ToString();
                    }
                }
            }
        }

        public XElement GetTaskAux(String repositoryName, String nameTask)
        {
            XElement task = XElement.Parse(@"<task repository="""" name="""" />");
            task.Attribute("repository").Value = repositoryName;
            task.Attribute("name").Value = nameTask;
            return task;
        }

        public XElement GetEntryAux(String repositoryName, String groupName, String teamName, String personName, String entryName)
        {
            XElement entry = XElement.Parse(@"<entry repositoryName="""" groupName="""" teamName="""" personName="""" entryName=""""   />");
            entry.Attribute("repositoryName").Value = repositoryName;
            entry.Attribute("groupName").Value = groupName;
            entry.Attribute("teamName").Value = teamName;
            entry.Attribute("personName").Value = personName;
            entry.Attribute("entryName").Value = entryName;
            return entry;
        }

        public XElement GetParent(XElement element, String type)
        {
            XElement aux = element.Parent;
            if (aux.Name == type)
            {
                return aux;
            }
            else
            {
                return GetParent(aux, type);
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
