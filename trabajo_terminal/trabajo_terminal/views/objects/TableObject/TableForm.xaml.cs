using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;


namespace GraphicNotes.Views.Objects
{
    /// <summary>
    /// Interaction logic for TableForm.xaml
    /// </summary>
    public partial class TableForm : DXWindow
    {
        public int Columns { get;set;}
        public int Rows { get; set; }

        public TableForm()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
           
            Columns = (int)(columnSpin.Value);
            Rows = (int)(rowSpin.Value);
            this.DialogResult =true;
            this.Hide();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Hide();
        }


    }
}
