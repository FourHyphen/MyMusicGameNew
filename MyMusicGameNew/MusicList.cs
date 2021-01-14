using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyMusicGameNew
{
    class MusicList
    {
        private List<Music> Musics { get; set; } = new List<Music>();

        public MusicList(bool isTest)
        {
            List<string> musicNames = GetMusicNames();
            CreateMusics(musicNames, isTest);
        }

        private List<string> GetMusicNames()
        {
            string musicsFilePath = Common.GetFilePathOfDependentEnvironment("/GameData/MusicList.json");
            return Common.GetStringListInJson(musicsFilePath);
        }

        private void CreateMusics(List<string> musicNames, bool isTest)
        {
            MusicFactory mf = new MusicFactory();
            foreach (string name in musicNames)
            {
                Music music = mf.Create(name, isTest);
                Musics.Add(music);
            }
        }

        public Music GetMusic(int index)
        {
            return Musics[index];
        }

        public int GetBestScore(int index)
        {
            return GetMusic(index).BestScore;
        }

        public int GetBestResultPerfect(int index)
        {
            return GetMusic(index).BestResultPerfect;
        }

        public int GetBestResultGood(int index)
        {
            return GetMusic(index).BestResultGood;
        }

        public int GetBestResultBad(int index)
        {
            return GetMusic(index).BestResultBad;
        }

        public void SetMusicNames(ListBox musicListBox)
        {
            foreach (Music music in Musics)
            {
                musicListBox.Items.Add(music.Name);
            }
        }
    }
}
