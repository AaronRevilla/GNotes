using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace GraphicNotes
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        ObservableCollection<GameData> _games=
        new ObservableCollection<GameData>();

        public Window1()
        {
            InitializeComponent();

            for (int i = 0; i < 50; i++)
            {
                GameData ds = new GameData() { GameName="Game"+i};
                _games.Add(ds);
            }
            
        }

        public ObservableCollection<GameData> Games
        { get { return _games; } }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    BitmapImage bi = imageEdit1.Source as BitmapImage;
        //    byte[] data;
        //    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(bi));
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        encoder.Save(ms);
        //        data = ms.ToArray();

        //        BitmapImage imageSource = new BitmapImage();
        //        imageSource.BeginInit();
        //        imageSource.StreamSource = new MemoryStream(data);
        //        imageSource.EndInit();

        //        // Assign the Source property of your image
           
        //    }

        //    Console.WriteLine(data[0] + " " + data[1] + " " + data[2] + " " + data[3]);
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int c = 0;
            foreach (GameData ds in _games)
            {
                ds.GameName = "Servidor"+c;
                c++;
            }
        }
    }

    public class GameData
    {
        public string GameName { get; set; }
        public string Creator { get; set; }
        public string Publisher { get; set; }
    }
}
