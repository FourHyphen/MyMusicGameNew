using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool IsTest { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SetEnvironmentCurrentDirectory(Environment.CurrentDirectory + "../../../");  // F5開始を想定

            Init();
        }

        private void Init()
        {
            MusicSelect.Init(this);
            PlayArea.Init(this, IsTest);
            ShowMusicSelect();
        }

        private void ShowMusicSelect()
        {
            MusicSelect.Visibility = Visibility.Visible;
            PlayArea.Visibility = Visibility.Hidden;
        }

        private void SetEnvironmentCurrentDirectory(string environmentDirPath)
        {
            // TODO: 配布を考えると、ここと同階層にGameDataディレクトリがある場合/ない場合で分岐すべき
            Environment.CurrentDirectory = environmentDirPath;
            IsTest = (Environment.CurrentDirectory.Contains("TestMyMusicGameNew"));
        }

        public void SetGameStatus(string status)
        {
            GameStatus.Content = status;
        }

        public void SetPlayingMusicStatus(string status)
        {
            PlayingMusicStatus.Content = status;
        }

        public void SetNotesNum(string notesNum)
        {
           NotesNum.Content = notesNum;
        }

        private void MainWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (DoGamePlaying())
            {
                PlayArea.Judge(e);
            }
        }

        private bool DoGamePlaying()
        {
            return PlayArea.DoGamePlaying();
        }

        public void GameStart(string musicName)
        {
            ShowPlayArea();
            PlayArea.GameStart(this, musicName);
        }

        private void ShowPlayArea()
        {
            MusicSelect.Visibility = Visibility.Hidden;
            PlayArea.Visibility = Visibility.Visible;
        }

        public void GameFinish()
        {
            ShowMusicSelect();
        }
    }
}
