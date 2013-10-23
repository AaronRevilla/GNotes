using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GraphicNotes.Configuration
{
    class ConfigurationHandler
    {
        private XDocument xDocument;
        private String[] paths = new String[3];

        public ConfigurationHandler()
        {
            paths[0] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GraphicsNotes");
            paths[1] = Path.Combine(paths[0], "Local");
            paths[2] = Path.Combine(paths[1], "config.xml");
        }

        public ConfigurationData GetConfigurationData(){
            LoadXMLConfiguration();
            ConfigurationData cd=new ConfigurationData();
            var query = from element in xDocument.Descendants("add") select element ;
            foreach (XElement aux in query)
            {
                String key = aux.Attributes("key").Single().Value.ToString();
                switch (key)
                {
                    case "ThemeName":
                        cd.ThemeName = aux.Attributes("value").Single().Value;
                        break;
                    case "DataPort":
                        cd.DataPort = Int32.Parse(aux.Attributes("value").Single().Value.ToString());
                        break;
                    case "FilePort":
                        cd.FilePort = Int32.Parse(aux.Attributes("value").Single().Value.ToString());
                        break;
                }
            }
            return cd;
        }

        public object GetValue(String key){
            if (xDocument == null)
                LoadXMLConfiguration();
            var query = xDocument.Descendants("add").Single(k => k.Attribute("key").Value.ToString().Equals(key));
            return query.Attributes("value").Single().Value;
        }

        public void LoadXMLConfiguration()
        {
            for (int i = 0; i < 2; i++)
            {
                if (!Directory.Exists(paths[i]))
                    Directory.CreateDirectory(paths[i]);
            }

            if (!File.Exists(paths[2]))
            {
                xDocument = XDocument.Parse(CreateDefaultConfiguration());
                xDocument.Save(paths[2]);
            }

            xDocument = XDocument.Load(paths[2]);
        }


        private String CreateDefaultConfiguration()
        {
            return String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <configuration>
                        <add key=""ThemeName"" value=""Office2013""/>
                        <add key=""DataPort"" value=""9090""/>
                        <add key=""FilePort"" value=""9091""/>
                    </configuration>", paths[2]);
        }
    }
}
