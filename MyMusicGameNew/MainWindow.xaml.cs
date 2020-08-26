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
            SetEnvironmentCurrentDirectory(Environment.CurrentDirectory + "../../../");  // F5開始を想定
            InitDisplay();
        }

        private void SetEnvironmentCurrentDirectory(string environmentDirPath)
        {
            // TODO: 配布を考えると、ここと同階層にGameDataディレクトリがある場合/ない場合で分岐すべき
            Environment.CurrentDirectory = environmentDirPath;
        }

        private void InitDisplay()
        {
            MusicListBox.Items.Add("Music1");
            SetGameStatus("Select Music");
            SetPlayingMusicStatus("Not");
        }

        private void SetGameStatus(string status)
        {
            GameStatus.Content = status;
        }

        private void SetPlayingMusicStatus(string status)
        {
            PlayingMusicStatus.Content = status;
        }

        private void GameStartButtonClick(object sender, RoutedEventArgs e)
        {
            GameStartCore();
        }

        private void GameStartCore()
        {
            if (MusicSelected())
            {
                PlayMusic();
                SetGameStatus("Playing");
            }
        }

        private bool MusicSelected()
        {
            return (MusicListBox.SelectedIndex >= 0);
        }

        private void PlayMusic()
        {
            System.Media.SoundPlayer Player = new System.Media.SoundPlayer("GameData/Music/carenginestart1.wav");
            Player.Play();  // 非同期再生
            SetPlayingMusicStatus("Playing");
        }
    }
}
