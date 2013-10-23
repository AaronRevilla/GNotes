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
using System.Collections.ObjectModel;
using System.Xml.Linq;
using GNTools;


namespace RepositoryServer
{
    /// <summary>
    /// Interaction logic for RepositoryServerForm.xaml
    /// </summary>
    public partial class RepositoryForm : DXWindow
    {
        public XElement Repository { get; set; }
        private bool editing = false;

        public RepositoryForm()
        {
            InitializeComponent();
        }

        public RepositoryForm(XElement repository)
        {
            InitializeComponent();
            editing = true;
            this.nameField.IsEnabled = false;
            this.nameField.Text = repository.Attribute("name").Value;
            this.enabledField.IsChecked = Boolean.Parse(repository.Attribute("enabled").Value);
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            errorField.Text = "";

            if (String.IsNullOrEmpty(nameField.Text)){
                nameField.Focus(); return;
            }

            if(String.IsNullOrEmpty(passwordField.Text)){
                passwordField.Focus(); return;
            }

            if (!editing)
            {
                XMLServerHandler aux = DataContext as XMLServerHandler;
                var query = from element in aux.XmlDocument.Root.Descendants("repository")
                            where element.Attribute("name").Value.ToUpper().Equals(nameField.Text.ToUpper())
                            select element;

                if (query.Count() > 0)
                {
                    errorField.Text = "The name of the repository is already in use.";
                    return;
                }
            }

            Repository = XElement.Parse(String.Format(@"<repository name=""{0}"" password=""{1}"" enabled=""{2}""></repository>",nameField.Text,passwordField.Text,enabledField.IsChecked.Value.ToString())); 
            this.DialogResult = true;
            this.Hide();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Hide();
        }


    }
}
