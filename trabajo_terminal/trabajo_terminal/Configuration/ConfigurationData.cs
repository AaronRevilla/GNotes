using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GraphicNotes.Configuration
{
    class ConfigurationData
    {
        public String ThemeName { get; set; }
        public int DataPort{ get; set; }
        public int FilePort { get; set; }

    }
}
