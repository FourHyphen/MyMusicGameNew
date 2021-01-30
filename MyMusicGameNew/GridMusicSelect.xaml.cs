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
        private MusicList Musics { get; set; }

        private MainWindow Main { get; set; }

        public GridMusicSelect()
        {
            InitializeComponent();
        }

        public void Init(MainWindow main)
        {
            Main = main;
            InitMusicList();
            InitDisplay();
        }

        private void InitMusicList()
        {
            Musics = new MusicList();
        }

        private void InitDisplay()
        {
            ResetMusicListBox();
            SetMusicListBox();
            Main.SetGameStatus("Select Music");
        }

        private void ResetMusicListBox()
        {
            int num = MusicListBox.Items.Count;
            for (int i = num - 1; i >= 0; i--)
            {
                MusicListBox.Items.RemoveAt(i);
            }
        }

        private void SetMusicListBox()
        {
            foreach (string musicName in Musics.Names)
            {
                MusicListBox.Items.Add(musicName);
            }
        }

        private void MusicListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // MusicListBoxをいったん初期化する際にも呼び出されるので、ユーザーが曲を選択した際のみ処理するようブロック
            if (MusicListBox.SelectedIndex >= 0)
            {
                MusicListBoxSelectionChangedCore();
            }
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
                double rate = GetNoteSpeedRate();
                Main.GameStart(music, rate);
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

        private double GetNoteSpeedRate()
        {
            foreach (RadioButton rb in NoteSpeedRateList.Items)
            {
                if (rb.IsChecked == true)
                {
                    return double.Parse((string)rb.Content);
                }
            }
            return 1.0;
        }

        public void ResetBestScore(int musicIndex)
        {
            Musics.GetMusic(musicIndex).ResetBestScore();
        }
    }
}
