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

        public MainWindow()
        {
            InitializeComponent();
            SetEnvironmentCurrentDirectory(Environment.CurrentDirectory + "../../../");  // F5開始を想定
            InitMusicList();
            InitDisplay();
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
            GetNotes(musicName);
            PlayMusic(musicName);
            SetGameStatus("Playing");
            SetPlayingMusicStatus("Playing");
        }

        private void GetNotes(string musicName)
        {
            string jsonPath = Common.GetFilePathOfDependentEnvironment("/GameData/Note/" + musicName + ".json");
            string jsonStr = Common.ReadFile(jsonPath);
            Notes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Note>>(jsonStr);
            NotesNum.Content = Notes.Count.ToString();
        }

        private void PlayMusic(string musicName)
        {
            PlayingMusic play = PlayingMusicFactory.Create(musicName, IsTest);
            play.PlayAsync();
        }
    }
}
