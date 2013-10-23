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
using System.Xml.Linq;


namespace RepositoryServer
{
    /// <summary>
    /// Interaction logic for CredentialsForm.xaml
    /// </summary>
    public partial class CredentialsForm : DXWindow
    {

        public String value="";
        private XElement repository;

        public CredentialsForm(XElement repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(valueField.Text))
            {
                valueField.Focus(); return;
            }
            else
            {
                value = valueField.Text;
            }

            if (repository.Attribute("password").Value.Equals(value))
            {
                this.DialogResult = true;
                this.Hide();
            }
            else
            {
                errorField.Text = "The password you entered is incorrect.";
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Hide();
        }
    }
}
