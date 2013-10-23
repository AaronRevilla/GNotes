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


namespace GraphicNotes.Core.Forms
{
    /// <summary>
    /// Interaction logic for TableForm.xaml
    /// </summary>
    public partial class ShareForm : DXWindow
    {
        public int DataPort { get;set;}
        public int FilePort { get; set; }
        public String Password { get; set; }


        public ShareForm()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(dataPortField.Text))
            {
                dataPortField.Focus(); return;
            }
            if (String.IsNullOrEmpty(dataPortField.Text))
            {
                dataPortField.Focus(); return;
            }

            DataPort = (int)(dataPortField.Value);
            FilePort = (int)(filePortField.Value);
            Password = passwordField.Text;
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
