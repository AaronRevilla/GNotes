using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace GraphicNotes
{
    partial class MainWindow : Window, IDisposable
    {
        private void SetBindings()
        {
            connectRepositoryServerButton.SetBinding(Button.ContentProperty, new Binding("RepositoryClient.State") { Converter = new EnumToObjectConverter("Connect", "Connecting...", "Disconnect") });
            ipRepositoryField.SetBinding(TextEdit.IsEnabledProperty, new Binding("RepositoryClient.State") { Converter = new EnumToObjectConverter(true, false, false) });
         
            synchronizeButton.SetBinding(Button.IsEnabledProperty, new Binding("RepositoryClient.State") { Converter = new EnumToObjectConverter(false, false, true) });
            synchronizeButton.SetBinding(Button.ContentProperty, new Binding("Synchronizer.IsSynchronizing") { Converter = new BoolToTextConverter("Synchronizing...", "Synchronize") });

            loadLocalRepositoryButton.SetBinding(Button.ContentProperty, new Binding("XmlRepositoryHandler.IsLoaded") { Converter=new BoolToTextConverter("Unload","Load")});
            closeSynPopupButton.SetBinding(Button.VisibilityProperty, new Binding("Synchronizer.IsSynchronizing") { Converter=new BoolToVisibilityInverseConverter()});
            synProgressBar.SetBinding(ProgressBarEdit.VisibilityProperty, new Binding("Synchronizer.IsSynchronizing") { Converter = new BoolToVisibilityConverter() });
            bDocumentServerState.SetBinding(BarStaticItem.ContentProperty, new Binding("CurrentDocument.Server.IsRunning"){Converter=new BoolToTextConverter("Sharing...","")});
            bDocumentClientState.SetBinding(BarStaticItem.ContentProperty, new Binding("CurrentDocument.IsCollaborating") { Converter = new BoolToTextConverter("Collaborating...", "") });
            bConnectingState.SetBinding(BarStaticItem.ContentProperty, new Binding("ConnectingState"));
        }
    }
}
