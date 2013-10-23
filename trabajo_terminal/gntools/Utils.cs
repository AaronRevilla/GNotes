using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Threading;


namespace GNTools
{
    public class Utils
    {
        [Conditional("DEBUG")]
        public static void WriteLine(String format, params object[] args)
        {
            Console.WriteLine(format, args);
           
        }

    

    }
}
