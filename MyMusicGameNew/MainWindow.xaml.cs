using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMusicGameNew
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitDisplay();
        }

        private void InitDisplay()
        {
            MusicListBox.Items.Add("Music1");
            SetGameStatus("Select Music");
        }

        private void GameStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (MusicSelected())
            {
                SetGameStatus("Playing");
            }
        }

        private bool MusicSelected()
        {
            return (MusicListBox.SelectedIndex >= 0);
        }

        private void SetGameStatus(string status)
        {
            GameStatus.Content = status;
        }
    }
}
