using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GNTools;
using System.ComponentModel;

namespace GNTools
{
    public class RepositoryUserCounter:INotifyPropertyChanged
    {
        int counter = 0;
        public int Counter
        {
          get { return counter; }
          set { counter = value;
          if (counter == 0)
              NotifyPropertyChanged("Close");
          }
        }

        private XMLRepositoryHandler repository;
        public XMLRepositoryHandler Repository
        {
            get { return repository; }
            set { repository = value; }
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
