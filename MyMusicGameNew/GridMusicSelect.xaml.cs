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
    public partial class GridMusicSelect : UserControl
    {
        private bool IsTest { get; set; }

        private MusicList Musics { get; set; }

        private MainWindow Main { get; set; }

        public GridMusicSelect()
        {
            InitializeComponent();
        }

        public void Init(MainWindow main, bool isTest)
        {
            IsTest = isTest;
            Main = main;
            InitMusicList();
            InitDisplay();
        }

        private void InitMusicList()
        {
            Musics = new MusicList(IsTest);
        }

        private void InitDisplay()
        {
            SetMusicListBox();
            Main.SetGameStatus("Select Music");
            Main.SetPlayingMusicStatus("Not");
        }

        private void SetMusicListBox()
        {
            Musics.SetMusicNames(MusicListBox);
        }

        private void MusicListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MusicListBoxSelectionChangedCore();
        }

        private void MusicListBoxSelectionChangedCore()
        {
            GameStartButton.Content = "Game Start";
            SetBestScore();
        }

        private void SetBestScore()
        {
            int now = MusicListBox.SelectedIndex;
            BestScore.Content = Musics.GetBestScore(now);
            BestResultPerfect.Content = Musics.GetBestResultPerfect(now);
            BestResultGood.Content = Musics.GetBestResultGood(now);
            BestResultBad.Content = Musics.GetBestResultBad(now);
        }

        private void GameStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (ReadyGameStart())
            {
                Music music = GetSelectedMusic();
                Main.GameStart(music);
            }
        }

        private bool ReadyGameStart()
        {
            return (MusicSelected());
        }

        private bool MusicSelected()
        {
            return (MusicListBox.SelectedIndex >= 0);
        }

        private Music GetSelectedMusic()
        {
            return Musics.GetMusic(MusicListBox.SelectedIndex);
        }
    }
}
