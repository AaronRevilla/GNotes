using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GraphicNotes.Views.Workspace
{
    public class CanvasWorkspaceContainer: ContentControl
    {

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        public static readonly DependencyProperty IsEditingProperty =
          DependencyProperty.Register("IsEditing", typeof(bool),
                                      typeof(CanvasWorkspaceContainer),
                                      new FrameworkPropertyMetadata(false));

        static CanvasWorkspaceContainer()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasWorkspaceContainer), new FrameworkPropertyMetadata(typeof(CanvasWorkspaceContainer)));
        }
    }
}
