using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphicNotes.Views.Objects
{
   public class FileReader
    {
    string filePath = null;

        public FileReader(string r)
        {
            filePath = r;
        }

        public string getLine(int line)
        {
            string[] lines = File.ReadAllLines(filePath);
            if (line <= lines.Count() && line > 0)
            {
                return lines[line - 1];
            }
            else
            {
                return null;
            }
        }

        public string[] getLines()
        {
            return File.ReadAllLines(filePath);
        }

        public string getAllDocument()
        {
            return File.ReadAllText(filePath);
        }

        public int numLines()
        {
            return File.ReadAllLines(filePath).Count();
        }
    }
}
