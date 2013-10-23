using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GNTools
{
    public class XMLServerHandler
    {
        

        private String[] paths = new String[4];
        private XDocument xmlDocument;
        private object locker = new Object();
        public XDocument XmlDocument { get { return xmlDocument; } }

        public XMLServerHandler() {
            paths[0] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GraphicsNotes");
            paths[1] = Path.Combine(paths[0], "Server");
            paths[2] = Path.Combine(paths[1], "Repositories");
            paths[3] = Path.Combine(paths[1], "ServerData.xml");
            LoadXMLServer();
        }

        public void LoadXMLServer()
        {
            for(int i=0; i<3; i++){
                if (!Directory.Exists(paths[i]))
                    Directory.CreateDirectory(paths[i]);
                }

            if (!File.Exists(paths[3]))
            {
                xmlDocument = XDocument.Parse(CreateXMLSkeleton());
                xmlDocument.Save(paths[3]);
            }

            xmlDocument = XDocument.Load(paths[3]);
        }

        



        //private Repository GetRepositoryFromElement(XElement element)
        //{
        //    Repository repository = new Repository(element.Attributes("Name").Single().Value, element.Attributes("Password").Single().Value);
        //    repository.Enabled =Boolean.Parse(element.Attributes("Enabled").Single().Value);
        //    return repository;
        //}

        public void AddRepository(XElement repository)
        {
            XElement element = xmlDocument.Descendants("repositories").Single();
            element.Add(repository);
            CreateRepositoryFile(repository);
            SaveChanges();
           
        }

        public void DeleteRepository(XElement repository)
        {
            repository.Remove();
            SaveChanges();
        }

        public void EditRepository(XElement repository)
        {
            XElement element = xmlDocument.Descendants("repository").Single(e => e.Attribute("name").Value == repository.Attribute("name").Value);
            element.Attribute("password").Value = repository.Attribute("password").Value;
            element.Attribute("enabled").Value = repository.Attribute("enabled").Value;
            SaveChanges();
        }



        private void CreateRepositoryFile(XElement repository){
            Console.WriteLine(repository==null?"Null":repository.Name);
            String repositoryFolder = Path.Combine(paths[2], repository.Attribute("name").Value);
             if (!Directory.Exists(repositoryFolder))
                    Directory.CreateDirectory(repositoryFolder);
             XDocument aux = XDocument.Parse(CreateXMLRepositorySkeleton(repository));
             aux.Save(Path.Combine(repositoryFolder, repository.Attribute("name").Value + ".xml"));
        }


        private string CreateXMLSkeleton()
        {
            return String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
 
                    <server>
                        <configuration dataPort=""9090"" filePort=""9091"" workingDirectory=""{0}""/>
                        <repositories>

                        </repositories>
                    </server>", paths[2]);
        }

        private string CreateXMLRepositorySkeleton(XElement repository)
        {
            return String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>

                    <repository name=""{0}"" password=""{1}"" enabled=""{2}"">
                        <groups>

                        </groups>
                        <tasks>

                        </tasks>
                    </repository>", 
                       repository.Attribute("name").Value, 
                       repository.Attribute("password").Value, 
                       repository.Attribute("enabled").Value);
        }

        public void  SaveChanges()
        {
            lock (locker)
            {
                xmlDocument.Save(paths[3]);
            }
        }
           
    }
}
