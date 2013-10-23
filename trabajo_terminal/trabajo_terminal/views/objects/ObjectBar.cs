using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace GraphicNotes.Views.Objects
{
    class ObjectBar:ContentControl
    {
        private BaseObject element;
        private Button deleteButton;

        public Button DeleteButton
        {
            get { return deleteButton; }
            set { deleteButton = value; }
        }
        private Button editButton;

        public Button EditButton
        {
            get { return editButton; }
            set { editButton = value; }
        }
        private Button lockButton;

        public Button LockButton
        {
            get { return lockButton; }
            set { lockButton = value; }
        }


        public ObjectBar()
        {
            this.Loaded+=ObjectBar_Loaded;
        }

        static ObjectBar()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ObjectBar), new FrameworkPropertyMetadata(typeof(ObjectBar)));
        }

        private void ObjectBar_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Loaded wins");
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Console.WriteLine("OnApplyTemplate wins");
            deleteButton = this.Template.FindName("PART_DeleteButton", this) as Button;
            editButton = this.Template.FindName("PART_EditButton", this) as Button;
            lockButton = this.Template.FindName("PART_LockButton", this) as Button;
        }
    }
}
