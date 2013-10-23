using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GraphicNotes
{
    partial class MainWindow : Window, IDisposable
    {

        private void SetTheme()
        {
            ThemeManager.ThemeChanged += OnApplicationThemeChanged;
            ThemeManager.ApplicationThemeName = "Office2013";
        }

        private void SelectCurrentThemeItem()
        {
            string themeName = ThemeManager.ApplicationThemeName;
            foreach (GalleryItemGroup group in gTheme.Gallery.Groups)
            {
                foreach (GalleryItem item in group.Items)
                {
                    if (Convert.ToString(item.Caption).Replace(" ", "") == themeName)
                    {
                        gTheme.Gallery.ItemClick -= OnThemeItemClick;
                        item.IsChecked = true;
                        gTheme.Gallery.ItemClick += new GalleryItemEventHandler(OnThemeItemClick);
                        return;
                    }
                }
            }
        }

        private void OnApplicationThemeChanged(DependencyObject sender, ThemeChangedRoutedEventArgs e)
        {
            SelectCurrentThemeItem();
            UpdateApplicationButtonLargeIcon(e.ThemeName);
        }


        private void UpdateApplicationButtonLargeIcon(string themeName)
        {
            if (ribbonControl == null || string.IsNullOrEmpty(themeName)) return;
            if (themeName == Theme.MetropolisDarkName || themeName == Theme.MetropolisLightName || themeName == Theme.Office2013Name)
            {
                ribbonControl.ApplicationButtonLargeIcon = null;
                ribbonControl.RibbonStyle = RibbonStyle.Office2010;
            }
            else
            {
                ribbonControl.ApplicationButtonLargeIcon = new BitmapImage(new Uri(@"/GraphicNotes;component/Images/ribbon-application-32x32.png", UriKind.Relative));
                ribbonControl.RibbonStyle = RibbonStyle.Office2007;
            }
        }

        protected void OnThemeDropDownGalleryInit(object sender, DropDownGalleryEventArgs e)
        {
            Gallery gallery = e.DropDownGallery.Gallery;
            gallery.AllowHoverImages = false;
            gallery.IsItemCaptionVisible = true;
            gallery.ItemGlyphLocation = Dock.Top;
            gallery.IsGroupCaptionVisible = DefaultBoolean.True;
        }
        protected void OnThemeItemClick(object sender, GalleryItemEventArgs e)
        {
            string themeName = (string)e.Item.Caption;
            themeName = themeName.Replace(" ", string.Empty);
            //Theme = Theme.FindTheme(themeName); 
            ThemeManager.ApplicationThemeName = themeName;
        }
    }
}
