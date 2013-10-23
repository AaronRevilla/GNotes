using DevExpress.Xpf.Editors;
using GraphicNotes.Views.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace GraphicNotes.Views.Objects
{
    class TextObject:BaseObject
    {

        private Popup editPopup;
        private CanvasWorkspaceContainer canvasWorkspaceContainer;

        private TextEdit titleEdit;
        private TextEdit contentEdit;
        private Separator separator;
        private ObjectBar objectBar;


        private bool isSeparatorVisible = true;
        public bool IsSeparatorVisible
        {
            get { return isSeparatorVisible; }
            set{
                if (isSeparatorVisible != value)
                {
                    isSeparatorVisible = value;
                    if (isSeparatorVisible == true)
                        separator.Visibility = Visibility.Visible;
                    else
                        separator.Visibility = Visibility.Collapsed;
                }
            }
        }

        private bool isTitleVisible = true;
        public bool IsTitleVisible
        {
            get { return isTitleVisible; }
            set
            {
                if (isTitleVisible != value)
                {
                    isTitleVisible = value;
                    if (isTitleVisible == true)
                        titleEdit.Visibility = Visibility.Visible;
                    else
                        titleEdit.Visibility = Visibility.Collapsed;
                }
            }
        }

        private String titleFont = "";
        public String TitleFont
        {
            get { return titleFont; }
            set
            {
                if (!titleFont.Equals(value))
                {
                    titleFont = value;
                    titleEdit.FontFamily = new FontFamily(titleFont);
                }
            }
        }

        private String contentFont = "";
        public String ContentFont
        {
            get { return contentFont; }
            set
            {
                if (!contentFont.Equals(value))
                {
                    contentFont = value;
                    contentEdit.FontFamily = new FontFamily(contentFont);
                }
            }
        }

        private int titleSize;
        public int TitleSize
        {
            get { return titleSize; }
            set
            {
                if (titleSize != value)
                {
                    titleSize = value;
                    titleEdit.FontSize = titleSize;
                }
            }
        }

        private int contentSize;
        public int ContentSize
        {
            get { return contentSize; }
            set
            {
                if (contentSize != value)
                {
                    contentSize = value;
                    contentEdit.FontSize = contentSize;
                }
            }
        }

        private bool isTitleBold = false;
        public bool IsTitleBold
        {
            get { return isTitleBold; }
            set
            {
                if (isTitleBold != value)
                {
                    isTitleBold = value;
                    if (isTitleBold == true)
                        titleEdit.FontWeight = FontWeights.Bold;
                    else
                        titleEdit.FontWeight = FontWeights.Normal;
                }
            }
        }

        private bool isContentBold = false;
        public bool IsContentBold
        {
            get { return isContentBold; }
            set
            {
                if (isContentBold != value)
                {
                    isContentBold = value;
                    if (isContentBold == true)
                        contentEdit.FontWeight = FontWeights.Bold;
                    else
                        contentEdit.FontWeight = FontWeights.Normal;
                }
            }
        }

        private bool isTitleItalic = false;
        public bool IsTitleItalic
        {
            get { return isTitleItalic; }
            set
            {
                if (isTitleItalic != value)
                {
                    isTitleItalic = value;
                    if (isTitleItalic == true)
                        titleEdit.FontStyle = FontStyles.Italic;
                    else
                       titleEdit.FontStyle = FontStyles.Normal;
                }
            }
        }

        private bool isContentItalic = false;
        public bool IsContentItalic
        {
            get { return isContentItalic; }
            set
            {
                if (isContentItalic != value)
                {
                    isContentItalic = value;
                    if (isContentItalic == true)
                        contentEdit.FontStyle = FontStyles.Italic;
                    else
                        contentEdit.FontStyle = FontStyles.Normal;
                }
            }
        }

        private Color titleColor = Colors.Black;
        public Color TitleColor
        {
            get { return titleColor; }
            set
            {
                if (!titleColor.Equals(value))
                {
                    titleColor = value;
                    titleEdit.Foreground = new SolidColorBrush(titleColor);
                }
            }
        }

        private Color contentColor = Colors.Black;
        public Color ContentColor
        {
            get { return contentColor; }
            set
            {
                if (!contentColor.Equals(value))
                {
                    contentColor = value;
                    contentEdit.Foreground = new SolidColorBrush(contentColor);
                }
            }
        }

        static TextObject()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextObject), new FrameworkPropertyMetadata(typeof(TextObject)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            (this.Template.FindName("PART_GridContainer", this) as Grid).DataContext = this;
            //TextBoxes
            titleEdit = this.Template.FindName("PART_Title", this) as TextEdit;
            contentEdit = this.Template.FindName("PART_Content", this) as TextEdit;
            separator = this.Template.FindName("PART_Separator", this) as Separator;

            TitleSize = (int)titleEdit.FontSize;
            TitleFont = titleEdit.FontFamily.ToString();

            ContentSize = (int)contentEdit.FontSize;
            ContentFont = contentEdit.FontFamily.ToString();

            //ObjectBar
            objectBar = this.Template.FindName("PART_ObjectBar", this) as ObjectBar;
            objectBar.Loaded += objectBar_Loaded;

            //CanvasWorkspace
            canvasWorkspaceContainer = UIUtils.FindAncestor(VisualTreeHelper.GetParent(this), typeof(CanvasWorkspaceContainer)) as CanvasWorkspaceContainer;

            //EditPopup
          editPopup = this.Template.FindName("PART_EditPopup", this) as Popup;
          editPopup.Closed += editPopup_Closed;

          //Title
          (this.Template.FindName("PART_IsTitleVisible", this) as CheckEdit).SetBinding(CheckEdit.IsCheckedProperty, new Binding("IsTitleVisible") { Mode = BindingMode.TwoWay });
            
           //Separator
          (this.Template.FindName("PART_IsSeparatorVisible", this) as CheckEdit).SetBinding(CheckEdit.IsCheckedProperty, new Binding("IsSeparatorVisible") { Mode = BindingMode.TwoWay });

            //Font
          (this.Template.FindName("PART_TitleFont", this) as ComboBoxEdit).SetBinding(ComboBoxEdit.TextProperty, new Binding("TitleFont") { Mode = BindingMode.TwoWay });
          (this.Template.FindName("PART_ContentFont", this) as ComboBoxEdit).SetBinding(ComboBoxEdit.TextProperty, new Binding("ContentFont") { Mode = BindingMode.TwoWay });

          //Font Size
          (this.Template.FindName("PART_TitleSize", this) as SpinEdit).SetBinding(SpinEdit.TextProperty, new Binding("TitleSize") { Mode = BindingMode.TwoWay });
          (this.Template.FindName("PART_ContentSize", this) as SpinEdit).SetBinding(SpinEdit.TextProperty, new Binding("ContentSize") { Mode = BindingMode.TwoWay });    

          //Is Bold
          (this.Template.FindName("PART_BoldTitleButton", this) as ToggleButton).SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsTitleBold") { Mode = BindingMode.TwoWay });
          (this.Template.FindName("PART_BoldContentButton", this) as ToggleButton).SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsContentBold") { Mode = BindingMode.TwoWay });
          
          //FontStyle
          (this.Template.FindName("PART_ItalicTitleButton", this) as ToggleButton).SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsTitleItalic") { Mode = BindingMode.TwoWay });
          (this.Template.FindName("PART_ItalicContentButton", this) as ToggleButton).SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsContentItalic") { Mode = BindingMode.TwoWay });

           //ForeColor
          (this.Template.FindName("PART_TitleColor", this) as PopupColorEdit).SetBinding(PopupColorEdit.ColorProperty, new Binding("TitleColor") { Mode=BindingMode.TwoWay });
          (this.Template.FindName("PART_ContentColor", this) as PopupColorEdit).SetBinding(PopupColorEdit.ColorProperty, new Binding("ContentColor") { Mode = BindingMode.TwoWay });
        }

        void editPopup_Closed(object sender, EventArgs e)
        {
            canvasWorkspaceContainer.IsEditing = false;
        }

        void objectBar_Loaded(object sender, RoutedEventArgs e)
        {
            objectBar.EditButton.Click += EditButton_Click;
        }

        void EditButton_Click(object sender, RoutedEventArgs e)
        {
            editPopup.IsOpen = true;
            canvasWorkspaceContainer.IsEditing = true;
        }
    }
}
