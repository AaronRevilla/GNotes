using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using GraphicNotes.Views.Workspace;
using System.Windows.Media;
using GraphicNotes.Views.Adorners;
using GNTools;
using GraphicNotes.Core;
using GraphicNotes.Communication;

namespace GraphicNotes.Views.Objects
{
    public class BaseObject: ContentControl
    {
        private Document document;

        public enum ObjectState
        {
            Locked,
            Unlocked,
            Waiting,
            Dormant
        }

        public String ID
        {
            get { return (String)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public static readonly DependencyProperty IDProperty =
          DependencyProperty.Register("ID", typeof(String),
                                      typeof(BaseObject),
                                      new FrameworkPropertyMetadata(""));

        public ObjectState State
        {
            get { return (ObjectState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
          DependencyProperty.Register("State", typeof(ObjectState),
                                      typeof(BaseObject),
                                      new FrameworkPropertyMetadata(ObjectState.Dormant));

        private Collaborator remoteUser = new Collaborator();
        public Collaborator RemoteUser
        {
            get { return remoteUser; }
        }
        //public bool IsSelected
        //{
        //    get { return (bool)GetValue(IsSelectedProperty); }
        //    set { SetValue(IsSelectedProperty, value); }
        //}

        //public static readonly DependencyProperty IsSelectedProperty =
        //  DependencyProperty.Register("IsSelected", typeof(bool),
        //                              typeof(BaseObject),
        //                              new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty MoveThumbTemplateProperty =
            DependencyProperty.RegisterAttached("MoveThumbTemplate", typeof(ControlTemplate), typeof(BaseObject));

        public static ControlTemplate GetMoveThumbTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(MoveThumbTemplateProperty);
        }

        public static void SetMoveThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(MoveThumbTemplateProperty, value);
        }

        static BaseObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseObject), new FrameworkPropertyMetadata(typeof(BaseObject)));
        }

        public BaseObject()
        {
            this.Loaded += new RoutedEventHandler(this.BaseObject_Loaded);
            remoteUser.Color = Brushes.Yellow;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            CanvasWorkspace canvasWorkspace = VisualTreeHelper.GetParent(this) as CanvasWorkspace;

            if (canvasWorkspace != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None && document.Client.State==CommonNetworkEnums.ClientState.Disconnected)
                {
                    this.State = ObjectState.Unlocked;
                }
                else
                {
                    if (this.State!=ObjectState.Unlocked)
                    {
                        canvasWorkspace.DeselectAll();
                        Select();
                    }
                }
            }

            e.Handled = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            document = this.DataContext as Document;
        }

        private void BaseObject_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (this.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;

                MoveThumb thumb =
                    this.Template.FindName("PART_MoveThumb", this) as MoveThumb;

                if (contentPresenter != null && thumb != null)
                {
                    UIElement contentVisual =
                        VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;

                    if (contentVisual != null)
                    {
                        ControlTemplate template =
                            BaseObject.GetMoveThumbTemplate(contentVisual) as ControlTemplate;

                        if (template != null)
                        {
                            thumb.Template = template;
                        }
                    }
                }
                ResizeDecorator resizeDecorator =
                    this.Template.FindName("PART_ResizeDecorator", this) as ResizeDecorator;
                if (resizeDecorator != null)
                    resizeDecorator.ShowAdorner();
            }

            
        }


        public void Select()
        {
            if (document.Client.State == CommonNetworkEnums.ClientState.Connected)
            {
                this.State = ObjectState.Waiting;
            }
            else
            {
                this.State = ObjectState.Unlocked;
            }
        }

        public void Deselect()
        {
            if (document.Client.State == CommonNetworkEnums.ClientState.Connected)
            {
                this.State = ObjectState.Dormant;
            }
            else
            {
                this.State = ObjectState.Dormant;
            }
        }
       

    }
}
