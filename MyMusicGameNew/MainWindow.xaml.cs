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
        private bool IsTest { get; set; }

        private List<string> MusicList { get; set; }

        private List<Note> Notes { get; set; }

        private System.Timers.Timer GameFinishTimer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SetEnvironmentCurrentDirectory(Environment.CurrentDirectory + "../../../");  // F5開始を想定
            InitMusicList();
            InitDisplay();
        }

        ~MainWindow()
        {
            if (GameFinishTimer != null)
            {
                GameFinishTimer.Stop();
                GameFinishTimer.Dispose();
            }
        }

        private void SetEnvironmentCurrentDirectory(string environmentDirPath)
        {
            // TODO: 配布を考えると、ここと同階層にGameDataディレクトリがある場合/ない場合で分岐すべき
            Environment.CurrentDirectory = environmentDirPath;
            IsTest = (Environment.CurrentDirectory.Contains("TestMyMusicGameNew"));
        }

        private void InitMusicList()
        {
            string musicsFilePath = Common.GetFilePathOfDependentEnvironment("/GameData/MusicList.json");
            MusicList = Common.GetStringListInJson(musicsFilePath);
        }

        private void InitDisplay()
        {
            SetMusicListBox();
            SetGameStatus("Select Music");
            SetPlayingMusicStatus("Not");
        }

        private void SetMusicListBox()
        {
            foreach (string name in MusicList)
            {
                MusicListBox.Items.Add(name);
            }
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
            GameStart();
        }

        private void GameStart()
        {
            if (MusicSelected())
            {
                string musicName = GetSelectedMusicName();
                GameStartCore(musicName);
            }
        }

        private bool MusicSelected()
        {
            return (MusicListBox.SelectedIndex >= 0);
        }

        private string GetSelectedMusicName()
        {
            return (string)MusicListBox.Items[MusicListBox.SelectedIndex];
        }

        private void GameStartCore(string musicName)
        {
            Music music = new MusicFactory().Create(musicName, isTest: IsTest);
            PlayMusic(music);
            SetNotesNum(music);
            SetGameFinishedTimer(music.TimeSecond);
            SetGameStatus("Playing");
            SetPlayingMusicStatus("Playing");
        }

        private void PlayMusic(Music music)
        {
            music.PlayAsync();
        }

        private void SetNotesNum(Music music)
        {
            NotesNum.Content = music.Notes.Count.ToString();
        }

        private void SetGameFinishedTimer(int musicTimeSecond)
        {
            GameFinishTimer = new System.Timers.Timer();
            GameFinishTimer.Interval = (musicTimeSecond + 1) * 1000;  // 1[s]余裕を持たせる
            GameFinishTimer.Elapsed += (s, e) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    ProcessGameFinished();
                }));
            };
            GameFinishTimer.Start();
        }

        private void ProcessGameFinished()
        {
            GameFinishTimer.Stop();
            GameFinishTimer.Dispose();
            SetGameStatus("Finished");
            SetPlayingMusicStatus("Finished");
        }
    }
}
