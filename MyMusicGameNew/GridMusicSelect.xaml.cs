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
    /// <summary>
    /// GridMusicSelect.xaml の相互作用ロジック
    /// </summary>
    public partial class GridMusicSelect : UserControl
    {
        private List<string> MusicList { get; set; }

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
            string musicsFilePath = Common.GetFilePathOfDependentEnvironment("/GameData/MusicList.json");
            MusicList = Common.GetStringListInJson(musicsFilePath);
        }

        private void InitDisplay()
        {
            SetMusicListBox();
            Main.SetGameStatus("Select Music");
            Main.SetPlayingMusicStatus("Not");
        }

        private void SetMusicListBox()
        {
            foreach (string name in MusicList)
            {
                MusicListBox.Items.Add(name);
            }
        }

        private void GameStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (ReadyGameStart())
            {
                string musicName = GetSelectedMusicName();
                Main.GameStart(musicName);
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

        private string GetSelectedMusicName()
        {
            return (string)MusicListBox.Items[MusicListBox.SelectedIndex];
        }
    }
}
