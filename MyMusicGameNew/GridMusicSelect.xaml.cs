using System.Windows;
using System.Windows.Controls;

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
                GamePlaying.NoteDirection noteDirection = GetNoteDirection();
                Main.GameStart(music, rate, noteDirection);
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

        private GamePlaying.NoteDirection GetNoteDirection()
        {
            foreach (RadioButton rb in NoteDirectionList.Items)
            {
                if (rb.IsChecked == true)
                {
                    return GamePlaying.ToNoteDirection(rb.Name);
                }
            }
            return GamePlaying.NoteDirection.TopToBottom;
        }

        /// <summary>
        /// テストで使用している(MainWindowDriverから呼び出す)ため削除しないこと
        /// </summary>
        /// <param name="musicIndex"></param>
        public void SetNoteSpeedRate(double noteSpeedRate)
        {
            foreach (RadioButton rb in NoteSpeedRateList.Items)
            {
                if (double.Parse((string)rb.Content) == noteSpeedRate)
                {
                    rb.IsChecked = true;
                    return;
                }
            }
        }

        /// <summary>
        /// テストで使用している(MainWindowDriverから呼び出す)ため削除しないこと
        /// </summary>
        /// <param name="noteDirection"></param>
        public void SetNoteDirection(GamePlaying.NoteDirection noteDirection)
        {
            foreach (RadioButton rb in NoteDirectionList.Items)
            {
                GamePlaying.NoteDirection rbDirection = GamePlaying.ToNoteDirection(rb.Name);
                if (rbDirection == noteDirection)
                {
                    rb.IsChecked = true;
                    return;
                }
            }
        }

        /// <summary>
        /// テストで使用している(MainWindowDriverから呼び出す)ため削除しないこと
        /// </summary>
        /// <param name="musicIndex"></param>
        public void ResetBestScore(int musicIndex)
        {
            Musics.GetMusic(musicIndex).ResetBestScore();
        }
    }
}
