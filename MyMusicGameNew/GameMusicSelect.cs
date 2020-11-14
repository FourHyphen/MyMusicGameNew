using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GameMusicSelect : GameState
    {
        private List<string> MusicList { get; set; }

        public GameMusicSelect(MainWindow main) : base(main) { }

        public void Init()
        {
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
            SetGameStatus("Select Music");
            SetPlayingMusicStatus("Not");
        }

        private void SetMusicListBox()
        {
            foreach (string name in MusicList)
            {
                Main.MusicListBox.Items.Add(name);
            }
        }

        public bool MusicSelected()
        {
            return (Main.MusicListBox.SelectedIndex >= 0);
        }

        public string GetSelectedMusicName()
        {
            return (string)Main.MusicListBox.Items[Main.MusicListBox.SelectedIndex];
        }
    }
}
