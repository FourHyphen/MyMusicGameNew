using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyMusicGameNew
{
    public class MusicFactory
    {
        public Music Create(string musicName, bool isTest=false)
        {
            MusicInfo info = GetMusicInfo(musicName);
            List<Note> notes = GetNotes(info.Note);
            PlayingMusic play = GetPlayingMusic(info.MusicData, isTest);
            return new Music(musicName, info.TimeSecond, notes, play);
        }

        private MusicInfo GetMusicInfo(string musicName)
        {
            string jsonPath = Common.GetFilePathOfDependentEnvironment("/GameData/MusicInfo/" + musicName + ".json");
            string jsonStr = Common.ReadFile(jsonPath);
            return JsonConvert.DeserializeObject<MusicInfo>(jsonStr);
        }

        private List<Note> GetNotes(string notePath)
        {
            string jsonPath = Common.GetFilePathOfDependentEnvironment(notePath);
            string jsonStr = Common.ReadFile(jsonPath);
            return JsonConvert.DeserializeObject<List<Note>>(jsonStr);
        }

        private PlayingMusic GetPlayingMusic(string musicDataPath, bool isTest)
        {
            return PlayingMusicFactory.Create(musicDataPath, isTest);
        }
    }
}
